// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Migrations.DmMigrationsSqlGenerator
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;



namespace Microsoft.EntityFrameworkCore.Migrations
{
  public class DmMigrationsSqlGenerator : MigrationsSqlGenerator
  {
    public DmMigrationsSqlGenerator(
      [NotNull] MigrationsSqlGeneratorDependencies dependencies,
      [NotNull] IMigrationsAnnotationProvider migrationsAnnotations)
      : base(dependencies)
    {
    }

    protected override void ColumnDefinition(
      AddColumnOperation operation,
      IModel model,
      MigrationCommandListBuilder builder)
    {
      base.ColumnDefinition(((ColumnOperation) operation).Schema, ((ColumnOperation) operation).Table, ((ColumnOperation) operation).Name, (ColumnOperation) operation, model, builder);
    }

    protected override void Generate(
      MigrationOperation operation,
      IModel model,
      MigrationCommandListBuilder builder)
    {
      Check.NotNull<MigrationOperation>(operation, nameof (operation));
      Check.NotNull<MigrationCommandListBuilder>(builder, nameof (builder));
      switch (operation)
      {
        case DmCreateUserOperation operation1:
          this.Generate(operation1, model, builder);
          break;
        case DmDropUserOperation operation2:
          this.Generate(operation2, model, builder);
          break;
        default:
          base.Generate(operation, model, builder);
          break;
      }
    }

    protected override void Generate(
      [NotNull] AlterDatabaseOperation operation,
      [CanBeNull] IModel model,
      [NotNull] MigrationCommandListBuilder builder)
    {
      throw new NotSupportedException("AlterDatabaseOperation does not support");
    }

    protected override void Generate(
      [NotNull] CreateSequenceOperation operation,
      [CanBeNull] IModel model,
      [NotNull] MigrationCommandListBuilder builder)
    {
      if (operation.ClrType != typeof (long))
        throw new NotSupportedException("CreateSequenceOperation only support long type");
      base.Generate(operation, model, builder);
    }

