#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		public DmUpdateSqlGenerator([NotNull] UpdateSqlGeneratorDependencies dependencies)
			: base(dependencies)
		{
		}

		public virtual ResultSetMapping AppendBulkInsertOperation(StringBuilder commandStringBuilder, IReadOnlyList<IReadOnlyModificationCommand> modificationCommands, int commandPosition)
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			if (modificationCommands.Count == 1 && modificationCommands[0].ColumnModifications.All(delegate(IColumnModification o)
			{
				int result;
				if (o.IsKey && o.IsRead)
				{
					IProperty property2 = o.Property;
					result = ((property2 != null && ((IReadOnlyProperty)(object)property2).GetValueGenerationStrategy() == DmValueGenerationStrategy.IdentityColumn) ? 1 : 0);
				}
				else
				{
					result = 1;
				}
				return (byte)result != 0;
			}))
			{
				return base.AppendInsertOperation(commandStringBuilder, modificationCommands[0], commandPosition);
			}
			List<IColumnModification> list = (from o in modificationCommands[0].ColumnModifications
				where o.IsRead
				select o).ToList();
			List<IColumnModification> list2 = (from o in modificationCommands[0].ColumnModifications
				where o.IsWrite
				select o).ToList();
			List<IColumnModification> keyOperations = (from o in modificationCommands[0].ColumnModifications
				where o.IsKey
				select o).ToList();
			bool flag = list2.Count == 0;
			List<IColumnModification> list3 = modificationCommands[0].ColumnModifications.Where(delegate(IColumnModification o)
			{
				IProperty property = o.Property;
				return property == null || ((IReadOnlyProperty)(object)property).GetValueGenerationStrategy() != DmValueGenerationStrategy.IdentityColumn;
			}).ToList();
			List<IColumnModification> nonwrite_keys = (from o in modificationCommands[0].ColumnModifications
				where o.IsKey
				where !o.IsWrite
				select o).ToList();
			if (flag)
			{
				if (list3.Count == 0 || list.Count == 0)
				{
					foreach (IReadOnlyModificationCommand modificationCommand in modificationCommands)
					{
						base.AppendInsertOperation(commandStringBuilder, modificationCommand, commandPosition);
					}
					return (ResultSetMapping)((list.Count != 0) ? 2 : 0);
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

		private ResultSetMapping AppendBulkInsertWithoutServerValues(StringBuilder commandStringBuilder, IReadOnlyList<IReadOnlyModificationCommand> modificationCommands, List<IColumnModification> writeOperations)
		{
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			Debug.Assert(writeOperations.Count > 0);
			string tableName = modificationCommands[0].TableName;
			string schema = modificationCommands[0].Schema;
			base.AppendInsertCommandHeader(commandStringBuilder, tableName, schema, (IReadOnlyList<IColumnModification>)writeOperations);
			base.AppendValuesHeader(commandStringBuilder, (IReadOnlyList<IColumnModification>)writeOperations);
			AppendValues(commandStringBuilder, writeOperations, null);
			for (int i = 1; i < modificationCommands.Count; i++)
			{
				commandStringBuilder.Append(",").AppendLine();
				AppendValues(commandStringBuilder, (from o in modificationCommands[i].ColumnModifications
					where o.IsWrite
					select o).ToList(), null);
			}
			commandStringBuilder.Append(base.SqlGenerationHelper.StatementTerminator).AppendLine();
			return (ResultSetMapping)0;
		}

		private void AppandTempArrayInit(StringBuilder commandStringBuilder, IReadOnlyList<IReadOnlyModificationCommand> modificationCommands, int commandPosition)
		{
			commandStringBuilder.Append("c").Append(commandPosition).Append(" = NEW rrr")
				.Append(commandPosition)
				.Append("[")
				.Append(modificationCommands.Count.ToString())
				.AppendLine("];");
			int i;
			for (i = 0; i < modificationCommands.Count; i++)
			{
				commandStringBuilder.AppendJoin(modificationCommands[i].ColumnModifications, base.SqlGenerationHelper, delegate(StringBuilder sb, IColumnModification o, ISqlGenerationHelper helper)
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

		private void AppendSelectIdentity(StringBuilder commandStringBuilder, IReadOnlyList<IReadOnlyModificationCommand> modificationCommands, List<IColumnModification> nonwrite_keys, int commandPosition)
		{
			string tableName = modificationCommands[0].TableName;
			string schema = modificationCommands[0].Schema;
			commandStringBuilder.Append("SELECT ").AppendJoin(nonwrite_keys, base.SqlGenerationHelper, delegate(StringBuilder sb, IColumnModification o, ISqlGenerationHelper helper)
			{
				sb.Append("MAX(");
				helper.DelimitIdentifier(sb, o.ColumnName);
				sb.Append(")");
			}, ",").Append(" INTO ")
				.AppendJoin(nonwrite_keys, base.SqlGenerationHelper, delegate(StringBuilder sb, IColumnModification o, ISqlGenerationHelper helper)
				{
					helper.DelimitIdentifier(sb, "V_" + o.ColumnName);
				}, ",")
				.Append(" FROM ");
			base.SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, tableName, schema);
			commandStringBuilder.AppendLine(";");
		}

		private ResultSetMapping AppendBulkInsertWithServerValues(StringBuilder commandStringBuilder, IReadOnlyList<IReadOnlyModificationCommand> modificationCommands, int commandPosition, List<IColumnModification> writeOperations, List<IColumnModification> keyOperations, List<IColumnModification> readOperations, List<IColumnModification> nonwrite_keys)
		{
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			AppendDeclareTable(commandStringBuilder, "@inserted", commandPosition, writeOperations, nonwrite_keys, "\"_Position\" int");
			string tableName = modificationCommands[0].TableName;
			string schema = modificationCommands[0].Schema;
			IReadOnlyList<IColumnModification> columnModifications = modificationCommands[0].ColumnModifications;
			commandStringBuilder.AppendLine("BEGIN");
			AppandTempArrayInit(commandStringBuilder, modificationCommands, commandPosition);
			if (nonwrite_keys != null && nonwrite_keys.Count > 0)
			{
				AppendSelectIdentity(commandStringBuilder, modificationCommands, nonwrite_keys, commandPosition);
			}
			base.AppendInsertCommandHeader(commandStringBuilder, tableName, schema, (IReadOnlyList<IColumnModification>)writeOperations);
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
			return (ResultSetMapping)1;
		}

		private ResultSetMapping AppendBulkInsertWithServerValuesOnly(StringBuilder commandStringBuilder, IReadOnlyList<IReadOnlyModificationCommand> modificationCommands, int commandPosition, List<IColumnModification> nonIdentityOperations, List<IColumnModification> keyOperations, List<IColumnModification> readOperations, List<IColumnModification> nonwrite_keys)
		{
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			AppendDeclareTable(commandStringBuilder, "@inserted", commandPosition, keyOperations, nonwrite_keys);
			string tableName = modificationCommands[0].TableName;
			string schema = modificationCommands[0].Schema;
			commandStringBuilder.AppendLine(" BEGIN");
			AppendSelectIdentity(commandStringBuilder, modificationCommands, nonwrite_keys, commandPosition);
			base.AppendInsertCommandHeader(commandStringBuilder, tableName, schema, (IReadOnlyList<IColumnModification>)nonIdentityOperations);
			base.AppendValuesHeader(commandStringBuilder, (IReadOnlyList<IColumnModification>)nonIdentityOperations);
			AppendValues(commandStringBuilder, nonIdentityOperations, null);
			for (int i = 1; i < modificationCommands.Count; i++)
			{
				commandStringBuilder.Append(",").AppendLine();
				AppendValues(commandStringBuilder, nonIdentityOperations, null);
			}
			commandStringBuilder.Append(base.SqlGenerationHelper.StatementTerminator);
			AppendSelectCommand(commandStringBuilder, readOperations, keyOperations, nonwrite_keys, "@inserted", commandPosition, tableName, schema);
			commandStringBuilder.AppendLine(" END;");
			return (ResultSetMapping)1;
		}

		private void AppendMergeCommandHeader([NotNull] StringBuilder commandStringBuilder, [NotNull] string name, [CanBeNull] string schema, [NotNull] string toInsertTableAlias, [NotNull] IReadOnlyList<ModificationCommand> modificationCommands, [NotNull] IReadOnlyList<ColumnModification> writeOperations, string additionalColumns = null)
		{
			commandStringBuilder.Append("MERGE ");
			base.SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, name, schema);
			commandStringBuilder.Append(" USING (");
			base.AppendValuesHeader(commandStringBuilder, (IReadOnlyList<IColumnModification>)writeOperations);
			AppendValues(commandStringBuilder, (IReadOnlyList<IColumnModification>)writeOperations, "0");
			for (int i = 1; i < modificationCommands.Count; i++)
			{
				commandStringBuilder.Append(",").AppendLine();
				AppendValues(commandStringBuilder, (from o in modificationCommands[i].ColumnModifications
					where o.IsWrite
					select o).ToList(), i.ToString(CultureInfo.InvariantCulture));
			}
			commandStringBuilder.Append(") AS ").Append(toInsertTableAlias).Append(" (")
				.AppendJoin(writeOperations, base.SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
				{
					helper.DelimitIdentifier(sb, o.ColumnName);
				});
			if (additionalColumns != null)
			{
				commandStringBuilder.Append(", ").Append(additionalColumns);
			}
			commandStringBuilder.Append(")").AppendLine(" ON 1=0").AppendLine("WHEN NOT MATCHED THEN");
			commandStringBuilder.Append("INSERT ").Append("(").AppendJoin(writeOperations, base.SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
			{
				helper.DelimitIdentifier(sb, o.ColumnName);
			})
				.Append(")");
			base.AppendValuesHeader(commandStringBuilder, (IReadOnlyList<IColumnModification>)writeOperations);
			commandStringBuilder.Append("(").AppendJoin(writeOperations, toInsertTableAlias, base.SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, string alias, ISqlGenerationHelper helper)
			{
				sb.Append(alias).Append(".");
				helper.DelimitIdentifier(sb, o.ColumnName);
			}).Append(")");
		}

		private void AppendValues(StringBuilder commandStringBuilder, IReadOnlyList<IColumnModification> operations, string additionalLiteral)
		{
			if (operations.Count <= 0)
			{
				return;
			}
			commandStringBuilder.Append("(").AppendJoin(operations, base.SqlGenerationHelper, delegate(StringBuilder sb, IColumnModification o, ISqlGenerationHelper helper)
			{
				if (o.IsWrite)
				{
					helper.GenerateParameterName(sb, o.ParameterName);
				}
				else if (RelationalPropertyExtensions.GetDefaultValueSql((IReadOnlyProperty)(object)o.Property) != null)
				{
					string value = RelationalPropertyExtensions.GetDefaultValueSql((IReadOnlyProperty)(object)o.Property).ToString();
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

		private void AppendDeclareTable(StringBuilder commandStringBuilder, string name, int index, IReadOnlyList<IColumnModification> operations, IReadOnlyList<IColumnModification> nonwrite_keys, string additionalColumns = null)
		{
			commandStringBuilder.Append("DECLARE ");
			if (operations != null && operations.Count > 0 && operations.Any((IColumnModification o) => o.IsWrite))
			{
				commandStringBuilder.Append(" TYPE rrr").Append(index).Append(" IS RECORD (")
					.AppendJoin(operations, this, delegate(StringBuilder sb, IColumnModification o, DmUpdateSqlGenerator generator)
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
				commandStringBuilder.Append(")").Append(base.SqlGenerationHelper.StatementTerminator).AppendLine();
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
			commandStringBuilder.AppendJoin(nonwrite_keys, this, delegate(StringBuilder sb, IColumnModification o, DmUpdateSqlGenerator generator)
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
			string text = RelationalPropertyExtensions.GetColumnType(property);
			if (text == null)
			{
				IProperty val = property.FindFirstPrincipal();
				object obj = ((val != null) ? RelationalPropertyExtensions.GetColumnType(val) : null);
				if (obj == null)
				{
					RelationalTypeMapping obj2 = base.Dependencies.TypeMappingSource.FindMapping(((IReadOnlyPropertyBase)property).ClrType);
					obj = ((obj2 != null) ? obj2.StoreType : null);
				}
				text = (string)obj;
			}
			if (((IReadOnlyPropertyBase)property).ClrType == typeof(byte[]) && text != null && (text.Equals("rowversion", StringComparison.OrdinalIgnoreCase) || text.Equals("timestamp", StringComparison.OrdinalIgnoreCase)))
			{
				return ((IReadOnlyProperty)property).IsNullable ? "varbinary(8)" : "binary(8)";
			}
			return text;
		}

		private void AppendOutputClause(StringBuilder commandStringBuilder, IReadOnlyList<ColumnModification> operations, string tableName, int tableIndex, string additionalColumns = null)
		{
			commandStringBuilder.AppendLine().Append("OUTPUT ").AppendJoin(operations, base.SqlGenerationHelper, delegate(StringBuilder sb, ColumnModification o, ISqlGenerationHelper helper)
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
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			string tableName = command.TableName;
			string schema = command.Schema;
			IReadOnlyList<IColumnModification> columnModifications = command.ColumnModifications;
			List<IColumnModification> list = columnModifications.Where((IColumnModification o) => o.IsWrite).ToList();
			AppendDeclareTable(commandStringBuilder, "@inserted", commandPosition, (IReadOnlyList<IColumnModification>)keyOperations, (IReadOnlyList<IColumnModification>)nonwrite_keys);
			commandStringBuilder.AppendLine("BEGIN");
			base.AppendInsertCommandHeader(commandStringBuilder, tableName, schema, (IReadOnlyList<IColumnModification>)list);
			AppendOutputClause(commandStringBuilder, keyOperations, "@inserted", commandPosition);
			base.AppendValuesHeader(commandStringBuilder, (IReadOnlyList<IColumnModification>)list);
			AppendValues(commandStringBuilder, list, null);
			commandStringBuilder.Append(base.SqlGenerationHelper.StatementTerminator);
			return AppendSelectCommand(commandStringBuilder, (IReadOnlyList<IColumnModification>)readOperations, (IReadOnlyList<IColumnModification>)keyOperations, null, "@inserted", commandPosition, tableName, schema);
		}

		protected virtual void AppendInsertSelect([NotNull] StringBuilder commandStringBuilder, [NotNull] string name, [CanBeNull] string schema, [NotNull] IReadOnlyList<IColumnModification> operations, int commandPosition)
		{
			commandStringBuilder.Append("SELECT ");
			commandStringBuilder.AppendJoin(operations, base.SqlGenerationHelper, delegate(StringBuilder sb, IColumnModification o, ISqlGenerationHelper helper)
			{
				helper.DelimitIdentifier(sb, o.ColumnName);
			});
			commandStringBuilder.Append(" FROM ARRAY C").Append(commandPosition).Append(";");
		}

		private ResultSetMapping AppendSelectCommand(StringBuilder commandStringBuilder, IReadOnlyList<IColumnModification> readOperations, IReadOnlyList<IColumnModification> keyOperations, IReadOnlyList<IColumnModification> nonwrite_keyOperations, string insertedTableName, int insertedTableIndex, string tableName, string schema, string orderColumn = null)
		{
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			bool flag = false;
			commandStringBuilder.AppendLine().Append("SELECT ").AppendJoin(readOperations, base.SqlGenerationHelper, delegate(StringBuilder sb, IColumnModification o, ISqlGenerationHelper helper)
			{
				helper.DelimitIdentifier(sb, o.ColumnName, "t");
			})
				.Append(" FROM ");
			base.SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, tableName, schema);
			commandStringBuilder.Append(" \"t\"").AppendLine();
			if (keyOperations.Count > 0 && keyOperations.Any((IColumnModification o) => o.IsWrite))
			{
				commandStringBuilder.Append(" WHERE EXISTS(SELECT * FROM ").Append("ARRAY c").Append(insertedTableIndex)
					.Append(" \"i\"")
					.Append(" WHERE ")
					.AppendJoin(keyOperations, delegate(StringBuilder sb, IColumnModification c)
					{
						sb.Append("(");
						if (((IReadOnlyPropertyBase)c.Property).ClrType == typeof(string))
						{
							sb.Append("TEXT_EQUAL(");
							base.SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "t");
							sb.Append(", ");
							base.SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "i");
							sb.Append(")");
						}
						else if (((IReadOnlyPropertyBase)c.Property).ClrType == typeof(byte[]))
						{
							sb.Append("BLOB_EQUAL(");
							base.SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "t");
							sb.Append(", ");
							base.SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "i");
							sb.Append(")");
						}
						else
						{
							base.SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "t");
							sb.Append(" = ");
							base.SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "i");
						}
						sb.Append(" OR (");
						base.SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "t");
						sb.Append(" IS NULL AND ");
						base.SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "i");
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
				commandStringBuilder.AppendJoin(nonwrite_keyOperations, base.SqlGenerationHelper, delegate(StringBuilder sb, IColumnModification o, ISqlGenerationHelper helper)
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
				base.SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, "ROWID", "t");
			}
			commandStringBuilder.Append(base.SqlGenerationHelper.StatementTerminator).AppendLine().AppendLine();
			return (ResultSetMapping)2;
		}

		protected override ResultSetMapping AppendSelectAffectedCountCommand(StringBuilder commandStringBuilder, string name, string schema, int commandPosition)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			commandStringBuilder.Append("SELECT sql%ROWCOUNT").Append(base.SqlGenerationHelper.StatementTerminator).AppendLine()
				.AppendLine();
			return (ResultSetMapping)2;
		}

		public override void AppendBatchHeader(StringBuilder commandStringBuilder)
		{
			commandStringBuilder.AppendLine();
		}

		protected override void AppendIdentityWhereCondition(StringBuilder commandStringBuilder, IColumnModification columnModification)
		{
			base.SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, columnModification.ColumnName);
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
			base.AppendNextSequenceValueOperation(stringBuilder, name, schema);
			return stringBuilder.ToString();
		}

		public override void AppendNextSequenceValueOperation(StringBuilder commandStringBuilder, string name, string schema)
		{
			commandStringBuilder.Append("SELECT ");
			base.SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, name, schema);
			commandStringBuilder.Append(".NEXTVAL");
		}
	}
}
