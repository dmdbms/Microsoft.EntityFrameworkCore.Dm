using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

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
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			SqlExpression val = (SqlExpression)base.VisitBinary(binaryExpression);
			if (val == null)
			{
				return null;
			}
			SqlBinaryExpression val2 = (SqlBinaryExpression)(object)((val is SqlBinaryExpression) ? val : null);
			return (Expression)(object)((val2 != null && _arithmeticOperatorTypes.Contains(val2.OperatorType) && (_dateTimeDataTypes.Contains(GetProviderType(val2.Left)) || _dateTimeDataTypes.Contains(GetProviderType(val2.Right)))) ? null : val);
		}

		public override SqlExpression TranslateLongCount(SqlExpression expression = null)
		{
			if (expression != null)
			{
				return base.Dependencies.SqlExpressionFactory.ApplyDefaultTypeMapping((SqlExpression)(object)base.Dependencies.SqlExpressionFactory.Function("COUNT", (IEnumerable<SqlExpression>)(object)new SqlExpression[1] { expression }, false, (IEnumerable<bool>)new bool[1], typeof(long), (RelationalTypeMapping)null));
			}
			return base.Dependencies.SqlExpressionFactory.ApplyDefaultTypeMapping((SqlExpression)(object)base.Dependencies.SqlExpressionFactory.Function("COUNT", (IEnumerable<SqlExpression>)(object)new SqlFragmentExpression[1] { base.Dependencies.SqlExpressionFactory.Fragment("*") }, false, (IEnumerable<bool>)new bool[1], typeof(long), (RelationalTypeMapping)null));
		}

		private static string GetProviderType(SqlExpression expression)
		{
			RelationalTypeMapping typeMapping = expression.TypeMapping;
			return (typeMapping != null) ? typeMapping.StoreType : null;
		}
	}
}
