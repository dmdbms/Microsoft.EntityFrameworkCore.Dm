using System;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations
{
	public class DmMigrationsSqlGenerator : MigrationsSqlGenerator
	{
		public DmMigrationsSqlGenerator([JetBrains.Annotations.NotNull] MigrationsSqlGeneratorDependencies dependencies, [JetBrains.Annotations.NotNull] IMigrationsAnnotationProvider migrationsAnnotations)
			: base(dependencies)
		{
		}

		protected override void ColumnDefinition(AddColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			ColumnDefinition(operation.Schema, operation.Table, operation.Name, operation, model, builder);
		}

		protected override void Generate(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
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

		protected override void Generate([JetBrains.Annotations.NotNull] AlterDatabaseOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
		{
			throw new NotSupportedException("AlterDatabaseOperation does not support");
		}

		protected override void Generate([JetBrains.Annotations.NotNull] CreateSequenceOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
		{
			if (operation.ClrType != typeof(long))
			{
				throw new NotSupportedException("CreateSequenceOperation only support long type");
			}
			base.Generate(operation, model, builder);
		}

		protected override void Generate(AlterColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
			IColumn column = model?.GetRelationalModel().FindTable(operation.Table, operation.Schema)?.Columns.FirstOrDefault((IColumn c) => c.Name == operation.Name);
			if (operation.ComputedColumnSql != null)
			{
				DropColumnOperation operation2 = new DropColumnOperation
				{
					Schema = operation.Schema,
					Table = operation.Table,
					Name = operation.Name
				};
				AddColumnOperation addColumnOperation = new AddColumnOperation
				{
					Schema = operation.Schema,
					Table = operation.Table,
					Name = operation.Name,
					ClrType = operation.ClrType,
					ColumnType = operation.ColumnType,
					IsUnicode = operation.IsUnicode,
					MaxLength = operation.MaxLength,
					IsRowVersion = operation.IsRowVersion,
					IsNullable = operation.IsNullable,
					DefaultValue = operation.DefaultValue,
					DefaultValueSql = operation.DefaultValueSql,
					ComputedColumnSql = operation.ComputedColumnSql,
					IsFixedLength = operation.IsFixedLength
				};
				addColumnOperation.AddAnnotations(operation.GetAnnotations());
				Generate(operation2, model, builder);
				Generate(addColumnOperation, model, builder);
				return;
			}
			bool flag = operation["Dm:ValueGenerationStrategy"] as DmValueGenerationStrategy? == DmValueGenerationStrategy.IdentityColumn;
			if (IsOldColumnSupported(model))
			{
				if (operation.OldColumn["Dm:ValueGenerationStrategy"] as DmValueGenerationStrategy? == DmValueGenerationStrategy.IdentityColumn && !flag)
				{
					DropIdentity(operation, builder);
				}
				if (operation.OldColumn.DefaultValue != null || (operation.OldColumn.DefaultValueSql != null && (operation.DefaultValue == null || operation.DefaultValueSql == null)))
				{
					DropDefaultConstraint(operation.Schema, operation.Table, operation.Name, builder);
				}
			}
			else
			{
				if (!flag)
				{
					DropIdentity(operation, builder);
				}
				if (operation.DefaultValue == null && operation.DefaultValueSql == null)
				{
					DropDefaultConstraint(operation.Schema, operation.Table, operation.Name, builder);
				}
			}
			builder.Append("ALTER TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" MODIFY ");
			ColumnDefinition(operation.Schema, operation.Table, operation.Name, operation, model, builder);
			builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			builder.EndCommand();
		}

		private static void DropIdentity(AlterColumnOperation operation, MigrationCommandListBuilder builder)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
			string value = "\nDECLARE\n   v_Count INTEGER;\nBEGIN\n SELECT\r\n        COUNT(*) INTO v_Count\r\nFROM\r\n        SYSCOLUMNS\r\nWHERE\r\n        ID =\r\n        (\r\n                SELECT\r\n                        ID\r\n                FROM\r\n                        SYSOBJECTS\r\n                WHERE\r\n                        NAME    ='" + operation.Table + "'\r\n                    AND TYPE$   ='SCHOBJ'\r\n                    AND SUBTYPE$='UTAB'\r\n                    AND SCHID   =\r\n                        (\r\n                                SELECT ID FROM SYSOBJECTS WHERE NAME = '" + operation.Schema + "' AND TYPE$='SCH'\r\n                        )\r\n        )\r\n    AND NAME         = '" + operation.Name + "'\r\n    AND INFO2 & 0X01 = 1;\n  IF v_Count > 0 THEN\n    EXECUTE IMMEDIATE 'ALTER  TABLE \"" + operation.Table + "\" DROP IDENTITY';\n  END IF;\nEND;";
			builder.AppendLine(value).EndCommand();
		}

		protected override void Generate(RenameIndexOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
			if (operation.NewName != null)
			{
				builder.Append("ALTER INDEX ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name)).Append(" RENAME TO ")
					.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.NewName))
					.Append(Dependencies.SqlGenerationHelper.StatementTerminator);
			}
			builder.EndCommand();
		}

		protected override void SequenceOptions(string schema, string name, SequenceOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation.IncrementBy, "IncrementBy");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation.IsCyclic, "IsCyclic");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
			RelationalTypeMapping mapping = Dependencies.TypeMappingSource.GetMapping(typeof(int));
			RelationalTypeMapping mapping2 = Dependencies.TypeMappingSource.GetMapping(typeof(long));
			builder.Append(" INCREMENT BY ").Append(mapping.GenerateSqlLiteral(operation.IncrementBy));
			if (operation.MinValue.HasValue)
			{
				builder.Append(" MINVALUE ").Append(mapping2.GenerateSqlLiteral(operation.MinValue));
			}
			else
			{
				builder.Append(" NOMINVALUE");
			}
			if (operation.MaxValue.HasValue)
			{
				builder.Append(" MAXVALUE ").Append(mapping2.GenerateSqlLiteral(operation.MaxValue));
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

		protected override void Generate([JetBrains.Annotations.NotNull] RestartSequenceOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
		{
			throw new NotSupportedException("RestartSequenceOperation does not support");
		}

		protected override void Generate(RenameTableOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
			if (operation.NewSchema != null)
			{
				throw new NotSupportedException("RenameTableOperation does not support rename newschema");
			}
			if (operation.NewName != null && operation.NewName != operation.Name)
			{
				builder.Append("ALTER TABLE ").Append((operation.Schema != null) ? Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name, operation.Schema) : Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name)).Append(" RENAME TO ")
					.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.NewName))
					.Append(Dependencies.SqlGenerationHelper.StatementTerminator)
					.EndCommand();
			}
		}

		protected override void Generate(EnsureSchemaOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
			builder.Append("DECLARE\r\n                    B BOOLEAN ;\r\n                  BEGIN\r\n                    SELECT COUNT(NAME) INTO B FROM SYSOBJECTS WHERE TYPE$= 'SCH' AND NAME = '" + operation.Name + "';\r\n                    IF B == 0 THEN\r\n                            EXECUTE IMMEDIATE 'CREATE SCHEMA \"" + operation.Name + "\" ';\r\n                    END IF;\r\n                    END;").EndCommand();
		}

		protected virtual void Generate([JetBrains.Annotations.NotNull] DmCreateUserOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
			builder.Append("BEGIN\n                             EXECUTE IMMEDIATE 'CREATE USER " + operation.UserName + " IDENTIFIED BY " + operation.Password + "';\n                             EXECUTE IMMEDIATE 'GRANT DBA TO " + operation.UserName + "';\n                           END;").EndCommand(suppressTransaction: true);
		}

		protected virtual void Generate([JetBrains.Annotations.NotNull] DmDropUserOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
			builder.Append("BEGIN\n                         EXECUTE IMMEDIATE 'DROP USER " + operation.UserName + " CASCADE';\n                       END;").EndCommand(suppressTransaction: true);
		}

		protected override void Generate([JetBrains.Annotations.NotNull] CreateIndexOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder, bool terminate = true)
		{
			if (operation.Filter != null)
			{
				throw new NotSupportedException("CreateIndexOperation does not support filter clause");
			}
			base.Generate(operation, model, builder);
		}

		protected void Generate(DropIndexOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			Generate(operation, model, builder, terminate: true);
		}

		protected override void Generate([JetBrains.Annotations.NotNull] DropIndexOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder, bool terminate)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
			builder.Append("DROP INDEX ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name));
			if (terminate)
			{
				builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator).EndCommand();
			}
		}

		protected override void Generate(RenameColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
			StringBuilder stringBuilder = new StringBuilder();
			if (operation.Schema != null)
			{
				stringBuilder.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Schema)).Append(".");
			}
			stringBuilder.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table));
			builder.Append("ALTER TABLE ").Append(stringBuilder.ToString()).Append(" ALTER COLUMN ")
				.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name))
				.Append(" RENAME TO ")
				.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.NewName))
				.Append(Dependencies.SqlGenerationHelper.StatementTerminator);
			builder.EndCommand();
		}

		protected override void Generate(InsertDataOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
			builder.AppendLine("DECLARE").AppendLine("   CNT  INT;").AppendLine("BEGIN ");
			builder.Append(" SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END INTO CNT FROM ").Append(" SYSCOLUMNS WHERE ID = (SELECT ID FROM SYSOBJECTS WHERE TYPE$ = 'SCHOBJ' AND SUBTYPE$ = 'UTAB' AND ");
			if (operation.Schema != null)
			{
				builder.Append(" SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND NAME = ").Append(Dependencies.TypeMappingSource.GetMapping(typeof(string)).GenerateSqlLiteral(operation.Schema)).Append(") AND ");
			}
			else
			{
				builder.Append(" SCHID = CURRENT_SCHID() AND ");
			}
			builder.Append("NAME = ").Append(Dependencies.TypeMappingSource.GetMapping(typeof(string)).GenerateSqlLiteral(operation.Table)).AppendLine(" ) AND INFO2&0X01 = 1;");
			builder.AppendLine("IF CNT > 0 THEN ");
			using (builder.Indent())
			{
				builder.Append("EXECUTE IMMEDIATE 'SET IDENTITY_INSERT ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" ON '")
					.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			}
			builder.AppendLine("END IF; ");
			base.Generate(operation, model, builder, terminate: false);
			builder.AppendLine("IF CNT > 0 THEN ");
			using (builder.Indent())
			{
				builder.Append("EXECUTE IMMEDIATE 'SET IDENTITY_INSERT ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema)).Append(" OFF'")
					.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			}
			builder.AppendLine("END IF; ").AppendLine("END;");
			builder.EndCommand();
		}

		protected override void Generate(CreateTableOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
		{
			base.Generate(operation, model, builder);
			AddColumnOperation[] array = operation.Columns.Where((AddColumnOperation c) => c.IsRowVersion).ToArray();
			if (array.Length != 0)
			{
				builder.Append("CREATE OR REPLACE TRIGGER ").AppendLine(Dependencies.SqlGenerationHelper.DelimitIdentifier("rowversion_" + operation.Name, operation.Schema)).Append("BEFORE INSERT OR UPDATE ON ")
					.AppendLine(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name, operation.Schema))
					.AppendLine("FOR EACH ROW")
					.AppendLine("BEGIN");
				AddColumnOperation[] array2 = array;
				foreach (AddColumnOperation addColumnOperation in array2)
				{
					builder.Append("  :NEW.").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(addColumnOperation.Name)).Append(" := NVL(:OLD.")
						.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(addColumnOperation.Name))
						.Append(", '00000000') + 1")
						.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
				}
				builder.Append("END").AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
			}
			EndStatement(builder);
		}

		protected override void UniqueConstraint([JetBrains.Annotations.NotNull] AddUniqueConstraintOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
			if (operation.Name == null)
			{
				operation.Name = Guid.NewGuid().ToString();
			}
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

		protected override void PrimaryKeyConstraint([JetBrains.Annotations.NotNull] AddPrimaryKeyOperation operation, [JetBrains.Annotations.CanBeNull] IModel model, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
			if (operation.Name == null)
			{
				operation.Name = Guid.NewGuid().ToString();
			}
			base.PrimaryKeyConstraint(operation, model, builder);
		}

		protected override void ForeignKeyConstraint(AddForeignKeyOperation operation, IModel model, MigrationCommandListBuilder builder)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operation, "operation");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
			if (operation.PrincipalColumns == null)
			{
				throw new NotSupportedException("AddForeignKeyOperation does not support references columns is null");
			}
			if (operation.Name == null)
			{
				operation.Name = Guid.NewGuid().ToString();
			}
			base.ForeignKeyConstraint(operation, model, builder);
		}

		protected virtual void DropDefaultConstraint([JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] string tableName, [JetBrains.Annotations.NotNull] string columnName, [JetBrains.Annotations.NotNull] MigrationCommandListBuilder builder)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(tableName, "tableName");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(columnName, "columnName");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
			builder.Append("ALTER TABLE ").Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(tableName, schema)).Append(" MODIFY ")
				.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(columnName))
				.Append(" DEFAULT NULL")
				.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator)
				.EndCommand();
		}
	}
}
