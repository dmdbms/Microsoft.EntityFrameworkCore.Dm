using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.Dm.Update.Internal
{
	public class DmUpdateSqlGenerator : UpdateSqlGenerator, IDmUpdateSqlGenerator, IUpdateSqlGenerator
	{
		private const string InsertedTableBaseName = "@inserted";

		private const string ToInsertTableAlias = "i";

		private const string PositionColumnName = "_Position";

		private const string PositionColumnDeclaration = "\"_Position\" int";

		private const string FullPositionColumnName = "i._Position";

		public DmUpdateSqlGenerator([JetBrains.Annotations.NotNull] UpdateSqlGeneratorDependencies dependencies)
			: base(dependencies)
		{
		}

		public override ResultSetMapping AppendInsertOperation(StringBuilder commandStringBuilder, ModificationCommand command, int commandPosition)
		{
			return base.AppendInsertOperation(commandStringBuilder, command, commandPosition);
		}

		public virtual ResultSetMapping AppendBulkInsertOperation(StringBuilder commandStringBuilder, IReadOnlyList<ModificationCommand> modificationCommands, int commandPosition)
		{
			if (modificationCommands.Count == 1 && modificationCommands[0].ColumnModifications.All(delegate(ColumnModification o)
			{
				int result;
				if (o.IsKey && o.IsRead)
				{
					IProperty property = o.Property;
					result = ((property != null && property.GetValueGenerationStrategy() == DmValueGenerationStrategy.IdentityColumn) ? 1 : 0);
				}
				else
				{
					result = 1;
				}
				return (byte)result != 0;
			}))
			{
				return AppendInsertOperation(commandStringBuilder, modificationCommands[0], commandPosition);
			}
			List<ColumnModification> list = modificationCommands[0].ColumnModifications.Where((ColumnModification o) => o.IsRead).ToList();
			List<ColumnModification> list2 = modificationCommands[0].ColumnModifications.Where((ColumnModification o) => o.IsWrite).ToList();
			List<ColumnModification> keyOperations = modificationCommands[0].ColumnModifications.Where((ColumnModification o) => o.IsKey).ToList();
			bool flag = list2.Count == 0;
			List<ColumnModification> list3 = modificationCommands[0].ColumnModifications.Where((ColumnModification o) => o.Property.GetValueGenerationStrategy() != DmValueGenerationStrategy.IdentityColumn).ToList();
			List<ColumnModification> nonwrite_keys = (from o in modificationCommands[0].ColumnModifications
				where o.IsKey
				where !o.IsWrite
				select o).ToList();
			if (flag)
			{
				if (list3.Count == 0 || list.Count == 0)
				{
					foreach (ModificationCommand modificationCommand in modificationCommands)
					{
						AppendInsertOperation(commandStringBuilder, modificationCommand, commandPosition);
					}
					return (list.Count != 0) ? ResultSetMapping.LastInResultSet : ResultSetMapping.NoResultSet;
				}
				if (list3.Count > 1)
				{
					list3.RemoveRange(1, list3.Count - 1);
				}
			}
			if (list.Count == 0)
			{
				return AppendBulkInsertWithoutServerValues(commandStringBuilder, modificationCommands, list2);
			}
			if (flag)
			{
				return AppendBulkInsertWithServerValuesOnly(commandStringBuilder, modificationCommands, commandPosition, list3, keyOperations, list, nonwrite_keys);
			}
			return AppendBulkInsertWithServerValues(commandStringBuilder, modificationCommands, commandPosition, list2, keyOperations, list, nonwrite_keys);
		}

		private ResultSetMapping AppendBulkInsertWithoutServerValues(StringBuilder commandStringBuilder, IReadOnlyList<ModificationCommand> modificationCommands, List<ColumnModification> writeOperations)
		{
			string tableName = modificationCommands[0].TableName;
			string schema = modificationCommands[0].Schema;
			AppendInsertCommandHeader(commandStringBuilder, tableName, schema, writeOperations);
			AppendValuesHeader(commandStringBuilder, writeOperations);
			AppendValues(commandStringBuilder, writeOperations, null);
			for (int i = 1; i < modificationCommands.Count; i++)
			{
				commandStringBuilder.Append(",").AppendLine();
				AppendValues(commandStringBuilder, modificationCommands[i].ColumnModifications.Where((ColumnModification o) => o.IsWrite).ToList(), null);
			}
			commandStringBuilder.Append(SqlGenerationHelper.StatementTerminator).AppendLine();
			return ResultSetMapping.NoResultSet;
		}

		private void AppandTempArrayInit(StringBuilder commandStringBuilder, IReadOnlyList<ModificationCommand> modificationCommands, int commandPosition)
		{
			commandStringBuilder.Append("c").Append(commandPosition).Append(" = NEW rrr")
				.Append(commandPosition)
				.Append("[")
				.Append(modificationCommands.Count.ToString())
				.AppendLine("];");
			int i;
			for (i = 0; i < modificationCommands.Count; i++)
			{
				commandStringBuilder.AppendJoin(modificationCommands[i].ColumnModifications, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
				{
					if (o.IsWrite)
					{
						sb.Append("c").Append(commandPosition).Append("[")
							.Append((i + 1).ToString())
							.Append("].");
						helper.DelimitIdentifier(sb, o.ColumnName);
						sb.Append(" = ");
						helper.GenerateParameterName(sb, o.ParameterName);
						sb.Append(" ;\n");
					}
				}, "");
				bool flag = true;
				commandStringBuilder.Append("c").Append(commandPosition).Append("[")
					.Append((i + 1).ToString())
					.Append("].\"")
					.Append("_Position")
					.Append("\" = ")
					.Append(i)
					.AppendLine(";");
			}
		}

		private void AppendSelectIdentity(StringBuilder commandStringBuilder, IReadOnlyList<ModificationCommand> modificationCommands, List<ColumnModification> nonwrite_keys, int commandPosition)
		{
			string tableName = modificationCommands[0].TableName;
			string schema = modificationCommands[0].Schema;
			commandStringBuilder.Append("SELECT ").AppendJoin(nonwrite_keys, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
			{
				sb.Append("MAX(");
				helper.DelimitIdentifier(sb, o.ColumnName);
				sb.Append(")");
			}, ",").Append(" INTO ")
				.AppendJoin(nonwrite_keys, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
				{
					helper.DelimitIdentifier(sb, "V_" + o.ColumnName);
				}, ",")
				.Append(" FROM ");
			SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, tableName, schema);
			commandStringBuilder.AppendLine(";");
		}

		private ResultSetMapping AppendBulkInsertWithServerValues(StringBuilder commandStringBuilder, IReadOnlyList<ModificationCommand> modificationCommands, int commandPosition, List<ColumnModification> writeOperations, List<ColumnModification> keyOperations, List<ColumnModification> readOperations, List<ColumnModification> nonwrite_keys)
		{
			AppendDeclareTable(commandStringBuilder, "@inserted", commandPosition, writeOperations, nonwrite_keys, "\"_Position\" int");
			string tableName = modificationCommands[0].TableName;
			string schema = modificationCommands[0].Schema;
			IReadOnlyList<ColumnModification> columnModifications = modificationCommands[0].ColumnModifications;
			commandStringBuilder.AppendLine("BEGIN");
			AppandTempArrayInit(commandStringBuilder, modificationCommands, commandPosition);
			if (nonwrite_keys != null && nonwrite_keys.Count > 0)
			{
				AppendSelectIdentity(commandStringBuilder, modificationCommands, nonwrite_keys, commandPosition);
			}
			AppendInsertCommandHeader(commandStringBuilder, tableName, schema, writeOperations);
			AppendInsertSelect(commandStringBuilder, tableName, schema, writeOperations, commandPosition);
			if (nonwrite_keys != null && nonwrite_keys.Count > 0)
			{
				AppendSelectCommand(commandStringBuilder, readOperations, writeOperations, nonwrite_keys, "@inserted", commandPosition, tableName, schema, "_Position");
			}
			else
			{
				AppendSelectCommand(commandStringBuilder, readOperations, keyOperations, nonwrite_keys, "@inserted", commandPosition, tableName, schema, "_Position");
			}
			commandStringBuilder.AppendLine(" END;");
			return ResultSetMapping.NotLastInResultSet;
		}

		private ResultSetMapping AppendBulkInsertWithServerValuesOnly(StringBuilder commandStringBuilder, IReadOnlyList<ModificationCommand> modificationCommands, int commandPosition, List<ColumnModification> nonIdentityOperations, List<ColumnModification> keyOperations, List<ColumnModification> readOperations, List<ColumnModification> nonwrite_keys)
		{
			AppendDeclareTable(commandStringBuilder, "@inserted", commandPosition, keyOperations, nonwrite_keys);
			string tableName = modificationCommands[0].TableName;
			string schema = modificationCommands[0].Schema;
			commandStringBuilder.AppendLine(" BEGIN");
			AppendSelectIdentity(commandStringBuilder, modificationCommands, nonwrite_keys, commandPosition);
			AppendInsertCommandHeader(commandStringBuilder, tableName, schema, nonIdentityOperations);
			AppendValuesHeader(commandStringBuilder, nonIdentityOperations);
			AppendValues(commandStringBuilder, nonIdentityOperations, null);
			for (int i = 1; i < modificationCommands.Count; i++)
			{
				commandStringBuilder.Append(",").AppendLine();
				AppendValues(commandStringBuilder, nonIdentityOperations, null);
			}
			commandStringBuilder.Append(SqlGenerationHelper.StatementTerminator);
			AppendSelectCommand(commandStringBuilder, readOperations, keyOperations, nonwrite_keys, "@inserted", commandPosition, tableName, schema);
			commandStringBuilder.AppendLine(" END;");
			return ResultSetMapping.NotLastInResultSet;
		}

		private void AppendMergeCommandHeader([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] string toInsertTableAlias, [JetBrains.Annotations.NotNull] IReadOnlyList<ModificationCommand> modificationCommands, [JetBrains.Annotations.NotNull] IReadOnlyList<ColumnModification> writeOperations, string additionalColumns = null)
		{
			commandStringBuilder.Append("MERGE ");
			SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, name, schema);
			commandStringBuilder.Append(" USING (");
			AppendValuesHeader(commandStringBuilder, writeOperations);
			AppendValues(commandStringBuilder, writeOperations, "0");
			for (int i = 1; i < modificationCommands.Count; i++)
			{
				commandStringBuilder.Append(",").AppendLine();
				AppendValues(commandStringBuilder, modificationCommands[i].ColumnModifications.Where((ColumnModification o) => o.IsWrite).ToList(), i.ToString(CultureInfo.InvariantCulture));
			}
			commandStringBuilder.Append(") AS ").Append(toInsertTableAlias).Append(" (")
				.AppendJoin(writeOperations, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
				{
					helper.DelimitIdentifier(sb, o.ColumnName);
				});
			if (additionalColumns != null)
			{
				commandStringBuilder.Append(", ").Append(additionalColumns);
			}
			commandStringBuilder.Append(")").AppendLine(" ON 1=0").AppendLine("WHEN NOT MATCHED THEN");
			commandStringBuilder.Append("INSERT ").Append("(").AppendJoin(writeOperations, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
			{
				helper.DelimitIdentifier(sb, o.ColumnName);
			})
				.Append(")");
			AppendValuesHeader(commandStringBuilder, writeOperations);
			commandStringBuilder.Append("(").AppendJoin(writeOperations, toInsertTableAlias, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, string alias, ISqlGenerationHelper helper)
			{
				sb.Append(alias).Append(".");
				helper.DelimitIdentifier(sb, o.ColumnName);
			}).Append(")");
		}

		private void AppendValues(StringBuilder commandStringBuilder, IReadOnlyList<ColumnModification> operations, string additionalLiteral)
		{
			if (operations.Count <= 0)
			{
				return;
			}
			commandStringBuilder.Append("(").AppendJoin(operations, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
			{
				if (o.IsWrite)
				{
					helper.GenerateParameterName(sb, o.ParameterName);
				}
				else if (o.Property.GetDefaultValueSql() != null)
				{
					string value = o.Property.GetDefaultValueSql().ToString();
					sb.Append(value);
				}
				else
				{
					sb.Append("DEFAULT");
				}
			});
			if (additionalLiteral != null)
			{
				commandStringBuilder.Append(", ").Append(additionalLiteral);
			}
			commandStringBuilder.Append(")");
		}

		private void AppendDeclareTable(StringBuilder commandStringBuilder, string name, int index, IReadOnlyList<ColumnModification> operations, IReadOnlyList<ColumnModification> nonwrite_keys, string additionalColumns = null)
		{
			commandStringBuilder.Append("DECLARE ");
			if (operations != null && operations.Count > 0 && operations.Any((ColumnModification o) => o.IsWrite))
			{
				commandStringBuilder.Append(" TYPE rrr").Append(index).Append(" IS RECORD (")
					.AppendJoin(operations, this, delegate(StringBuilder sb, ColumnModification o, DmUpdateSqlGenerator generator)
					{
						if (o.IsWrite)
						{
							generator.SqlGenerationHelper.DelimitIdentifier(sb, o.ColumnName);
							if (generator.GetTypeNameForCopy(o.Property).Equals("INTEGER identity(1, 1)"))
							{
								sb.Append(" ").Append("integer");
							}
							else
							{
								sb.Append(" ").Append(generator.GetTypeNameForCopy(o.Property));
							}
						}
					});
				if (additionalColumns != null)
				{
					commandStringBuilder.Append(", ").Append(additionalColumns);
				}
				commandStringBuilder.Append(")").Append(SqlGenerationHelper.StatementTerminator).AppendLine();
				commandStringBuilder.Append("TYPE ccc").Append(index).Append(" IS ARRAY rrr")
					.Append(index)
					.AppendLine("[];")
					.Append("c")
					.Append(index)
					.Append(" ccc")
					.Append(index)
					.AppendLine("; ");
			}
			if (nonwrite_keys == null || nonwrite_keys.Count <= 0)
			{
				return;
			}
			commandStringBuilder.AppendJoin(nonwrite_keys, this, delegate(StringBuilder sb, ColumnModification o, DmUpdateSqlGenerator generator)
			{
				generator.SqlGenerationHelper.DelimitIdentifier(sb, "V_" + o.ColumnName);
				if (generator.GetTypeNameForCopy(o.Property).Equals("INTEGER identity(1, 1)"))
				{
					sb.Append(" ").Append("integer");
				}
				else
				{
					sb.Append(" ").Append(generator.GetTypeNameForCopy(o.Property));
				}
			}, ";\n").Append(";\n");
		}

		private string GetTypeNameForCopy(IProperty property)
		{
			string text = property.GetColumnType();
			if (text == null)
			{
				text = property.FindFirstPrincipal()?.GetColumnType() ?? Dependencies.TypeMappingSource.FindMapping(property.ClrType)?.StoreType;
			}
			if (property.ClrType == typeof(byte[]) && text != null && (text.Equals("rowversion", StringComparison.OrdinalIgnoreCase) || text.Equals("timestamp", StringComparison.OrdinalIgnoreCase)))
			{
				return property.IsNullable ? "varbinary(8)" : "binary(8)";
			}
			return text;
		}

		private void AppendOutputClause(StringBuilder commandStringBuilder, IReadOnlyList<ColumnModification> operations, string tableName, int tableIndex, string additionalColumns = null)
		{
			commandStringBuilder.AppendLine().Append("OUTPUT ").AppendJoin(operations, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
			{
				sb.Append("INSERTED.");
				helper.DelimitIdentifier(sb, o.ColumnName);
			});
			if (additionalColumns != null)
			{
				commandStringBuilder.Append(", ").Append(additionalColumns);
			}
			commandStringBuilder.AppendLine().Append("INTO ").Append(tableName)
				.Append(tableIndex);
		}

		private ResultSetMapping AppendInsertOperationWithServerKeys(StringBuilder commandStringBuilder, ModificationCommand command, IReadOnlyList<ColumnModification> keyOperations, IReadOnlyList<ColumnModification> readOperations, IReadOnlyList<ColumnModification> nonwrite_keys, int commandPosition)
		{
			string tableName = command.TableName;
			string schema = command.Schema;
			IReadOnlyList<ColumnModification> columnModifications = command.ColumnModifications;
			List<ColumnModification> operations = columnModifications.Where((ColumnModification o) => o.IsWrite).ToList();
			AppendDeclareTable(commandStringBuilder, "@inserted", commandPosition, keyOperations, nonwrite_keys);
			commandStringBuilder.AppendLine("BEGIN");
			AppendInsertCommandHeader(commandStringBuilder, tableName, schema, operations);
			AppendOutputClause(commandStringBuilder, keyOperations, "@inserted", commandPosition);
			AppendValuesHeader(commandStringBuilder, operations);
			AppendValues(commandStringBuilder, operations, null);
			commandStringBuilder.Append(SqlGenerationHelper.StatementTerminator);
			return AppendSelectCommand(commandStringBuilder, readOperations, keyOperations, null, "@inserted", commandPosition, tableName, schema);
		}

		protected virtual void AppendInsertSelect([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] IReadOnlyList<ColumnModification> operations, int commandPosition)
		{
			commandStringBuilder.Append("SELECT ");
			commandStringBuilder.AppendJoin(operations, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
			{
				helper.DelimitIdentifier(sb, o.ColumnName);
			});
			commandStringBuilder.Append(" FROM ARRAY C").Append(commandPosition).Append(";");
		}

		private ResultSetMapping AppendSelectCommand(StringBuilder commandStringBuilder, IReadOnlyList<ColumnModification> readOperations, IReadOnlyList<ColumnModification> keyOperations, IReadOnlyList<ColumnModification> nonwrite_keyOperations, string insertedTableName, int insertedTableIndex, string tableName, string schema, string orderColumn = null)
		{
			bool flag = false;
			commandStringBuilder.AppendLine().Append("SELECT ").AppendJoin(readOperations, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
			{
				helper.DelimitIdentifier(sb, o.ColumnName, "t");
			})
				.Append(" FROM ");
			SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, tableName, schema);
			commandStringBuilder.Append(" \"t\"").AppendLine();
			if (keyOperations.Count > 0 && keyOperations.Any((ColumnModification o) => o.IsWrite))
			{
				commandStringBuilder.Append(" WHERE EXISTS(SELECT * FROM ").Append("ARRAY c").Append(insertedTableIndex)
					.Append(" \"i\"")
					.Append(" WHERE ")
					.AppendJoin(keyOperations, delegate(StringBuilder sb, ColumnModification c)
					{
						sb.Append("(");
						if (c.Property.ClrType == typeof(string))
						{
							sb.Append("TEXT_EQUAL(");
							SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "t");
							sb.Append(", ");
							SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "i");
							sb.Append(")");
						}
						else if (c.Property.ClrType == typeof(byte[]))
						{
							sb.Append("BLOB_EQUAL(");
							SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "t");
							sb.Append(", ");
							SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "i");
							sb.Append(")");
						}
						else
						{
							SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "t");
							sb.Append(" = ");
							SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "i");
						}
						sb.Append(" OR (");
						SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "t");
						sb.Append(" IS NULL AND ");
						SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "i");
						sb.Append(" IS NULL )) ");
					}, " AND ")
					.Append(")");
				flag = true;
			}
			if (nonwrite_keyOperations != null && nonwrite_keyOperations.Count > 0)
			{
				if (flag)
				{
					commandStringBuilder.Append(" AND ");
				}
				else
				{
					commandStringBuilder.Append(" WHERE ");
				}
				commandStringBuilder.AppendJoin(nonwrite_keyOperations, SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
				{
					sb.Append("( ");
					helper.DelimitIdentifier(sb, o.ColumnName, "t");
					sb.Append(" > ");
					helper.DelimitIdentifier(sb, "V_" + o.ColumnName);
					sb.Append(" OR ");
					helper.DelimitIdentifier(sb, "V_" + o.ColumnName);
					sb.Append(" IS NULL) ");
				}, " AND ");
			}
			if (orderColumn != null)
			{
				commandStringBuilder.AppendLine().Append("ORDER BY ");
				SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, "ROWID", "t");
			}
			commandStringBuilder.Append(SqlGenerationHelper.StatementTerminator).AppendLine().AppendLine();
			return ResultSetMapping.LastInResultSet;
		}

		protected override ResultSetMapping AppendSelectAffectedCountCommand(StringBuilder commandStringBuilder, string name, string schema, int commandPosition)
		{
			commandStringBuilder.Append("SELECT sql%ROWCOUNT").Append(SqlGenerationHelper.StatementTerminator).AppendLine()
				.AppendLine();
			return ResultSetMapping.LastInResultSet;
		}

		public override void AppendBatchHeader(StringBuilder commandStringBuilder)
		{
			commandStringBuilder.AppendLine();
		}

		protected override void AppendIdentityWhereCondition(StringBuilder commandStringBuilder, ColumnModification columnModification)
		{
			SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, columnModification.ColumnName);
			commandStringBuilder.Append(" = ");
			commandStringBuilder.Append("scope_identity()");
		}

		protected override void AppendRowsAffectedWhereCondition(StringBuilder commandStringBuilder, int expectedRowsAffected)
		{
			commandStringBuilder.Append("sql%ROWCOUNT = ").Append(expectedRowsAffected.ToString(CultureInfo.InvariantCulture));
		}

		public override string GenerateNextSequenceValueOperation(string name, string schema)
		{
			StringBuilder stringBuilder = new StringBuilder();
			AppendNextSequenceValueOperation(stringBuilder, name, schema);
			return stringBuilder.ToString();
		}

		public override void AppendNextSequenceValueOperation(StringBuilder commandStringBuilder, string name, string schema)
		{
			commandStringBuilder.Append("SELECT ");
			SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, name, schema);
			commandStringBuilder.Append(".NEXTVAL");
		}
	}
}
