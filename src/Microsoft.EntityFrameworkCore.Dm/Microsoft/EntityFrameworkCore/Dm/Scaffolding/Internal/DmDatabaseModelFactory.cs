// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Scaffolding.Internal.DmDatabaseModelFactory
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Dm;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;



namespace Microsoft.EntityFrameworkCore.Dm.Scaffolding.Internal
{
  public class DmDatabaseModelFactory : IDatabaseModelFactory
  {
    private readonly IDiagnosticsLogger<DbLoggerCategory.Scaffolding> _logger;
    private const string NamePartRegex = "(?:(?:\\[(?<part{0}>(?:(?:\\]\\])|[^\\]])+)\\])|(?<part{0}>[^\\.\\[\\]]+))";
    private static readonly Regex _partExtractor = new Regex(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "^{0}(?:\\.{1})?$", (object) string.Format((IFormatProvider) CultureInfo.InvariantCulture, "(?:(?:\\[(?<part{0}>(?:(?:\\]\\])|[^\\]])+)\\])|(?<part{0}>[^\\.\\[\\]]+))", (object) 1), (object) string.Format((IFormatProvider) CultureInfo.InvariantCulture, "(?:(?:\\[(?<part{0}>(?:(?:\\]\\])|[^\\]])+)\\])|(?<part{0}>[^\\.\\[\\]]+))", (object) 2)), RegexOptions.Compiled, TimeSpan.FromMilliseconds(1000.0));
    private static readonly ISet<string> _dateTimePrecisionTypes = (ISet<string>) new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      "timestamp",
      "datetime",
      "time"
    };
    private static readonly ISet<string> _integerTypes = (ISet<string>) new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      "byte",
      "tinyint",
      "smallint",
      "int",
      "bigint",
      "bit"
    };
    private static readonly ISet<string> _decTypes = (ISet<string>) new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      "dec",
      "numeric",
      "decimal"
    };
    private static readonly Dictionary<string, long[]> _defaultSequenceMinMax = new Dictionary<string, long[]>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      {
        "tinyint",
        new long[2]{ 0L, (long) byte.MaxValue }
      },
      {
        "smallint",
        new long[2]{ (long) short.MinValue, (long) short.MaxValue }
      },
      {
        "int",
        new long[2]{ (long) int.MinValue, (long) int.MaxValue }
      },
      {
        "bigint",
        new long[2]{ long.MinValue, long.MaxValue }
      }
    };

    public DmDatabaseModelFactory(
      IDiagnosticsLogger<DbLoggerCategory.Scaffolding> logger)
    {
      Check.NotNull<IDiagnosticsLogger<DbLoggerCategory.Scaffolding>>(logger, nameof (logger));
      this._logger = logger;
    }

    private IEnumerable<DatabaseSequence> GetSequences(
      DbConnection connection,
      Func<string, string> schemaFilter)
    {
      DbConnection dbConnection= connection;
      using (DbCommand command = dbConnection.CreateCommand())
      {
        command.CommandText = "\nSELECT\r\n        SCHEMAS.NAME                                             AS SCH_NAME       ,\r\n        SEQS.NAME                                                AS SEQ_NAME       ,\r\n        SEQS.INFO3                                               AS SEQ_START_VALUE,\r\n        DBA_SEQUENCES.MIN_VALUE                                  AS SEQ_MIN_VALUE  ,\r\n        DBA_SEQUENCES.MAX_VALUE                                  AS SEQ_MAX_VALUE  ,\r\n        DBA_SEQUENCES.INCREMENT_BY                               AS SEQ_INCREMENT  ,\r\n        CASE DBA_SEQUENCES.CYCLE_FLAG WHEN 'Y' THEN 1 ELSE 0 END AS SEQ_CYCLE\r\nFROM\r\n        (\r\n                SELECT NAME, ID FROM SYSOBJECTS WHERE PID = UID() AND TYPE$='SCH'\r\n        )\r\n        SCHEMAS,\r\n        (\r\n                SELECT * FROM SYSOBJECTS WHERE SUBTYPE$ ='SEQ'\r\n        )\r\n        SEQS,\r\n        DBA_SEQUENCES\r\nWHERE\r\n        SEQS.SCHID   = SCHEMAS.ID \r\n    AND SEQS.NAME    = DBA_SEQUENCES.SEQUENCE_NAME\r\n    AND SCHEMAS.NAME = DBA_SEQUENCES.SEQUENCE_OWNER;";
        Func<string, string> func= schemaFilter;
        if (func != null)
        {
          DbCommand dbCommand = command;
          dbCommand.CommandText = dbCommand.CommandText + "\nAND " + func("SCHEMAS.NAME");
        }
        using (DbDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            DatabaseSequence sequence = new DatabaseSequence()
            {
              Schema = reader.GetValueOrDefault<string>("SCH_NAME"),
              Name = reader.GetValueOrDefault<string>("SEQ_NAME"),
              StoreType = "BIGINT",
              IsCyclic = new bool?(reader.GetValueOrDefault<int>("SEQ_CYCLE") > 0),
              IncrementBy = new int?((int) reader.GetValueOrDefault<long>("SEQ_INCREMENT")),
              StartValue = new long?(reader.GetValueOrDefault<long>("SEQ_START_VALUE")),
              MinValue = new long?(reader.GetValueOrDefault<long>("SEQ_MIN_VALUE")),
              MaxValue = new long?(reader.GetValueOrDefault<long>("SEQ_MAX_VALUE"))
            };
            this._logger.SequenceFound(DmDatabaseModelFactory.DisplayName(sequence.Schema, sequence.Name), sequence.StoreType, sequence.IsCyclic, sequence.IncrementBy, sequence.StartValue, sequence.MinValue, sequence.MaxValue);
            if (DmDatabaseModelFactory._defaultSequenceMinMax.ContainsKey(sequence.StoreType))
            {
              long defaultMin = DmDatabaseModelFactory._defaultSequenceMinMax[sequence.StoreType][0];
              DatabaseSequence databaseSequence1 = sequence;
              long? nullable1 = sequence.MinValue;
              long num1 = defaultMin;
              long? nullable2;
              if (!(nullable1.GetValueOrDefault() == num1 & nullable1.HasValue))
              {
                nullable2 = sequence.MinValue;
              }
              else
              {
                nullable1 = new long?();
                nullable2 = nullable1;
              }
              databaseSequence1.MinValue = nullable2;
              DatabaseSequence databaseSequence2 = sequence;
              nullable1 = sequence.StartValue;
              long num2 = defaultMin;
              long? nullable3;
              if (!(nullable1.GetValueOrDefault() == num2 & nullable1.HasValue))
              {
                nullable3 = sequence.StartValue;
              }
              else
              {
                nullable1 = new long?();
                nullable3 = nullable1;
              }
              databaseSequence2.StartValue = nullable3;
              long defaultMax = DmDatabaseModelFactory._defaultSequenceMinMax[sequence.StoreType][1];
              DatabaseSequence databaseSequence3 = sequence;
              nullable1 = sequence.MaxValue;
              long num3 = defaultMax;
              long? nullable4;
              if (!(nullable1.GetValueOrDefault() == num3 & nullable1.HasValue))
              {
                nullable4 = sequence.MaxValue;
              }
              else
              {
                nullable1 = new long?();
                nullable4 = nullable1;
              }
              databaseSequence3.MaxValue = nullable4;
            }
            yield return sequence;
            sequence = (DatabaseSequence) null;
          }
        }
      }
    }

    private void GetTables(
      DbConnection connection,
      Func<string, string, string> tableFilter,
      DatabaseModel databaseModel)
    {
      using (DbCommand command = connection.CreateCommand())
      {
        string tableFilter1 = "";
        command.CommandText = "\nSELECT SCH.NAME AS SCH_NAME, TAB.NAME AS TAB_NAME FROM\r\n(SELECT NAME,ID FROM SYSOBJECTS WHERE TYPE$ = 'SCH' AND PID = UID())SCH,\r\nSYSOBJECTS TAB\r\nWHERE TAB.SCHID = SCH.ID AND TAB.TYPE$ = 'SCHOBJ' AND TAB.SUBTYPE$ IN ('UTAB') AND TAB.NAME <> '##HISTOGRAMS_TABLE' AND TAB.NAME <> '##PLAN_TABLE'AND  TAB.NAME <> '__EFMigrationsHistory' AND TAB.INFO3 & 0XFF <> 64";
        if (tableFilter != null)
          tableFilter1 = " AND " + tableFilter("SCH.NAME", "TAB.NAME");
        command.CommandText += tableFilter1;
        using (DbDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            DatabaseTable databaseTable = new DatabaseTable()
            {
              Schema = reader.GetValueOrDefault<string>("SCH_NAME"),
              Name = reader.GetValueOrDefault<string>("TAB_NAME")
            };
            this._logger.TableFound(DmDatabaseModelFactory.DisplayName(databaseTable.Schema, databaseTable.Name));
            ((ICollection<DatabaseTable>) databaseModel.Tables).Add(databaseTable);
          }
        }
        this.GetColumns(connection, tableFilter1, databaseModel);
        this.GetKeys(connection, tableFilter1, databaseModel);
        this.GetIndexes(connection, tableFilter1, databaseModel);
        this.GetForeignKeys(connection, tableFilter1, databaseModel);
      }
    }

    private void GetColumns(
      DbConnection connection,
      string tableFilter,
      DatabaseModel databaseModel)
    {
      using (DmCommand command = (DmCommand) connection.CreateCommand())
      {
        ((DbCommand) command).CommandText = "SELECT\r\n        /*+ MAX_OPT_N_TABLES(5) */\r\n        SCH.NAME AS SCH_NAME   ,\r\n        TAB.NAME AS TAB_NAME   ,\r\n        COL.NAME AS COL_NAME   ,\r\n        COL.TYPE$                                     AS TYPE_NAME,\r\n        COL.LENGTH$                                   AS COL_LENGTH,\r\n        COL.SCALE AS COL_SCALE  ,\r\n        COL.DEFVAL AS COL_DEF    ,\r\n        COL.COLID + 1                                 AS COL_ORDINAL,\r\n        CASE COL.NULLABLE$ WHEN 'Y' THEN 1 ELSE 0 END AS IS_NULLABLE,\r\n        COL.INFO2 & 0X01                              AS IS_IDENTITY,\r\n        (\r\n                SELECT\r\n                        SF_GET_INDEX_KEY_SEQ(IND.KEYNUM, IND.KEYINFO, COL.COLID)\r\n                FROM\r\n                        SYS.SYSINDEXES IND,\r\n                        (\r\n                                SELECT\r\n                                        OBJ.NAME,\r\n                                        CON.ID,\r\n                                        CON.TYPE$  ,\r\n                                        CON.TABLEID,\r\n                                        CON.COLID,\r\n                                        CON.INDEXID\r\n                                FROM\r\n                                        SYS.SYSCONS    AS CON,\r\n                                        SYS.SYSOBJECTS AS OBJ\r\n                                WHERE\r\n                                        CON.TYPE$   = 'P'\r\n                                    AND OBJ.SUBTYPE$= 'CONS'\r\n                                    AND OBJ.ID = CON.ID\r\n                        )\r\n                        CON,\r\n                        (\r\n                                SELECT ID, NAME FROM SYS.SYSOBJECTS WHERE SUBTYPE$= 'INDEX'\r\n                        )\r\n                        OBJ_IND\r\n                WHERE\r\n                        CON.INDEXID = IND.ID\r\n                    AND IND.ID = OBJ_IND.ID\r\n                    AND CON.TABLEID = TAB.ID\r\n                    AND SF_COL_IS_IDX_KEY(IND.KEYNUM, IND.KEYINFO, COL.COLID)= 1\r\n        ) AS PK_ORDINAL\r\nFROM\r\n        SYS.SYSCOLUMNS COL,\r\n        (\r\n                SELECT\r\n                        ID,\r\n                        PID,\r\n                        NAME\r\n                FROM\r\n                        SYS.SYSOBJECTS\r\n                WHERE\r\n                        TYPE$ = 'SCH'\r\n                    AND PID = UID()\r\n        )\r\n        SCH,\r\n        (\r\n                SELECT\r\n                        ID,\r\n                        SCHID,\r\n                        NAME\r\n                FROM\r\n                        SYS.SYSOBJECTS\r\n                WHERE\r\n                        TYPE$     = 'SCHOBJ'\r\n                    AND SUBTYPE$ IN('UTAB')\r\n                    AND NAME<> '##HISTOGRAMS_TABLE'\r\n                    AND NAME<> '##PLAN_TABLE'\r\n                    AND INFO3 & 0XFF <> 64\r\n                    AND NAME<> '__EFMigrationsHistory'\r\n        )\r\n        TAB\r\nWHERE\r\n        SCH.ID = TAB.SCHID\r\n    AND TAB.ID = COL.ID " + tableFilter + "\r\nORDER BY\r\n        COL_NAME ASC";
        using (DbDataReader source = ((DbCommand) command).ExecuteReader())
        {
          foreach (IGrouping<(string, string), DbDataRecord> grouping in source.Cast<DbDataRecord>().GroupBy<DbDataRecord, (string, string)>((Func<DbDataRecord, (string, string)>) (ddr => (ddr.GetValueOrDefault<string>("SCH_NAME"), ddr.GetValueOrDefault<string>("TAB_NAME")))))
          {
            string tableSchema = grouping.Key.Item1;
            string tableName = grouping.Key.Item2;
            DatabaseTable databaseTable = ((IEnumerable<DatabaseTable>) databaseModel.Tables).Single<DatabaseTable>((Func<DatabaseTable, bool>) (t => t.Schema == tableSchema && t.Name == tableName));
            foreach (DbDataRecord record in (IEnumerable<DbDataRecord>) grouping)
            {
              string valueOrDefault1 = record.GetValueOrDefault<string>("COL_NAME");
              record.GetValueOrDefault<int>("COL_ORDINAL");
              string valueOrDefault2 = record.GetValueOrDefault<string>("TYPE_NAME");
              int valueOrDefault3 = record.GetValueOrDefault<int>("COL_LENGTH");
              int precision = valueOrDefault3;
              short valueOrDefault4 = record.GetValueOrDefault<short>("COL_SCALE");
              bool flag1 = record.GetValueOrDefault<int>("IS_NULLABLE") > 0;
              bool flag2 = record.GetValueOrDefault<short>("IS_IDENTITY") > (short) 0;
              string str1 = !flag2 ? record.GetValueOrDefault<string>("COL_DEF") : (string) null;
              string str2 = str1;
              string dmClrType = DmDatabaseModelFactory.GetDmClrType(valueOrDefault2, valueOrDefault3, precision, (int) valueOrDefault4);
              if (string.IsNullOrWhiteSpace(str1) || !string.IsNullOrWhiteSpace(str2))
                str1 = (string) null;
              DatabaseColumn databaseColumn = new DatabaseColumn()
              {
                Table = databaseTable,
                Name = valueOrDefault1,
                StoreType = dmClrType,
                IsNullable = flag1,
                DefaultValueSql = str1,
                ComputedColumnSql = str2,
                ValueGenerated = flag2 ? new ValueGenerated?((ValueGenerated) 1) : new ValueGenerated?()
              };
              ((ICollection<DatabaseColumn>) databaseTable.Columns).Add(databaseColumn);
            }
          }
        }
      }
    }

    private void GetKeys(DbConnection connection, string tableFilter, DatabaseModel databaseModel)
    {
      using (DbCommand command = connection.CreateCommand())
      {
        command.CommandText = "SELECT\r\n        /*+ MAX_OPT_N_TABLES(5) */\r\n        SCH.NAME                                                AS SCH_NAME,\r\n        TAB.NAME                                                    AS TAB_NAME ,\r\n        COL.NAME                                                   AS COL_NAME,\r\n        SF_GET_INDEX_KEY_SEQ(IND.KEYNUM, IND.KEYINFO, COL.COLID) AS PK_ORDINAL    ,\r\n        CON.NAME                                                   AS CONSTRAINT_NAME,\r\n        IND.XTYPE & 0X01\t\t\t\t\t\t\t\t\t\t    AS IS_CLUSTER,\r\n        CON.TYPE$                                                  AS CONSTRAINT_TYPE\r\nFROM\r\n        SYS.SYSINDEXES IND,\r\n        (\r\n                SELECT\r\n                        OBJ.NAME   ,\r\n                        CON.ID     ,\r\n                        CON.TYPE$  ,\r\n                        CON.TABLEID,\r\n                        CON.COLID  ,\r\n                        CON.INDEXID\r\n                FROM\r\n                        SYS.SYSCONS    AS CON,\r\n                        SYS.SYSOBJECTS AS OBJ\r\n                WHERE\r\n                        CON.TYPE$   IN ('P', 'U')\r\n                    AND OBJ.SUBTYPE$='CONS'\r\n                    AND OBJ.ID      =CON.ID\r\n        )\r\n        CON               ,\r\n        SYS.SYSCOLUMNS COL,\r\n        (\r\n                SELECT\r\n                        ID ,\r\n                        PID,\r\n                        NAME\r\n                FROM\r\n                        SYS.SYSOBJECTS\r\n                WHERE\r\n                        TYPE$ = 'SCH' AND PID =UID()\r\n        )\r\n        SCH,\r\n        (\r\n                SELECT\r\n                        ID   ,\r\n                        SCHID,\r\n                        NAME\r\n                FROM\r\n                        SYS.SYSOBJECTS\r\n                WHERE\r\n                        TYPE$    = 'SCHOBJ'\r\n                    AND SUBTYPE$ IN ('UTAB')\r\n                    AND NAME <> '##HISTOGRAMS_TABLE' \r\n                    AND NAME <> '##PLAN_TABLE'\r\n                    AND INFO3 & 0XFF <> 64\r\n                    AND NAME <> '__EFMigrationsHistory'\r\n        )\r\n        TAB,\r\n        (\r\n                SELECT ID, NAME FROM SYS.SYSOBJECTS WHERE SUBTYPE$='INDEX'\r\n        )\r\n        OBJ_IND\r\nWHERE\r\n        SCH.ID                                              = TAB.SCHID\r\n    AND CON.INDEXID                                            =IND.ID\r\n    AND IND.ID                                                 =OBJ_IND.ID\r\n    AND TAB.ID                                                  =COL.ID\r\n    AND CON.TABLEID                                            =TAB.ID\r\n    AND SF_COL_IS_IDX_KEY(IND.KEYNUM, IND.KEYINFO, COL.COLID)=1 " + tableFilter + " ORDER BY SCH_NAME ASC, TAB_NAME ASC, CONSTRAINT_NAME ASC";
        using (DbDataReader source1 = command.ExecuteReader())
        {
          foreach (IGrouping<(string, string), DbDataRecord> source2 in source1.Cast<DbDataRecord>().GroupBy<DbDataRecord, (string, string)>((Func<DbDataRecord, (string, string)>) (ddr => (ddr.GetValueOrDefault<string>("SCH_NAME"), ddr.GetValueOrDefault<string>("TAB_NAME")))))
          {
            string tableSchema = source2.Key.Item1;
            string tableName = source2.Key.Item2;
            DatabaseTable databaseTable = ((IEnumerable<DatabaseTable>) databaseModel.Tables).Single<DatabaseTable>((Func<DatabaseTable, bool>) (t => t.Schema == tableSchema && t.Name == tableName));
            IGrouping<string, DbDataRecord>[] array = source2.Where<DbDataRecord>((Func<DbDataRecord, bool>) (ddr => ddr.GetValueOrDefault<string>("CONSTRAINT_TYPE").Equals("P"))).GroupBy<DbDataRecord, string>((Func<DbDataRecord, string>) (ddr => ddr.GetValueOrDefault<string>("CONSTRAINT_NAME"))).ToArray<IGrouping<string, DbDataRecord>>();
            if (array.Length == 1)
            {
              IGrouping<string, DbDataRecord> grouping = array[0];
              this._logger.PrimaryKeyFound(grouping.Key, DmDatabaseModelFactory.DisplayName(tableSchema, tableName));
              DatabasePrimaryKey databasePrimaryKey = new DatabasePrimaryKey()
              {
                Table = databaseTable,
                Name = grouping.Key
              };
              foreach (DbDataRecord record in (IEnumerable<DbDataRecord>) grouping)
              {
                string columnName = record.GetValueOrDefault<string>("COL_NAME");
                DatabaseColumn databaseColumn = ((IEnumerable<DatabaseColumn>) databaseTable.Columns).FirstOrDefault<DatabaseColumn>((Func<DatabaseColumn, bool>) (c => c.Name == columnName)) ?? ((IEnumerable<DatabaseColumn>) databaseTable.Columns).FirstOrDefault<DatabaseColumn>((Func<DatabaseColumn, bool>) (c => c.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase)));
                ((ICollection<DatabaseColumn>) databasePrimaryKey.Columns).Add(databaseColumn);
              }
              databaseTable.PrimaryKey = databasePrimaryKey;
            }
            foreach (IGrouping<string, DbDataRecord> grouping in source2.Where<DbDataRecord>((Func<DbDataRecord, bool>) (ddr => ddr.GetValueOrDefault<string>("CONSTRAINT_TYPE").Equals("U"))).GroupBy<DbDataRecord, string>((Func<DbDataRecord, string>) (ddr => ddr.GetValueOrDefault<string>("CONSTRAINT_NAME"))).ToArray<IGrouping<string, DbDataRecord>>())
            {
              this._logger.UniqueConstraintFound(grouping.Key, DmDatabaseModelFactory.DisplayName(tableSchema, tableName));
              DatabaseUniqueConstraint uniqueConstraint = new DatabaseUniqueConstraint()
              {
                Table = databaseTable,
                Name = grouping.Key
              };
              foreach (DbDataRecord record in (IEnumerable<DbDataRecord>) grouping)
              {
                string columnName = record.GetValueOrDefault<string>("COL_NAME");
                DatabaseColumn databaseColumn = ((IEnumerable<DatabaseColumn>) databaseTable.Columns).FirstOrDefault<DatabaseColumn>((Func<DatabaseColumn, bool>) (c => c.Name == columnName)) ?? ((IEnumerable<DatabaseColumn>) databaseTable.Columns).FirstOrDefault<DatabaseColumn>((Func<DatabaseColumn, bool>) (c => c.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase)));
                ((ICollection<DatabaseColumn>) uniqueConstraint.Columns).Add(databaseColumn);
              }
              ((ICollection<DatabaseUniqueConstraint>) databaseTable.UniqueConstraints).Add(uniqueConstraint);
            }
          }
        }
      }
    }

    private void GetForeignKeys(
      DbConnection connection,
      string tableFilter,
      DatabaseModel databaseModel)
    {
      using (DbCommand command = connection.CreateCommand())
      {
        DbCommand dbCommand = command;
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(8575, 2);
        interpolatedStringHandler.AppendLiteral("SELECT\r\n        /*+ MAX_OPT_N_TABLES(5) */\r\n        T_REFED.SCHNAME          AS PKTABLE_SCHEM,\r\n        T_REFED.NAME             AS PKTABLE_NAME ,\r\n        T_REFED.REFED_COL_NAME   AS PKCOLUMN_NAME,\r\n        T_REF.SCHNAME            AS FKTABLE_SCHEM,\r\n        T_REF.NAME               AS FKTABLE_NAME ,\r\n        T_REF.REF_COL_NAME       AS FKCOLUMN_NAME,\r\n        T_REF.REF_KEYNO          AS KEY_SEQ      ,\r\n        SF_GET_UPD_RULE(FACTION) AS UPDATE_RULE  ,\r\n        SF_GET_DEL_RULE(FACTION) AS DELETE_RULE  ,\r\n        T_REF.REF_CON_NAME      AS FK_NAME\r\nFROM\r\n        (\r\n                SELECT\r\n                        T_REF_TAB.NAME                                                               AS NAME         ,\r\n                        T_REF_TAB.SCHNAME                                                            AS SCHNAME      ,\r\n                        T_REF_CON.FINDEXID                                                          AS REFED_IND_ID ,\r\n                        T_REF_CON.NAME                                                              AS REF_CON_NAME,\r\n                        SF_GET_INDEX_KEY_SEQ(T_REF_IND.KEYNUM, T_REF_IND.KEYINFO, T_REF_COL.COLID) AS REF_KEYNO    ,\r\n                        T_REF_COL.NAME                                                               AS REF_COL_NAME ,\r\n                        T_REF_CON.FACTION                                                           AS FACTION\r\n                FROM\r\n                        (\r\n                                SELECT\r\n                                        OBJ.NAME    ,\r\n                                        CON.TABLEID ,\r\n                                        CON.INDEXID ,\r\n                                        CON.FINDEXID,\r\n                                        CON.FACTION\r\n                                FROM\r\n                                        (\r\n                                                SELECT NAME, ID FROM SYS.SYSOBJECTS WHERE SUBTYPE$='CONS'\r\n                                        )\r\n                                        OBJ,\r\n                                        SYS.SYSCONS CON\r\n                                WHERE\r\n                                        CON.ID    = OBJ.ID\r\n                                    AND CON.TYPE$ = 'F'\r\n                        )AS T_REF_CON,\r\n                        (\r\n                                SELECT\r\n                                        TAB.NAME    AS NAME,\r\n                                        TAB.ID      AS ID  ,\r\n                                        SCH.NAME AS SCHNAME\r\n                                FROM\r\n                                        (\r\n                                                SELECT ID, NAME FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND PID = UID()\r\n                                        )\r\n                                        SCH,\r\n                                        (\r\n                                                SELECT\r\n                                                        ID   ,\r\n                                                        SCHID,\r\n                                                        NAME\r\n                                                FROM\r\n                                                        SYS.SYSOBJECTS\r\n                                                WHERE\r\n                                                        TYPE$    = 'SCHOBJ'\r\n                                                    AND SUBTYPE$ = 'UTAB'\r\n                                                    AND INFO3 & 0XFF <> 64\r\n                                        )\r\n                                        TAB\r\n                                WHERE\r\n                                        SCH.ID = TAB.SCHID ");
        interpolatedStringHandler.AppendFormatted(tableFilter);
        interpolatedStringHandler.AppendLiteral("\r\n                        )              AS T_REF_TAB ,\r\n                        SYS.SYSINDEXES AS T_REF_IND,\r\n                        (\r\n                                SELECT ID  , PID, NAME FROM SYS.SYSOBJECTS WHERE SUBTYPE$='INDEX'\r\n                        )              AS T_REF_IND_OBJ,\r\n                        SYS.SYSCOLUMNS AS T_REF_COL\r\n                WHERE\r\n                        T_REF_TAB.ID                                                             = T_REF_CON.TABLEID\r\n                    AND T_REF_TAB.ID                                                             = T_REF_IND_OBJ.PID\r\n                    AND T_REF_TAB.ID                                                             = T_REF_COL.ID\r\n                    AND T_REF_CON.INDEXID                                                       = T_REF_IND_OBJ.ID\r\n                    AND T_REF_IND.ID                                                            = T_REF_IND_OBJ.ID\r\n                    AND SF_COL_IS_IDX_KEY(T_REF_IND.KEYNUM, T_REF_IND.KEYINFO, T_REF_COL.COLID)=1\r\n        ) AS T_REF,\r\n        (\r\n                SELECT\r\n                        T_REFED_TAB.NAME                                                                   AS NAME           ,\r\n                        T_REFED_TAB.SCHNAME                                                                     AS SCHNAME        ,\r\n                        T_REFED_IND.ID                                                                    AS REFED_IND_ID   ,\r\n                        T_REFED_IND_OBJ.NAME                                                              AS REFED_CON_NAME,\r\n                        SF_GET_INDEX_KEY_SEQ(T_REFED_IND.KEYNUM, T_REFED_IND.KEYINFO, T_REFED_COL.COLID) AS REFED_KEYNO    ,\r\n                        T_REFED_COL.NAME                                                                   AS REFED_COL_NAME\r\n                FROM\r\n                        SYS.SYSCONS AS T_REFED_CON,\r\n                        (\r\n                                SELECT\r\n                                        TAB.NAME    AS NAME,\r\n                                        TAB.ID      AS ID  ,\r\n                                        SCH.NAME AS SCHNAME\r\n                                FROM\r\n                                        (\r\n                                                SELECT ID, NAME FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND PID = UID()\r\n                                        )\r\n                                        SCH,\r\n                                        (\r\n                                                SELECT\r\n                                                        ID   ,\r\n                                                        SCHID,\r\n                                                        NAME\r\n                                                FROM\r\n                                                        SYS.SYSOBJECTS\r\n                                                WHERE\r\n                                                        TYPE$    = 'SCHOBJ'\r\n                                                    AND SUBTYPE$ = 'UTAB'\r\n                                                    AND INFO3 & 0XFF <> 64 \r\n                                        )\r\n                                        TAB\r\n                                WHERE\r\n                                        SCH.ID = TAB.SCHID ");
        interpolatedStringHandler.AppendFormatted(tableFilter);
        interpolatedStringHandler.AppendLiteral("\r\n                        )              AS T_REFED_TAB ,\r\n                        SYS.SYSINDEXES AS T_REFED_IND,\r\n                        (\r\n                                SELECT ID, PID, NAME FROM SYS.SYSOBJECTS WHERE SUBTYPE$='INDEX'\r\n                        )              AS T_REFED_IND_OBJ,\r\n                        SYS.SYSCOLUMNS AS T_REFED_COL\r\n                WHERE\r\n                        T_REFED_TAB.ID                                                                 = T_REFED_CON.TABLEID\r\n                    AND T_REFED_CON.TYPE$                                                             IN('P','U')\r\n                    AND T_REFED_TAB.ID                                                                 = T_REFED_IND_OBJ.PID\r\n                    AND T_REFED_TAB.ID                                                                 = T_REFED_COL.ID\r\n                    AND T_REFED_CON.INDEXID                                                           = T_REFED_IND_OBJ.ID\r\n                    AND T_REFED_IND.ID                                                                = T_REFED_IND_OBJ.ID\r\n                    AND SF_COL_IS_IDX_KEY(T_REFED_IND.KEYNUM, T_REFED_IND.KEYINFO, T_REFED_COL.COLID)=1\r\n        ) AS T_REFED\r\nWHERE\r\n        T_REF.REFED_IND_ID = T_REFED.REFED_IND_ID\r\n    AND T_REF.REF_KEYNO    = T_REFED.REFED_KEYNO \r\nORDER BY\r\n        FKTABLE_SCHEM ASC,\r\n        FKTABLE_NAME ASC ,\r\n        FK_NAME ASC,\r\n        KEY_SEQ ASC");
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        dbCommand.CommandText = stringAndClear;
        using (DbDataReader source1 = command.ExecuteReader())
        {
          foreach (IGrouping<(string, string), DbDataRecord> source2 in source1.Cast<DbDataRecord>().GroupBy<DbDataRecord, (string, string)>((Func<DbDataRecord, (string, string)>) (ddr => (ddr.GetValueOrDefault<string>("FKTABLE_SCHEM"), ddr.GetValueOrDefault<string>("FKTABLE_NAME")))))
          {
            string tableSchema = source2.Key.Item1;
            string tableName = source2.Key.Item2;
            DatabaseTable databaseTable1 = ((IEnumerable<DatabaseTable>) databaseModel.Tables).Single<DatabaseTable>((Func<DatabaseTable, bool>) (t => t.Schema == tableSchema && t.Name == tableName));
            foreach (IGrouping<(string, string, string, string), DbDataRecord> grouping in source2.GroupBy<DbDataRecord, (string, string, string, string)>((Func<DbDataRecord, (string, string, string, string)>) (c => (c.GetValueOrDefault<string>("FK_NAME"), c.GetValueOrDefault<string>("PKTABLE_SCHEM"), c.GetValueOrDefault<string>("PKTABLE_NAME"), DmDatabaseModelFactory.ConvertToStringReferentialAction(c.GetValueOrDefault<int>("DELETE_RULE"))))))
            {
              string foreignKeyName = grouping.Key.Item1;
              string principalTableSchema = grouping.Key.Item2;
              string principalTableName = grouping.Key.Item3;
              string onDeleteAction = grouping.Key.Item4;
              this._logger.ForeignKeyFound(foreignKeyName, DmDatabaseModelFactory.DisplayName(databaseTable1.Schema, databaseTable1.Name), DmDatabaseModelFactory.DisplayName(principalTableSchema, principalTableName), onDeleteAction);
              DatabaseTable databaseTable2 = ((IEnumerable<DatabaseTable>) databaseModel.Tables).FirstOrDefault<DatabaseTable>((Func<DatabaseTable, bool>) (t => t.Schema == principalTableSchema && t.Name == principalTableName)) ?? ((IEnumerable<DatabaseTable>) databaseModel.Tables).FirstOrDefault<DatabaseTable>((Func<DatabaseTable, bool>) (t => t.Schema.Equals(principalTableSchema, StringComparison.OrdinalIgnoreCase) && t.Name.Equals(principalTableName, StringComparison.OrdinalIgnoreCase)));
              if (databaseTable2 == null)
              {
                this._logger.ForeignKeyReferencesMissingPrincipalTableWarning(foreignKeyName, DmDatabaseModelFactory.DisplayName(databaseTable1.Schema, databaseTable1.Name), DmDatabaseModelFactory.DisplayName(principalTableSchema, principalTableName));
              }
              else
              {
                DatabaseForeignKey databaseForeignKey = new DatabaseForeignKey()
                {
                  Name = foreignKeyName,
                  Table = databaseTable1,
                  PrincipalTable = databaseTable2,
                  OnDelete = DmDatabaseModelFactory.ConvertToReferentialAction(onDeleteAction)
                };
                bool flag = false;
                foreach (DbDataRecord record in (IEnumerable<DbDataRecord>) grouping)
                {
                  string columnName = record.GetValueOrDefault<string>("FKCOLUMN_NAME");
                  DatabaseColumn databaseColumn1 = ((IEnumerable<DatabaseColumn>) databaseTable1.Columns).FirstOrDefault<DatabaseColumn>((Func<DatabaseColumn, bool>) (c => c.Name == columnName)) ?? ((IEnumerable<DatabaseColumn>) databaseTable1.Columns).FirstOrDefault<DatabaseColumn>((Func<DatabaseColumn, bool>) (c => c.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase)));
                  string principalColumnName = record.GetValueOrDefault<string>("PKCOLUMN_NAME");
                  DatabaseColumn databaseColumn2 = ((IEnumerable<DatabaseColumn>) databaseForeignKey.PrincipalTable.Columns).FirstOrDefault<DatabaseColumn>((Func<DatabaseColumn, bool>) (c => c.Name == principalColumnName)) ?? ((IEnumerable<DatabaseColumn>) databaseForeignKey.PrincipalTable.Columns).FirstOrDefault<DatabaseColumn>((Func<DatabaseColumn, bool>) (c => c.Name.Equals(principalColumnName, StringComparison.OrdinalIgnoreCase)));
                  if (databaseColumn2 == null)
                  {
                    flag = true;
                    this._logger.ForeignKeyPrincipalColumnMissingWarning(foreignKeyName, DmDatabaseModelFactory.DisplayName(databaseTable1.Schema, databaseTable1.Name), principalColumnName, DmDatabaseModelFactory.DisplayName(principalTableSchema, principalTableName));
                    break;
                  }
                  ((ICollection<DatabaseColumn>) databaseForeignKey.Columns).Add(databaseColumn1);
                  ((ICollection<DatabaseColumn>) databaseForeignKey.PrincipalColumns).Add(databaseColumn2);
                }
                if (!flag)
                  ((ICollection<DatabaseForeignKey>) databaseTable1.ForeignKeys).Add(databaseForeignKey);
              }
            }
          }
        }
      }
    }

    private void GetIndexes(
      DbConnection connection,
      string tableFilter,
      DatabaseModel databaseModel)
    {
      using (DbCommand command = connection.CreateCommand())
      {
        command.CommandText = "SELECT\r\n        /*+ MAX_OPT_N_TABLES(5) */\r\n        DISTINCT \r\n        SCH.NAME                                                    AS SCH_NAME     ,\r\n        TAB.NAME                                                      AS TAB_NAME      ,\r\n        CASE IND.ISUNIQUE WHEN 'Y' THEN 1 ELSE 0 END                 AS IS_UNIQUE      ,\r\n        OBJ_IND.NAME                                                 AS IND_NAME      ,\r\n        IND.XTYPE & 0x01                \t\t\t\t\t\t  \t  AS IS_CLUSTER          ,\r\n        SF_GET_INDEX_KEY_SEQ(IND.KEYNUM, IND.KEYINFO, COL.COLID)   AS IND_ORDINAL,\r\n        COL.NAME                                                     AS COL_NAME     \r\nFROM\r\n        (\r\n                SELECT\r\n                        ID ,\r\n                        PID,\r\n                        NAME\r\n                FROM\r\n                        SYS.SYSOBJECTS\r\n                WHERE\r\n                        TYPE$ = 'SCH'\r\n                        AND PID =UID()\r\n        )\r\n        SCH,\r\n        (\r\n                SELECT\r\n                        ID   ,\r\n                        SCHID,\r\n                        NAME\r\n                FROM\r\n                        SYS.SYSOBJECTS\r\n                WHERE\r\n                        TYPE$    = 'SCHOBJ'\r\n                    AND SUBTYPE$ IN ('UTAB')\r\n                    AND NAME <> '##HISTOGRAMS_TABLE' \r\n                    AND NAME <> '##PLAN_TABLE'\r\n                    AND INFO3 & 0XFF <> 64\r\n                    AND NAME <> '__EFMigrationsHistory'\r\n        )\r\n        TAB,\r\n        (\r\n                SELECT ID, PID, NAME FROM SYS.SYSOBJECTS WHERE SUBTYPE$='INDEX'\r\n        )              AS OBJ_IND    ,\r\n        SYS.SYSINDEXES AS IND        ,\r\n        SYS.SYSCOLUMNS AS COL\r\nWHERE\r\n        TAB.ID                                                  =COL.ID\r\n    AND TAB.ID                                                  =OBJ_IND.PID\r\n    AND IND.ID                                                 =OBJ_IND.ID\r\n    AND TAB.SCHID                                               = SCH.ID\r\n    AND IND.FLAG & 0X01 = 0\r\n    AND SF_COL_IS_IDX_KEY(IND.KEYNUM, IND.KEYINFO, COL.COLID)=1 " + tableFilter + "\r\nORDER BY\r\n        SCH_NAME ASC,\r\n        TAB_NAME ASC      ,\r\n        IND_NAME ASC,\r\n        IND_ORDINAL ASC;";
        using (DbDataReader source1 = command.ExecuteReader())
        {
          foreach (IGrouping<(string, string), DbDataRecord> source2 in source1.Cast<DbDataRecord>().GroupBy<DbDataRecord, (string, string)>((Func<DbDataRecord, (string, string)>) (ddr => (ddr.GetValueOrDefault<string>("SCH_NAME"), ddr.GetValueOrDefault<string>("TAB_NAME")))))
          {
            string tableSchema = source2.Key.Item1;
            string tableName = source2.Key.Item2;
            DatabaseTable databaseTable = ((IEnumerable<DatabaseTable>) databaseModel.Tables).Single<DatabaseTable>((Func<DatabaseTable, bool>) (t => t.Schema == tableSchema && t.Name == tableName));
            foreach (IGrouping<(string, bool), DbDataRecord> grouping in source2.GroupBy<DbDataRecord, (string, bool)>((Func<DbDataRecord, (string, bool)>) (ddr => (ddr.GetValueOrDefault<string>("IND_NAME"), ddr.GetValueOrDefault<int>("IS_UNIQUE") > 0))).ToArray<IGrouping<(string, bool), DbDataRecord>>())
            {
              this._logger.IndexFound(grouping.Key.Item1, DmDatabaseModelFactory.DisplayName(tableSchema, tableName), grouping.Key.Item2);
              DatabaseIndex databaseIndex = new DatabaseIndex()
              {
                Table = databaseTable,
                Name = grouping.Key.Item1,
                IsUnique = grouping.Key.Item2
              };
              foreach (DbDataRecord record in (IEnumerable<DbDataRecord>) grouping)
              {
                string columnName = record.GetValueOrDefault<string>("COL_NAME");
                DatabaseColumn databaseColumn = ((IEnumerable<DatabaseColumn>) databaseTable.Columns).FirstOrDefault<DatabaseColumn>((Func<DatabaseColumn, bool>) (c => c.Name == columnName)) ?? ((IEnumerable<DatabaseColumn>) databaseTable.Columns).FirstOrDefault<DatabaseColumn>((Func<DatabaseColumn, bool>) (c => c.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase)));
                ((ICollection<DatabaseColumn>) databaseIndex.Columns).Add(databaseColumn);
              }
              ((ICollection<DatabaseIndex>) databaseTable.Indexes).Add(databaseIndex);
            }
          }
        }
      }
    }

    private static string DisplayName(string schema, string name) => (!string.IsNullOrEmpty(schema) ? schema + "." : "") + name;

    private string GetDefaultSchema(DbConnection connection)
    {
      using (DbCommand command = connection.CreateCommand())
      {
        command.CommandText = "SELECT SF_GET_SCHEMA_NAME_BY_ID(CURRENT_SCHID())";
        if (!(command.ExecuteScalar() is string schemaName))
          return (string) null;
        this._logger.DefaultSchemaFound(schemaName);
        return schemaName;
      }
    }

    private static string GetDmClrType(
      string dataTypeName,
      int maxLength,
      int precision,
      int scale)
    {
      if (DmDatabaseModelFactory._decTypes.Contains(dataTypeName))
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
        interpolatedStringHandler.AppendFormatted(dataTypeName);
        interpolatedStringHandler.AppendLiteral("(");
        interpolatedStringHandler.AppendFormatted<int>(precision);
        interpolatedStringHandler.AppendLiteral(", ");
        interpolatedStringHandler.AppendFormatted<int>(scale);
        interpolatedStringHandler.AppendLiteral(")");
        return interpolatedStringHandler.ToStringAndClear();
      }
      if (DmDatabaseModelFactory._dateTimePrecisionTypes.Contains(dataTypeName))
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
        interpolatedStringHandler.AppendFormatted(dataTypeName);
        interpolatedStringHandler.AppendLiteral("(");
        interpolatedStringHandler.AppendFormatted<int>(scale);
        interpolatedStringHandler.AppendLiteral(")");
        return interpolatedStringHandler.ToStringAndClear();
      }
      if (DmDatabaseModelFactory._integerTypes.Contains(dataTypeName))
        return dataTypeName;
      DefaultInterpolatedStringHandler interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(2, 2);
      interpolatedStringHandler1.AppendFormatted(dataTypeName);
      interpolatedStringHandler1.AppendLiteral("(");
      interpolatedStringHandler1.AppendFormatted<int>(maxLength);
      interpolatedStringHandler1.AppendLiteral(")");
      return interpolatedStringHandler1.ToStringAndClear();
    }

    private static ReferentialAction? ConvertToReferentialAction(
      string onDeleteAction)
    {
      string str = onDeleteAction;
      if (str == "NO ACTION")
        return new ReferentialAction?((ReferentialAction) 0);
      if (str == "CASCADE")
        return new ReferentialAction?((ReferentialAction) 2);
      if (str == "SET NULL")
        return new ReferentialAction?((ReferentialAction) 3);
      return str == "SET DEFAULT" ? new ReferentialAction?((ReferentialAction) 4) : new ReferentialAction?();
    }

    private static string ConvertToStringReferentialAction(int onDeleteAction)
    {
      switch (onDeleteAction)
      {
        case 0:
          return "CASCADE";
        case 2:
          return "SET NULL";
        case 3:
          return "NO ACTION";
        case 4:
          return "SET DEFAULT";
        default:
          return (string) null;
      }
    }

    private static Func<string, string> GenerateSchemaFilter(IReadOnlyList<string> schemas) => schemas.Any<string>() ? (Func<string, string>) (s =>
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(s);
      stringBuilder.Append(" IN (");
      stringBuilder.Append(string.Join(", ", schemas.Select<string, string>(new Func<string, string>(DmDatabaseModelFactory.EscapeLiteral))));
      stringBuilder.Append(")");
      return stringBuilder.ToString();
    }) : (Func<string, string>) null;

    private static (string Schema, string Table) Parse(string table)
    {
      Match match = DmDatabaseModelFactory._partExtractor.Match(table.Trim());
      if (!match.Success)
        throw new InvalidOperationException(DmStrings.InvalidTableToIncludeInScaffolding((object) table));
      string str1 = match.Groups["part1"].Value.Replace("]]", "]");
      string str2 = match.Groups["part2"].Value.Replace("]]", "]");
      return string.IsNullOrEmpty(str2) ? ((string) null, str1) : (str1, str2);
    }

        private static Func<string, string, string> GenerateTableFilter(IReadOnlyList<(string Schema, string Table)> tables, Func<string, string> schemaFilter)
        {
            if (schemaFilter != null || tables.Any())
            {
                return delegate (string s, string t)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    bool flag = false;
                    if (schemaFilter != null)
                    {
                        stringBuilder.Append("(").Append(schemaFilter(s));
                        flag = true;
                    }
                    if (tables.Any())
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
                        if (tables.Any())
                        {
                            stringBuilder.Append(t);
                            stringBuilder.Append(" IN (");
                            stringBuilder.Append(string.Join(", ", tables.Select(((string Schema, string Table) e) => EscapeLiteral(e.Table))));
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

        private static string EscapeLiteral(string s) => "N'" + s + "'";

    public DatabaseModel Create(
      string connectionString,
      DatabaseModelFactoryOptions options)
    {
      Check.NotEmpty(connectionString, nameof (connectionString));
      Check.NotNull<DatabaseModelFactoryOptions>(options, nameof (options));
      using (DmConnection connection = new DmConnection(connectionString))
        return this.Create((DbConnection) connection, options);
    }

    public DatabaseModel Create(
      DbConnection connection,
      DatabaseModelFactoryOptions options)
    {
      Check.NotNull<DbConnection>(connection, nameof (connection));
      Check.NotNull<DatabaseModelFactoryOptions>(options, nameof (options));
      DatabaseModel databaseModel = new DatabaseModel();
      bool flag = connection.State == ConnectionState.Open;
      if (!flag)
        connection.Open();
      try
      {
        databaseModel.DefaultSchema = this.GetDefaultSchema(connection);
        List<string> list1 = options.Schemas.ToList<string>();
        Func<string, string> schemaFilter = DmDatabaseModelFactory.GenerateSchemaFilter((IReadOnlyList<string>) list1);
        List<string> list2 = options.Tables.ToList<string>();
        Func<string, string, string> tableFilter = DmDatabaseModelFactory.GenerateTableFilter((IReadOnlyList<(string, string)>) list2.Select<string, (string, string)>(new Func<string, (string, string)>(DmDatabaseModelFactory.Parse)).ToList<(string, string)>(), schemaFilter);
        foreach (DatabaseSequence sequence in this.GetSequences(connection, schemaFilter))
        {
          sequence.Database = databaseModel;
          ((ICollection<DatabaseSequence>) databaseModel.Sequences).Add(sequence);
        }
        this.GetTables(connection, tableFilter, databaseModel);
        foreach (string schemaName in list1.Except<string>(((IEnumerable<DatabaseSequence>) databaseModel.Sequences).Select<DatabaseSequence, string>((Func<DatabaseSequence, string>) (s => s.Schema)).Concat<string>(((IEnumerable<DatabaseTable>) databaseModel.Tables).Select<DatabaseTable, string>((Func<DatabaseTable, string>) (t => t.Schema)))))
          this._logger.MissingSchemaWarning(schemaName);
        foreach (string str in list2)
        {
          (string Schema, string Table) = DmDatabaseModelFactory.Parse(str);
          if (!((IEnumerable<DatabaseTable>) databaseModel.Tables).Any<DatabaseTable>((Func<DatabaseTable, bool>) (t => !string.IsNullOrEmpty(Schema) && t.Schema == Schema || t.Name == Table)))
            this._logger.MissingTableWarning(str);
        }
        return databaseModel;
      }
      finally
      {
        if (!flag)
          connection.Close();
      }
    }
  }
}