    protected override void Generate(
      AlterColumnOperation operation,
      IModel model,
      MigrationCommandListBuilder builder)
    {
      Check.NotNull<AlterColumnOperation>(operation, nameof (operation));
      Check.NotNull<MigrationCommandListBuilder>(builder, nameof (builder));
      IColumn icolumn;
      if (model == null)
      {
        icolumn = (IColumn) null;
      }
      else
      {
        ITable table = RelationalModelExtensions.GetRelationalModel(model).FindTable(((ColumnOperation) operation).Table, ((ColumnOperation) operation).Schema);
        icolumn = table != null ? table.Columns.FirstOrDefault<IColumn>((Func<IColumn, bool>) (c => ((IColumnBase) c).Name == ((ColumnOperation) operation).Name)) : (IColumn) null;
      }
      if (((ColumnOperation) operation).ComputedColumnSql != null)
      {
        DropColumnOperation dropColumnOperation = new DropColumnOperation()
        {
          Schema = ((ColumnOperation) operation).Schema,
          Table = ((ColumnOperation) operation).Table,
          Name = ((ColumnOperation) operation).Name
        };
        AddColumnOperation addColumnOperation1 = new AddColumnOperation();
        ((ColumnOperation) addColumnOperation1).Schema = ((ColumnOperation) operation).Schema;
        ((ColumnOperation) addColumnOperation1).Table = ((ColumnOperation) operation).Table;
        ((ColumnOperation) addColumnOperation1).Name = ((ColumnOperation) operation).Name;
        ((ColumnOperation) addColumnOperation1).ClrType = ((ColumnOperation) operation).ClrType;
        ((ColumnOperation) addColumnOperation1).ColumnType = ((ColumnOperation) operation).ColumnType;
        ((ColumnOperation) addColumnOperation1).IsUnicode = ((ColumnOperation) operation).IsUnicode;
        ((ColumnOperation) addColumnOperation1).MaxLength = ((ColumnOperation) operation).MaxLength;
        ((ColumnOperation) addColumnOperation1).IsRowVersion = ((ColumnOperation) operation).IsRowVersion;
        ((ColumnOperation) addColumnOperation1).IsNullable = ((ColumnOperation) operation).IsNullable;
        ((ColumnOperation) addColumnOperation1).DefaultValue = ((ColumnOperation) operation).DefaultValue;
        ((ColumnOperation) addColumnOperation1).DefaultValueSql = ((ColumnOperation) operation).DefaultValueSql;
        ((ColumnOperation) addColumnOperation1).ComputedColumnSql = ((ColumnOperation) operation).ComputedColumnSql;
        ((ColumnOperation) addColumnOperation1).IsFixedLength = ((ColumnOperation) operation).IsFixedLength;
        AddColumnOperation addColumnOperation2 = addColumnOperation1;
        ((AnnotatableBase) addColumnOperation2).AddAnnotations((IEnumerable<IAnnotation>) ((AnnotatableBase) operation).GetAnnotations());
        this.Generate(dropColumnOperation, model, builder, true);
        this.Generate(addColumnOperation2, model, builder, true);
      }
      else
      {
        DmValueGenerationStrategy? nullable1 = ((AnnotatableBase) operation)["Dm:ValueGenerationStrategy"] as DmValueGenerationStrategy?;
        DmValueGenerationStrategy generationStrategy1 = DmValueGenerationStrategy.IdentityColumn;
        bool flag = nullable1.GetValueOrDefault() == generationStrategy1 & nullable1.HasValue;
        if (this.IsOldColumnSupported(model))
        {
          DmValueGenerationStrategy? nullable2 = ((AnnotatableBase) operation.OldColumn)["Dm:ValueGenerationStrategy"] as DmValueGenerationStrategy?;
          DmValueGenerationStrategy generationStrategy2 = DmValueGenerationStrategy.IdentityColumn;
          if (nullable2.GetValueOrDefault() == generationStrategy2 & nullable2.HasValue && !flag)
            DmMigrationsSqlGenerator.DropIdentity(operation, builder);
          if (operation.OldColumn.DefaultValue != null || operation.OldColumn.DefaultValueSql != null && (((ColumnOperation) operation).DefaultValue == null || ((ColumnOperation) operation).DefaultValueSql == null))
            this.DropDefaultConstraint(((ColumnOperation) operation).Schema, ((ColumnOperation) operation).Table, ((ColumnOperation) operation).Name, builder);
        }
        else
        {
          if (!flag)
            DmMigrationsSqlGenerator.DropIdentity(operation, builder);
          if (((ColumnOperation) operation).DefaultValue == null && ((ColumnOperation) operation).DefaultValueSql == null)
            this.DropDefaultConstraint(((ColumnOperation) operation).Schema, ((ColumnOperation) operation).Table, ((ColumnOperation) operation).Name, builder);
        }
        builder.Append("ALTER TABLE ").Append(this.Dependencies.SqlGenerationHelper.DelimitIdentifier(((ColumnOperation) operation).Table, ((ColumnOperation) operation).Schema)).Append(" MODIFY ");
        base.ColumnDefinition(((ColumnOperation) operation).Schema, ((ColumnOperation) operation).Table, ((ColumnOperation) operation).Name, (ColumnOperation) operation, model, builder);
        builder.AppendLine(this.Dependencies.SqlGenerationHelper.StatementTerminator);
        builder.EndCommand(false);
      }
    }

