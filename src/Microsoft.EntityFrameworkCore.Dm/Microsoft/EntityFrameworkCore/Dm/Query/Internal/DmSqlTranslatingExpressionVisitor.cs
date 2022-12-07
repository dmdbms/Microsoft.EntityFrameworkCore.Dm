// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmSqlTranslatingExpressionVisitor
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq.Expressions;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class DmSqlTranslatingExpressionVisitor : RelationalSqlTranslatingExpressionVisitor
  {
    private static readonly HashSet<string> _dateTimeDataTypes = new HashSet<string>()
    {
      "time",
      "date",
      "datetime",
      "datetime2",
      "datetimeoffset"
    };
    private static readonly HashSet<ExpressionType> _arithmeticOperatorTypes = new HashSet<ExpressionType>()
    {
      ExpressionType.Add,
      ExpressionType.Subtract,
      ExpressionType.Multiply,
      ExpressionType.Divide,
      ExpressionType.Modulo
    };

    public DmSqlTranslatingExpressionVisitor(
      RelationalSqlTranslatingExpressionVisitorDependencies dependencies,
      QueryCompilationContext queryCompilationContext,
      QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor)
      : base(dependencies, queryCompilationContext, queryableMethodTranslatingExpressionVisitor)
    {
    }

    protected override Expression VisitBinary(BinaryExpression binaryExpression)
    {
      SqlExpression sqlExpression = (SqlExpression) base.VisitBinary(binaryExpression);
      return sqlExpression == null ? (Expression) null : (!(sqlExpression is SqlBinaryExpression binaryExpression1) || !DmSqlTranslatingExpressionVisitor._arithmeticOperatorTypes.Contains(binaryExpression1.OperatorType) || !DmSqlTranslatingExpressionVisitor._dateTimeDataTypes.Contains(DmSqlTranslatingExpressionVisitor.GetProviderType(binaryExpression1.Left)) && !DmSqlTranslatingExpressionVisitor._dateTimeDataTypes.Contains(DmSqlTranslatingExpressionVisitor.GetProviderType(binaryExpression1.Right)) ? (Expression) sqlExpression : (Expression) null);
    }

    public override SqlExpression TranslateLongCount(SqlExpression expression = null) => expression != null ? this.Dependencies.SqlExpressionFactory.ApplyDefaultTypeMapping((SqlExpression) this.Dependencies.SqlExpressionFactory.Function("COUNT", (IEnumerable<SqlExpression>) new SqlExpression[1]
    {
      expression
    }, false, (IEnumerable<bool>) new bool[1], typeof (long), (RelationalTypeMapping) null)) : this.Dependencies.SqlExpressionFactory.ApplyDefaultTypeMapping((SqlExpression) this.Dependencies.SqlExpressionFactory.Function("COUNT", (IEnumerable<SqlExpression>) new SqlFragmentExpression[1]
    {
      this.Dependencies.SqlExpressionFactory.Fragment("*")
    }, false, (IEnumerable<bool>) new bool[1], typeof (long), (RelationalTypeMapping) null));

    private static string GetProviderType(SqlExpression expression) => expression.TypeMapping?.StoreType;
  }
}
