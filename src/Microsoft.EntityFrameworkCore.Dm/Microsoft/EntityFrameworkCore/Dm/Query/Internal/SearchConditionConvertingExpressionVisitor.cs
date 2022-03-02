using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

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
			return condition ? sqlExpression : BuildCompareToExpression(sqlExpression);
		}

		private Expression ConvertToValue(SqlExpression sqlExpression, bool condition)
		{
			return condition ? _sqlExpressionFactory.Case(new CaseWhenClause[1]
			{
				new CaseWhenClause(sqlExpression, _sqlExpressionFactory.ApplyDefaultTypeMapping(_sqlExpressionFactory.Constant(true)))
			}, _sqlExpressionFactory.Constant(false)) : sqlExpression;
		}

		private SqlExpression BuildCompareToExpression(SqlExpression sqlExpression)
		{
			return _sqlExpressionFactory.Equal(sqlExpression, _sqlExpressionFactory.Constant(true));
		}

		protected override Expression VisitCase(CaseExpression caseExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			bool isSearchCondition2 = caseExpression.Operand == null;
			_isSearchCondition = false;
			SqlExpression operand = (SqlExpression)Visit(caseExpression.Operand);
			List<CaseWhenClause> list = new List<CaseWhenClause>();
			foreach (CaseWhenClause whenClause in caseExpression.WhenClauses)
			{
				_isSearchCondition = isSearchCondition2;
				SqlExpression test = (SqlExpression)Visit(whenClause.Test);
				_isSearchCondition = false;
				SqlExpression result = (SqlExpression)Visit(whenClause.Result);
				list.Add(new CaseWhenClause(test, result));
			}
			_isSearchCondition = false;
			SqlExpression elseResult = (SqlExpression)Visit(caseExpression.ElseResult);
			_isSearchCondition = isSearchCondition;
			return ApplyConversion(caseExpression.Update(operand, list, elseResult), condition: false);
		}

		protected override Expression VisitColumn(ColumnExpression columnExpression)
		{
			return ApplyConversion(columnExpression, condition: false);
		}

		protected override Expression VisitExists(ExistsExpression existsExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SelectExpression subquery = (SelectExpression)Visit(existsExpression.Subquery);
			_isSearchCondition = isSearchCondition;
			return ApplyConversion(existsExpression.Update(subquery), condition: true);
		}

		protected override Expression VisitFromSql(FromSqlExpression fromSqlExpression)
		{
			return fromSqlExpression;
		}

		protected override Expression VisitIn(InExpression inExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SqlExpression item = (SqlExpression)Visit(inExpression.Item);
			SelectExpression subquery = (SelectExpression)Visit(inExpression.Subquery);
			SqlExpression values = (SqlExpression)Visit(inExpression.Values);
			_isSearchCondition = isSearchCondition;
			return ApplyConversion(inExpression.Update(item, values, subquery), condition: true);
		}

		protected override Expression VisitLike(LikeExpression likeExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SqlExpression match = (SqlExpression)Visit(likeExpression.Match);
			SqlExpression pattern = (SqlExpression)Visit(likeExpression.Pattern);
			SqlExpression escapeChar = (SqlExpression)Visit(likeExpression.EscapeChar);
			_isSearchCondition = isSearchCondition;
			return ApplyConversion(likeExpression.Update(match, pattern, escapeChar), condition: true);
		}

		protected override Expression VisitSelect(SelectExpression selectExpression)
		{
			bool flag = false;
			bool isSearchCondition = _isSearchCondition;
			List<ProjectionExpression> list = new List<ProjectionExpression>();
			_isSearchCondition = false;
			foreach (ProjectionExpression item in selectExpression.Projection)
			{
				ProjectionExpression projectionExpression = (ProjectionExpression)Visit(item);
				list.Add(projectionExpression);
				flag = flag || projectionExpression != item;
			}
			List<TableExpressionBase> list2 = new List<TableExpressionBase>();
			foreach (TableExpressionBase table in selectExpression.Tables)
			{
				TableExpressionBase tableExpressionBase = (TableExpressionBase)Visit(table);
				flag = flag || tableExpressionBase != table;
				list2.Add(tableExpressionBase);
			}
			_isSearchCondition = true;
			SqlExpression sqlExpression = (SqlExpression)Visit(selectExpression.Predicate);
			flag |= sqlExpression != selectExpression.Predicate;
			List<SqlExpression> list3 = new List<SqlExpression>();
			_isSearchCondition = false;
			foreach (SqlExpression item2 in selectExpression.GroupBy)
			{
				SqlExpression sqlExpression2 = (SqlExpression)Visit(item2);
				flag = flag || sqlExpression2 != item2;
				list3.Add(sqlExpression2);
			}
			_isSearchCondition = true;
			SqlExpression sqlExpression3 = (SqlExpression)Visit(selectExpression.Having);
			flag |= sqlExpression3 != selectExpression.Having;
			List<OrderingExpression> list4 = new List<OrderingExpression>();
			_isSearchCondition = false;
			foreach (OrderingExpression ordering in selectExpression.Orderings)
			{
				SqlExpression sqlExpression4 = (SqlExpression)Visit(ordering.Expression);
				flag |= sqlExpression4 != ordering.Expression;
				list4.Add(ordering.Update(sqlExpression4));
			}
			SqlExpression sqlExpression5 = (SqlExpression)Visit(selectExpression.Offset);
			flag |= sqlExpression5 != selectExpression.Offset;
			SqlExpression sqlExpression6 = (SqlExpression)Visit(selectExpression.Limit);
			flag |= sqlExpression6 != selectExpression.Limit;
			_isSearchCondition = isSearchCondition;
			return flag ? selectExpression.Update(list, list2, sqlExpression, list3, sqlExpression3, list4, sqlExpression6, sqlExpression5) : selectExpression;
		}

		protected override Expression VisitSqlBinary(SqlBinaryExpression sqlBinaryExpression)
		{
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
			SqlExpression left = (SqlExpression)Visit(sqlBinaryExpression.Left);
			SqlExpression right = (SqlExpression)Visit(sqlBinaryExpression.Right);
			_isSearchCondition = isSearchCondition;
			sqlBinaryExpression = sqlBinaryExpression.Update(left, right);
			bool condition = sqlBinaryExpression.OperatorType == ExpressionType.AndAlso || sqlBinaryExpression.OperatorType == ExpressionType.OrElse || sqlBinaryExpression.OperatorType == ExpressionType.Equal || sqlBinaryExpression.OperatorType == ExpressionType.NotEqual || sqlBinaryExpression.OperatorType == ExpressionType.GreaterThan || sqlBinaryExpression.OperatorType == ExpressionType.GreaterThanOrEqual || sqlBinaryExpression.OperatorType == ExpressionType.LessThan || sqlBinaryExpression.OperatorType == ExpressionType.LessThanOrEqual;
			return ApplyConversion(sqlBinaryExpression, condition);
		}

		protected override Expression VisitSqlUnary(SqlUnaryExpression sqlUnaryExpression)
		{
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
			SqlExpression operand = (SqlExpression)Visit(sqlUnaryExpression.Operand);
			_isSearchCondition = isSearchCondition;
			return ApplyConversion(sqlUnaryExpression.Update(operand), condition);
		}

		protected override Expression VisitSqlConstant(SqlConstantExpression sqlConstantExpression)
		{
			return ApplyConversion(sqlConstantExpression, condition: false);
		}

		protected override Expression VisitSqlFragment(SqlFragmentExpression sqlFragmentExpression)
		{
			return sqlFragmentExpression;
		}

		protected override Expression VisitSqlFunction(SqlFunctionExpression sqlFunctionExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SqlExpression instance = (SqlExpression)Visit(sqlFunctionExpression.Instance);
			SqlExpression[] array = new SqlExpression[sqlFunctionExpression.Arguments.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (SqlExpression)Visit(sqlFunctionExpression.Arguments[i]);
			}
			_isSearchCondition = isSearchCondition;
			SqlFunctionExpression sqlExpression = sqlFunctionExpression.Update(instance, array);
			bool condition = string.Equals(sqlFunctionExpression.Name, "FREETEXT") || string.Equals(sqlFunctionExpression.Name, "CONTAINS");
			return ApplyConversion(sqlExpression, condition);
		}

		protected override Expression VisitSqlParameter(SqlParameterExpression sqlParameterExpression)
		{
			return ApplyConversion(sqlParameterExpression, condition: false);
		}

		protected override Expression VisitTable(TableExpression tableExpression)
		{
			return tableExpression;
		}

		protected override Expression VisitProjection(ProjectionExpression projectionExpression)
		{
			SqlExpression expression = (SqlExpression)Visit(projectionExpression.Expression);
			return projectionExpression.Update(expression);
		}

		protected override Expression VisitOrdering(OrderingExpression orderingExpression)
		{
			SqlExpression expression = (SqlExpression)Visit(orderingExpression.Expression);
			return orderingExpression.Update(expression);
		}

		protected override Expression VisitCrossJoin(CrossJoinExpression crossJoinExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			TableExpressionBase table = (TableExpressionBase)Visit(crossJoinExpression.Table);
			_isSearchCondition = isSearchCondition;
			return crossJoinExpression.Update(table);
		}

		protected override Expression VisitCrossApply(CrossApplyExpression crossApplyExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			TableExpressionBase table = (TableExpressionBase)Visit(crossApplyExpression.Table);
			_isSearchCondition = isSearchCondition;
			return crossApplyExpression.Update(table);
		}

		protected override Expression VisitOuterApply(OuterApplyExpression outerApplyExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			TableExpressionBase table = (TableExpressionBase)Visit(outerApplyExpression.Table);
			_isSearchCondition = isSearchCondition;
			return outerApplyExpression.Update(table);
		}

		protected override Expression VisitInnerJoin(InnerJoinExpression innerJoinExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			TableExpressionBase table = (TableExpressionBase)Visit(innerJoinExpression.Table);
			_isSearchCondition = true;
			SqlExpression joinPredicate = (SqlExpression)Visit(innerJoinExpression.JoinPredicate);
			_isSearchCondition = isSearchCondition;
			return innerJoinExpression.Update(table, joinPredicate);
		}

		protected override Expression VisitLeftJoin(LeftJoinExpression leftJoinExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			TableExpressionBase table = (TableExpressionBase)Visit(leftJoinExpression.Table);
			_isSearchCondition = true;
			SqlExpression joinPredicate = (SqlExpression)Visit(leftJoinExpression.JoinPredicate);
			_isSearchCondition = isSearchCondition;
			return leftJoinExpression.Update(table, joinPredicate);
		}

		protected override Expression VisitScalarSubquery(ScalarSubqueryExpression scalarSubqueryExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			SelectExpression subquery = (SelectExpression)Visit(scalarSubqueryExpression.Subquery);
			_isSearchCondition = isSearchCondition;
			return ApplyConversion(scalarSubqueryExpression.Update(subquery), condition: false);
		}

		protected override Expression VisitRowNumber(RowNumberExpression rowNumberExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			bool flag = false;
			List<SqlExpression> list = new List<SqlExpression>();
			foreach (SqlExpression partition in rowNumberExpression.Partitions)
			{
				SqlExpression sqlExpression = (SqlExpression)Visit(partition);
				flag = flag || sqlExpression != partition;
				list.Add(sqlExpression);
			}
			List<OrderingExpression> list2 = new List<OrderingExpression>();
			foreach (OrderingExpression ordering in rowNumberExpression.Orderings)
			{
				OrderingExpression orderingExpression = (OrderingExpression)Visit(ordering);
				flag = flag || orderingExpression != ordering;
				list2.Add(orderingExpression);
			}
			_isSearchCondition = isSearchCondition;
			return ApplyConversion(rowNumberExpression.Update(list, list2), condition: false);
		}

		protected override Expression VisitExcept(ExceptExpression exceptExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SelectExpression source = (SelectExpression)Visit(exceptExpression.Source1);
			SelectExpression source2 = (SelectExpression)Visit(exceptExpression.Source2);
			_isSearchCondition = isSearchCondition;
			return exceptExpression.Update(source, source2);
		}

		protected override Expression VisitIntersect(IntersectExpression intersectExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SelectExpression source = (SelectExpression)Visit(intersectExpression.Source1);
			SelectExpression source2 = (SelectExpression)Visit(intersectExpression.Source2);
			_isSearchCondition = isSearchCondition;
			return intersectExpression.Update(source, source2);
		}

		protected override Expression VisitUnion(UnionExpression unionExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SelectExpression source = (SelectExpression)Visit(unionExpression.Source1);
			SelectExpression source2 = (SelectExpression)Visit(unionExpression.Source2);
			_isSearchCondition = isSearchCondition;
			return unionExpression.Update(source, source2);
		}

		protected override Expression VisitCollate([JetBrains.Annotations.NotNull] CollateExpression collateExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SqlExpression operand = (SqlExpression)Visit(collateExpression.Operand);
			_isSearchCondition = isSearchCondition;
			return ApplyConversion(collateExpression.Update(operand), condition: false);
		}

		protected override Expression VisitDistinct([JetBrains.Annotations.NotNull] DistinctExpression distinctExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SqlExpression operand = (SqlExpression)Visit(distinctExpression.Operand);
			_isSearchCondition = isSearchCondition;
			return ApplyConversion(distinctExpression.Update(operand), condition: false);
		}

		protected override Expression VisitTableValuedFunction([JetBrains.Annotations.NotNull] TableValuedFunctionExpression tableValuedFunctionExpression)
		{
			bool isSearchCondition = _isSearchCondition;
			_isSearchCondition = false;
			SqlExpression[] array = new SqlExpression[tableValuedFunctionExpression.Arguments.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (SqlExpression)Visit(tableValuedFunctionExpression.Arguments[i]);
			}
			_isSearchCondition = isSearchCondition;
			return tableValuedFunctionExpression.Update(array);
		}
	}
}
