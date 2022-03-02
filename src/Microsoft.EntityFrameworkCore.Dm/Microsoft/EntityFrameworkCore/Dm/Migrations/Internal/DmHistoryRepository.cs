using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Dm.Migrations.Internal
{
	public class DmHistoryRepository : HistoryRepository
	{
		protected override string ExistsSql
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("SELECT ID FROM SYS.SYSOBJECTS WHERE ");
				if (TableSchema != null)
				{
					stringBuilder.Append(" SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND NAME = ").Append(Dependencies.TypeMappingSource.GetMapping(typeof(string)).GenerateSqlLiteral(TableSchema)).Append(") AND ");
				}
				else
				{
					stringBuilder.Append(" SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND NAME = ").Append(" (SELECT USER())").Append(") AND ");
				}
				stringBuilder.Append("NAME = ").Append(Dependencies.TypeMappingSource.GetMapping(typeof(string)).GenerateSqlLiteral(TableName)).Append(";");
				return stringBuilder.ToString();
			}
		}

		public DmHistoryRepository([JetBrains.Annotations.NotNull] HistoryRepositoryDependencies dependencies)
			: base(dependencies)
		{
		}

		protected override bool InterpretExistsResult(object value)
		{
			return value != null;
		}

		public override string GetCreateIfNotExistsScript()
		{
			IndentedStringBuilder indentedStringBuilder = new IndentedStringBuilder();
			indentedStringBuilder.AppendLine("DECLARE CNT INT; BEGIN ").Append(" SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END INTO CNT FROM SYSOBJECTS WHERE NAME = ").Append(Dependencies.TypeMappingSource.GetMapping(typeof(string)).GenerateSqlLiteral(TableName))
				.Append(" ");
			if (TableSchema != null)
			{
				indentedStringBuilder.Append(" AND ").Append(" SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND NAME = ").Append(Dependencies.TypeMappingSource.GetMapping(typeof(string)).GenerateSqlLiteral(TableSchema))
					.Append(") ");
			}
			indentedStringBuilder.Append(";");
			using (indentedStringBuilder.Indent())
			{
				indentedStringBuilder.Append("IF CNT == 0 THEN ").AppendLines(GetCreateScript()).Append("END IF; ");
			}
			indentedStringBuilder.AppendLine("END; ");
			return indentedStringBuilder.ToString();
		}

		public override string GetBeginIfNotExistsScript(string migrationId)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(migrationId, "migrationId");
			return new StringBuilder().AppendLine("DECLARE CNT INT; BEGIN ").Append(" SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END INTO CNT FROM ").Append(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema))
				.Append(" WHERE ")
				.Append(SqlGenerationHelper.DelimitIdentifier(MigrationIdColumnName))
				.Append(" = ")
				.Append(Dependencies.TypeMappingSource.GetMapping(typeof(string)).GenerateSqlLiteral(migrationId))
				.AppendLine(";")
				.AppendLine("IF CNT == 0 THEN ")
				.ToString();
		}

		public override string GetBeginIfExistsScript(string migrationId)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(migrationId, "migrationId");
			return new StringBuilder().AppendLine("DECLARE ").AppendLine("   CNT INT;").AppendLine("BEGIN ")
				.Append("SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END INTO CNT FROM ")
				.Append(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema))
				.Append(" WHERE ")
				.Append(SqlGenerationHelper.DelimitIdentifier(MigrationIdColumnName))
				.Append(" = ")
				.Append(Dependencies.TypeMappingSource.GetMapping(typeof(string)).GenerateSqlLiteral(migrationId))
				.AppendLine(";")
				.AppendLine("IF CNT == 1 THEN ")
				.ToString();
		}

		public override string GetEndIfScript()
		{
			return new StringBuilder().Append("END").AppendLine(SqlGenerationHelper.StatementTerminator).ToString();
		}
	}
}
