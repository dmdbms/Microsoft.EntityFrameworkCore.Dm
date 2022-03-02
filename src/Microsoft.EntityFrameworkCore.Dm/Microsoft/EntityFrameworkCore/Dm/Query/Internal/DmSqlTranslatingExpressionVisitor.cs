using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmSqlTranslatingExpressionVisitor : RelationalSqlTranslatingExpressionVisitor
	{
		private static readonly HashSet<string> _dateTimeDataTypes = new HashSet<string> { "time", "date", "datetime", "datetime2", "datetimeoffset" };

		private static readonly HashSet<ExpressionType> _arithmeticOperatorTypes = new HashSet<ExpressionType>
		{
			ExpressionType.Add,
			ExpressionType.Subtract,
			ExpressionType.Multiply,
			ExpressionType.Divide,
			ExpressionType.Modulo
		};

		public DmSqlTranslatingExpressionVisitor(RelationalSqlTranslatingExpressionVisitorDependencies dependencies, QueryCompilationContext queryCompilationContext, QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor)
			: base(dependencies, queryCompilationContext, queryableMethodTranslatingExpressionVisitor)
		{
		}

		protected override Expression VisitBinary(BinaryExpression binaryExpression)
		{
			SqlExpression sqlExpression = (SqlExpression)base.VisitBinary(binaryExpression);
			if (sqlExpression == null)
			{
				return null;
			}
			SqlBinaryExpression sqlBinaryExpression = sqlExpression as SqlBinaryExpression;
			return (sqlBinaryExpression != null && _arithmeticOperatorTypes.Contains(sqlBinaryExpression.OperatorType) && (_dateTimeDataTypes.Contains(GetProviderType(sqlBinaryExpression.Left)) || _dateTimeDataTypes.Contains(GetProviderType(sqlBinaryExpression.Right)))) ? null : sqlExpression;
		}

		public override SqlExpression TranslateLongCount(SqlExpression expression = null)
		{
			if (expression != null)
			{
				return Dependencies.SqlExpressionFactory.ApplyDefaultTypeMapping(Dependencies.SqlExpressionFactory.Function("COUNT", new SqlExpression[1] { expression }, nullable: false, new bool[1], typeof(long)));
			}
			return Dependencies.SqlExpressionFactory.ApplyDefaultTypeMapping(Dependencies.SqlExpressionFactory.Function("COUNT", new SqlFragmentExpression[1] { Dependencies.SqlExpressionFactory.Fragment("*") }, nullable: false, new bool[1], typeof(long)));
		}

		private static string GetProviderType(SqlExpression expression)
		{
			return expression.TypeMapping?.StoreType;
		}
	}
}
