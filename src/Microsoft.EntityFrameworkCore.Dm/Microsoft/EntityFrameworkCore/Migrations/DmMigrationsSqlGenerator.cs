using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations
{
	public class DmMigrationsSqlGenerator : MigrationsSqlGenerator
	{
		public DmMigrationsSqlGenerator([NotNull] MigrationsSqlGeneratorDependencies dependencies, [NotNull] IMigrationsAnnotationProvider migrationsAnnotations)
			: base(dependencies)
		{
		}

		protected override void ColumnDefinition(AddColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			base.ColumnDefinition(((ColumnOperation)operation).Schema, ((ColumnOperation)operation).Table, ((ColumnOperation)operation).Name, (ColumnOperation)(object)operation, model, builder);
		}

		protected override void Generate(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			Check.NotNull<MigrationOperation>(operation, "operation");
			Check.NotNull<MigrationCommandListBuilder>(builder, "builder");
			DmCreateUserOperation dmCreateUserOperation = operation as DmCreateUserOperation;
			if (dmCreateUserOperation != null)
			{
				Generate(dmCreateUserOperation, model, builder);
				return;
			}
			DmDropUserOperation dmDropUserOperation = operation as DmDropUserOperation;
			if (dmDropUserOperation != null)
			{
				Generate(dmDropUserOperation, model, builder);
			}
			else
			{
				base.Generate(operation, model, builder);
			}
		}

		protected override void Generate([NotNull] AlterDatabaseOperation operation, [CanBeNull] IModel model, [NotNull] MigrationCommandListBuilder builder)
		{
			throw new NotSupportedException("AlterDatabaseOperation does not support");
		}

		protected override void Generate([NotNull] CreateSequenceOperation operation, [CanBeNull] IModel model, [NotNull] MigrationCommandListBuilder builder)
		{
			if (operation.ClrType != typeof(long))
			{
				throw new NotSupportedException("CreateSequenceOperation only support long type");
			}
			base.Generate(operation, model, builder);
		}

		protected override void Generate(AlterColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Expected O, but got Unknown
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Expected O, but got Unknown
			AlterColumnOperation operation2 = operation;
			Check.NotNull<AlterColumnOperation>(operation2, "operation");
			Check.NotNull<MigrationCommandListBuilder>(builder, "builder");
			object obj;
			if (model == null)
			{
				obj = null;
			}
			else
			{
				ITable obj2 = RelationalModelExtensions.GetRelationalModel(model).FindTable(((ColumnOperation)operation2).Table, ((ColumnOperation)operation2).Schema);
				obj = ((obj2 != null) ? obj2.Columns.FirstOrDefault((Func<IColumn, bool>)((IColumn c) => ((IColumnBase)c).Name == ((ColumnOperation)operation2).Name)) : null);
			}
			IColumn val = (IColumn)obj;
			if (((ColumnOperation)operation2).ComputedColumnSql != null)
			{
				DropColumnOperation val2 = new DropColumnOperation();
				val2.Schema=(((ColumnOperation)operation2).Schema);
				val2.Table=(((ColumnOperation)operation2).Table);
				val2.Name=(((ColumnOperation)operation2).Name);
				DropColumnOperation val3 = val2;
				AddColumnOperation val4 = new AddColumnOperation();
				((ColumnOperation)val4).Schema=(((ColumnOperation)operation2).Schema);
				((ColumnOperation)val4).Table = (((ColumnOperation)operation2).Table);
				((ColumnOperation)val4).Name = (((ColumnOperation)operation2).Name);
				((ColumnOperation)val4).ClrType = (((ColumnOperation)operation2).ClrType);
				((ColumnOperation)val4).ColumnType = (((ColumnOperation)operation2).ColumnType);
				((ColumnOperation)val4).IsUnicode = (((ColumnOperation)operation2).IsUnicode);
				((ColumnOperation)val4).MaxLength = (((ColumnOperation)operation2).MaxLength);
				((ColumnOperation)val4).IsRowVersion = (((ColumnOperation)operation2).IsRowVersion);
				((ColumnOperation)val4).IsNullable = (((ColumnOperation)operation2).IsNullable);
				((ColumnOperation)val4).DefaultValue = (((ColumnOperation)operation2).DefaultValue);
				((ColumnOperation)val4).DefaultValueSql = (((ColumnOperation)operation2).DefaultValueSql);
				((ColumnOperation)val4).ComputedColumnSql = (((ColumnOperation)operation2).ComputedColumnSql);
				((ColumnOperation)val4).IsFixedLength = (((ColumnOperation)operation2).IsFixedLength);
				AddColumnOperation val5 = val4;
				((AnnotatableBase)val5).AddAnnotations((IEnumerable<IAnnotation>)((AnnotatableBase)operation2).GetAnnotations());
				base.Generate(val3, model, builder, true);
                base.Generate(val5, model, builder, true);
				return;
			}
			bool flag = ((AnnotatableBase)operation2)["Dm:ValueGenerationStrategy"] as DmValueGenerationStrategy? == DmValueGenerationStrategy.IdentityColumn;
			if (base.IsOldColumnSupported(model))
			{
				if (((AnnotatableBase)operation2.OldColumn)["Dm:ValueGenerationStrategy"] as DmValueGenerationStrategy? == DmValueGenerationStrategy.IdentityColumn && !flag)
				{
					DropIdentity(operation2, builder);
				}
				if (operation2.OldColumn.DefaultValue != null || (operation2.OldColumn.DefaultValueSql != null && (((ColumnOperation)operation2).DefaultValue == null || ((ColumnOperation)operation2).DefaultValueSql == null)))
				{
					DropDefaultConstraint(((ColumnOperation)operation2).Schema, ((ColumnOperation)operation2).Table, ((ColumnOperation)operation2).Name, builder);
				}
			}
			else
			{
				if (!flag)
				{
					DropIdentity(operation2, builder);
				}
				if (((ColumnOperation)operation2).DefaultValue == null && ((ColumnOperation)operation2).DefaultValueSql == null)
				{
					DropDefaultConstraint(((ColumnOperation)operation2).Schema, ((ColumnOperation)operation2).Table, ((ColumnOperation)operation2).Name, builder);
				}
			}
			builder.Append("ALTER TABLE ").Append(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation2.Table, ((ColumnOperation)operation2).Schema)).Append(" MODIFY ");
            base.ColumnDefinition(((ColumnOperation)operation2).Schema, ((ColumnOperation)operation2).Table, ((ColumnOperation)operation2).Name, (ColumnOperation)(object)operation2, model, builder);
			builder.AppendLine(base.Dependencies.SqlGenerationHelper.StatementTerminator);
			builder.EndCommand(false);
		}

		private static void DropIdentity(AlterColumnOperation operation, MigrationCommandListBuilder builder)
		{
			Check.NotNull<AlterColumnOperation>(operation, "operation");
			Check.NotNull<MigrationCommandListBuilder>(builder, "builder");
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(722, 4);
			defaultInterpolatedStringHandler.AppendLiteral("\nDECLARE\n   v_Count INTEGER;\nBEGIN\n SELECT\r\n        COUNT(*) INTO v_Count\r\nFROM\r\n        SYSCOLUMNS\r\nWHERE\r\n        ID =\r\n        (\r\n                SELECT\r\n                        ID\r\n                FROM\r\n                        SYSOBJECTS\r\n                WHERE\r\n                        NAME    ='");
			defaultInterpolatedStringHandler.AppendFormatted(((ColumnOperation)operation).Table);
			defaultInterpolatedStringHandler.AppendLiteral("'\r\n                    AND TYPE$   ='SCHOBJ'\r\n                    AND SUBTYPE$='UTAB'\r\n                    AND SCHID   =\r\n                        (\r\n                                SELECT ID FROM SYSOBJECTS WHERE NAME = '");
			defaultInterpolatedStringHandler.AppendFormatted(((ColumnOperation)operation).Schema);
			defaultInterpolatedStringHandler.AppendLiteral("' AND TYPE$='SCH'\r\n                        )\r\n        )\r\n    AND NAME         = '");
			defaultInterpolatedStringHandler.AppendFormatted(((ColumnOperation)operation).Name);
			defaultInterpolatedStringHandler.AppendLiteral("'\r\n    AND INFO2 & 0X01 = 1;\n  IF v_Count > 0 THEN\n    EXECUTE IMMEDIATE 'ALTER  TABLE \"");
			defaultInterpolatedStringHandler.AppendFormatted(((ColumnOperation)operation).Table);
			defaultInterpolatedStringHandler.AppendLiteral("\" DROP IDENTITY';\n  END IF;\nEND;");
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			builder.AppendLine(text).EndCommand(false);
		}

		protected override void Generate(RenameIndexOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			Check.NotNull<RenameIndexOperation>(operation, "operation");
			Check.NotNull<MigrationCommandListBuilder>(builder, "builder");
			if (operation.NewName != null)
			{
				builder.Append("ALTER INDEX ").Append(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name)).Append(" RENAME TO ")
					.Append(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.NewName))
					.Append(base.Dependencies.SqlGenerationHelper.StatementTerminator);
			}
			builder.EndCommand(false);
		}

		protected override void SequenceOptions(string schema, string name, SequenceOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			Check.NotEmpty(name, "name");
			Check.NotNull(operation.IncrementBy, "IncrementBy");
			Check.NotNull(operation.IsCyclic, "IsCyclic");
			Check.NotNull<MigrationCommandListBuilder>(builder, "builder");
			RelationalTypeMapping mapping = RelationalTypeMappingSourceExtensions.GetMapping(base.Dependencies.TypeMappingSource, typeof(int));
			RelationalTypeMapping mapping2 = RelationalTypeMappingSourceExtensions.GetMapping(base.Dependencies.TypeMappingSource, typeof(long));
			builder.Append(" INCREMENT BY ").Append(mapping.GenerateSqlLiteral((object)operation.IncrementBy));
			if (operation.MinValue.HasValue)
			{
				builder.Append(" MINVALUE ").Append(mapping2.GenerateSqlLiteral((object)operation.MinValue));
			}
			else
			{
				builder.Append(" NOMINVALUE");
			}
			if (operation.MaxValue.HasValue)
			{
				builder.Append(" MAXVALUE ").Append(mapping2.GenerateSqlLiteral((object)operation.MaxValue));
			}
			else
			{
				builder.Append(" NOMAXVALUE");
			}
			builder.Append(operation.IsCyclic ? " CYCLE" : " NOCYCLE");
		}

		protected override void Generate(RenameSequenceOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			throw new NotSupportedException("RenameSequenceOperation does not support");
		}

		protected override void Generate([NotNull] RestartSequenceOperation operation, [CanBeNull] IModel model, [NotNull] MigrationCommandListBuilder builder)
		{
			throw new NotSupportedException("RestartSequenceOperation does not support");
		}

		protected override void Generate(RenameTableOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			Check.NotNull<RenameTableOperation>(operation, "operation");
			Check.NotNull<MigrationCommandListBuilder>(builder, "builder");
			if (operation.NewSchema != null)
			{
				throw new NotSupportedException("RenameTableOperation does not support rename newschema");
			}
			if (operation.NewName != null && operation.NewName != operation.Name)
			{
				builder.Append("ALTER TABLE ").Append((operation.Schema != null) ? base.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name, operation.Schema) : base.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name)).Append(" RENAME TO ")
					.Append(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.NewName))
					.Append(base.Dependencies.SqlGenerationHelper.StatementTerminator)
					.EndCommand(false);
			}
		}

		protected override void Generate(EnsureSchemaOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			Check.NotNull<EnsureSchemaOperation>(operation, "operation");
			Check.NotNull<MigrationCommandListBuilder>(builder, "builder");
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(321, 2);
			defaultInterpolatedStringHandler.AppendLiteral("DECLARE\r\n                    B BOOLEAN ;\r\n                  BEGIN\r\n                    SELECT COUNT(NAME) INTO B FROM SYSOBJECTS WHERE TYPE$= 'SCH' AND NAME = '");
			defaultInterpolatedStringHandler.AppendFormatted(operation.Name);
			defaultInterpolatedStringHandler.AppendLiteral("';\r\n                    IF B == 0 THEN\r\n                            EXECUTE IMMEDIATE 'CREATE SCHEMA \"");
			defaultInterpolatedStringHandler.AppendFormatted(operation.Name);
			defaultInterpolatedStringHandler.AppendLiteral("\" ';\r\n                    END IF;\r\n                    END;");
			builder.Append(defaultInterpolatedStringHandler.ToStringAndClear()).EndCommand(false);
		}

		protected virtual void Generate([NotNull] DmCreateUserOperation operation, [CanBeNull] IModel model, [NotNull] MigrationCommandListBuilder builder)
		{
			Check.NotNull(operation, "operation");
			Check.NotNull<MigrationCommandListBuilder>(builder, "builder");
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(179, 3);
			defaultInterpolatedStringHandler.AppendLiteral("BEGIN\n                             EXECUTE IMMEDIATE 'CREATE USER ");
			defaultInterpolatedStringHandler.AppendFormatted(operation.UserName);
			defaultInterpolatedStringHandler.AppendLiteral(" IDENTIFIED BY ");
			defaultInterpolatedStringHandler.AppendFormatted(operation.Password);
			defaultInterpolatedStringHandler.AppendLiteral("';\n                             EXECUTE IMMEDIATE 'GRANT DBA TO ");
			defaultInterpolatedStringHandler.AppendFormatted(operation.UserName);
			defaultInterpolatedStringHandler.AppendLiteral("';\n                           END;");
			builder.Append(defaultInterpolatedStringHandler.ToStringAndClear()).EndCommand(true);
		}

		protected virtual void Generate([NotNull] DmDropUserOperation operation, [CanBeNull] IModel model, [NotNull] MigrationCommandListBuilder builder)
		{
			Check.NotNull(operation, "operation");
			Check.NotNull<MigrationCommandListBuilder>(builder, "builder");
			builder.Append("BEGIN\n                         EXECUTE IMMEDIATE 'DROP USER " + operation.UserName + " CASCADE';\n                       END;").EndCommand(true);
		}

		protected override void Generate([NotNull] CreateIndexOperation operation, [CanBeNull] IModel model, [NotNull] MigrationCommandListBuilder builder, bool terminate = true)
		{
			if (operation.Filter != null)
			{
				throw new NotSupportedException("CreateIndexOperation does not support filter clause");
			}
			base.Generate(operation, model, builder, true);
		}

		protected void Generate(DropIndexOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			base.Generate(operation, model, builder, true);
		}

		protected override void Generate([NotNull] DropIndexOperation operation, [CanBeNull] IModel model, [NotNull] MigrationCommandListBuilder builder, bool terminate)
		{
			Check.NotNull<DropIndexOperation>(operation, "operation");
			Check.NotNull<MigrationCommandListBuilder>(builder, "builder");
			builder.Append("DROP INDEX ").Append(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name));
			if (terminate)
			{
				builder.AppendLine(base.Dependencies.SqlGenerationHelper.StatementTerminator).EndCommand(false);
			}
		}

		protected override void Generate(RenameColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			Check.NotNull<RenameColumnOperation>(operation, "operation");
			Check.NotNull<MigrationCommandListBuilder>(builder, "builder");
			StringBuilder stringBuilder = new StringBuilder();
			if (operation.Schema != null)
			{
				stringBuilder.Append(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Schema)).Append(".");
			}
			stringBuilder.Append(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table));
			builder.Append("ALTER TABLE ").Append(stringBuilder.ToString()).Append(" ALTER COLUMN ")
				.Append(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name))
				.Append(" RENAME TO ")
				.Append(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.NewName))
				.Append(base.Dependencies.SqlGenerationHelper.StatementTerminator);
			builder.EndCommand(false);
		}

		protected override void Generate(InsertDataOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
		{
			Check.NotNull<InsertDataOperation>(operation, "operation");
			Check.NotNull<MigrationCommandListBuilder>(builder, "builder");
			builder.AppendLine("DECLARE").AppendLine("   CNT  INT;").AppendLine("BEGIN ");
			builder.Append(" SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END INTO CNT FROM ").Append(" SYSCOLUMNS WHERE ID = (SELECT ID FROM SYSOBJECTS WHERE TYPE$ = 'SCHOBJ' AND SUBTYPE$ = 'UTAB' AND ");
			if (operation.Schema != null)
			{
				builder.Append(" SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND NAME = ").Append(RelationalTypeMappingSourceExtensions.GetMapping(base.Dependencies.TypeMappingSource, typeof(string)).GenerateSqlLiteral((object)operation.Schema)).Append(") AND ");
			}
			else
			{
				builder.Append(" SCHID = CURRENT_SCHID() AND ");
			}
			builder.Append("NAME = ").Append(RelationalTypeMappingSourceExtensions.GetMapping(base.Dependencies.TypeMappingSource, typeof(string)).GenerateSqlLiteral((object)operation.Table)).AppendLine(" ) AND INFO2&0X01 = 1;");
			builder.AppendLine("IF CNT > 0 THEN ");
			using (builder.Indent())
			{
				builder.Append("EXECUTE IMMEDIATE 'SET IDENTITY_INSERT ").Append(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" ON '")
					.AppendLine(base.Dependencies.SqlGenerationHelper.StatementTerminator);
			}
			builder.AppendLine("END IF; ");
			base.Generate(operation, model, builder, false);
			builder.AppendLine("IF CNT > 0 THEN ");
			using (builder.Indent())
			{
				builder.Append("EXECUTE IMMEDIATE 'SET IDENTITY_INSERT ").Append(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" OFF'")
					.AppendLine(base.Dependencies.SqlGenerationHelper.StatementTerminator);
			}
			builder.AppendLine("END IF; ").AppendLine("END;");
			builder.EndCommand(false);
		}

		protected override void Generate(CreateTableOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
		{
			base.Generate(operation, model, builder, true);
			AddColumnOperation[] array = (from c in operation.Columns
				where ((ColumnOperation)c).IsRowVersion
				select c).ToArray();
			if (array.Length != 0)
			{
				builder.Append("CREATE OR REPLACE TRIGGER ").AppendLine(base.Dependencies.SqlGenerationHelper.DelimitIdentifier("rowversion_" + ((TableOperation)operation).Name, ((TableOperation)operation).Schema)).Append("BEFORE INSERT OR UPDATE ON ")
					.AppendLine(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(((TableOperation)operation).Name, ((TableOperation)operation).Schema))
					.AppendLine("FOR EACH ROW")
					.AppendLine("BEGIN");
				AddColumnOperation[] array2 = array;
				foreach (AddColumnOperation val in array2)
				{
					builder.Append("  :NEW.").Append(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(((ColumnOperation)val).Name)).Append(" := NVL(:OLD.")
						.Append(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(((ColumnOperation)val).Name))
						.Append(", '00000000') + 1")
						.AppendLine(base.Dependencies.SqlGenerationHelper.StatementTerminator);
				}
				builder.Append("END").AppendLine(base.Dependencies.SqlGenerationHelper.StatementTerminator);
			}
			base.EndStatement(builder, false);
		}

		protected override void UniqueConstraint([NotNull] AddUniqueConstraintOperation operation, [CanBeNull] IModel model, [NotNull] MigrationCommandListBuilder builder)
		{
			Check.NotNull<AddUniqueConstraintOperation>(operation, "operation");
			Check.NotNull<MigrationCommandListBuilder>(builder, "builder");
			if (operation.Name == null)
			{
				operation.Name=(Guid.NewGuid().ToString());
			}
			base.UniqueConstraint(operation, model, builder);
		}

		protected override void ColumnDefinition([CanBeNull] string schema, [NotNull] string table, [NotNull] string name, ColumnOperation operation, [CanBeNull] IModel model, [NotNull] MigrationCommandListBuilder builder)
		{
			Check.NotEmpty(name, "name");
			Check.NotNull<ColumnOperation>(operation, "operation");
			Check.NotNull(operation.ClrType, "ClrType");
			Check.NotNull<MigrationCommandListBuilder>(builder, "builder");
			if (operation.ComputedColumnSql != null)
			{
				builder.Append(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(name)).Append(" AS (").Append(operation.ComputedColumnSql)
					.Append(")");
				return;
			}
			builder.Append(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(name)).Append(" ").Append(operation.ColumnType ?? base.GetColumnType(schema, table, name, operation, model));
			string text = ((AnnotatableBase)operation)["Dm:Identity"] as string;
			if (text != null || ((AnnotatableBase)operation)["Dm:ValueGenerationStrategy"] as DmValueGenerationStrategy? == DmValueGenerationStrategy.IdentityColumn)
			{
				builder.Append(" IDENTITY");
				if (!string.IsNullOrEmpty(text) && text != "1, 1")
				{
					builder.Append("(").Append(text).Append(")");
				}
			}
			else
			{
				base.DefaultValue(operation.DefaultValue, operation.DefaultValueSql, operation.ColumnType, builder);
			}
			builder.Append(operation.IsNullable ? " NULL" : " NOT NULL");
		}

		protected override void PrimaryKeyConstraint([NotNull] AddPrimaryKeyOperation operation, [CanBeNull] IModel model, [NotNull] MigrationCommandListBuilder builder)
		{
			Check.NotNull<AddPrimaryKeyOperation>(operation, "operation");
			Check.NotNull<MigrationCommandListBuilder>(builder, "builder");
			if (operation.Name == null)
			{
				operation.Name=(Guid.NewGuid().ToString());
			}
			base.PrimaryKeyConstraint(operation, model, builder);
		}

		protected override void ForeignKeyConstraint(AddForeignKeyOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			Check.NotNull<AddForeignKeyOperation>(operation, "operation");
			Check.NotNull<MigrationCommandListBuilder>(builder, "builder");
			if (operation.PrincipalColumns == null)
			{
				throw new NotSupportedException("AddForeignKeyOperation does not support references columns is null");
			}
			if (operation.Name == null)
			{
				operation.Name=(Guid.NewGuid().ToString());
			}
			base.ForeignKeyConstraint(operation, model, builder);
		}

		protected virtual void DropDefaultConstraint([CanBeNull] string schema, [NotNull] string tableName, [NotNull] string columnName, [NotNull] MigrationCommandListBuilder builder)
		{
			Check.NotEmpty(tableName, "tableName");
			Check.NotEmpty(columnName, "columnName");
			Check.NotNull<MigrationCommandListBuilder>(builder, "builder");
			builder.Append("ALTER TABLE ").Append(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(tableName, schema)).Append(" MODIFY ")
				.Append(base.Dependencies.SqlGenerationHelper.DelimitIdentifier(columnName))
				.Append(" DEFAULT NULL")
				.AppendLine(base.Dependencies.SqlGenerationHelper.StatementTerminator)
				.EndCommand(false);
		}
	}
}
