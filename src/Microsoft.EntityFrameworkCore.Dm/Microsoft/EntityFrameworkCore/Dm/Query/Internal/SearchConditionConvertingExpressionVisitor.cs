// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.SearchConditionConvertingExpressionVisitor
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class SearchConditionConvertingExpressionVisitor : SqlExpressionVisitor
  {
    private bool _isSearchCondition;
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public SearchConditionConvertingExpressionVisitor(ISqlExpressionFactory sqlExpressionFactory) => this._sqlExpressionFactory = sqlExpressionFactory;

    private Expression ApplyConversion(SqlExpression sqlExpression, bool condition) => !this._isSearchCondition ? this.ConvertToValue(sqlExpression, condition) : this.ConvertToSearchCondition(sqlExpression, condition);

    private Expression ConvertToSearchCondition(
      SqlExpression sqlExpression,
      bool condition)
    {
      return !condition ? (Expression) this.BuildCompareToExpression(sqlExpression) : (Expression) sqlExpression;
    }

    private Expression ConvertToValue(SqlExpression sqlExpression, bool condition)
    {
      CaseExpression caseExpression;
      if (!condition)
        caseExpression = (CaseExpression) sqlExpression;
      else
        caseExpression = this._sqlExpressionFactory.Case((IReadOnlyList<CaseWhenClause>) new CaseWhenClause[1]
        {
          new CaseWhenClause(sqlExpression, this._sqlExpressionFactory.ApplyDefaultTypeMapping((SqlExpression) this._sqlExpressionFactory.Constant((object) true, (RelationalTypeMapping) null)))
        }, (SqlExpression) this._sqlExpressionFactory.Constant((object) false, (RelationalTypeMapping) null));
      return (Expression) caseExpression;
    }

    private SqlExpression BuildCompareToExpression(SqlExpression sqlExpression) => (SqlExpression) this._sqlExpressionFactory.Equal(sqlExpression, (SqlExpression) this._sqlExpressionFactory.Constant((object) true, (RelationalTypeMapping) null));

    protected override Expression VisitCase(CaseExpression caseExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      bool flag = caseExpression.Operand == null;
      this._isSearchCondition = false;
      SqlExpression sqlExpression1 = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) caseExpression.Operand);
      List<CaseWhenClause> caseWhenClauseList = new List<CaseWhenClause>();
      foreach (CaseWhenClause whenClause in (IEnumerable<CaseWhenClause>) caseExpression.WhenClauses)
      {
        this._isSearchCondition = flag;
        SqlExpression sqlExpression2 = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) whenClause.Test);
        this._isSearchCondition = false;
        SqlExpression sqlExpression3 = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) whenClause.Result);
        caseWhenClauseList.Add(new CaseWhenClause(sqlExpression2, sqlExpression3));
      }
      this._isSearchCondition = false;
      SqlExpression sqlExpression4 = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) caseExpression.ElseResult);
      this._isSearchCondition = isSearchCondition;
      return this.ApplyConversion((SqlExpression) caseExpression.Update(sqlExpression1, (IReadOnlyList<CaseWhenClause>) caseWhenClauseList, sqlExpression4), false);
    }

    protected override Expression VisitColumn(ColumnExpression columnExpression) => this.ApplyConversion((SqlExpression) columnExpression, false);

    protected override Expression VisitExists(ExistsExpression existsExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      this._isSearchCondition = false;
      SelectExpression selectExpression = (SelectExpression) ((ExpressionVisitor) this).Visit((Expression) existsExpression.Subquery);
      this._isSearchCondition = isSearchCondition;
      return this.ApplyConversion((SqlExpression) existsExpression.Update(selectExpression), true);
    }

    protected override Expression VisitFromSql(FromSqlExpression fromSqlExpression) => (Expression) fromSqlExpression;

    protected override Expression VisitIn(InExpression inExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      this._isSearchCondition = false;
      SqlExpression sqlExpression1 = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) inExpression.Item);
      SelectExpression selectExpression = (SelectExpression) ((ExpressionVisitor) this).Visit((Expression) inExpression.Subquery);
      SqlExpression sqlExpression2 = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) inExpression.Values);
      this._isSearchCondition = isSearchCondition;
      return this.ApplyConversion((SqlExpression) inExpression.Update(sqlExpression1, sqlExpression2, selectExpression), true);
    }

    protected override Expression VisitLike(LikeExpression likeExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      this._isSearchCondition = false;
      SqlExpression sqlExpression1 = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) likeExpression.Match);
      SqlExpression sqlExpression2 = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) likeExpression.Pattern);
      SqlExpression sqlExpression3 = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) likeExpression.EscapeChar);
      this._isSearchCondition = isSearchCondition;
      return this.ApplyConversion((SqlExpression) likeExpression.Update(sqlExpression1, sqlExpression2, sqlExpression3), true);
    }

    protected override Expression VisitSelect(SelectExpression selectExpression)
    {
      bool flag1 = false;
      bool isSearchCondition = this._isSearchCondition;
      List<ProjectionExpression> projectionExpressionList = new List<ProjectionExpression>();
      this._isSearchCondition = false;
      foreach (ProjectionExpression node in (IEnumerable<ProjectionExpression>) selectExpression.Projection)
      {
        ProjectionExpression projectionExpression = (ProjectionExpression) ((ExpressionVisitor) this).Visit((Expression) node);
        projectionExpressionList.Add(projectionExpression);
        flag1 |= projectionExpression != node;
      }
      List<TableExpressionBase> tableExpressionBaseList = new List<TableExpressionBase>();
      foreach (TableExpressionBase table in (IEnumerable<TableExpressionBase>) selectExpression.Tables)
      {
        TableExpressionBase tableExpressionBase = (TableExpressionBase) ((ExpressionVisitor) this).Visit((Expression) table);
        flag1 |= tableExpressionBase != table;
        tableExpressionBaseList.Add(tableExpressionBase);
      }
      this._isSearchCondition = true;
      SqlExpression sqlExpression1 = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) selectExpression.Predicate);
      bool flag2 = flag1 | sqlExpression1 != selectExpression.Predicate;
      List<SqlExpression> sqlExpressionList = new List<SqlExpression>();
      this._isSearchCondition = false;
      foreach (SqlExpression node in (IEnumerable<SqlExpression>) selectExpression.GroupBy)
      {
        SqlExpression sqlExpression2 = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) node);
        flag2 |= sqlExpression2 != node;
        sqlExpressionList.Add(sqlExpression2);
      }
      this._isSearchCondition = true;
      SqlExpression sqlExpression3 = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) selectExpression.Having);
      bool flag3 = flag2 | sqlExpression3 != selectExpression.Having;
      List<OrderingExpression> orderingExpressionList = new List<OrderingExpression>();
      this._isSearchCondition = false;
      foreach (OrderingExpression ordering in (IEnumerable<OrderingExpression>) selectExpression.Orderings)
      {
        SqlExpression sqlExpression4 = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) ordering.Expression);
        flag3 |= sqlExpression4 != ordering.Expression;
        orderingExpressionList.Add(ordering.Update(sqlExpression4));
      }
      SqlExpression sqlExpression5 = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) selectExpression.Offset);
      bool flag4 = flag3 | sqlExpression5 != selectExpression.Offset;
      SqlExpression sqlExpression6 = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) selectExpression.Limit);
      bool flag5 = flag4 | sqlExpression6 != selectExpression.Limit;
      this._isSearchCondition = isSearchCondition;
      return flag5 ? (Expression) selectExpression.Update((IReadOnlyList<ProjectionExpression>) projectionExpressionList, (IReadOnlyList<TableExpressionBase>) tableExpressionBaseList, sqlExpression1, (IReadOnlyList<SqlExpression>) sqlExpressionList, sqlExpression3, (IReadOnlyList<OrderingExpression>) orderingExpressionList, sqlExpression6, sqlExpression5) : (Expression) selectExpression;
    }

    protected override Expression VisitSqlBinary(SqlBinaryExpression sqlBinaryExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      switch (sqlBinaryExpression.OperatorType)
      {
        case ExpressionType.AndAlso:
        case ExpressionType.OrElse:
          this._isSearchCondition = true;
          break;
        default:
          this._isSearchCondition = false;
          break;
      }
      SqlExpression sqlExpression1 = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) sqlBinaryExpression.Left);
      SqlExpression sqlExpression2 = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) sqlBinaryExpression.Right);
      this._isSearchCondition = isSearchCondition;
      sqlBinaryExpression = sqlBinaryExpression.Update(sqlExpression1, sqlExpression2);
      bool condition = sqlBinaryExpression.OperatorType == ExpressionType.AndAlso || sqlBinaryExpression.OperatorType == ExpressionType.OrElse || sqlBinaryExpression.OperatorType == ExpressionType.Equal || sqlBinaryExpression.OperatorType == ExpressionType.NotEqual || sqlBinaryExpression.OperatorType == ExpressionType.GreaterThan || sqlBinaryExpression.OperatorType == ExpressionType.GreaterThanOrEqual || sqlBinaryExpression.OperatorType == ExpressionType.LessThan || sqlBinaryExpression.OperatorType == ExpressionType.LessThanOrEqual;
      return this.ApplyConversion((SqlExpression) sqlBinaryExpression, condition);
    }

    protected override Expression VisitSqlUnary(SqlUnaryExpression sqlUnaryExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      bool condition;
      switch (sqlUnaryExpression.OperatorType)
      {
        case ExpressionType.Convert:
        case ExpressionType.Negate:
          this._isSearchCondition = false;
          condition = false;
          break;
        case ExpressionType.Equal:
        case ExpressionType.NotEqual:
          this._isSearchCondition = false;
          condition = true;
          break;
        case ExpressionType.Not:
          this._isSearchCondition = true;
          condition = true;
          break;
        default:
          throw new InvalidOperationException("Unknown operator type encountered in SqlUnaryExpression.");
      }
      SqlExpression sqlExpression = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) sqlUnaryExpression.Operand);
      this._isSearchCondition = isSearchCondition;
      return this.ApplyConversion((SqlExpression) sqlUnaryExpression.Update(sqlExpression), condition);
    }

    protected override Expression VisitSqlConstant(
      SqlConstantExpression sqlConstantExpression)
    {
      return this.ApplyConversion((SqlExpression) sqlConstantExpression, false);
    }

    protected override Expression VisitSqlFragment(
      SqlFragmentExpression sqlFragmentExpression)
    {
      return (Expression) sqlFragmentExpression;
    }

    protected override Expression VisitSqlFunction(
      SqlFunctionExpression sqlFunctionExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      this._isSearchCondition = false;
      SqlExpression sqlExpression = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) sqlFunctionExpression.Instance);
      SqlExpression[] sqlExpressionArray = new SqlExpression[((IReadOnlyCollection<SqlExpression>) sqlFunctionExpression.Arguments).Count];
      for (int index = 0; index < sqlExpressionArray.Length; ++index)
        sqlExpressionArray[index] = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) sqlFunctionExpression.Arguments[index]);
      this._isSearchCondition = isSearchCondition;
      return this.ApplyConversion((SqlExpression) sqlFunctionExpression.Update(sqlExpression, (IReadOnlyList<SqlExpression>) sqlExpressionArray), string.Equals(sqlFunctionExpression.Name, "FREETEXT") || string.Equals(sqlFunctionExpression.Name, "CONTAINS"));
    }

    protected override Expression VisitSqlParameter(
      SqlParameterExpression sqlParameterExpression)
    {
      return this.ApplyConversion((SqlExpression) sqlParameterExpression, false);
    }

    protected override Expression VisitTable(TableExpression tableExpression) => (Expression) tableExpression;

    protected override Expression VisitProjection(ProjectionExpression projectionExpression)
    {
      SqlExpression sqlExpression = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) projectionExpression.Expression);
      return (Expression) projectionExpression.Update(sqlExpression);
    }

    protected override Expression VisitOrdering(OrderingExpression orderingExpression)
    {
      SqlExpression sqlExpression = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) orderingExpression.Expression);
      return (Expression) orderingExpression.Update(sqlExpression);
    }

    protected override Expression VisitCrossJoin(CrossJoinExpression crossJoinExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      this._isSearchCondition = false;
      TableExpressionBase tableExpressionBase = (TableExpressionBase) ((ExpressionVisitor) this).Visit((Expression) ((JoinExpressionBase) crossJoinExpression).Table);
      this._isSearchCondition = isSearchCondition;
      return (Expression) crossJoinExpression.Update(tableExpressionBase);
    }

    protected override Expression VisitCrossApply(CrossApplyExpression crossApplyExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      this._isSearchCondition = false;
      TableExpressionBase tableExpressionBase = (TableExpressionBase) ((ExpressionVisitor) this).Visit((Expression) ((JoinExpressionBase) crossApplyExpression).Table);
      this._isSearchCondition = isSearchCondition;
      return (Expression) crossApplyExpression.Update(tableExpressionBase);
    }

    protected override Expression VisitOuterApply(OuterApplyExpression outerApplyExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      this._isSearchCondition = false;
      TableExpressionBase tableExpressionBase = (TableExpressionBase) ((ExpressionVisitor) this).Visit((Expression) ((JoinExpressionBase) outerApplyExpression).Table);
      this._isSearchCondition = isSearchCondition;
      return (Expression) outerApplyExpression.Update(tableExpressionBase);
    }

    protected override Expression VisitInnerJoin(InnerJoinExpression innerJoinExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      this._isSearchCondition = false;
      TableExpressionBase tableExpressionBase = (TableExpressionBase) ((ExpressionVisitor) this).Visit((Expression) ((JoinExpressionBase) innerJoinExpression).Table);
      this._isSearchCondition = true;
      SqlExpression sqlExpression = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) ((PredicateJoinExpressionBase) innerJoinExpression).JoinPredicate);
      this._isSearchCondition = isSearchCondition;
      return (Expression) innerJoinExpression.Update(tableExpressionBase, sqlExpression);
    }

    protected override Expression VisitLeftJoin(LeftJoinExpression leftJoinExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      this._isSearchCondition = false;
      TableExpressionBase tableExpressionBase = (TableExpressionBase) ((ExpressionVisitor) this).Visit((Expression) ((JoinExpressionBase) leftJoinExpression).Table);
      this._isSearchCondition = true;
      SqlExpression sqlExpression = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) ((PredicateJoinExpressionBase) leftJoinExpression).JoinPredicate);
      this._isSearchCondition = isSearchCondition;
      return (Expression) leftJoinExpression.Update(tableExpressionBase, sqlExpression);
    }

    protected override Expression VisitScalarSubquery(
      ScalarSubqueryExpression scalarSubqueryExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      SelectExpression selectExpression = (SelectExpression) ((ExpressionVisitor) this).Visit((Expression) scalarSubqueryExpression.Subquery);
      this._isSearchCondition = isSearchCondition;
      return this.ApplyConversion((SqlExpression) scalarSubqueryExpression.Update(selectExpression), false);
    }

    protected override Expression VisitRowNumber(RowNumberExpression rowNumberExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      this._isSearchCondition = false;
      bool flag = false;
      List<SqlExpression> sqlExpressionList = new List<SqlExpression>();
      foreach (SqlExpression partition in (IEnumerable<SqlExpression>) rowNumberExpression.Partitions)
      {
        SqlExpression sqlExpression = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) partition);
        flag |= sqlExpression != partition;
        sqlExpressionList.Add(sqlExpression);
      }
      List<OrderingExpression> orderingExpressionList = new List<OrderingExpression>();
      foreach (OrderingExpression ordering in (IEnumerable<OrderingExpression>) rowNumberExpression.Orderings)
      {
        OrderingExpression orderingExpression = (OrderingExpression) ((ExpressionVisitor) this).Visit((Expression) ordering);
        flag |= orderingExpression != ordering;
        orderingExpressionList.Add(orderingExpression);
      }
      this._isSearchCondition = isSearchCondition;
      return this.ApplyConversion((SqlExpression) rowNumberExpression.Update((IReadOnlyList<SqlExpression>) sqlExpressionList, (IReadOnlyList<OrderingExpression>) orderingExpressionList), false);
    }

    protected override Expression VisitExcept(ExceptExpression exceptExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      this._isSearchCondition = false;
      SelectExpression selectExpression1 = (SelectExpression) ((ExpressionVisitor) this).Visit((Expression) ((SetOperationBase) exceptExpression).Source1);
      SelectExpression selectExpression2 = (SelectExpression) ((ExpressionVisitor) this).Visit((Expression) ((SetOperationBase) exceptExpression).Source2);
      this._isSearchCondition = isSearchCondition;
      return (Expression) exceptExpression.Update(selectExpression1, selectExpression2);
    }

    protected override Expression VisitIntersect(IntersectExpression intersectExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      this._isSearchCondition = false;
      SelectExpression selectExpression1 = (SelectExpression) ((ExpressionVisitor) this).Visit((Expression) ((SetOperationBase) intersectExpression).Source1);
      SelectExpression selectExpression2 = (SelectExpression) ((ExpressionVisitor) this).Visit((Expression) ((SetOperationBase) intersectExpression).Source2);
      this._isSearchCondition = isSearchCondition;
      return (Expression) intersectExpression.Update(selectExpression1, selectExpression2);
    }

    protected override Expression VisitUnion(UnionExpression unionExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      this._isSearchCondition = false;
      SelectExpression selectExpression1 = (SelectExpression) ((ExpressionVisitor) this).Visit((Expression) ((SetOperationBase) unionExpression).Source1);
      SelectExpression selectExpression2 = (SelectExpression) ((ExpressionVisitor) this).Visit((Expression) ((SetOperationBase) unionExpression).Source2);
      this._isSearchCondition = isSearchCondition;
      return (Expression) unionExpression.Update(selectExpression1, selectExpression2);
    }

    protected override Expression VisitCollate([NotNull] CollateExpression collateExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      this._isSearchCondition = false;
      SqlExpression sqlExpression = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) collateExpression.Operand);
      this._isSearchCondition = isSearchCondition;
      return this.ApplyConversion((SqlExpression) collateExpression.Update(sqlExpression), false);
    }

    protected override Expression VisitDistinct([NotNull] DistinctExpression distinctExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      this._isSearchCondition = false;
      SqlExpression sqlExpression = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) distinctExpression.Operand);
      this._isSearchCondition = isSearchCondition;
      return this.ApplyConversion((SqlExpression) distinctExpression.Update(sqlExpression), false);
    }

    protected override Expression VisitTableValuedFunction(
      [NotNull] TableValuedFunctionExpression tableValuedFunctionExpression)
    {
      bool isSearchCondition = this._isSearchCondition;
      this._isSearchCondition = false;
      SqlExpression[] sqlExpressionArray = new SqlExpression[((IReadOnlyCollection<SqlExpression>) tableValuedFunctionExpression.Arguments).Count];
      for (int index = 0; index < sqlExpressionArray.Length; ++index)
        sqlExpressionArray[index] = (SqlExpression) ((ExpressionVisitor) this).Visit((Expression) tableValuedFunctionExpression.Arguments[index]);
      this._isSearchCondition = isSearchCondition;
      return (Expression) tableValuedFunctionExpression.Update((IReadOnlyList<SqlExpression>) sqlExpressionArray);
    }
  }
}
