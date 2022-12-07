// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmQuerySqlGenerator
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class DmQuerySqlGenerator : QuerySqlGenerator
  {
    public DmQuerySqlGenerator(QuerySqlGeneratorDependencies dependencies)
      : base(dependencies)
    {
    }

    private bool HasStringChild(SqlBinaryExpression binaryExpression)
    {
      if (binaryExpression.Left != null && ((Expression) binaryExpression.Left).Type == typeof (string) || binaryExpression.Right != null && ((Expression) binaryExpression.Right).Type == typeof (string))
        return true;
      if (binaryExpression.Left != null && binaryExpression.Left is SqlBinaryExpression)
        return this.HasStringChild((SqlBinaryExpression) binaryExpression.Left);
      return binaryExpression.Right != null && binaryExpression.Right is SqlBinaryExpression && this.HasStringChild((SqlBinaryExpression) binaryExpression.Right);
    }

    protected override string GenerateOperator(SqlBinaryExpression binaryExpression) => binaryExpression.OperatorType == ExpressionType.Add && (((Expression) binaryExpression.Left).Type == typeof (string) || ((Expression) binaryExpression.Right).Type == typeof (string) || binaryExpression.Left is SqlBinaryExpression && this.HasStringChild((SqlBinaryExpression) binaryExpression.Left) || binaryExpression.Right is SqlBinaryExpression && this.HasStringChild((SqlBinaryExpression) binaryExpression.Right)) ? "||" : base.GenerateOperator(binaryExpression);

    protected override void GenerateTop(SelectExpression selectExpression)
    {
      if (selectExpression.Limit == null || selectExpression.Offset != null)
        return;
      this.Sql.Append("TOP(");
      ((ExpressionVisitor) this).Visit((Expression) selectExpression.Limit);
      this.Sql.Append(") ");
    }

    protected override void GenerateLimitOffset(SelectExpression selectExpression)
    {
      if (selectExpression.Offset == null)
        return;
      this.Sql.AppendLine().Append("OFFSET ");
      ((ExpressionVisitor) this).Visit((Expression) selectExpression.Offset);
      this.Sql.Append(" ROWS");
      if (selectExpression.Limit != null)
      {
        this.Sql.Append(" FETCH NEXT ");
        ((ExpressionVisitor) this).Visit((Expression) selectExpression.Limit);
        this.Sql.Append(" ROWS ONLY");
      }
    }

    protected override Expression VisitSqlFunction(
      SqlFunctionExpression sqlFunctionExpression)
    {
      if (sqlFunctionExpression.Name.StartsWith("@@", StringComparison.Ordinal))
      {
        this.Sql.Append(sqlFunctionExpression.Name);
        return (Expression) sqlFunctionExpression;
      }
      if (sqlFunctionExpression.Name == "COUNT")
      {
        if (((Expression) sqlFunctionExpression).Type == typeof (int))
        {
          this.Sql.Append(" CAST(");
          base.VisitSqlFunction(sqlFunctionExpression);
          this.Sql.Append(" AS INT) ");
          return (Expression) sqlFunctionExpression;
        }
      }
      else if (sqlFunctionExpression.Name == "SUM")
      {
        if (((Expression) sqlFunctionExpression).Type == typeof (int))
        {
          this.Sql.Append(" CAST(");
          base.VisitSqlFunction(sqlFunctionExpression);
          this.Sql.Append(" AS INT) ");
          return (Expression) sqlFunctionExpression;
        }
      }
      else if ((sqlFunctionExpression.Name == "TRUNCATE" || sqlFunctionExpression.Name == "ROUND") && ((Expression) sqlFunctionExpression).Type == typeof (double))
      {
        this.Sql.Append(" CAST(");
        base.VisitSqlFunction(sqlFunctionExpression);
        this.Sql.Append(" AS DOUBLE)");
        return (Expression) sqlFunctionExpression;
      }
      if (!sqlFunctionExpression.IsBuiltIn && string.IsNullOrEmpty(sqlFunctionExpression.Schema))
        sqlFunctionExpression = new SqlFunctionExpression("SYSDBA", sqlFunctionExpression.Name, (IEnumerable<SqlExpression>) sqlFunctionExpression.Arguments, sqlFunctionExpression.IsNullable, (IEnumerable<bool>) sqlFunctionExpression.ArgumentsPropagateNullability, ((Expression) sqlFunctionExpression).Type, ((SqlExpression) sqlFunctionExpression).TypeMapping);
      return base.VisitSqlFunction(sqlFunctionExpression);
    }

    private static bool RequiresBrackets(SqlExpression expression) => expression is SqlBinaryExpression || expression is LikeExpression;
  }
}
