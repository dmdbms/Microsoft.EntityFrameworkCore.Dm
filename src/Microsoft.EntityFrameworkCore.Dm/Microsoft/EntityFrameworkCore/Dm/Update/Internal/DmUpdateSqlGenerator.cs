// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Update.Internal.DmUpdateSqlGenerator
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;



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

    public virtual ResultSetMapping AppendBulkInsertOperation(
      StringBuilder commandStringBuilder,
      IReadOnlyList<IReadOnlyModificationCommand> modificationCommands,
      int commandPosition)
    {
      if (((IReadOnlyCollection<IReadOnlyModificationCommand>) modificationCommands).Count == 1 && ((IEnumerable<IColumnModification>) modificationCommands[0].ColumnModifications).All<IColumnModification>((Func<IColumnModification, bool>) (o =>
      {
        if (!o.IsKey || !o.IsRead)
          return true;
        IProperty property = o.Property;
        return property != null && ((IReadOnlyProperty) property).GetValueGenerationStrategy() == DmValueGenerationStrategy.IdentityColumn;
      })))
        return this.AppendInsertOperation(commandStringBuilder, modificationCommands[0], commandPosition);
      List<IColumnModification> list1 = ((IEnumerable<IColumnModification>) modificationCommands[0].ColumnModifications).Where<IColumnModification>((Func<IColumnModification, bool>) (o => o.IsRead)).ToList<IColumnModification>();
      List<IColumnModification> list2 = ((IEnumerable<IColumnModification>) modificationCommands[0].ColumnModifications).Where<IColumnModification>((Func<IColumnModification, bool>) (o => o.IsWrite)).ToList<IColumnModification>();
      List<IColumnModification> list3 = ((IEnumerable<IColumnModification>) modificationCommands[0].ColumnModifications).Where<IColumnModification>((Func<IColumnModification, bool>) (o => o.IsKey)).ToList<IColumnModification>();
      bool flag = list2.Count == 0;
      List<IColumnModification> list4 = ((IEnumerable<IColumnModification>) modificationCommands[0].ColumnModifications).Where<IColumnModification>((Func<IColumnModification, bool>) (o =>
      {
        IProperty property = o.Property;
        return property == null || ((IReadOnlyProperty) property).GetValueGenerationStrategy() != DmValueGenerationStrategy.IdentityColumn;
      })).ToList<IColumnModification>();
      List<IColumnModification> list5 = ((IEnumerable<IColumnModification>) modificationCommands[0].ColumnModifications).Where<IColumnModification>((Func<IColumnModification, bool>) (o => o.IsKey)).Where<IColumnModification>((Func<IColumnModification, bool>) (o => !o.IsWrite)).ToList<IColumnModification>();
      if (flag)
      {
        if (list4.Count == 0 || list1.Count == 0)
        {
          foreach (IReadOnlyModificationCommand modificationCommand in (IEnumerable<IReadOnlyModificationCommand>) modificationCommands)
            this.AppendInsertOperation(commandStringBuilder, modificationCommand, commandPosition);
          return list1.Count == 0 ? (ResultSetMapping) 0 : (ResultSetMapping) 2;
        }
        if (list4.Count > 1)
          list4.RemoveRange(1, list4.Count - 1);
      }
      if (list1.Count == 0)
        return this.AppendBulkInsertWithoutServerValues(commandStringBuilder, modificationCommands, list2);
      return flag ? this.AppendBulkInsertWithServerValuesOnly(commandStringBuilder, modificationCommands, commandPosition, list4, list3, list1, list5) : this.AppendBulkInsertWithServerValues(commandStringBuilder, modificationCommands, commandPosition, list2, list3, list1, list5);
    }

    private ResultSetMapping AppendBulkInsertWithoutServerValues(
      StringBuilder commandStringBuilder,
      IReadOnlyList<IReadOnlyModificationCommand> modificationCommands,
      List<IColumnModification> writeOperations)
    {
      Debug.Assert(writeOperations.Count > 0);
      string tableName = modificationCommands[0].TableName;
      string schema = modificationCommands[0].Schema;
      this.AppendInsertCommandHeader(commandStringBuilder, tableName, schema, (IReadOnlyList<IColumnModification>) writeOperations);
      this.AppendValuesHeader(commandStringBuilder, (IReadOnlyList<IColumnModification>) writeOperations);
      this.AppendValues(commandStringBuilder, (IReadOnlyList<IColumnModification>) writeOperations, (string) null);
      for (int index = 1; index < ((IReadOnlyCollection<IReadOnlyModificationCommand>) modificationCommands).Count; ++index)
      {
        commandStringBuilder.Append(",").AppendLine();
        this.AppendValues(commandStringBuilder, (IReadOnlyList<IColumnModification>) ((IEnumerable<IColumnModification>) modificationCommands[index].ColumnModifications).Where<IColumnModification>((Func<IColumnModification, bool>) (o => o.IsWrite)).ToList<IColumnModification>(), (string) null);
      }
      commandStringBuilder.Append(this.SqlGenerationHelper.StatementTerminator).AppendLine();
      return (ResultSetMapping) 0;
    }

    private void AppandTempArrayInit(
      StringBuilder commandStringBuilder,
      IReadOnlyList<IReadOnlyModificationCommand> modificationCommands,
      int commandPosition)
    {
      commandStringBuilder.Append("c").Append(commandPosition).Append(" = NEW rrr").Append(commandPosition).Append("[").Append(((IReadOnlyCollection<IReadOnlyModificationCommand>) modificationCommands).Count.ToString()).AppendLine("];");
      int num;
      for (int i = 0; i < ((IReadOnlyCollection<IReadOnlyModificationCommand>) modificationCommands).Count; num = i++)
      {
        commandStringBuilder.AppendJoin<IColumnModification, ISqlGenerationHelper>((IEnumerable<IColumnModification>) modificationCommands[i].ColumnModifications, this.SqlGenerationHelper, (Action<StringBuilder, IColumnModification, ISqlGenerationHelper>) ((sb, o, helper) =>
        {
          if (!o.IsWrite)
            return;
          sb.Append("c").Append(commandPosition).Append("[").Append((i + 1).ToString()).Append("].");
          helper.DelimitIdentifier(sb, o.ColumnName);
          sb.Append(" = ");
          helper.GenerateParameterName(sb, o.ParameterName);
          sb.Append(" ;\n");
        }), "");
        StringBuilder stringBuilder = commandStringBuilder.Append("c").Append(commandPosition).Append("[");
        num = i + 1;
        string str = num.ToString();
        stringBuilder.Append(str).Append("].\"").Append("_Position").Append("\" = ").Append(i).AppendLine(";");
      }
    }

    private void AppendSelectIdentity(
      StringBuilder commandStringBuilder,
      IReadOnlyList<IReadOnlyModificationCommand> modificationCommands,
      List<IColumnModification> nonwrite_keys,
      int commandPosition)
    {
      string tableName = modificationCommands[0].TableName;
      string schema = modificationCommands[0].Schema;
      commandStringBuilder.Append("SELECT ").AppendJoin<IColumnModification, ISqlGenerationHelper>((IEnumerable<IColumnModification>) nonwrite_keys, this.SqlGenerationHelper, (Action<StringBuilder, IColumnModification, ISqlGenerationHelper>) ((sb, o, helper) =>
      {
        sb.Append("MAX(");
        helper.DelimitIdentifier(sb, o.ColumnName);
        sb.Append(")");
      }), ",").Append(" INTO ").AppendJoin<IColumnModification, ISqlGenerationHelper>((IEnumerable<IColumnModification>) nonwrite_keys, this.SqlGenerationHelper, (Action<StringBuilder, IColumnModification, ISqlGenerationHelper>) ((sb, o, helper) => helper.DelimitIdentifier(sb, "V_" + o.ColumnName)), ",").Append(" FROM ");
      this.SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, tableName, schema);
      commandStringBuilder.AppendLine(";");
    }

    private ResultSetMapping AppendBulkInsertWithServerValues(
      StringBuilder commandStringBuilder,
      IReadOnlyList<IReadOnlyModificationCommand> modificationCommands,
      int commandPosition,
      List<IColumnModification> writeOperations,
      List<IColumnModification> keyOperations,
      List<IColumnModification> readOperations,
      List<IColumnModification> nonwrite_keys)
    {
      this.AppendDeclareTable(commandStringBuilder, "@inserted", commandPosition, (IReadOnlyList<IColumnModification>) writeOperations, (IReadOnlyList<IColumnModification>) nonwrite_keys, "\"_Position\" int");
      string tableName = modificationCommands[0].TableName;
      string schema = modificationCommands[0].Schema;
      IReadOnlyList<IColumnModification> columnModifications = modificationCommands[0].ColumnModifications;
      commandStringBuilder.AppendLine("BEGIN");
      this.AppandTempArrayInit(commandStringBuilder, modificationCommands, commandPosition);
      if (nonwrite_keys != null && nonwrite_keys.Count > 0)
        this.AppendSelectIdentity(commandStringBuilder, modificationCommands, nonwrite_keys, commandPosition);
      this.AppendInsertCommandHeader(commandStringBuilder, tableName, schema, (IReadOnlyList<IColumnModification>) writeOperations);
      this.AppendInsertSelect(commandStringBuilder, tableName, schema, (IReadOnlyList<IColumnModification>) writeOperations, commandPosition);
      if (nonwrite_keys != null && nonwrite_keys.Count > 0)
        this.AppendSelectCommand(commandStringBuilder, (IReadOnlyList<IColumnModification>) readOperations, (IReadOnlyList<IColumnModification>) writeOperations, (IReadOnlyList<IColumnModification>) nonwrite_keys, "@inserted", commandPosition, tableName, schema, "_Position");
      else
        this.AppendSelectCommand(commandStringBuilder, (IReadOnlyList<IColumnModification>) readOperations, (IReadOnlyList<IColumnModification>) keyOperations, (IReadOnlyList<IColumnModification>) nonwrite_keys, "@inserted", commandPosition, tableName, schema, "_Position");
      commandStringBuilder.AppendLine(" END;");
      return (ResultSetMapping) 1;
    }

    private ResultSetMapping AppendBulkInsertWithServerValuesOnly(
      StringBuilder commandStringBuilder,
      IReadOnlyList<IReadOnlyModificationCommand> modificationCommands,
      int commandPosition,
      List<IColumnModification> nonIdentityOperations,
      List<IColumnModification> keyOperations,
      List<IColumnModification> readOperations,
      List<IColumnModification> nonwrite_keys)
    {
      this.AppendDeclareTable(commandStringBuilder, "@inserted", commandPosition, (IReadOnlyList<IColumnModification>) keyOperations, (IReadOnlyList<IColumnModification>) nonwrite_keys);
      string tableName = modificationCommands[0].TableName;
      string schema = modificationCommands[0].Schema;
      commandStringBuilder.AppendLine(" BEGIN");
      this.AppendSelectIdentity(commandStringBuilder, modificationCommands, nonwrite_keys, commandPosition);
      this.AppendInsertCommandHeader(commandStringBuilder, tableName, schema, (IReadOnlyList<IColumnModification>) nonIdentityOperations);
      this.AppendValuesHeader(commandStringBuilder, (IReadOnlyList<IColumnModification>) nonIdentityOperations);
      this.AppendValues(commandStringBuilder, (IReadOnlyList<IColumnModification>) nonIdentityOperations, (string) null);
      for (int index = 1; index < ((IReadOnlyCollection<IReadOnlyModificationCommand>) modificationCommands).Count; ++index)
      {
        commandStringBuilder.Append(",").AppendLine();
        this.AppendValues(commandStringBuilder, (IReadOnlyList<IColumnModification>) nonIdentityOperations, (string) null);
      }
      commandStringBuilder.Append(this.SqlGenerationHelper.StatementTerminator);
      this.AppendSelectCommand(commandStringBuilder, (IReadOnlyList<IColumnModification>) readOperations, (IReadOnlyList<IColumnModification>) keyOperations, (IReadOnlyList<IColumnModification>) nonwrite_keys, "@inserted", commandPosition, tableName, schema);
      commandStringBuilder.AppendLine(" END;");
      return (ResultSetMapping) 1;
    }

    private void AppendMergeCommandHeader(
      [NotNull] StringBuilder commandStringBuilder,
      [NotNull] string name,
      [CanBeNull] string schema,
      [NotNull] string toInsertTableAlias,
      [NotNull] IReadOnlyList<ModificationCommand> modificationCommands,
      [NotNull] IReadOnlyList<ColumnModification> writeOperations,
      string additionalColumns = null)
    {
      commandStringBuilder.Append("MERGE ");
      this.SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, name, schema);
      commandStringBuilder.Append(" USING (");
      this.AppendValuesHeader(commandStringBuilder, (IReadOnlyList<IColumnModification>) writeOperations);
      this.AppendValues(commandStringBuilder, (IReadOnlyList<IColumnModification>) writeOperations, "0");
      for (int index = 1; index < ((IReadOnlyCollection<ModificationCommand>) modificationCommands).Count; ++index)
      {
        commandStringBuilder.Append(",").AppendLine();
        this.AppendValues(commandStringBuilder, (IReadOnlyList<IColumnModification>) ((IEnumerable<IColumnModification>) modificationCommands[index].ColumnModifications).Where<IColumnModification>((Func<IColumnModification, bool>) (o => o.IsWrite)).ToList<IColumnModification>(), index.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
      commandStringBuilder.Append(") AS ").Append(toInsertTableAlias).Append(" (").AppendJoin<ColumnModification, ISqlGenerationHelper>((IEnumerable<ColumnModification>) writeOperations, this.SqlGenerationHelper, (Action<StringBuilder, ColumnModification, ISqlGenerationHelper>) ((sb, o, helper) => helper.DelimitIdentifier(sb, o.ColumnName)));
      if (additionalColumns != null)
        commandStringBuilder.Append(", ").Append(additionalColumns);
      commandStringBuilder.Append(")").AppendLine(" ON 1=0").AppendLine("WHEN NOT MATCHED THEN");
      commandStringBuilder.Append("INSERT ").Append("(").AppendJoin<ColumnModification, ISqlGenerationHelper>((IEnumerable<ColumnModification>) writeOperations, this.SqlGenerationHelper, (Action<StringBuilder, ColumnModification, ISqlGenerationHelper>) ((sb, o, helper) => helper.DelimitIdentifier(sb, o.ColumnName))).Append(")");
      this.AppendValuesHeader(commandStringBuilder, (IReadOnlyList<IColumnModification>) writeOperations);
      commandStringBuilder.Append("(").AppendJoin<ColumnModification, string, ISqlGenerationHelper>((IEnumerable<ColumnModification>) writeOperations, toInsertTableAlias, this.SqlGenerationHelper, (Action<StringBuilder, ColumnModification, string, ISqlGenerationHelper>) ((sb, o, alias, helper) =>
      {
        sb.Append(alias).Append(".");
        helper.DelimitIdentifier(sb, o.ColumnName);
      })).Append(")");
    }

    private void AppendValues(
      StringBuilder commandStringBuilder,
      IReadOnlyList<IColumnModification> operations,
      string additionalLiteral)
    {
      if (((IReadOnlyCollection<IColumnModification>) operations).Count <= 0)
        return;
      commandStringBuilder.Append("(").AppendJoin<IColumnModification, ISqlGenerationHelper>((IEnumerable<IColumnModification>) operations, this.SqlGenerationHelper, (Action<StringBuilder, IColumnModification, ISqlGenerationHelper>) ((sb, o, helper) =>
      {
        if (o.IsWrite)
          helper.GenerateParameterName(sb, o.ParameterName);
        else if (RelationalPropertyExtensions.GetDefaultValueSql((IReadOnlyProperty) o.Property) != null)
        {
          string str = RelationalPropertyExtensions.GetDefaultValueSql((IReadOnlyProperty) o.Property).ToString();
          sb.Append(str);
        }
        else
          sb.Append("DEFAULT");
      }));
      if (additionalLiteral != null)
        commandStringBuilder.Append(", ").Append(additionalLiteral);
      commandStringBuilder.Append(")");
    }

    private void AppendDeclareTable(
      StringBuilder commandStringBuilder,
      string name,
      int index,
      IReadOnlyList<IColumnModification> operations,
      IReadOnlyList<IColumnModification> nonwrite_keys,
      string additionalColumns = null)
    {
      commandStringBuilder.Append("DECLARE ");
      if (operations != null && ((IReadOnlyCollection<IColumnModification>) operations).Count > 0 && ((IEnumerable<IColumnModification>) operations).Any<IColumnModification>((Func<IColumnModification, bool>) (o => o.IsWrite)))
      {
        commandStringBuilder.Append(" TYPE rrr").Append(index).Append(" IS RECORD (").AppendJoin<IColumnModification, DmUpdateSqlGenerator>((IEnumerable<IColumnModification>) operations, this, (Action<StringBuilder, IColumnModification, DmUpdateSqlGenerator>) ((sb, o, generator) =>
        {
          if (!o.IsWrite)
            return;
          generator.SqlGenerationHelper.DelimitIdentifier(sb, o.ColumnName);
          if (generator.GetTypeNameForCopy(o.Property).Equals("INTEGER identity(1, 1)"))
            sb.Append(" ").Append("integer");
          else
            sb.Append(" ").Append(generator.GetTypeNameForCopy(o.Property));
        }));
        if (additionalColumns != null)
          commandStringBuilder.Append(", ").Append(additionalColumns);
        commandStringBuilder.Append(")").Append(this.SqlGenerationHelper.StatementTerminator).AppendLine();
        commandStringBuilder.Append("TYPE ccc").Append(index).Append(" IS ARRAY rrr").Append(index).AppendLine("[];").Append("c").Append(index).Append(" ccc").Append(index).AppendLine("; ");
      }
      if (nonwrite_keys == null || ((IReadOnlyCollection<IColumnModification>) nonwrite_keys).Count <= 0)
        return;
      commandStringBuilder.AppendJoin<IColumnModification, DmUpdateSqlGenerator>((IEnumerable<IColumnModification>) nonwrite_keys, this, (Action<StringBuilder, IColumnModification, DmUpdateSqlGenerator>) ((sb, o, generator) =>
      {
        generator.SqlGenerationHelper.DelimitIdentifier(sb, "V_" + o.ColumnName);
        if (generator.GetTypeNameForCopy(o.Property).Equals("INTEGER identity(1, 1)"))
          sb.Append(" ").Append("integer");
        else
          sb.Append(" ").Append(generator.GetTypeNameForCopy(o.Property));
      }), ";\n").Append(";\n");
    }

    private string GetTypeNameForCopy(IProperty property)
    {
      string str = RelationalPropertyExtensions.GetColumnType(property);
      if (str == null)
      {
        IProperty firstPrincipal = property.FindFirstPrincipal();
        str = (firstPrincipal != null ? RelationalPropertyExtensions.GetColumnType(firstPrincipal) : (string) null) ?? this.Dependencies.TypeMappingSource.FindMapping(((IReadOnlyPropertyBase) property).ClrType)?.StoreType;
      }
      return ((IReadOnlyPropertyBase) property).ClrType == typeof (byte[]) && str != null && (str.Equals("rowversion", StringComparison.OrdinalIgnoreCase) || str.Equals("timestamp", StringComparison.OrdinalIgnoreCase)) ? (((IReadOnlyProperty) property).IsNullable ? "varbinary(8)" : "binary(8)") : str;
    }

    private void AppendOutputClause(
      StringBuilder commandStringBuilder,
      IReadOnlyList<ColumnModification> operations,
      string tableName,
      int tableIndex,
      string additionalColumns = null)
    {
      commandStringBuilder.AppendLine().Append("OUTPUT ").AppendJoin<ColumnModification, ISqlGenerationHelper>((IEnumerable<ColumnModification>) operations, this.SqlGenerationHelper, (Action<StringBuilder, ColumnModification, ISqlGenerationHelper>) ((sb, o, helper) =>
      {
        sb.Append("INSERTED.");
        helper.DelimitIdentifier(sb, o.ColumnName);
      }));
      if (additionalColumns != null)
        commandStringBuilder.Append(", ").Append(additionalColumns);
      commandStringBuilder.AppendLine().Append("INTO ").Append(tableName).Append(tableIndex);
    }

    private ResultSetMapping AppendInsertOperationWithServerKeys(
      StringBuilder commandStringBuilder,
      ModificationCommand command,
      IReadOnlyList<ColumnModification> keyOperations,
      IReadOnlyList<ColumnModification> readOperations,
      IReadOnlyList<ColumnModification> nonwrite_keys,
      int commandPosition)
    {
      string tableName = command.TableName;
      string schema = command.Schema;
      List<IColumnModification> list = ((IEnumerable<IColumnModification>) command.ColumnModifications).Where<IColumnModification>((Func<IColumnModification, bool>) (o => o.IsWrite)).ToList<IColumnModification>();
      this.AppendDeclareTable(commandStringBuilder, "@inserted", commandPosition, (IReadOnlyList<IColumnModification>) keyOperations, (IReadOnlyList<IColumnModification>) nonwrite_keys);
      commandStringBuilder.AppendLine("BEGIN");
      this.AppendInsertCommandHeader(commandStringBuilder, tableName, schema, (IReadOnlyList<IColumnModification>) list);
      this.AppendOutputClause(commandStringBuilder, keyOperations, "@inserted", commandPosition);
      this.AppendValuesHeader(commandStringBuilder, (IReadOnlyList<IColumnModification>) list);
      this.AppendValues(commandStringBuilder, (IReadOnlyList<IColumnModification>) list, (string) null);
      commandStringBuilder.Append(this.SqlGenerationHelper.StatementTerminator);
      return this.AppendSelectCommand(commandStringBuilder, (IReadOnlyList<IColumnModification>) readOperations, (IReadOnlyList<IColumnModification>) keyOperations, (IReadOnlyList<IColumnModification>) null, "@inserted", commandPosition, tableName, schema);
    }

    protected virtual void AppendInsertSelect(
      [NotNull] StringBuilder commandStringBuilder,
      [NotNull] string name,
      [CanBeNull] string schema,
      [NotNull] IReadOnlyList<IColumnModification> operations,
      int commandPosition)
    {
      commandStringBuilder.Append("SELECT ");
      commandStringBuilder.AppendJoin<IColumnModification, ISqlGenerationHelper>((IEnumerable<IColumnModification>) operations, this.SqlGenerationHelper, (Action<StringBuilder, IColumnModification, ISqlGenerationHelper>) ((sb, o, helper) => helper.DelimitIdentifier(sb, o.ColumnName)));
      commandStringBuilder.Append(" FROM ARRAY C").Append(commandPosition).Append(";");
    }

    private ResultSetMapping AppendSelectCommand(
      StringBuilder commandStringBuilder,
      IReadOnlyList<IColumnModification> readOperations,
      IReadOnlyList<IColumnModification> keyOperations,
      IReadOnlyList<IColumnModification> nonwrite_keyOperations,
      string insertedTableName,
      int insertedTableIndex,
      string tableName,
      string schema,
      string orderColumn = null)
    {
      bool flag = false;
      commandStringBuilder.AppendLine().Append("SELECT ").AppendJoin<IColumnModification, ISqlGenerationHelper>((IEnumerable<IColumnModification>) readOperations, this.SqlGenerationHelper, (Action<StringBuilder, IColumnModification, ISqlGenerationHelper>) ((sb, o, helper) => helper.DelimitIdentifier(sb, o.ColumnName, "t"))).Append(" FROM ");
      this.SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, tableName, schema);
      commandStringBuilder.Append(" \"t\"").AppendLine();
      if (((IReadOnlyCollection<IColumnModification>) keyOperations).Count > 0 && ((IEnumerable<IColumnModification>) keyOperations).Any<IColumnModification>((Func<IColumnModification, bool>) (o => o.IsWrite)))
      {
        commandStringBuilder.Append(" WHERE EXISTS(SELECT * FROM ").Append("ARRAY c").Append(insertedTableIndex).Append(" \"i\"").Append(" WHERE ").AppendJoin<IColumnModification>((IEnumerable<IColumnModification>) keyOperations, (Action<StringBuilder, IColumnModification>) ((sb, c) =>
        {
          sb.Append("(");
          if (((IReadOnlyPropertyBase) c.Property).ClrType == typeof (string))
          {
            sb.Append("TEXT_EQUAL(");
            this.SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "t");
            sb.Append(", ");
            this.SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "i");
            sb.Append(")");
          }
          else if (((IReadOnlyPropertyBase) c.Property).ClrType == typeof (byte[]))
          {
            sb.Append("BLOB_EQUAL(");
            this.SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "t");
            sb.Append(", ");
            this.SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "i");
            sb.Append(")");
          }
          else
          {
            this.SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "t");
            sb.Append(" = ");
            this.SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "i");
          }
          sb.Append(" OR (");
          this.SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "t");
          sb.Append(" IS NULL AND ");
          this.SqlGenerationHelper.DelimitIdentifier(sb, c.ColumnName, "i");
          sb.Append(" IS NULL )) ");
        }), " AND ").Append(")");
        flag = true;
      }
      if (nonwrite_keyOperations != null && ((IReadOnlyCollection<IColumnModification>) nonwrite_keyOperations).Count > 0)
      {
        if (flag)
          commandStringBuilder.Append(" AND ");
        else
          commandStringBuilder.Append(" WHERE ");
        commandStringBuilder.AppendJoin<IColumnModification, ISqlGenerationHelper>((IEnumerable<IColumnModification>) nonwrite_keyOperations, this.SqlGenerationHelper, (Action<StringBuilder, IColumnModification, ISqlGenerationHelper>) ((sb, o, helper) =>
        {
          sb.Append("( ");
          helper.DelimitIdentifier(sb, o.ColumnName, "t");
          sb.Append(" > ");
          helper.DelimitIdentifier(sb, "V_" + o.ColumnName);
          sb.Append(" OR ");
          helper.DelimitIdentifier(sb, "V_" + o.ColumnName);
          sb.Append(" IS NULL) ");
        }), " AND ");
      }
      if (orderColumn != null)
      {
        commandStringBuilder.AppendLine().Append("ORDER BY ");
        this.SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, "ROWID", "t");
      }
      commandStringBuilder.Append(this.SqlGenerationHelper.StatementTerminator).AppendLine().AppendLine();
      return (ResultSetMapping) 2;
    }

    protected override ResultSetMapping AppendSelectAffectedCountCommand(
      StringBuilder commandStringBuilder,
      string name,
      string schema,
      int commandPosition)
    {
      commandStringBuilder.Append("SELECT sql%ROWCOUNT").Append(this.SqlGenerationHelper.StatementTerminator).AppendLine().AppendLine();
      return (ResultSetMapping) 2;
    }

    public override void AppendBatchHeader(StringBuilder commandStringBuilder) => commandStringBuilder.AppendLine();

    protected override void AppendIdentityWhereCondition(
      StringBuilder commandStringBuilder,
      IColumnModification columnModification)
    {
      this.SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, columnModification.ColumnName);
      commandStringBuilder.Append(" = ");
      commandStringBuilder.Append("scope_identity()");
    }

    protected override void AppendRowsAffectedWhereCondition(
      StringBuilder commandStringBuilder,
      int expectedRowsAffected)
    {
      commandStringBuilder.Append("sql%ROWCOUNT = ").Append(expectedRowsAffected.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public override string GenerateNextSequenceValueOperation(string name, string schema)
    {
      StringBuilder stringBuilder = new StringBuilder();
      base.AppendNextSequenceValueOperation(stringBuilder, name, schema);
      return stringBuilder.ToString();
    }

    public override void AppendNextSequenceValueOperation(
      StringBuilder commandStringBuilder,
      string name,
      string schema)
    {
      commandStringBuilder.Append("SELECT ");
      this.SqlGenerationHelper.DelimitIdentifier(commandStringBuilder, name, schema);
      commandStringBuilder.Append(".NEXTVAL");
    }
  }
}
