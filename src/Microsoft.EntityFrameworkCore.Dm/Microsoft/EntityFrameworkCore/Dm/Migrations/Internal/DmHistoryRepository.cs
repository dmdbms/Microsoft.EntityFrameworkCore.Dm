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
				if (base.TableSchema != null)
				{
					stringBuilder.Append(" SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND NAME = ").Append(RelationalTypeMappingSourceExtensions.GetMapping(base.Dependencies.TypeMappingSource, typeof(string)).GenerateSqlLiteral((object)base.TableSchema)).Append(") AND ");
				}
				else
				{
					stringBuilder.Append(" SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND NAME = ").Append(" (SELECT USER())").Append(") AND ");
				}
				stringBuilder.Append("NAME = ").Append(RelationalTypeMappingSourceExtensions.GetMapping(base.Dependencies.TypeMappingSource, typeof(string)).GenerateSqlLiteral((object)base.TableName)).Append(";");
				return stringBuilder.ToString();
			}
		}

		public DmHistoryRepository([NotNull] HistoryRepositoryDependencies dependencies)
			: base(dependencies)
		{
		}

		protected override bool InterpretExistsResult(object value)
		{
			return value != null;
		}

		public override string GetCreateIfNotExistsScript()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			IndentedStringBuilder val = new IndentedStringBuilder();
			val.AppendLine("DECLARE CNT INT; BEGIN ").Append(" SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END INTO CNT FROM SYSOBJECTS WHERE NAME = ").Append(RelationalTypeMappingSourceExtensions.GetMapping(base.Dependencies.TypeMappingSource, typeof(string)).GenerateSqlLiteral((object)base.TableName))
				.Append(" ");
			if (base.TableSchema != null)
			{
				val.Append(" AND ").Append(" SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND NAME = ").Append(RelationalTypeMappingSourceExtensions.GetMapping(base.Dependencies.TypeMappingSource, typeof(string)).GenerateSqlLiteral((object)base.TableSchema))
					.Append(") ");
			}
			val.Append(";");
			using (val.Indent())
			{
				val.Append("IF CNT == 0 THEN ").AppendLines(base.GetCreateScript(), false).Append("END IF; ");
			}
			val.AppendLine("END; ");
			return ((object)val).ToString();
		}

		public override string GetBeginIfNotExistsScript(string migrationId)
		{
			Check.NotEmpty(migrationId, "migrationId");
			return new StringBuilder().AppendLine("DECLARE CNT INT; BEGIN ").Append(" SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END INTO CNT FROM ").Append(base.SqlGenerationHelper.DelimitIdentifier(base.TableName, base.TableSchema))
				.Append(" WHERE ")
				.Append(base.SqlGenerationHelper.DelimitIdentifier(base.MigrationIdColumnName))
				.Append(" = ")
				.Append(RelationalTypeMappingSourceExtensions.GetMapping(base.Dependencies.TypeMappingSource, typeof(string)).GenerateSqlLiteral((object)migrationId))
				.AppendLine(";")
				.AppendLine("IF CNT == 0 THEN ")
				.ToString();
		}

		public override string GetBeginIfExistsScript(string migrationId)
		{
			Check.NotEmpty(migrationId, "migrationId");
			return new StringBuilder().AppendLine("DECLARE ").AppendLine("   CNT INT;").AppendLine("BEGIN ")
				.Append("SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END INTO CNT FROM ")
				.Append(base.SqlGenerationHelper.DelimitIdentifier(base.TableName, base.TableSchema))
				.Append(" WHERE ")
				.Append(base.SqlGenerationHelper.DelimitIdentifier(base.MigrationIdColumnName))
				.Append(" = ")
				.Append(RelationalTypeMappingSourceExtensions.GetMapping(base.Dependencies.TypeMappingSource, typeof(string)).GenerateSqlLiteral((object)migrationId))
				.AppendLine(";")
				.AppendLine("IF CNT == 1 THEN ")
				.ToString();
		}

		public override string GetEndIfScript()
		{
			return new StringBuilder().Append("END").AppendLine(base.SqlGenerationHelper.StatementTerminator).ToString();
		}
	}
}