    private static void DropIdentity(
      AlterColumnOperation operation,
      MigrationCommandListBuilder builder)
    {
      Check.NotNull<AlterColumnOperation>(operation, nameof (operation));
      Check.NotNull<MigrationCommandListBuilder>(builder, nameof (builder));
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(722, 4);
      interpolatedStringHandler.AppendLiteral("\nDECLARE\n   v_Count INTEGER;\nBEGIN\n SELECT\r\n        COUNT(*) INTO v_Count\r\nFROM\r\n        SYSCOLUMNS\r\nWHERE\r\n        ID =\r\n        (\r\n                SELECT\r\n                        ID\r\n                FROM\r\n                        SYSOBJECTS\r\n                WHERE\r\n                        NAME    ='");
      interpolatedStringHandler.AppendFormatted(((ColumnOperation) operation).Table);
      interpolatedStringHandler.AppendLiteral("'\r\n                    AND TYPE$   ='SCHOBJ'\r\n                    AND SUBTYPE$='UTAB'\r\n                    AND SCHID   =\r\n                        (\r\n                                SELECT ID FROM SYSOBJECTS WHERE NAME = '");
      interpolatedStringHandler.AppendFormatted(((ColumnOperation) operation).Schema);
      interpolatedStringHandler.AppendLiteral("' AND TYPE$='SCH'\r\n                        )\r\n        )\r\n    AND NAME         = '");
      interpolatedStringHandler.AppendFormatted(((ColumnOperation) operation).Name);
      interpolatedStringHandler.AppendLiteral("'\r\n    AND INFO2 & 0X01 = 1;\n  IF v_Count > 0 THEN\n    EXECUTE IMMEDIATE 'ALTER  TABLE \"");
      interpolatedStringHandler.AppendFormatted(((ColumnOperation) operation).Table);
      interpolatedStringHandler.AppendLiteral("\" DROP IDENTITY';\n  END IF;\nEND;");
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      builder.AppendLine(stringAndClear).EndCommand(false);
    }

