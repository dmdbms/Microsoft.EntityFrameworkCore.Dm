using System;
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
			if (binaryExpression.Left != null && binaryExpression.Left.Type == typeof(string))
			{
				return true;
			}
			if (binaryExpression.Right != null && binaryExpression.Right.Type == typeof(string))
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
			if (binaryExpression.OperatorType == ExpressionType.Add)
			{
				if (binaryExpression.Left.Type == typeof(string) || binaryExpression.Right.Type == typeof(string))
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
				Sql.Append("TOP(");
				Visit(selectExpression.Limit);
				Sql.Append(") ");
			}
		}

		protected override void GenerateLimitOffset(SelectExpression selectExpression)
		{
			if (selectExpression.Offset != null)
			{
				Sql.AppendLine().Append("OFFSET ");
				Visit(selectExpression.Offset);
				Sql.Append(" ROWS");
				if (selectExpression.Limit != null)
				{
					Sql.Append(" FETCH NEXT ");
					Visit(selectExpression.Limit);
					Sql.Append(" ROWS ONLY");
				}
			}
		}

		protected override Expression VisitSqlFunction(SqlFunctionExpression sqlFunctionExpression)
		{
			if (sqlFunctionExpression.Name.StartsWith("@@", StringComparison.Ordinal))
			{
				Sql.Append(sqlFunctionExpression.Name);
				return sqlFunctionExpression;
			}
			if (sqlFunctionExpression.Name == "COUNT")
			{
				if (sqlFunctionExpression.Type == typeof(int))
				{
					Sql.Append(" CAST(");
					base.VisitSqlFunction(sqlFunctionExpression);
					Sql.Append(" AS INT) ");
					return sqlFunctionExpression;
				}
			}
			else if (sqlFunctionExpression.Name == "SUM")
			{
				if (sqlFunctionExpression.Type == typeof(int))
				{
					Sql.Append(" CAST(");
					base.VisitSqlFunction(sqlFunctionExpression);
					Sql.Append(" AS INT) ");
					return sqlFunctionExpression;
				}
			}
			else if ((sqlFunctionExpression.Name == "TRUNCATE" || sqlFunctionExpression.Name == "ROUND") && sqlFunctionExpression.Type == typeof(double))
			{
				Sql.Append(" CAST(");
				base.VisitSqlFunction(sqlFunctionExpression);
				Sql.Append(" AS DOUBLE)");
				return sqlFunctionExpression;
			}
			if (!sqlFunctionExpression.IsBuiltIn && string.IsNullOrEmpty(sqlFunctionExpression.Schema))
			{
				sqlFunctionExpression = new SqlFunctionExpression("SYSDBA", sqlFunctionExpression.Name, sqlFunctionExpression.Arguments, sqlFunctionExpression.IsNullable, sqlFunctionExpression.ArgumentsPropagateNullability, sqlFunctionExpression.Type, sqlFunctionExpression.TypeMapping);
			}
			return base.VisitSqlFunction(sqlFunctionExpression);
		}

		private static bool RequiresBrackets(SqlExpression expression)
		{
			return expression is SqlBinaryExpression || expression is LikeExpression;
		}
	}
}
