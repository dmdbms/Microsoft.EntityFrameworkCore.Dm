using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

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
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected O, but got Unknown
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Expected O, but got Unknown
			if (binaryExpression.Left != null && ((Expression)(object)binaryExpression.Left).Type == typeof(string))
			{
				return true;
			}
			if (binaryExpression.Right != null && ((Expression)(object)binaryExpression.Right).Type == typeof(string))
			{
				return true;
			}
			if (binaryExpression.Left != null && binaryExpression.Left is SqlBinaryExpression)
			{
				return HasStringChild((SqlBinaryExpression)binaryExpression.Left);
			}
			if (binaryExpression.Right != null && binaryExpression.Right is SqlBinaryExpression)
			{
				return HasStringChild((SqlBinaryExpression)binaryExpression.Right);
			}
			return false;
		}

		protected override string GenerateOperator(SqlBinaryExpression binaryExpression)
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Expected O, but got Unknown
			if (binaryExpression.OperatorType == ExpressionType.Add)
			{
				if (((Expression)(object)binaryExpression.Left).Type == typeof(string) || ((Expression)(object)binaryExpression.Right).Type == typeof(string))
				{
					return "||";
				}
				if (binaryExpression.Left is SqlBinaryExpression && HasStringChild((SqlBinaryExpression)binaryExpression.Left))
				{
					return "||";
				}
				if (binaryExpression.Right is SqlBinaryExpression && HasStringChild((SqlBinaryExpression)binaryExpression.Right))
				{
					return "||";
				}
			}
			return base.GenerateOperator(binaryExpression);
		}

		protected override void GenerateTop(SelectExpression selectExpression)
		{
			if (selectExpression.Limit != null && selectExpression.Offset == null)
			{
				base.Sql.Append("TOP(");
				((ExpressionVisitor)(object)this).Visit((Expression?)(object)selectExpression.Limit);
				base.Sql.Append(") ");
			}
		}

		protected override void GenerateLimitOffset(SelectExpression selectExpression)
		{
			if (selectExpression.Offset != null)
			{
				base.Sql.AppendLine().Append("OFFSET ");
				((ExpressionVisitor)(object)this).Visit((Expression?)(object)selectExpression.Offset);
				base.Sql.Append(" ROWS");
				if (selectExpression.Limit != null)
				{
					base.Sql.Append(" FETCH NEXT ");
					((ExpressionVisitor)(object)this).Visit((Expression?)(object)selectExpression.Limit);
					base.Sql.Append(" ROWS ONLY");
				}
			}
		}

		protected override Expression VisitSqlFunction(SqlFunctionExpression sqlFunctionExpression)
		{
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Expected O, but got Unknown
			if (sqlFunctionExpression.Name.StartsWith("@@", StringComparison.Ordinal))
			{
				base.Sql.Append(sqlFunctionExpression.Name);
				return (Expression)(object)sqlFunctionExpression;
			}
			if (sqlFunctionExpression.Name == "COUNT")
			{
				if (((Expression)(object)sqlFunctionExpression).Type == typeof(int))
				{
					base.Sql.Append(" CAST(");
					base.VisitSqlFunction(sqlFunctionExpression);
					base.Sql.Append(" AS INT) ");
					return (Expression)(object)sqlFunctionExpression;
				}
			}
			else if (sqlFunctionExpression.Name == "SUM")
			{
				if (((Expression)(object)sqlFunctionExpression).Type == typeof(int))
				{
					base.Sql.Append(" CAST(");
					base.VisitSqlFunction(sqlFunctionExpression);
					base.Sql.Append(" AS INT) ");
					return (Expression)(object)sqlFunctionExpression;
				}
			}
			else if ((sqlFunctionExpression.Name == "TRUNCATE" || sqlFunctionExpression.Name == "ROUND") && ((Expression)(object)sqlFunctionExpression).Type == typeof(double))
			{
				base.Sql.Append(" CAST(");
				base.VisitSqlFunction(sqlFunctionExpression);
				base.Sql.Append(" AS DOUBLE)");
				return (Expression)(object)sqlFunctionExpression;
			}
			if (!sqlFunctionExpression.IsBuiltIn && string.IsNullOrEmpty(sqlFunctionExpression.Schema))
			{
				sqlFunctionExpression = new SqlFunctionExpression("SYSDBA", sqlFunctionExpression.Name, (IEnumerable<SqlExpression>)sqlFunctionExpression.Arguments, sqlFunctionExpression.IsNullable, (IEnumerable<bool>)sqlFunctionExpression.ArgumentsPropagateNullability, ((Expression)(object)sqlFunctionExpression).Type, ((SqlExpression)sqlFunctionExpression).TypeMapping);
			}
			return base.VisitSqlFunction(sqlFunctionExpression);
		}

		private static bool RequiresBrackets(SqlExpression expression)
		{
			return expression is SqlBinaryExpression || expression is LikeExpression;
		}
	}
}
