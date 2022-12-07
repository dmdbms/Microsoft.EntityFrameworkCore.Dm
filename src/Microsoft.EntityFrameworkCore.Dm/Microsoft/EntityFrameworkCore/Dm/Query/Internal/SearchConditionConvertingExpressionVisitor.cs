using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class SearchConditionConvertingExpressionVisitor : SqlExpressionVisitor
	{
		private bool _isSearchCondition;

		private readonly ISqlExpressionFactory _sqlExpressionFactory;

		public SearchConditionConvertingExpressionVisitor(ISqlExpressionFactory sqlExpressionFactory)
		{
			_sqlExpressionFactory = sqlExpressionFactory;
		}

		private Expression ApplyConversion(SqlExpression sqlExpression, bool condition)
		{
			return _isSearchCondition ? ConvertToSearchCondition(sqlExpression, condition) : ConvertToValue(sqlExpression, condition);
		}

		private Expression ConvertToSearchCondition(SqlExpression sqlExpression, bool condition)
		{
			return (Expression)(object)(condition ? sqlExpression : BuildCompareToExpression(sqlExpression));
		}

		private Expression ConvertToValue(SqlExpression sqlExpression, bool condition)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			return condition ? ((Expression)(object)_sqlExpressionFactory.Case((IReadOnlyList<CaseWhenClause>)(object)new CaseWhenClause[1]
			{
				new CaseWhenClause(sqlExpression, _sqlExpressionFactory.ApplyDefaultTypeMapping((SqlExpression)(object)_sqlExpressionFactory.Constant((object)true, (RelationalTypeMapping)null)))
			}, (SqlExpression)(object)_sqlExpressionFactory.Constant((object)false, (RelationalTypeMapping)null))) : ((Expression)(object)sqlExpression);
		}

		private SqlExpression BuildCompareToExpression(SqlExpression sqlExpression)
		{
			return (SqlExpression)(object)_sqlExpressionFactory.Equal(sqlExpression, (SqlExpression)(object)_sqlExpressionFactory.Constant((object)true, (RelationalTypeMapping)null));
		}

		protected override Expression VisitCase(CaseExpression caseExpression)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Expected O, but got Unknown
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			bool isSearchCondition2 = caseExpression.Operand == null;
			_isSearchCondition = false;
			SqlExpression val = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)caseExpression.Operand);
			List<CaseWhenClause> list = new List<CaseWhenClause>();
			foreach (CaseWhenClause whenClause in caseExpression.WhenClauses)
			{
				_isSearchCondition = isSearchCondition2;
				SqlExpression val2 = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)whenClause.Test);
				_isSearchCondition = false;
				SqlExpression val3 = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)whenClause.Result);
				list.Add(new CaseWhenClause(val2, val3));
			}
			_isSearchCondition = false;
			SqlExpression val4 = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)caseExpression.ElseResult);
			_isSearchCondition = isSearchCondition;
			return ApplyConversion((SqlExpression)(object)caseExpression.Update(val, (IReadOnlyList<CaseWhenClause>)list, val4), condition: false);
		}

		protected override Expression VisitColumn(ColumnExpression columnExpression)
		{
			return ApplyConversion((SqlExpression)(object)columnExpression, condition: false);
		}

		protected override Expression VisitExists(ExistsExpression existsExpression)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SelectExpression val = (SelectExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)existsExpression.Subquery);
			_isSearchCondition = isSearchCondition;
			return ApplyConversion((SqlExpression)(object)existsExpression.Update(val), condition: true);
		}

		protected override Expression VisitFromSql(FromSqlExpression fromSqlExpression)
		{
			return (Expression)(object)fromSqlExpression;
		}

		protected override Expression VisitIn(InExpression inExpression)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SqlExpression val = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)inExpression.Item);
			SelectExpression val2 = (SelectExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)inExpression.Subquery);
			SqlExpression val3 = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)inExpression.Values);
			_isSearchCondition = isSearchCondition;
			return ApplyConversion((SqlExpression)(object)inExpression.Update(val, val3, val2), condition: true);
		}

		protected override Expression VisitLike(LikeExpression likeExpression)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SqlExpression val = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)likeExpression.Match);
			SqlExpression val2 = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)likeExpression.Pattern);
			SqlExpression val3 = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)likeExpression.EscapeChar);
			_isSearchCondition = isSearchCondition;
			return ApplyConversion((SqlExpression)(object)likeExpression.Update(val, val2, val3), condition: true);
		}

		protected override Expression VisitSelect(SelectExpression selectExpression)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Expected O, but got Unknown
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Expected O, but got Unknown
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Expected O, but got Unknown
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Expected O, but got Unknown
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Expected O, but got Unknown
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Expected O, but got Unknown
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Expected O, but got Unknown
			bool flag = false;
			bool isSearchCondition = _isSearchCondition;
			List<ProjectionExpression> list = new List<ProjectionExpression>();
			_isSearchCondition = false;
			foreach (ProjectionExpression item in selectExpression.Projection)
			{
				ProjectionExpression val = (ProjectionExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)item);
				list.Add(val);
				flag = flag || val != item;
			}
			List<TableExpressionBase> list2 = new List<TableExpressionBase>();
			foreach (TableExpressionBase table in selectExpression.Tables)
			{
				TableExpressionBase val2 = (TableExpressionBase)((ExpressionVisitor)(object)this).Visit((Expression?)(object)table);
				flag = flag || val2 != table;
				list2.Add(val2);
			}
			_isSearchCondition = true;
			SqlExpression val3 = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)selectExpression.Predicate);
			flag |= val3 != selectExpression.Predicate;
			List<SqlExpression> list3 = new List<SqlExpression>();
			_isSearchCondition = false;
			foreach (SqlExpression item2 in selectExpression.GroupBy)
			{
				SqlExpression val4 = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)item2);
				flag = flag || val4 != item2;
				list3.Add(val4);
			}
			_isSearchCondition = true;
			SqlExpression val5 = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)selectExpression.Having);
			flag |= val5 != selectExpression.Having;
			List<OrderingExpression> list4 = new List<OrderingExpression>();
			_isSearchCondition = false;
			foreach (OrderingExpression ordering in selectExpression.Orderings)
			{
				SqlExpression val6 = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)ordering.Expression);
				flag |= val6 != ordering.Expression;
				list4.Add(ordering.Update(val6));
			}
			SqlExpression val7 = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)selectExpression.Offset);
			flag |= val7 != selectExpression.Offset;
			SqlExpression val8 = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)selectExpression.Limit);
			flag |= val8 != selectExpression.Limit;
			_isSearchCondition = isSearchCondition;
			return flag ? ((Expression)(object)selectExpression.Update((IReadOnlyList<ProjectionExpression>)list, (IReadOnlyList<TableExpressionBase>)list2, val3, (IReadOnlyList<SqlExpression>)list3, val5, (IReadOnlyList<OrderingExpression>)list4, val8, val7)) : ((Expression)(object)selectExpression);
		}

		protected override Expression VisitSqlBinary(SqlBinaryExpression sqlBinaryExpression)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			ExpressionType operatorType = sqlBinaryExpression.OperatorType;
			ExpressionType expressionType = operatorType;
			if (expressionType == ExpressionType.AndAlso || expressionType == ExpressionType.OrElse)
			{
				_isSearchCondition = true;
			}
			else
			{
				_isSearchCondition = false;
			}
			SqlExpression val = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)sqlBinaryExpression.Left);
			SqlExpression val2 = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)sqlBinaryExpression.Right);
			_isSearchCondition = isSearchCondition;
			sqlBinaryExpression = sqlBinaryExpression.Update(val, val2);
			bool condition = sqlBinaryExpression.OperatorType == ExpressionType.AndAlso || sqlBinaryExpression.OperatorType == ExpressionType.OrElse || sqlBinaryExpression.OperatorType == ExpressionType.Equal || sqlBinaryExpression.OperatorType == ExpressionType.NotEqual || sqlBinaryExpression.OperatorType == ExpressionType.GreaterThan || sqlBinaryExpression.OperatorType == ExpressionType.GreaterThanOrEqual || sqlBinaryExpression.OperatorType == ExpressionType.LessThan || sqlBinaryExpression.OperatorType == ExpressionType.LessThanOrEqual;
			return ApplyConversion((SqlExpression)(object)sqlBinaryExpression, condition);
		}

		protected override Expression VisitSqlUnary(SqlUnaryExpression sqlUnaryExpression)
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			bool condition;
			switch (sqlUnaryExpression.OperatorType)
			{
			case ExpressionType.Not:
				_isSearchCondition = true;
				condition = true;
				break;
			case ExpressionType.Convert:
			case ExpressionType.Negate:
				_isSearchCondition = false;
				condition = false;
				break;
			case ExpressionType.Equal:
			case ExpressionType.NotEqual:
				_isSearchCondition = false;
				condition = true;
				break;
			default:
				throw new InvalidOperationException("Unknown operator type encountered in SqlUnaryExpression.");
			}
			SqlExpression val = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)sqlUnaryExpression.Operand);
			_isSearchCondition = isSearchCondition;
			return ApplyConversion((SqlExpression)(object)sqlUnaryExpression.Update(val), condition);
		}

		protected override Expression VisitSqlConstant(SqlConstantExpression sqlConstantExpression)
		{
			return ApplyConversion((SqlExpression)(object)sqlConstantExpression, condition: false);
		}

		protected override Expression VisitSqlFragment(SqlFragmentExpression sqlFragmentExpression)
		{
			return (Expression)(object)sqlFragmentExpression;
		}

		protected override Expression VisitSqlFunction(SqlFunctionExpression sqlFunctionExpression)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SqlExpression val = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)sqlFunctionExpression.Instance);
			SqlExpression[] array = (SqlExpression[])(object)new SqlExpression[sqlFunctionExpression.Arguments.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)sqlFunctionExpression.Arguments[i]);
			}
			_isSearchCondition = isSearchCondition;
			SqlFunctionExpression sqlExpression = sqlFunctionExpression.Update(val, (IReadOnlyList<SqlExpression>)array);
			bool condition = string.Equals(sqlFunctionExpression.Name, "FREETEXT") || string.Equals(sqlFunctionExpression.Name, "CONTAINS");
			return ApplyConversion((SqlExpression)(object)sqlExpression, condition);
		}

		protected override Expression VisitSqlParameter(SqlParameterExpression sqlParameterExpression)
		{
			return ApplyConversion((SqlExpression)(object)sqlParameterExpression, condition: false);
		}

		protected override Expression VisitTable(TableExpression tableExpression)
		{
			return (Expression)(object)tableExpression;
		}

		protected override Expression VisitProjection(ProjectionExpression projectionExpression)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			SqlExpression val = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)projectionExpression.Expression);
			return (Expression)(object)projectionExpression.Update(val);
		}

		protected override Expression VisitOrdering(OrderingExpression orderingExpression)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			SqlExpression val = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)orderingExpression.Expression);
			return (Expression)(object)orderingExpression.Update(val);
		}

		protected override Expression VisitCrossJoin(CrossJoinExpression crossJoinExpression)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			TableExpressionBase val = (TableExpressionBase)((ExpressionVisitor)(object)this).Visit((Expression?)(object)((JoinExpressionBase)crossJoinExpression).Table);
			_isSearchCondition = isSearchCondition;
			return (Expression)(object)crossJoinExpression.Update(val);
		}

		protected override Expression VisitCrossApply(CrossApplyExpression crossApplyExpression)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			TableExpressionBase val = (TableExpressionBase)((ExpressionVisitor)(object)this).Visit((Expression?)(object)((JoinExpressionBase)crossApplyExpression).Table);
			_isSearchCondition = isSearchCondition;
			return (Expression)(object)crossApplyExpression.Update(val);
		}

		protected override Expression VisitOuterApply(OuterApplyExpression outerApplyExpression)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			TableExpressionBase val = (TableExpressionBase)((ExpressionVisitor)(object)this).Visit((Expression?)(object)((JoinExpressionBase)outerApplyExpression).Table);
			_isSearchCondition = isSearchCondition;
			return (Expression)(object)outerApplyExpression.Update(val);
		}

		protected override Expression VisitInnerJoin(InnerJoinExpression innerJoinExpression)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			TableExpressionBase val = (TableExpressionBase)((ExpressionVisitor)(object)this).Visit((Expression?)(object)((JoinExpressionBase)innerJoinExpression).Table);
			_isSearchCondition = true;
			SqlExpression val2 = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)((PredicateJoinExpressionBase)innerJoinExpression).JoinPredicate);
			_isSearchCondition = isSearchCondition;
			return (Expression)(object)innerJoinExpression.Update(val, val2);
		}

		protected override Expression VisitLeftJoin(LeftJoinExpression leftJoinExpression)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			TableExpressionBase val = (TableExpressionBase)((ExpressionVisitor)(object)this).Visit((Expression?)(object)((JoinExpressionBase)leftJoinExpression).Table);
			_isSearchCondition = true;
			SqlExpression val2 = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)((PredicateJoinExpressionBase)leftJoinExpression).JoinPredicate);
			_isSearchCondition = isSearchCondition;
			return (Expression)(object)leftJoinExpression.Update(val, val2);
		}

		protected override Expression VisitScalarSubquery(ScalarSubqueryExpression scalarSubqueryExpression)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			SelectExpression val = (SelectExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)scalarSubqueryExpression.Subquery);
			_isSearchCondition = isSearchCondition;
			return ApplyConversion((SqlExpression)(object)scalarSubqueryExpression.Update(val), condition: false);
		}

		protected override Expression VisitRowNumber(RowNumberExpression rowNumberExpression)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			bool flag = false;
			List<SqlExpression> list = new List<SqlExpression>();
			foreach (SqlExpression partition in rowNumberExpression.Partitions)
			{
				SqlExpression val = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)partition);
				flag = flag || val != partition;
				list.Add(val);
			}
			List<OrderingExpression> list2 = new List<OrderingExpression>();
			foreach (OrderingExpression ordering in rowNumberExpression.Orderings)
			{
				OrderingExpression val2 = (OrderingExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)ordering);
				flag = flag || val2 != ordering;
				list2.Add(val2);
			}
			_isSearchCondition = isSearchCondition;
			return ApplyConversion((SqlExpression)(object)rowNumberExpression.Update((IReadOnlyList<SqlExpression>)list, (IReadOnlyList<OrderingExpression>)list2), condition: false);
		}

		protected override Expression VisitExcept(ExceptExpression exceptExpression)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SelectExpression val = (SelectExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)((SetOperationBase)exceptExpression).Source1);
			SelectExpression val2 = (SelectExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)((SetOperationBase)exceptExpression).Source2);
			_isSearchCondition = isSearchCondition;
			return (Expression)(object)exceptExpression.Update(val, val2);
		}

		protected override Expression VisitIntersect(IntersectExpression intersectExpression)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SelectExpression val = (SelectExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)((SetOperationBase)intersectExpression).Source1);
			SelectExpression val2 = (SelectExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)((SetOperationBase)intersectExpression).Source2);
			_isSearchCondition = isSearchCondition;
			return (Expression)(object)intersectExpression.Update(val, val2);
		}

		protected override Expression VisitUnion(UnionExpression unionExpression)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SelectExpression val = (SelectExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)((SetOperationBase)unionExpression).Source1);
			SelectExpression val2 = (SelectExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)((SetOperationBase)unionExpression).Source2);
			_isSearchCondition = isSearchCondition;
			return (Expression)(object)unionExpression.Update(val, val2);
		}

		protected override Expression VisitCollate([NotNull] CollateExpression collateExpression)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SqlExpression val = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)collateExpression.Operand);
			_isSearchCondition = isSearchCondition;
			return ApplyConversion((SqlExpression)(object)collateExpression.Update(val), condition: false);
		}

		protected override Expression VisitDistinct([NotNull] DistinctExpression distinctExpression)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SqlExpression val = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)distinctExpression.Operand);
			_isSearchCondition = isSearchCondition;
			return ApplyConversion((SqlExpression)(object)distinctExpression.Update(val), condition: false);
		}

		protected override Expression VisitTableValuedFunction([NotNull] TableValuedFunctionExpression tableValuedFunctionExpression)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SqlExpression[] array = (SqlExpression[])(object)new SqlExpression[tableValuedFunctionExpression.Arguments.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (SqlExpression)((ExpressionVisitor)(object)this).Visit((Expression?)(object)tableValuedFunctionExpression.Arguments[i]);
			}
			_isSearchCondition = isSearchCondition;
			return (Expression)(object)tableValuedFunctionExpression.Update((IReadOnlyList<SqlExpression>)array);
		}
	}
}