    protected virtual void Generate(
      RenameIndexOperation operation,
      IModel model,
      MigrationCommandListBuilder builder)
    {
      Check.NotNull<RenameIndexOperation>(operation, nameof (operation));
      Check.NotNull<MigrationCommandListBuilder>(builder, nameof (builder));
      if (operation.NewName != null)
        builder.Append("ALTER INDEX ").Append(this.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name)).Append(" RENAME TO ").Append(this.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.NewName)).Append(this.Dependencies.SqlGenerationHelper.StatementTerminator);
      builder.EndCommand(false);
    }

    protected virtual void SequenceOptions(
      string schema,
      string name,
      SequenceOperation operation,
      IModel model,
      MigrationCommandListBuilder builder)
    {
      Check.NotEmpty(name, nameof (name));
      Check.NotNull<int>(operation.IncrementBy, "IncrementBy");
      Check.NotNull<bool>(operation.IsCyclic, "IsCyclic");
      Check.NotNull<MigrationCommandListBuilder>(builder, nameof (builder));
      RelationalTypeMapping mapping1 = RelationalTypeMappingSourceExtensions.GetMapping(this.Dependencies.TypeMappingSource, typeof (int));
      RelationalTypeMapping mapping2 = RelationalTypeMappingSourceExtensions.GetMapping(this.Dependencies.TypeMappingSource, typeof (long));
      builder.Append(" INCREMENT BY ").Append(mapping1.GenerateSqlLiteral((object) operation.IncrementBy));
      if (operation.MinValue.HasValue)
        builder.Append(" MINVALUE ").Append(mapping2.GenerateSqlLiteral((object) operation.MinValue));
      else
        builder.Append(" NOMINVALUE");
      if (operation.MaxValue.HasValue)
        builder.Append(" MAXVALUE ").Append(mapping2.GenerateSqlLiteral((object) operation.MaxValue));
      else
        builder.Append(" NOMAXVALUE");
      builder.Append(operation.IsCyclic ? " CYCLE" : " NOCYCLE");
    }

    protected virtual void Generate(
      RenameSequenceOperation operation,
      IModel model,
      MigrationCommandListBuilder builder)
    {
      throw new NotSupportedException("RenameSequenceOperation does not support");
    }

    protected virtual void Generate(
      [NotNull] RestartSequenceOperation operation,
      [CanBeNull] IModel model,
      [NotNull] MigrationCommandListBuilder builder)
    {
      throw new NotSupportedException("RestartSequenceOperation does not support");
    }

    protected virtual void Generate(
      RenameTableOperation operation,
      IModel model,
      MigrationCommandListBuilder builder)
    {
      Check.NotNull<RenameTableOperation>(operation, nameof (operation));
      Check.NotNull<MigrationCommandListBuilder>(builder, nameof (builder));
      if (operation.NewSchema != null)
        throw new NotSupportedException("RenameTableOperation does not support rename newschema");
      if (operation.NewName == null || !(operation.NewName != operation.Name))
        return;
      builder.Append("ALTER TABLE ").Append(operation.Schema != null ? this.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name, operation.Schema) : this.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name)).Append(" RENAME TO ").Append(this.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.NewName)).Append(this.Dependencies.SqlGenerationHelper.StatementTerminator).EndCommand(false);
    }

    protected virtual void Generate(
      EnsureSchemaOperation operation,
      IModel model,
      MigrationCommandListBuilder builder)
    {
      Check.NotNull<EnsureSchemaOperation>(operation, nameof (operation));
      Check.NotNull<MigrationCommandListBuilder>(builder, nameof (builder));
      MigrationCommandListBuilder commandListBuilder = builder;
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(321, 2);
      interpolatedStringHandler.AppendLiteral("DECLARE\r\n                    B BOOLEAN ;\r\n                  BEGIN\r\n                    SELECT COUNT(NAME) INTO B FROM SYSOBJECTS WHERE TYPE$= 'SCH' AND NAME = '");
      interpolatedStringHandler.AppendFormatted(operation.Name);
      interpolatedStringHandler.AppendLiteral("';\r\n                    IF B == 0 THEN\r\n                            EXECUTE IMMEDIATE 'CREATE SCHEMA \"");
      interpolatedStringHandler.AppendFormatted(operation.Name);
      interpolatedStringHandler.AppendLiteral("\" ';\r\n                    END IF;\r\n                    END;");
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      commandListBuilder.Append(stringAndClear).EndCommand(false);
    }

    protected virtual void Generate(
      [NotNull] DmCreateUserOperation operation,
      [CanBeNull] IModel model,
      [NotNull] MigrationCommandListBuilder builder)
    {
      Check.NotNull<DmCreateUserOperation>(operation, nameof (operation));
      Check.NotNull<MigrationCommandListBuilder>(builder, nameof (builder));
      MigrationCommandListBuilder commandListBuilder = builder;
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(179, 3);
      interpolatedStringHandler.AppendLiteral("BEGIN\n                             EXECUTE IMMEDIATE 'CREATE USER ");
      interpolatedStringHandler.AppendFormatted(operation.UserName);
      interpolatedStringHandler.AppendLiteral(" IDENTIFIED BY ");
      interpolatedStringHandler.AppendFormatted(operation.Password);
      interpolatedStringHandler.AppendLiteral("';\n                             EXECUTE IMMEDIATE 'GRANT DBA TO ");
      interpolatedStringHandler.AppendFormatted(operation.UserName);
      interpolatedStringHandler.AppendLiteral("';\n                           END;");
      string stringAndClear = interpolatedStringHandler.ToStringAndClear();
      commandListBuilder.Append(stringAndClear).EndCommand(true);
    }

    protected virtual void Generate(
      [NotNull] DmDropUserOperation operation,
      [CanBeNull] IModel model,
      [NotNull] MigrationCommandListBuilder builder)
    {
      Check.NotNull<DmDropUserOperation>(operation, nameof (operation));
      Check.NotNull<MigrationCommandListBuilder>(builder, nameof (builder));
      builder.Append("BEGIN\n                         EXECUTE IMMEDIATE 'DROP USER " + operation.UserName + " CASCADE';\n                       END;").EndCommand(true);
    }

    protected virtual void Generate(
      [NotNull] CreateIndexOperation operation,
      [CanBeNull] IModel model,
      [NotNull] MigrationCommandListBuilder builder,
      bool terminate = true)
    {
      if (operation.Filter != null)
        throw new NotSupportedException("CreateIndexOperation does not support filter clause");
      base.Generate(operation, model, builder, true);
    }

    protected void Generate(
      DropIndexOperation operation,
      IModel model,
      MigrationCommandListBuilder builder)
    {
      base.Generate(operation, model, builder, true);
    }

    protected virtual void Generate(
      [NotNull] DropIndexOperation operation,
      [CanBeNull] IModel model,
      [NotNull] MigrationCommandListBuilder builder,
      bool terminate)
    {
      Check.NotNull<DropIndexOperation>(operation, nameof (operation));
      Check.NotNull<MigrationCommandListBuilder>(builder, nameof (builder));
      builder.Append("DROP INDEX ").Append(this.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name));
      if (!terminate)
        return;
      builder.AppendLine(this.Dependencies.SqlGenerationHelper.StatementTerminator).EndCommand(false);
    }

    protected virtual void Generate(
      RenameColumnOperation operation,
      IModel model,
      MigrationCommandListBuilder builder)
    {
      Check.NotNull<RenameColumnOperation>(operation, nameof (operation));
      Check.NotNull<MigrationCommandListBuilder>(builder, nameof (builder));
      StringBuilder stringBuilder = new StringBuilder();
      if (operation.Schema != null)
        stringBuilder.Append(this.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Schema)).Append(".");
      stringBuilder.Append(this.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table));
      builder.Append("ALTER TABLE ").Append(stringBuilder.ToString()).Append(" ALTER COLUMN ").Append(this.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name)).Append(" RENAME TO ").Append(this.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.NewName)).Append(this.Dependencies.SqlGenerationHelper.StatementTerminator);
      builder.EndCommand(false);
    }

    protected virtual void Generate(
      InsertDataOperation operation,
      IModel model,
      MigrationCommandListBuilder builder,
      bool terminate = true)
    {
      Check.NotNull<InsertDataOperation>(operation, nameof (operation));
      Check.NotNull<MigrationCommandListBuilder>(builder, nameof (builder));
      builder.AppendLine("DECLARE").AppendLine("   CNT  INT;").AppendLine("BEGIN ");
      builder.Append(" SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END INTO CNT FROM ").Append(" SYSCOLUMNS WHERE ID = (SELECT ID FROM SYSOBJECTS WHERE TYPE$ = 'SCHOBJ' AND SUBTYPE$ = 'UTAB' AND ");
      if (operation.Schema != null)
        builder.Append(" SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND NAME = ").Append(RelationalTypeMappingSourceExtensions.GetMapping(this.Dependencies.TypeMappingSource, typeof (string)).GenerateSqlLiteral((object) operation.Schema)).Append(") AND ");
      else
        builder.Append(" SCHID = CURRENT_SCHID() AND ");
      builder.Append("NAME = ").Append(RelationalTypeMappingSourceExtensions.GetMapping(this.Dependencies.TypeMappingSource, typeof (string)).GenerateSqlLiteral((object) operation.Table)).AppendLine(" ) AND INFO2&0X01 = 1;");
      builder.AppendLine("IF CNT > 0 THEN ");
      using (builder.Indent())
        builder.Append("EXECUTE IMMEDIATE 'SET IDENTITY_INSERT ").Append(this.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" ON '").AppendLine(this.Dependencies.SqlGenerationHelper.StatementTerminator);
      builder.AppendLine("END IF; ");
      base.Generate(operation, model, builder, false);
      builder.AppendLine("IF CNT > 0 THEN ");
      using (builder.Indent())
        builder.Append("EXECUTE IMMEDIATE 'SET IDENTITY_INSERT ").Append(this.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" OFF'").AppendLine(this.Dependencies.SqlGenerationHelper.StatementTerminator);
      builder.AppendLine("END IF; ").AppendLine("END;");
      builder.EndCommand(false);
    }

    protected virtual void Generate(
      CreateTableOperation operation,
      IModel model,
      MigrationCommandListBuilder builder,
      bool terminate = true)
    {
      base.Generate(operation, model, builder, true);
      AddColumnOperation[] array = ((IEnumerable<AddColumnOperation>) operation.Columns).Where<AddColumnOperation>((Func<AddColumnOperation, bool>) (c => ((ColumnOperation) c).IsRowVersion)).ToArray<AddColumnOperation>();
      if (array.Length != 0)
      {
        builder.Append("CREATE OR REPLACE TRIGGER ").AppendLine(this.Dependencies.SqlGenerationHelper.DelimitIdentifier("rowversion_" + ((TableOperation) operation).Name, ((TableOperation) operation).Schema)).Append("BEFORE INSERT OR UPDATE ON ").AppendLine(this.Dependencies.SqlGenerationHelper.DelimitIdentifier(((TableOperation) operation).Name, ((TableOperation) operation).Schema)).AppendLine("FOR EACH ROW").AppendLine("BEGIN");
        foreach (AddColumnOperation addColumnOperation in array)
          builder.Append("  :NEW.").Append(this.Dependencies.SqlGenerationHelper.DelimitIdentifier(((ColumnOperation) addColumnOperation).Name)).Append(" := NVL(:OLD.").Append(this.Dependencies.SqlGenerationHelper.DelimitIdentifier(((ColumnOperation) addColumnOperation).Name)).Append(", '00000000') + 1").AppendLine(this.Dependencies.SqlGenerationHelper.StatementTerminator);
        builder.Append("END").AppendLine(this.Dependencies.SqlGenerationHelper.StatementTerminator);
      }
      this.EndStatement(builder, false);
    }

    protected virtual void UniqueConstraint(
      [NotNull] AddUniqueConstraintOperation operation,
      [CanBeNull] IModel model,
      [NotNull] MigrationCommandListBuilder builder)
    {
      Check.NotNull<AddUniqueConstraintOperation>(operation, nameof (operation));
      Check.NotNull<MigrationCommandListBuilder>(builder, nameof (builder));
      if (operation.Name == null)
        operation.Name = Guid.NewGuid().ToString();
      base.UniqueConstraint(operation, model, builder);
    }

        protected override void ColumnDefinition([JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.NotNull] string name, ColumnOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
        {
            Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
            Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
            Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation.ClrType, "ClrType");
            Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
            if (operation.ComputedColumnSql != null)
            {
                builder.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(name)).Append(" AS (").Append(operation.ComputedColumnSql)
                    .Append(")");
                return;
            }
            builder.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(name)).Append(" ").Append(operation.ColumnType ?? GetColumnType(schema, table, name, operation, model));
            string text = operation["Dm:Identity"] as string;
            if (text != null || operation["Dm:ValueGenerationStrategy"] as DmValueGenerationStrategy? == DmValueGenerationStrategy.IdentityColumn)
            {
                builder.Append(" IDENTITY");
                if (!string.IsNullOrEmpty(text) && text != "1, 1")
                {
                    builder.Append("(").Append(text).Append(")");
                }
            }
            else
            {
                DefaultValue(operation.DefaultValue, operation.DefaultValueSql, operation.ColumnType, builder);
            }
            builder.Append(operation.IsNullable ? " NULL" : " NOT NULL");
        }

        protected virtual void PrimaryKeyConstraint(
      [NotNull] AddPrimaryKeyOperation operation,
      [CanBeNull] IModel model,
      [NotNull] MigrationCommandListBuilder builder)
    {
      Check.NotNull<AddPrimaryKeyOperation>(operation, nameof (operation));
      Check.NotNull<MigrationCommandListBuilder>(builder, nameof (builder));
      if (operation.Name == null)
        operation.Name = Guid.NewGuid().ToString();
      base.PrimaryKeyConstraint(operation, model, builder);
    }

    protected virtual void ForeignKeyConstraint(
      AddForeignKeyOperation operation,
      IModel model,
      MigrationCommandListBuilder builder)
    {
      Check.NotNull<AddForeignKeyOperation>(operation, nameof (operation));
      Check.NotNull<MigrationCommandListBuilder>(builder, nameof (builder));
      if (operation.PrincipalColumns == null)
        throw new NotSupportedException("AddForeignKeyOperation does not support references columns is null");
      if (operation.Name == null)
        operation.Name = Guid.NewGuid().ToString();
      base.ForeignKeyConstraint(operation, model, builder);
    }

    protected virtual void DropDefaultConstraint(
      [CanBeNull] string schema,
      [NotNull] string tableName,
      [NotNull] string columnName,
      [NotNull] MigrationCommandListBuilder builder)
    {
      Check.NotEmpty(tableName, nameof (tableName));
      Check.NotEmpty(columnName, nameof (columnName));
      Check.NotNull<MigrationCommandListBuilder>(builder, nameof (builder));
      builder.Append("ALTER TABLE ").Append(this.Dependencies.SqlGenerationHelper.DelimitIdentifier(tableName, schema)).Append(" MODIFY ").Append(this.Dependencies.SqlGenerationHelper.DelimitIdentifier(columnName)).Append(" DEFAULT NULL").AppendLine(this.Dependencies.SqlGenerationHelper.StatementTerminator).EndCommand(false);
    }
  }
}
