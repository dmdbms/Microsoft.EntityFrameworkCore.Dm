// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Migrations.Internal.DmHistoryRepository
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using System.Text;



namespace Microsoft.EntityFrameworkCore.Dm.Migrations.Internal
{
  public class DmHistoryRepository : HistoryRepository
  {
    public DmHistoryRepository([NotNull] HistoryRepositoryDependencies dependencies)
      : base(dependencies)
    {
    }

    protected override string ExistsSql
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("SELECT ID FROM SYS.SYSOBJECTS WHERE ");
        if (this.TableSchema != null)
          stringBuilder.Append(" SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND NAME = ").Append(RelationalTypeMappingSourceExtensions.GetMapping(this.Dependencies.TypeMappingSource, typeof (string)).GenerateSqlLiteral((object) this.TableSchema)).Append(") AND ");
        else
          stringBuilder.Append(" SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND NAME = ").Append(" (SELECT USER())").Append(") AND ");
        stringBuilder.Append("NAME = ").Append(RelationalTypeMappingSourceExtensions.GetMapping(this.Dependencies.TypeMappingSource, typeof (string)).GenerateSqlLiteral((object) this.TableName)).Append(";");
        return stringBuilder.ToString();
      }
    }

    protected override bool InterpretExistsResult(object value) => value != null;

    public override string GetCreateIfNotExistsScript()
    {
      IndentedStringBuilder indentedStringBuilder = new IndentedStringBuilder();
      indentedStringBuilder.AppendLine("DECLARE CNT INT; BEGIN ").Append(" SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END INTO CNT FROM SYSOBJECTS WHERE NAME = ").Append(RelationalTypeMappingSourceExtensions.GetMapping(this.Dependencies.TypeMappingSource, typeof (string)).GenerateSqlLiteral((object) this.TableName)).Append(" ");
      if (this.TableSchema != null)
        indentedStringBuilder.Append(" AND ").Append(" SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND NAME = ").Append(RelationalTypeMappingSourceExtensions.GetMapping(this.Dependencies.TypeMappingSource, typeof (string)).GenerateSqlLiteral((object) this.TableSchema)).Append(") ");
      indentedStringBuilder.Append(";");
      using (indentedStringBuilder.Indent())
        indentedStringBuilder.Append("IF CNT == 0 THEN ").AppendLines(this.GetCreateScript(), false).Append("END IF; ");
      indentedStringBuilder.AppendLine("END; ");
      return ((object) indentedStringBuilder).ToString();
    }

    public override string GetBeginIfNotExistsScript(string migrationId)
    {
      Check.NotEmpty(migrationId, nameof (migrationId));
      return new StringBuilder().AppendLine("DECLARE CNT INT; BEGIN ").Append(" SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END INTO CNT FROM ").Append(this.SqlGenerationHelper.DelimitIdentifier(this.TableName, this.TableSchema)).Append(" WHERE ").Append(this.SqlGenerationHelper.DelimitIdentifier(this.MigrationIdColumnName)).Append(" = ").Append(RelationalTypeMappingSourceExtensions.GetMapping(this.Dependencies.TypeMappingSource, typeof (string)).GenerateSqlLiteral((object) migrationId)).AppendLine(";").AppendLine("IF CNT == 0 THEN ").ToString();
    }

    public override string GetBeginIfExistsScript(string migrationId)
    {
      Check.NotEmpty(migrationId, nameof (migrationId));
      return new StringBuilder().AppendLine("DECLARE ").AppendLine("   CNT INT;").AppendLine("BEGIN ").Append("SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END INTO CNT FROM ").Append(this.SqlGenerationHelper.DelimitIdentifier(this.TableName, this.TableSchema)).Append(" WHERE ").Append(this.SqlGenerationHelper.DelimitIdentifier(this.MigrationIdColumnName)).Append(" = ").Append(RelationalTypeMappingSourceExtensions.GetMapping(this.Dependencies.TypeMappingSource, typeof (string)).GenerateSqlLiteral((object) migrationId)).AppendLine(";").AppendLine("IF CNT == 1 THEN ").ToString();
    }

    public override string GetEndIfScript() => new StringBuilder().Append("END").AppendLine(this.SqlGenerationHelper.StatementTerminator).ToString();
  }
}
