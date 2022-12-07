using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Dm;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Dm.Scaffolding.Internal
{
	public class DmDatabaseModelFactory : IDatabaseModelFactory
	{
		private readonly IDiagnosticsLogger<DbLoggerCategory.Scaffolding> _logger;

		private const string NamePartRegex = "(?:(?:\\[(?<part{0}>(?:(?:\\]\\])|[^\\]])+)\\])|(?<part{0}>[^\\.\\[\\]]+))";

		private static readonly Regex _partExtractor = new Regex(string.Format(CultureInfo.InvariantCulture, "^{0}(?:\\.{1})?$", string.Format(CultureInfo.InvariantCulture, "(?:(?:\\[(?<part{0}>(?:(?:\\]\\])|[^\\]])+)\\])|(?<part{0}>[^\\.\\[\\]]+))", 1), string.Format(CultureInfo.InvariantCulture, "(?:(?:\\[(?<part{0}>(?:(?:\\]\\])|[^\\]])+)\\])|(?<part{0}>[^\\.\\[\\]]+))", 2)), RegexOptions.Compiled, TimeSpan.FromMilliseconds(1000.0));

		private static readonly ISet<string> _dateTimePrecisionTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "timestamp", "datetime", "time" };

		private static readonly ISet<string> _integerTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "byte", "tinyint", "smallint", "int", "bigint", "bit" };

		private static readonly ISet<string> _decTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "dec", "numeric", "decimal" };

		private static readonly Dictionary<string, long[]> _defaultSequenceMinMax = new Dictionary<string, long[]>(StringComparer.OrdinalIgnoreCase)
		{
			{
				"tinyint",
				new long[2] { 0L, 255L }
			},
			{
				"smallint",
				new long[2] { -32768L, 32767L }
			},
			{
				"int",
				new long[2] { -2147483648L, 2147483647L }
			},
			{
				"bigint",
				new long[2] { -9223372036854775808L, 9223372036854775807L }
			}
		};

		public DmDatabaseModelFactory(IDiagnosticsLogger<DbLoggerCategory.Scaffolding> logger)
		{
			Check.NotNull<IDiagnosticsLogger<DbLoggerCategory.Scaffolding>>(logger, "logger");
			_logger = logger;
		}

		private IEnumerable<DatabaseSequence> GetSequences(DbConnection connection, Func<string, string> schemaFilter)
		{
			using DbCommand command = connection.CreateCommand();
			command.CommandText = "\nSELECT\r\n        SCHEMAS.NAME                                             AS SCH_NAME       ,\r\n        SEQS.NAME                                                AS SEQ_NAME       ,\r\n        SEQS.INFO3                                               AS SEQ_START_VALUE,\r\n        DBA_SEQUENCES.MIN_VALUE                                  AS SEQ_MIN_VALUE  ,\r\n        DBA_SEQUENCES.MAX_VALUE                                  AS SEQ_MAX_VALUE  ,\r\n        DBA_SEQUENCES.INCREMENT_BY                               AS SEQ_INCREMENT  ,\r\n        CASE DBA_SEQUENCES.CYCLE_FLAG WHEN 'Y' THEN 1 ELSE 0 END AS SEQ_CYCLE\r\nFROM\r\n        (\r\n                SELECT NAME, ID FROM SYSOBJECTS WHERE PID = UID() AND TYPE$='SCH'\r\n        )\r\n        SCHEMAS,\r\n        (\r\n                SELECT * FROM SYSOBJECTS WHERE SUBTYPE$ ='SEQ'\r\n        )\r\n        SEQS,\r\n        DBA_SEQUENCES\r\nWHERE\r\n        SEQS.SCHID   = SCHEMAS.ID \r\n    AND SEQS.NAME    = DBA_SEQUENCES.SEQUENCE_NAME\r\n    AND SCHEMAS.NAME = DBA_SEQUENCES.SEQUENCE_OWNER;";
			if (schemaFilter != null)
			{
				command.CommandText = command.CommandText + "\nAND " + schemaFilter("SCHEMAS.NAME");
			}
			using DbDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				DatabaseSequence val = new DatabaseSequence();
				val.Schema=(reader.GetValueOrDefault<string>("SCH_NAME"));
				val.Name=(reader.GetValueOrDefault<string>("SEQ_NAME"));
				val.StoreType=("BIGINT");
				val.IsCyclic=((bool?)(reader.GetValueOrDefault<int>("SEQ_CYCLE") > 0));
				val.IncrementBy=((int?)(int)reader.GetValueOrDefault<long>("SEQ_INCREMENT"));
				val.StartValue=((long?)reader.GetValueOrDefault<long>("SEQ_START_VALUE"));
				val.MinValue=((long?)reader.GetValueOrDefault<long>("SEQ_MIN_VALUE"));
				val.MaxValue=((long?)reader.GetValueOrDefault<long>("SEQ_MAX_VALUE"));
				DatabaseSequence sequence = val;
				_logger.SequenceFound(DisplayName(sequence.Schema, sequence.Name), sequence.StoreType, sequence.IsCyclic, sequence.IncrementBy, sequence.StartValue, sequence.MinValue, sequence.MaxValue);
				if (_defaultSequenceMinMax.ContainsKey(sequence.StoreType))
				{
					long defaultMin = _defaultSequenceMinMax[sequence.StoreType][0];
					sequence.MinValue=((sequence.MinValue == defaultMin) ? null : sequence.MinValue);
					sequence.StartValue=((sequence.StartValue == defaultMin) ? null : sequence.StartValue);
					long defaultMax = _defaultSequenceMinMax[sequence.StoreType][1];
					sequence.MaxValue=((sequence.MaxValue == defaultMax) ? null : sequence.MaxValue);
				}
				yield return sequence;
			}
		}

		private void GetTables(DbConnection connection, Func<string, string, string> tableFilter, DatabaseModel databaseModel)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			using DbCommand dbCommand = connection.CreateCommand();
			string text = "";
			dbCommand.CommandText = "\nSELECT SCH.NAME AS SCH_NAME, TAB.NAME AS TAB_NAME FROM\r\n(SELECT NAME,ID FROM SYSOBJECTS WHERE TYPE$ = 'SCH' AND PID = UID())SCH,\r\nSYSOBJECTS TAB\r\nWHERE TAB.SCHID = SCH.ID AND TAB.TYPE$ = 'SCHOBJ' AND TAB.SUBTYPE$ IN ('UTAB') AND TAB.NAME <> '##HISTOGRAMS_TABLE' AND TAB.NAME <> '##PLAN_TABLE'AND  TAB.NAME <> '__EFMigrationsHistory' AND TAB.INFO3 & 0XFF <> 64";
			if (tableFilter != null)
			{
				text = " AND " + tableFilter("SCH.NAME", "TAB.NAME");
			}
			dbCommand.CommandText += text;
			using (DbDataReader dbDataReader = dbCommand.ExecuteReader())
			{
				while (dbDataReader.Read())
				{
					DatabaseTable val = new DatabaseTable();
					val.Schema=(dbDataReader.GetValueOrDefault<string>("SCH_NAME"));
					val.Name=(dbDataReader.GetValueOrDefault<string>("TAB_NAME"));
					DatabaseTable val2 = val;
					_logger.TableFound(DisplayName(val2.Schema, val2.Name));
					databaseModel.Tables.Add(val2);
				}
			}
			GetColumns(connection, text, databaseModel);
			GetKeys(connection, text, databaseModel);
			GetIndexes(connection, text, databaseModel);
			GetForeignKeys(connection, text, databaseModel);
		}

		private void GetColumns(DbConnection connection, string tableFilter, DatabaseModel databaseModel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Expected O, but got Unknown
			DmCommand val = (DmCommand)connection.CreateCommand();
			try
			{
				((DbCommand)(object)val).CommandText = "SELECT\r\n        /*+ MAX_OPT_N_TABLES(5) */\r\n        SCH.NAME AS SCH_NAME   ,\r\n        TAB.NAME AS TAB_NAME   ,\r\n        COL.NAME AS COL_NAME   ,\r\n        COL.TYPE$                                     AS TYPE_NAME,\r\n        COL.LENGTH$                                   AS COL_LENGTH,\r\n        COL.SCALE AS COL_SCALE  ,\r\n        COL.DEFVAL AS COL_DEF    ,\r\n        COL.COLID + 1                                 AS COL_ORDINAL,\r\n        CASE COL.NULLABLE$ WHEN 'Y' THEN 1 ELSE 0 END AS IS_NULLABLE,\r\n        COL.INFO2 & 0X01                              AS IS_IDENTITY,\r\n        (\r\n                SELECT\r\n                        SF_GET_INDEX_KEY_SEQ(IND.KEYNUM, IND.KEYINFO, COL.COLID)\r\n                FROM\r\n                        SYS.SYSINDEXES IND,\r\n                        (\r\n                                SELECT\r\n                                        OBJ.NAME,\r\n                                        CON.ID,\r\n                                        CON.TYPE$  ,\r\n                                        CON.TABLEID,\r\n                                        CON.COLID,\r\n                                        CON.INDEXID\r\n                                FROM\r\n                                        SYS.SYSCONS    AS CON,\r\n                                        SYS.SYSOBJECTS AS OBJ\r\n                                WHERE\r\n                                        CON.TYPE$   = 'P'\r\n                                    AND OBJ.SUBTYPE$= 'CONS'\r\n                                    AND OBJ.ID = CON.ID\r\n                        )\r\n                        CON,\r\n                        (\r\n                                SELECT ID, NAME FROM SYS.SYSOBJECTS WHERE SUBTYPE$= 'INDEX'\r\n                        )\r\n                        OBJ_IND\r\n                WHERE\r\n                        CON.INDEXID = IND.ID\r\n                    AND IND.ID = OBJ_IND.ID\r\n                    AND CON.TABLEID = TAB.ID\r\n                    AND SF_COL_IS_IDX_KEY(IND.KEYNUM, IND.KEYINFO, COL.COLID)= 1\r\n        ) AS PK_ORDINAL\r\nFROM\r\n        SYS.SYSCOLUMNS COL,\r\n        (\r\n                SELECT\r\n                        ID,\r\n                        PID,\r\n                        NAME\r\n                FROM\r\n                        SYS.SYSOBJECTS\r\n                WHERE\r\n                        TYPE$ = 'SCH'\r\n                    AND PID = UID()\r\n        )\r\n        SCH,\r\n        (\r\n                SELECT\r\n                        ID,\r\n                        SCHID,\r\n                        NAME\r\n                FROM\r\n                        SYS.SYSOBJECTS\r\n                WHERE\r\n                        TYPE$     = 'SCHOBJ'\r\n                    AND SUBTYPE$ IN('UTAB')\r\n                    AND NAME<> '##HISTOGRAMS_TABLE'\r\n                    AND NAME<> '##PLAN_TABLE'\r\n                    AND INFO3 & 0XFF <> 64\r\n                    AND NAME<> '__EFMigrationsHistory'\r\n        )\r\n        TAB\r\nWHERE\r\n        SCH.ID = TAB.SCHID\r\n    AND TAB.ID = COL.ID " + tableFilter + "\r\nORDER BY\r\n        COL_NAME ASC";
				using DbDataReader source = ((DbCommand)(object)val).ExecuteReader();
				IEnumerable<IGrouping<(string, string), DbDataRecord>> enumerable = from DbDataRecord ddr in source
					group ddr by (ddr.GetValueOrDefault<string>("SCH_NAME"), ddr.GetValueOrDefault<string>("TAB_NAME"));
				foreach (IGrouping<(string, string), DbDataRecord> item2 in enumerable)
				{
					string tableSchema = item2.Key.Item1;
					string tableName = item2.Key.Item2;
					DatabaseTable val2 = databaseModel.Tables.Single((DatabaseTable t) => t.Schema == tableSchema && t.Name == tableName);
					foreach (DbDataRecord item3 in item2)
					{
						string valueOrDefault = item3.GetValueOrDefault<string>("COL_NAME");
						int valueOrDefault2 = item3.GetValueOrDefault<int>("COL_ORDINAL");
						string valueOrDefault3 = item3.GetValueOrDefault<string>("TYPE_NAME");
						int valueOrDefault4 = item3.GetValueOrDefault<int>("COL_LENGTH");
						int precision = valueOrDefault4;
						short valueOrDefault5 = item3.GetValueOrDefault<short>("COL_SCALE");
						bool isNullable = item3.GetValueOrDefault<int>("IS_NULLABLE") > 0;
						bool flag = item3.GetValueOrDefault<short>("IS_IDENTITY") > 0;
						string text = ((!flag) ? item3.GetValueOrDefault<string>("COL_DEF") : null);
						string text2 = text;
						string dmClrType = GetDmClrType(valueOrDefault3, valueOrDefault4, precision, valueOrDefault5);
						if (string.IsNullOrWhiteSpace(text) || !string.IsNullOrWhiteSpace(text2))
						{
							text = null;
						}
						DatabaseColumn val3 = new DatabaseColumn();
						val3.Table=(val2);
						val3.Name=(valueOrDefault);
						val3.StoreType=(dmClrType);
						val3.IsNullable=(isNullable);
						val3.DefaultValueSql=(text);
						val3.ComputedColumnSql=(text2);
						val3.ValueGenerated=(flag ? new ValueGenerated?((ValueGenerated)1) : null);
						DatabaseColumn item = val3;
						val2.Columns.Add(item);
					}
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		private void GetKeys(DbConnection connection, string tableFilter, DatabaseModel databaseModel)
		{
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Expected O, but got Unknown
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Expected O, but got Unknown
			using DbCommand dbCommand = connection.CreateCommand();
			dbCommand.CommandText = "SELECT\r\n        /*+ MAX_OPT_N_TABLES(5) */\r\n        SCH.NAME                                                AS SCH_NAME,\r\n        TAB.NAME                                                    AS TAB_NAME ,\r\n        COL.NAME                                                   AS COL_NAME,\r\n        SF_GET_INDEX_KEY_SEQ(IND.KEYNUM, IND.KEYINFO, COL.COLID) AS PK_ORDINAL    ,\r\n        CON.NAME                                                   AS CONSTRAINT_NAME,\r\n        IND.XTYPE & 0X01\t\t\t\t\t\t\t\t\t\t    AS IS_CLUSTER,\r\n        CON.TYPE$                                                  AS CONSTRAINT_TYPE\r\nFROM\r\n        SYS.SYSINDEXES IND,\r\n        (\r\n                SELECT\r\n                        OBJ.NAME   ,\r\n                        CON.ID     ,\r\n                        CON.TYPE$  ,\r\n                        CON.TABLEID,\r\n                        CON.COLID  ,\r\n                        CON.INDEXID\r\n                FROM\r\n                        SYS.SYSCONS    AS CON,\r\n                        SYS.SYSOBJECTS AS OBJ\r\n                WHERE\r\n                        CON.TYPE$   IN ('P', 'U')\r\n                    AND OBJ.SUBTYPE$='CONS'\r\n                    AND OBJ.ID      =CON.ID\r\n        )\r\n        CON               ,\r\n        SYS.SYSCOLUMNS COL,\r\n        (\r\n                SELECT\r\n                        ID ,\r\n                        PID,\r\n                        NAME\r\n                FROM\r\n                        SYS.SYSOBJECTS\r\n                WHERE\r\n                        TYPE$ = 'SCH' AND PID =UID()\r\n        )\r\n        SCH,\r\n        (\r\n                SELECT\r\n                        ID   ,\r\n                        SCHID,\r\n                        NAME\r\n                FROM\r\n                        SYS.SYSOBJECTS\r\n                WHERE\r\n                        TYPE$    = 'SCHOBJ'\r\n                    AND SUBTYPE$ IN ('UTAB')\r\n                    AND NAME <> '##HISTOGRAMS_TABLE' \r\n                    AND NAME <> '##PLAN_TABLE'\r\n                    AND INFO3 & 0XFF <> 64\r\n                    AND NAME <> '__EFMigrationsHistory'\r\n        )\r\n        TAB,\r\n        (\r\n                SELECT ID, NAME FROM SYS.SYSOBJECTS WHERE SUBTYPE$='INDEX'\r\n        )\r\n        OBJ_IND\r\nWHERE\r\n        SCH.ID                                              = TAB.SCHID\r\n    AND CON.INDEXID                                            =IND.ID\r\n    AND IND.ID                                                 =OBJ_IND.ID\r\n    AND TAB.ID                                                  =COL.ID\r\n    AND CON.TABLEID                                            =TAB.ID\r\n    AND SF_COL_IS_IDX_KEY(IND.KEYNUM, IND.KEYINFO, COL.COLID)=1 " + tableFilter + " ORDER BY SCH_NAME ASC, TAB_NAME ASC, CONSTRAINT_NAME ASC";
			using DbDataReader source = dbCommand.ExecuteReader();
			IEnumerable<IGrouping<(string, string), DbDataRecord>> enumerable = from DbDataRecord ddr in source
				group ddr by (ddr.GetValueOrDefault<string>("SCH_NAME"), ddr.GetValueOrDefault<string>("TAB_NAME"));
			foreach (IGrouping<(string, string), DbDataRecord> item3 in enumerable)
			{
				string tableSchema = item3.Key.Item1;
				string tableName = item3.Key.Item2;
				DatabaseTable val = databaseModel.Tables.Single((DatabaseTable t) => t.Schema == tableSchema && t.Name == tableName);
				IGrouping<string, DbDataRecord>[] array = (from ddr in item3
					where ddr.GetValueOrDefault<string>("CONSTRAINT_TYPE").Equals("P")
					group ddr by ddr.GetValueOrDefault<string>("CONSTRAINT_NAME")).ToArray();
				if (array.Length == 1)
				{
					IGrouping<string, DbDataRecord> grouping = array[0];
					_logger.PrimaryKeyFound(grouping.Key, DisplayName(tableSchema, tableName));
					DatabasePrimaryKey val2 = new DatabasePrimaryKey();
					val2.Table=(val);
					val2.Name=(grouping.Key);
					DatabasePrimaryKey val3 = val2;
					foreach (DbDataRecord item4 in grouping)
					{
						string columnName2 = item4.GetValueOrDefault<string>("COL_NAME");
						DatabaseColumn item = ((IEnumerable<DatabaseColumn>)val.Columns).FirstOrDefault((Func<DatabaseColumn, bool>)((DatabaseColumn c) => c.Name == columnName2)) ?? ((IEnumerable<DatabaseColumn>)val.Columns).FirstOrDefault((Func<DatabaseColumn, bool>)((DatabaseColumn c) => c.Name.Equals(columnName2, StringComparison.OrdinalIgnoreCase)));
						val3.Columns.Add(item);
					}
					val.PrimaryKey=(val3);
				}
				IGrouping<string, DbDataRecord>[] array2 = (from ddr in item3
					where ddr.GetValueOrDefault<string>("CONSTRAINT_TYPE").Equals("U")
					group ddr by ddr.GetValueOrDefault<string>("CONSTRAINT_NAME")).ToArray();
				IGrouping<string, DbDataRecord>[] array3 = array2;
				foreach (IGrouping<string, DbDataRecord> grouping2 in array3)
				{
					_logger.UniqueConstraintFound(grouping2.Key, DisplayName(tableSchema, tableName));
					DatabaseUniqueConstraint val4 = new DatabaseUniqueConstraint();
					val4.Table=(val);
					val4.Name=(grouping2.Key);
					DatabaseUniqueConstraint val5 = val4;
					foreach (DbDataRecord item5 in grouping2)
					{
						string columnName = item5.GetValueOrDefault<string>("COL_NAME");
						DatabaseColumn item2 = ((IEnumerable<DatabaseColumn>)val.Columns).FirstOrDefault((Func<DatabaseColumn, bool>)((DatabaseColumn c) => c.Name == columnName)) ?? ((IEnumerable<DatabaseColumn>)val.Columns).FirstOrDefault((Func<DatabaseColumn, bool>)((DatabaseColumn c) => c.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase)));
						val5.Columns.Add(item2);
					}
					val.UniqueConstraints.Add(val5);
				}
			}
		}

		private void GetForeignKeys(DbConnection connection, string tableFilter, DatabaseModel databaseModel)
		{
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Expected O, but got Unknown
			using DbCommand dbCommand = connection.CreateCommand();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8575, 2);
			defaultInterpolatedStringHandler.AppendLiteral("SELECT\r\n        /*+ MAX_OPT_N_TABLES(5) */\r\n        T_REFED.SCHNAME          AS PKTABLE_SCHEM,\r\n        T_REFED.NAME             AS PKTABLE_NAME ,\r\n        T_REFED.REFED_COL_NAME   AS PKCOLUMN_NAME,\r\n        T_REF.SCHNAME            AS FKTABLE_SCHEM,\r\n        T_REF.NAME               AS FKTABLE_NAME ,\r\n        T_REF.REF_COL_NAME       AS FKCOLUMN_NAME,\r\n        T_REF.REF_KEYNO          AS KEY_SEQ      ,\r\n        SF_GET_UPD_RULE(FACTION) AS UPDATE_RULE  ,\r\n        SF_GET_DEL_RULE(FACTION) AS DELETE_RULE  ,\r\n        T_REF.REF_CON_NAME      AS FK_NAME\r\nFROM\r\n        (\r\n                SELECT\r\n                        T_REF_TAB.NAME                                                               AS NAME         ,\r\n                        T_REF_TAB.SCHNAME                                                            AS SCHNAME      ,\r\n                        T_REF_CON.FINDEXID                                                          AS REFED_IND_ID ,\r\n                        T_REF_CON.NAME                                                              AS REF_CON_NAME,\r\n                        SF_GET_INDEX_KEY_SEQ(T_REF_IND.KEYNUM, T_REF_IND.KEYINFO, T_REF_COL.COLID) AS REF_KEYNO    ,\r\n                        T_REF_COL.NAME                                                               AS REF_COL_NAME ,\r\n                        T_REF_CON.FACTION                                                           AS FACTION\r\n                FROM\r\n                        (\r\n                                SELECT\r\n                                        OBJ.NAME    ,\r\n                                        CON.TABLEID ,\r\n                                        CON.INDEXID ,\r\n                                        CON.FINDEXID,\r\n                                        CON.FACTION\r\n                                FROM\r\n                                        (\r\n                                                SELECT NAME, ID FROM SYS.SYSOBJECTS WHERE SUBTYPE$='CONS'\r\n                                        )\r\n                                        OBJ,\r\n                                        SYS.SYSCONS CON\r\n                                WHERE\r\n                                        CON.ID    = OBJ.ID\r\n                                    AND CON.TYPE$ = 'F'\r\n                        )AS T_REF_CON,\r\n                        (\r\n                                SELECT\r\n                                        TAB.NAME    AS NAME,\r\n                                        TAB.ID      AS ID  ,\r\n                                        SCH.NAME AS SCHNAME\r\n                                FROM\r\n                                        (\r\n                                                SELECT ID, NAME FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND PID = UID()\r\n                                        )\r\n                                        SCH,\r\n                                        (\r\n                                                SELECT\r\n                                                        ID   ,\r\n                                                        SCHID,\r\n                                                        NAME\r\n                                                FROM\r\n                                                        SYS.SYSOBJECTS\r\n                                                WHERE\r\n                                                        TYPE$    = 'SCHOBJ'\r\n                                                    AND SUBTYPE$ = 'UTAB'\r\n                                                    AND INFO3 & 0XFF <> 64\r\n                                        )\r\n                                        TAB\r\n                                WHERE\r\n                                        SCH.ID = TAB.SCHID ");
			defaultInterpolatedStringHandler.AppendFormatted(tableFilter);
			defaultInterpolatedStringHandler.AppendLiteral("\r\n                        )              AS T_REF_TAB ,\r\n                        SYS.SYSINDEXES AS T_REF_IND,\r\n                        (\r\n                                SELECT ID  , PID, NAME FROM SYS.SYSOBJECTS WHERE SUBTYPE$='INDEX'\r\n                        )              AS T_REF_IND_OBJ,\r\n                        SYS.SYSCOLUMNS AS T_REF_COL\r\n                WHERE\r\n                        T_REF_TAB.ID                                                             = T_REF_CON.TABLEID\r\n                    AND T_REF_TAB.ID                                                             = T_REF_IND_OBJ.PID\r\n                    AND T_REF_TAB.ID                                                             = T_REF_COL.ID\r\n                    AND T_REF_CON.INDEXID                                                       = T_REF_IND_OBJ.ID\r\n                    AND T_REF_IND.ID                                                            = T_REF_IND_OBJ.ID\r\n                    AND SF_COL_IS_IDX_KEY(T_REF_IND.KEYNUM, T_REF_IND.KEYINFO, T_REF_COL.COLID)=1\r\n        ) AS T_REF,\r\n        (\r\n                SELECT\r\n                        T_REFED_TAB.NAME                                                                   AS NAME           ,\r\n                        T_REFED_TAB.SCHNAME                                                                     AS SCHNAME        ,\r\n                        T_REFED_IND.ID                                                                    AS REFED_IND_ID   ,\r\n                        T_REFED_IND_OBJ.NAME                                                              AS REFED_CON_NAME,\r\n                        SF_GET_INDEX_KEY_SEQ(T_REFED_IND.KEYNUM, T_REFED_IND.KEYINFO, T_REFED_COL.COLID) AS REFED_KEYNO    ,\r\n                        T_REFED_COL.NAME                                                                   AS REFED_COL_NAME\r\n                FROM\r\n                        SYS.SYSCONS AS T_REFED_CON,\r\n                        (\r\n                                SELECT\r\n                                        TAB.NAME    AS NAME,\r\n                                        TAB.ID      AS ID  ,\r\n                                        SCH.NAME AS SCHNAME\r\n                                FROM\r\n                                        (\r\n                                                SELECT ID, NAME FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND PID = UID()\r\n                                        )\r\n                                        SCH,\r\n                                        (\r\n                                                SELECT\r\n                                                        ID   ,\r\n                                                        SCHID,\r\n                                                        NAME\r\n                                                FROM\r\n                                                        SYS.SYSOBJECTS\r\n                                                WHERE\r\n                                                        TYPE$    = 'SCHOBJ'\r\n                                                    AND SUBTYPE$ = 'UTAB'\r\n                                                    AND INFO3 & 0XFF <> 64 \r\n                                        )\r\n                                        TAB\r\n                                WHERE\r\n                                        SCH.ID = TAB.SCHID ");
			defaultInterpolatedStringHandler.AppendFormatted(tableFilter);
			defaultInterpolatedStringHandler.AppendLiteral("\r\n                        )              AS T_REFED_TAB ,\r\n                        SYS.SYSINDEXES AS T_REFED_IND,\r\n                        (\r\n                                SELECT ID, PID, NAME FROM SYS.SYSOBJECTS WHERE SUBTYPE$='INDEX'\r\n                        )              AS T_REFED_IND_OBJ,\r\n                        SYS.SYSCOLUMNS AS T_REFED_COL\r\n                WHERE\r\n                        T_REFED_TAB.ID                                                                 = T_REFED_CON.TABLEID\r\n                    AND T_REFED_CON.TYPE$                                                             IN('P','U')\r\n                    AND T_REFED_TAB.ID                                                                 = T_REFED_IND_OBJ.PID\r\n                    AND T_REFED_TAB.ID                                                                 = T_REFED_COL.ID\r\n                    AND T_REFED_CON.INDEXID                                                           = T_REFED_IND_OBJ.ID\r\n                    AND T_REFED_IND.ID                                                                = T_REFED_IND_OBJ.ID\r\n                    AND SF_COL_IS_IDX_KEY(T_REFED_IND.KEYNUM, T_REFED_IND.KEYINFO, T_REFED_COL.COLID)=1\r\n        ) AS T_REFED\r\nWHERE\r\n        T_REF.REFED_IND_ID = T_REFED.REFED_IND_ID\r\n    AND T_REF.REF_KEYNO    = T_REFED.REFED_KEYNO \r\nORDER BY\r\n        FKTABLE_SCHEM ASC,\r\n        FKTABLE_NAME ASC ,\r\n        FK_NAME ASC,\r\n        KEY_SEQ ASC");
			dbCommand.CommandText = defaultInterpolatedStringHandler.ToStringAndClear();
			using DbDataReader source = dbCommand.ExecuteReader();
			IEnumerable<IGrouping<(string, string), DbDataRecord>> enumerable = from DbDataRecord ddr in source
				group ddr by (ddr.GetValueOrDefault<string>("FKTABLE_SCHEM"), ddr.GetValueOrDefault<string>("FKTABLE_NAME"));
			foreach (IGrouping<(string, string), DbDataRecord> item4 in enumerable)
			{
				string tableSchema = item4.Key.Item1;
				string tableName = item4.Key.Item2;
				DatabaseTable val = databaseModel.Tables.Single((DatabaseTable t) => t.Schema == tableSchema && t.Name == tableName);
				IEnumerable<IGrouping<(string, string, string, string), DbDataRecord>> enumerable2 = from c in item4
					group c by (c.GetValueOrDefault<string>("FK_NAME"), c.GetValueOrDefault<string>("PKTABLE_SCHEM"), c.GetValueOrDefault<string>("PKTABLE_NAME"), ConvertToStringReferentialAction(c.GetValueOrDefault<int>("DELETE_RULE")));
				foreach (IGrouping<(string, string, string, string), DbDataRecord> item5 in enumerable2)
				{
					string item = item5.Key.Item1;
					string principalTableSchema = item5.Key.Item2;
					string principalTableName = item5.Key.Item3;
					string item2 = item5.Key.Item4;
					_logger.ForeignKeyFound(item, DisplayName(val.Schema, val.Name), DisplayName(principalTableSchema, principalTableName), item2);
					DatabaseTable val2 = ((IEnumerable<DatabaseTable>)databaseModel.Tables).FirstOrDefault((Func<DatabaseTable, bool>)((DatabaseTable t) => t.Schema == principalTableSchema && t.Name == principalTableName)) ?? ((IEnumerable<DatabaseTable>)databaseModel.Tables).FirstOrDefault((Func<DatabaseTable, bool>)((DatabaseTable t) => t.Schema.Equals(principalTableSchema, StringComparison.OrdinalIgnoreCase) && t.Name.Equals(principalTableName, StringComparison.OrdinalIgnoreCase)));
					if (val2 == null)
					{
						_logger.ForeignKeyReferencesMissingPrincipalTableWarning(item, DisplayName(val.Schema, val.Name), DisplayName(principalTableSchema, principalTableName));
						continue;
					}
					DatabaseForeignKey val3 = new DatabaseForeignKey();
					val3.Name=(item);
					val3.Table=(val);
					val3.PrincipalTable=(val2);
					val3.OnDelete=(ConvertToReferentialAction(item2));
					DatabaseForeignKey val4 = val3;
					bool flag = false;
					foreach (DbDataRecord item6 in item5)
					{
						string columnName = item6.GetValueOrDefault<string>("FKCOLUMN_NAME");
						DatabaseColumn item3 = ((IEnumerable<DatabaseColumn>)val.Columns).FirstOrDefault((Func<DatabaseColumn, bool>)((DatabaseColumn c) => c.Name == columnName)) ?? ((IEnumerable<DatabaseColumn>)val.Columns).FirstOrDefault((Func<DatabaseColumn, bool>)((DatabaseColumn c) => c.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase)));
						string principalColumnName = item6.GetValueOrDefault<string>("PKCOLUMN_NAME");
						DatabaseColumn val5 = ((IEnumerable<DatabaseColumn>)val4.PrincipalTable.Columns).FirstOrDefault((Func<DatabaseColumn, bool>)((DatabaseColumn c) => c.Name == principalColumnName)) ?? ((IEnumerable<DatabaseColumn>)val4.PrincipalTable.Columns).FirstOrDefault((Func<DatabaseColumn, bool>)((DatabaseColumn c) => c.Name.Equals(principalColumnName, StringComparison.OrdinalIgnoreCase)));
						if (val5 == null)
						{
							flag = true;
							_logger.ForeignKeyPrincipalColumnMissingWarning(item, DisplayName(val.Schema, val.Name), principalColumnName, DisplayName(principalTableSchema, principalTableName));
							break;
						}
						val4.Columns.Add(item3);
						val4.PrincipalColumns.Add(val5);
					}
					if (!flag)
					{
						val.ForeignKeys.Add(val4);
					}
				}
			}
		}

		private void GetIndexes(DbConnection connection, string tableFilter, DatabaseModel databaseModel)
		{
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Expected O, but got Unknown
			using DbCommand dbCommand = connection.CreateCommand();
			dbCommand.CommandText = "SELECT\r\n        /*+ MAX_OPT_N_TABLES(5) */\r\n        DISTINCT \r\n        SCH.NAME                                                    AS SCH_NAME     ,\r\n        TAB.NAME                                                      AS TAB_NAME      ,\r\n        CASE IND.ISUNIQUE WHEN 'Y' THEN 1 ELSE 0 END                 AS IS_UNIQUE      ,\r\n        OBJ_IND.NAME                                                 AS IND_NAME      ,\r\n        IND.XTYPE & 0x01                \t\t\t\t\t\t  \t  AS IS_CLUSTER          ,\r\n        SF_GET_INDEX_KEY_SEQ(IND.KEYNUM, IND.KEYINFO, COL.COLID)   AS IND_ORDINAL,\r\n        COL.NAME                                                     AS COL_NAME     \r\nFROM\r\n        (\r\n                SELECT\r\n                        ID ,\r\n                        PID,\r\n                        NAME\r\n                FROM\r\n                        SYS.SYSOBJECTS\r\n                WHERE\r\n                        TYPE$ = 'SCH'\r\n                        AND PID =UID()\r\n        )\r\n        SCH,\r\n        (\r\n                SELECT\r\n                        ID   ,\r\n                        SCHID,\r\n                        NAME\r\n                FROM\r\n                        SYS.SYSOBJECTS\r\n                WHERE\r\n                        TYPE$    = 'SCHOBJ'\r\n                    AND SUBTYPE$ IN ('UTAB')\r\n                    AND NAME <> '##HISTOGRAMS_TABLE' \r\n                    AND NAME <> '##PLAN_TABLE'\r\n                    AND INFO3 & 0XFF <> 64\r\n                    AND NAME <> '__EFMigrationsHistory'\r\n        )\r\n        TAB,\r\n        (\r\n                SELECT ID, PID, NAME FROM SYS.SYSOBJECTS WHERE SUBTYPE$='INDEX'\r\n        )              AS OBJ_IND    ,\r\n        SYS.SYSINDEXES AS IND        ,\r\n        SYS.SYSCOLUMNS AS COL\r\nWHERE\r\n        TAB.ID                                                  =COL.ID\r\n    AND TAB.ID                                                  =OBJ_IND.PID\r\n    AND IND.ID                                                 =OBJ_IND.ID\r\n    AND TAB.SCHID                                               = SCH.ID\r\n    AND IND.FLAG & 0X01 = 0\r\n    AND SF_COL_IS_IDX_KEY(IND.KEYNUM, IND.KEYINFO, COL.COLID)=1 " + tableFilter + "\r\nORDER BY\r\n        SCH_NAME ASC,\r\n        TAB_NAME ASC      ,\r\n        IND_NAME ASC,\r\n        IND_ORDINAL ASC;";
			using DbDataReader source = dbCommand.ExecuteReader();
			IEnumerable<IGrouping<(string, string), DbDataRecord>> enumerable = from DbDataRecord ddr in source
				group ddr by (ddr.GetValueOrDefault<string>("SCH_NAME"), ddr.GetValueOrDefault<string>("TAB_NAME"));
			foreach (IGrouping<(string, string), DbDataRecord> item2 in enumerable)
			{
				string tableSchema = item2.Key.Item1;
				string tableName = item2.Key.Item2;
				DatabaseTable val = databaseModel.Tables.Single((DatabaseTable t) => t.Schema == tableSchema && t.Name == tableName);
				IGrouping<(string, bool), DbDataRecord>[] array = (from ddr in item2
					group ddr by (ddr.GetValueOrDefault<string>("IND_NAME"), ddr.GetValueOrDefault<int>("IS_UNIQUE") > 0)).ToArray();
				IGrouping<(string, bool), DbDataRecord>[] array2 = array;
				foreach (IGrouping<(string, bool), DbDataRecord> grouping in array2)
				{
					_logger.IndexFound(grouping.Key.Item1, DisplayName(tableSchema, tableName), grouping.Key.Item2);
					DatabaseIndex val2 = new DatabaseIndex();
					val2.Table=(val);
					val2.Name=(grouping.Key.Item1);
					val2.IsUnique=(grouping.Key.Item2);
					DatabaseIndex val3 = val2;
					foreach (DbDataRecord item3 in grouping)
					{
						string columnName = item3.GetValueOrDefault<string>("COL_NAME");
						DatabaseColumn item = ((IEnumerable<DatabaseColumn>)val.Columns).FirstOrDefault((Func<DatabaseColumn, bool>)((DatabaseColumn c) => c.Name == columnName)) ?? ((IEnumerable<DatabaseColumn>)val.Columns).FirstOrDefault((Func<DatabaseColumn, bool>)((DatabaseColumn c) => c.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase)));
						val3.Columns.Add(item);
					}
					val.Indexes.Add(val3);
				}
			}
		}

		private static string DisplayName(string schema, string name)
		{
			return ((!string.IsNullOrEmpty(schema)) ? (schema + ".") : "") + name;
		}

		private string GetDefaultSchema(DbConnection connection)
		{
			using DbCommand dbCommand = connection.CreateCommand();
			dbCommand.CommandText = "SELECT SF_GET_SCHEMA_NAME_BY_ID(CURRENT_SCHID())";
			string text = dbCommand.ExecuteScalar() as string;
			if (text != null)
			{
				_logger.DefaultSchemaFound(text);
				return text;
			}
			return null;
		}

		private static string GetDmClrType(string dataTypeName, int maxLength, int precision, int scale)
		{
			if (_decTypes.Contains(dataTypeName))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
				defaultInterpolatedStringHandler.AppendFormatted(dataTypeName);
				defaultInterpolatedStringHandler.AppendLiteral("(");
				defaultInterpolatedStringHandler.AppendFormatted(precision);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted(scale);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (_dateTimePrecisionTypes.Contains(dataTypeName))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted(dataTypeName);
				defaultInterpolatedStringHandler.AppendLiteral("(");
				defaultInterpolatedStringHandler.AppendFormatted(scale);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (!_integerTypes.Contains(dataTypeName))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted(dataTypeName);
				defaultInterpolatedStringHandler.AppendLiteral("(");
				defaultInterpolatedStringHandler.AppendFormatted(maxLength);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			return dataTypeName;
		}

		private static ReferentialAction? ConvertToReferentialAction(string onDeleteAction)
		{
			return onDeleteAction switch
			{
				"NO ACTION" => (ReferentialAction)0, 
				"CASCADE" => (ReferentialAction)2, 
				"SET NULL" => (ReferentialAction)3, 
				"SET DEFAULT" => (ReferentialAction)4, 
				_ => null, 
			};
		}

		private static string ConvertToStringReferentialAction(int onDeleteAction)
		{
			return onDeleteAction switch
			{
				3 => "NO ACTION", 
				0 => "CASCADE", 
				2 => "SET NULL", 
				4 => "SET DEFAULT", 
				_ => null, 
			};
		}

		private static Func<string, string> GenerateSchemaFilter(IReadOnlyList<string> schemas)
		{
			IReadOnlyList<string> schemas2 = schemas;
			if (schemas2.Any())
			{
				return delegate(string s)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(s);
					stringBuilder.Append(" IN (");
					stringBuilder.Append(string.Join(", ", schemas2.Select(new Func<string, string>(EscapeLiteral))));
					stringBuilder.Append(")");
					return stringBuilder.ToString();
				};
			}
			return null;
		}

		private static (string Schema, string Table) Parse(string table)
		{
			Match match = _partExtractor.Match(table.Trim());
			if (!match.Success)
			{
				throw new InvalidOperationException(DmStrings.InvalidTableToIncludeInScaffolding(table));
			}
			string text = match.Groups["part1"].Value.Replace("]]", "]");
			string text2 = match.Groups["part2"].Value.Replace("]]", "]");
			return string.IsNullOrEmpty(text2) ? (null, text) : (text, text2);
		}

		private static Func<string, string, string> GenerateTableFilter(IReadOnlyList<(string Schema, string Table)> tables, Func<string, string> schemaFilter)
		{
			Func<string, string> schemaFilter2 = schemaFilter;
			IReadOnlyList<(string Schema, string Table)> tables2 = tables;
			if (schemaFilter2 != null || tables2.Any())
			{
				return delegate(string s, string t)
				{
					StringBuilder stringBuilder = new StringBuilder();
					bool flag = false;
					if (schemaFilter2 != null)
					{
						stringBuilder.Append("(").Append(schemaFilter2(s));
						flag = true;
					}
					if (tables2.Any())
					{
						if (flag)
						{
							stringBuilder.AppendLine().Append("OR ");
						}
						else
						{
							stringBuilder.Append("(");
							flag = true;
						}
						if (tables2.Any())
						{
							stringBuilder.Append(t);
							stringBuilder.Append(" IN (");
							stringBuilder.Append(string.Join(", ", tables2.Select<(string, string), string>(((string Schema, string Table) e) => EscapeLiteral(e.Table))));
							stringBuilder.Append(")");
						}
					}
					if (flag)
					{
						stringBuilder.Append(")");
					}
					return stringBuilder.ToString();
				};
			}
			return null;
		}

		private static string EscapeLiteral(string s)
		{
			return "N'" + s + "'";
		}

		public DatabaseModel Create(string connectionString, DatabaseModelFactoryOptions options)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			Check.NotEmpty(connectionString, "connectionString");
			Check.NotNull<DatabaseModelFactoryOptions>(options, "options");
			DmConnection val = new DmConnection(connectionString);
			try
			{
				return Create((DbConnection)(object)val, options);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		public DatabaseModel Create(DbConnection connection, DatabaseModelFactoryOptions options)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			Check.NotNull(connection, "connection");
			Check.NotNull<DatabaseModelFactoryOptions>(options, "options");
			DatabaseModel val = new DatabaseModel();
			bool flag = connection.State == ConnectionState.Open;
			if (!flag)
			{
				connection.Open();
			}
			try
			{
				val.DefaultSchema=(GetDefaultSchema(connection));
				List<string> list = options.Schemas.ToList();
				Func<string, string> schemaFilter = GenerateSchemaFilter(list);
				List<string> list2 = options.Tables.ToList();
				Func<string, string, string> tableFilter = GenerateTableFilter(list2.Select(new Func<string, (string, string)>(Parse)).ToList(), schemaFilter);
				foreach (DatabaseSequence sequence in GetSequences(connection, schemaFilter))
				{
					sequence.Database=(val);
					val.Sequences.Add(sequence);
				}
				GetTables(connection, tableFilter, val);
				foreach (string item in list.Except((from s in val.Sequences
					select s.Schema).Concat(from t in val.Tables
					select t.Schema)))
				{
					_logger.MissingSchemaWarning(item);
				}
				foreach (string item2 in list2)
				{
					var (Schema, Table) = Parse(item2);
					if (!val.Tables.Any((DatabaseTable t) => (!string.IsNullOrEmpty(Schema) && t.Schema == Schema) || t.Name == Table))
					{
						_logger.MissingTableWarning(item2);
					}
				}
				return val;
			}
			finally
			{
				if (!flag)
				{
					connection.Close();
				}
			}
		}
	}
}
