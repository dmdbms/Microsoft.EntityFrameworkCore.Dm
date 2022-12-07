// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmSqlNullabilityProcessor
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class DmSqlNullabilityProcessor
  {
    private readonly List<ColumnExpression> _nonNullableColumns;
    private readonly ISqlExpressionFactory _sqlExpressionFactory;
    private bool _canCache;

    public DmSqlNullabilityProcessor(
      [NotNull] RelationalParameterBasedSqlProcessorDependencies dependencies,
      bool useRelationalNulls)
    {
      Check.NotNull<RelationalParameterBasedSqlProcessorDependencies>(dependencies, nameof (dependencies));
      this._sqlExpressionFactory = dependencies.SqlExpressionFactory;
      this.UseRelationalNulls = useRelationalNulls;
      this._nonNullableColumns = new List<ColumnExpression>();
      this.ParameterValues = (IReadOnlyDictionary<string, object>) null;
    }

    protected virtual bool UseRelationalNulls { get; }

    protected virtual IReadOnlyDictionary<string, object?> ParameterValues { get; private set; }

    public virtual SelectExpression Process(
      [NotNull] SelectExpression selectExpression,
      [NotNull] IReadOnlyDictionary<string, object?> parameterValues,
      out bool canCache)
    {
      Check.NotNull<SelectExpression>(selectExpression, nameof (selectExpression));
      Check.NotNull<IReadOnlyDictionary<string, object>>(parameterValues, nameof (parameterValues));
      this._canCache = true;
      this._nonNullableColumns.Clear();
      this.ParameterValues = parameterValues;
      SelectExpression selectExpression1 = this.Visit(selectExpression);
      canCache = this._canCache;
      return selectExpression1;
    }

    protected virtual void DoNotCache() => this._canCache = false;

    protected virtual void AddNonNullableColumn([NotNull] ColumnExpression columnExpression) => this._nonNullableColumns.Add(Check.NotNull<ColumnExpression>(columnExpression, nameof (columnExpression)));

    protected virtual TableExpressionBase Visit(
      [NotNull] TableExpressionBase tableExpressionBase)
    {
      Check.NotNull<TableExpressionBase>(tableExpressionBase, nameof (tableExpressionBase));
      switch (tableExpressionBase)
      {
        case CrossApplyExpression crossApplyExpression:
          return (TableExpressionBase) crossApplyExpression.Update(this.Visit(((JoinExpressionBase) crossApplyExpression).Table));
        case CrossJoinExpression crossJoinExpression:
          return (TableExpressionBase) crossJoinExpression.Update(this.Visit(((JoinExpressionBase) crossJoinExpression).Table));
        case ExceptExpression exceptExpression:
          SelectExpression selectExpression1 = this.Visit(((SetOperationBase) exceptExpression).Source1);
          SelectExpression selectExpression2 = this.Visit(((SetOperationBase) exceptExpression).Source2);
          return (TableExpressionBase) exceptExpression.Update(selectExpression1, selectExpression2);
        case FromSqlExpression fromSqlExpression:
          return (TableExpressionBase) fromSqlExpression;
        case InnerJoinExpression innerJoinExpression:
          TableExpressionBase tableExpressionBase1 = this.Visit(((JoinExpressionBase) innerJoinExpression).Table);
          SqlExpression expression = this.ProcessJoinPredicate(((PredicateJoinExpressionBase) innerJoinExpression).JoinPredicate);
          bool? boolConstantValue = DmSqlNullabilityProcessor.TryGetBoolConstantValue(expression);
          bool flag = true;
          return boolConstantValue.GetValueOrDefault() == flag & boolConstantValue.HasValue ? (TableExpressionBase) new CrossJoinExpression(tableExpressionBase1) : (TableExpressionBase) innerJoinExpression.Update(tableExpressionBase1, expression);
        case IntersectExpression intersectExpression:
          SelectExpression selectExpression3 = this.Visit(((SetOperationBase) intersectExpression).Source1);
          SelectExpression selectExpression4 = this.Visit(((SetOperationBase) intersectExpression).Source2);
          return (TableExpressionBase) intersectExpression.Update(selectExpression3, selectExpression4);
        case LeftJoinExpression leftJoinExpression:
          TableExpressionBase tableExpressionBase2 = this.Visit(((JoinExpressionBase) leftJoinExpression).Table);
          SqlExpression sqlExpression1 = this.ProcessJoinPredicate(((PredicateJoinExpressionBase) leftJoinExpression).JoinPredicate);
          return (TableExpressionBase) leftJoinExpression.Update(tableExpressionBase2, sqlExpression1);
        case OuterApplyExpression outerApplyExpression:
          return (TableExpressionBase) outerApplyExpression.Update(this.Visit(((JoinExpressionBase) outerApplyExpression).Table));
        case SelectExpression selectExpression7:
          return (TableExpressionBase) this.Visit(selectExpression7);
        case TableValuedFunctionExpression functionExpression:
          List<SqlExpression> sqlExpressionList = new List<SqlExpression>();
          foreach (SqlExpression sqlExpression2 in (IEnumerable<SqlExpression>) functionExpression.Arguments)
            sqlExpressionList.Add(this.Visit(sqlExpression2, out bool _));
          return (TableExpressionBase) functionExpression.Update((IReadOnlyList<SqlExpression>) sqlExpressionList);
        case TableExpression tableExpression:
          return (TableExpressionBase) tableExpression;
        case UnionExpression unionExpression:
          SelectExpression selectExpression5 = this.Visit(((SetOperationBase) unionExpression).Source1);
          SelectExpression selectExpression6 = this.Visit(((SetOperationBase) unionExpression).Source2);
          return (TableExpressionBase) unionExpression.Update(selectExpression5, selectExpression6);
        default:
          throw new InvalidOperationException(RelationalStrings.UnhandledExpressionInVisitor((object) tableExpressionBase, (object) ((object) tableExpressionBase).GetType(), (object) "SqlNullabilityProcessor"));
      }
    }

    protected virtual SelectExpression Visit([NotNull] SelectExpression selectExpression)
    {
      Check.NotNull<SelectExpression>(selectExpression, nameof (selectExpression));
      bool flag1 = false;
      List<ProjectionExpression> projectionExpressionList = (List<ProjectionExpression>) selectExpression.Projection;
      bool nullable;
      for (int index1 = 0; index1 < ((IReadOnlyCollection<ProjectionExpression>) selectExpression.Projection).Count; ++index1)
      {
        ProjectionExpression projectionExpression1 = selectExpression.Projection[index1];
        ProjectionExpression projectionExpression2 = projectionExpression1.Update(this.Visit(projectionExpression1.Expression, out nullable));
        if (projectionExpression2 != projectionExpression1 && projectionExpressionList == selectExpression.Projection)
        {
          projectionExpressionList = new List<ProjectionExpression>();
          for (int index2 = 0; index2 < index1; ++index2)
            projectionExpressionList.Add(selectExpression.Projection[index2]);
          flag1 = true;
        }
        if (projectionExpressionList != selectExpression.Projection)
          projectionExpressionList.Add(projectionExpression2);
      }
      List<TableExpressionBase> tableExpressionBaseList = (List<TableExpressionBase>) selectExpression.Tables;
      for (int index3 = 0; index3 < ((IReadOnlyCollection<TableExpressionBase>) selectExpression.Tables).Count; ++index3)
      {
        TableExpressionBase table = selectExpression.Tables[index3];
        TableExpressionBase tableExpressionBase = this.Visit(table);
        if (tableExpressionBase != table && tableExpressionBaseList == selectExpression.Tables)
        {
          tableExpressionBaseList = new List<TableExpressionBase>();
          for (int index4 = 0; index4 < index3; ++index4)
            tableExpressionBaseList.Add(selectExpression.Tables[index4]);
          flag1 = true;
        }
        if (tableExpressionBaseList != selectExpression.Tables)
          tableExpressionBaseList.Add(tableExpressionBase);
      }
      SqlExpression expression1 = this.Visit(selectExpression.Predicate, true, out nullable);
      bool flag2 = flag1 | expression1 != selectExpression.Predicate;
      bool? boolConstantValue1 = DmSqlNullabilityProcessor.TryGetBoolConstantValue(expression1);
      nullable = true;
      if (boolConstantValue1.GetValueOrDefault() == nullable & boolConstantValue1.HasValue)
      {
        expression1 = (SqlExpression) null;
        flag2 = true;
      }
      List<SqlExpression> sqlExpressionList = (List<SqlExpression>) selectExpression.GroupBy;
      for (int index5 = 0; index5 < ((IReadOnlyCollection<SqlExpression>) selectExpression.GroupBy).Count; ++index5)
      {
        SqlExpression sqlExpression1 = selectExpression.GroupBy[index5];
        SqlExpression sqlExpression2 = this.Visit(sqlExpression1, out nullable);
        if (sqlExpression2 != sqlExpression1 && sqlExpressionList == selectExpression.GroupBy)
        {
          sqlExpressionList = new List<SqlExpression>();
          for (int index6 = 0; index6 < index5; ++index6)
            sqlExpressionList.Add(selectExpression.GroupBy[index6]);
          flag2 = true;
        }
        if (sqlExpressionList != selectExpression.GroupBy)
          sqlExpressionList.Add(sqlExpression2);
      }
      SqlExpression expression2 = this.Visit(selectExpression.Having, true, out nullable);
      bool flag3 = flag2 | expression2 != selectExpression.Having;
      bool? boolConstantValue2 = DmSqlNullabilityProcessor.TryGetBoolConstantValue(expression2);
      nullable = true;
      if (boolConstantValue2.GetValueOrDefault() == nullable & boolConstantValue2.HasValue)
      {
        expression2 = (SqlExpression) null;
        flag3 = true;
      }
      List<OrderingExpression> orderingExpressionList = (List<OrderingExpression>) selectExpression.Orderings;
      for (int index7 = 0; index7 < ((IReadOnlyCollection<OrderingExpression>) selectExpression.Orderings).Count; ++index7)
      {
        OrderingExpression ordering = selectExpression.Orderings[index7];
        OrderingExpression orderingExpression = ordering.Update(this.Visit(ordering.Expression, out nullable));
        if (orderingExpression != ordering && orderingExpressionList == selectExpression.Orderings)
        {
          orderingExpressionList = new List<OrderingExpression>();
          for (int index8 = 0; index8 < index7; ++index8)
            orderingExpressionList.Add(selectExpression.Orderings[index8]);
          flag3 = true;
        }
        if (orderingExpressionList != selectExpression.Orderings)
          orderingExpressionList.Add(orderingExpression);
      }
      SqlExpression sqlExpression3 = this.Visit(selectExpression.Offset, out nullable);
      bool flag4 = flag3 | sqlExpression3 != selectExpression.Offset;
      SqlExpression sqlExpression4 = this.Visit(selectExpression.Limit, out nullable);
      return flag4 | sqlExpression4 != selectExpression.Limit ? selectExpression.Update((IReadOnlyList<ProjectionExpression>) projectionExpressionList, (IReadOnlyList<TableExpressionBase>) tableExpressionBaseList, expression1, (IReadOnlyList<SqlExpression>) sqlExpressionList, expression2, (IReadOnlyList<OrderingExpression>) orderingExpressionList, sqlExpression4, sqlExpression3) : selectExpression;
    }

    protected virtual SqlExpression? Visit(
      SqlExpression? sqlExpression,
      out bool nullable)
    {
      return this.Visit(sqlExpression, false, out nullable);
    }

    protected virtual SqlExpression? Visit(
      SqlExpression? sqlExpression,
      bool allowOptimizedExpansion,
      out bool nullable)
    {
      return this.Visit(sqlExpression, allowOptimizedExpansion, false, out nullable);
    }

    private SqlExpression? Visit(
      SqlExpression? sqlExpression,
      bool allowOptimizedExpansion,
      bool preserveNonNullableColumns,
      out bool nullable)
    {
      if (sqlExpression == null)
      {
        nullable = false;
        return sqlExpression;
      }
      int count = this._nonNullableColumns.Count;
      if (true)
        ;
      SqlExpression sqlExpression1;
      switch (sqlExpression)
      {
        case CaseExpression caseExpression:
          sqlExpression1 = this.VisitCase(caseExpression, allowOptimizedExpansion, out nullable);
          break;
        case CollateExpression collateExpression:
          sqlExpression1 = this.VisitCollate(collateExpression, allowOptimizedExpansion, out nullable);
          break;
        case ColumnExpression columnExpression:
          sqlExpression1 = this.VisitColumn(columnExpression, allowOptimizedExpansion, out nullable);
          break;
        case DistinctExpression distinctExpression:
          sqlExpression1 = this.VisitDistinct(distinctExpression, allowOptimizedExpansion, out nullable);
          break;
        case ExistsExpression existsExpression:
          sqlExpression1 = this.VisitExists(existsExpression, allowOptimizedExpansion, out nullable);
          break;
        case InExpression inExpression:
          sqlExpression1 = this.VisitIn(inExpression, allowOptimizedExpansion, out nullable);
          break;
        case LikeExpression likeExpression:
          sqlExpression1 = this.VisitLike(likeExpression, allowOptimizedExpansion, out nullable);
          break;
        case RowNumberExpression rowNumberExpression:
          sqlExpression1 = this.VisitRowNumber(rowNumberExpression, allowOptimizedExpansion, out nullable);
          break;
        case ScalarSubqueryExpression scalarSubqueryExpression:
          sqlExpression1 = this.VisitScalarSubquery(scalarSubqueryExpression, allowOptimizedExpansion, out nullable);
          break;
        case SqlBinaryExpression sqlBinaryExpression:
          sqlExpression1 = this.VisitSqlBinary(sqlBinaryExpression, allowOptimizedExpansion, out nullable);
          break;
        case SqlConstantExpression sqlConstantExpression:
          sqlExpression1 = this.VisitSqlConstant(sqlConstantExpression, allowOptimizedExpansion, out nullable);
          break;
        case SqlFragmentExpression sqlFragmentExpression:
          sqlExpression1 = this.VisitSqlFragment(sqlFragmentExpression, allowOptimizedExpansion, out nullable);
          break;
        case SqlFunctionExpression sqlFunctionExpression:
          sqlExpression1 = this.VisitSqlFunction(sqlFunctionExpression, allowOptimizedExpansion, out nullable);
          break;
        case SqlParameterExpression sqlParameterExpression:
          sqlExpression1 = this.VisitSqlParameter(sqlParameterExpression, allowOptimizedExpansion, out nullable);
          break;
        case SqlUnaryExpression sqlUnaryExpression:
          sqlExpression1 = this.VisitSqlUnary(sqlUnaryExpression, allowOptimizedExpansion, out nullable);
          break;
        default:
          sqlExpression1 = this.VisitCustomSqlExpression(sqlExpression, allowOptimizedExpansion, out nullable);
          break;
      }
      if (true)
        ;
      SqlExpression sqlExpression2 = sqlExpression1;
      if (!preserveNonNullableColumns)
        this.RestoreNonNullableColumnsList(count);
      return sqlExpression2;
    }

    protected virtual SqlExpression VisitCustomSqlExpression(
      [NotNull] SqlExpression sqlExpression,
      bool allowOptimizedExpansion,
      out bool nullable)
    {
      throw new InvalidOperationException(RelationalStrings.UnhandledExpressionInVisitor((object) sqlExpression, (object) ((object) sqlExpression).GetType(), (object) "SqlNullabilityProcessor"));
    }

        protected virtual SqlExpression VisitCase([NotNull] CaseExpression caseExpression, bool allowOptimizedExpansion, out bool nullable)
        {
            //IL_00d4: Unknown result type (might be due to invalid IL or missing references)
            //IL_00de: Expected O, but got Unknown
            Check.NotNull<CaseExpression>(caseExpression, "caseExpression");
            nullable = caseExpression.ElseResult == null;
            int count = _nonNullableColumns.Count;
            bool nullable2;
            SqlExpression val = Visit(caseExpression.Operand, out nullable2);
            List<CaseWhenClause> list = new List<CaseWhenClause>();
            bool allowOptimizedExpansion2 = caseExpression.Operand == null;
            bool flag = false;
            foreach (CaseWhenClause whenClause in caseExpression.WhenClauses)
            {
                SqlExpression val2 = Visit(whenClause.Test, allowOptimizedExpansion2, preserveNonNullableColumns: true, out nullable2);
                bool? flag2 = TryGetBoolConstantValue(val2);
                if (flag2.HasValue)
                {
                    bool valueOrDefault = flag2.GetValueOrDefault();
                    if (true)
                    {
                        if (!valueOrDefault)
                        {
                            RestoreNonNullableColumnsList(count);
                            continue;
                        }
                        flag = true;
                    }
                }
                bool nullable3;
                SqlExpression val3 = Visit(whenClause.Result, out nullable3);
                nullable |= nullable3;
                list.Add(new CaseWhenClause(val2, val3));
                RestoreNonNullableColumnsList(count);
                if (flag)
                {
                    break;
                }
            }
            SqlExpression val4 = null;
            if (!flag)
            {
                val4 = Visit(caseExpression.ElseResult, out var nullable4);
                nullable |= nullable4;
            }
            if (list.Count == 0)
            {
                return (SqlExpression)(((object)val4) ?? ((object)_sqlExpressionFactory.Constant((object)null, ((SqlExpression)caseExpression).TypeMapping)));
            }
            return (SqlExpression)((val4 == null && list.Count == 1 && TryGetBoolConstantValue(list[0].Test) == true) ? ((object)list[0].Result) : ((object)caseExpression.Update(val, (IReadOnlyList<CaseWhenClause>)list, val4)));
        }

        protected virtual SqlExpression VisitCollate(
      [NotNull] CollateExpression collateExpression,
      bool allowOptimizedExpansion,
      out bool nullable)
    {
      Check.NotNull<CollateExpression>(collateExpression, nameof (collateExpression));
      return (SqlExpression) collateExpression.Update(this.Visit(collateExpression.Operand, out nullable));
    }

    protected virtual SqlExpression VisitColumn(
      [NotNull] ColumnExpression columnExpression,
      bool allowOptimizedExpansion,
      out bool nullable)
    {
      Check.NotNull<ColumnExpression>(columnExpression, nameof (columnExpression));
      nullable = columnExpression.IsNullable && !this._nonNullableColumns.Contains(columnExpression);
      return (SqlExpression) columnExpression;
    }

    protected virtual SqlExpression VisitDistinct(
      [NotNull] DistinctExpression distinctExpression,
      bool allowOptimizedExpansion,
      out bool nullable)
    {
      Check.NotNull<DistinctExpression>(distinctExpression, nameof (distinctExpression));
      return (SqlExpression) distinctExpression.Update(this.Visit(distinctExpression.Operand, out nullable));
    }

    protected virtual SqlExpression VisitExists(
      [NotNull] ExistsExpression existsExpression,
      bool allowOptimizedExpansion,
      out bool nullable)
    {
      Check.NotNull<ExistsExpression>(existsExpression, nameof (existsExpression));
      SelectExpression selectExpression = this.Visit(existsExpression.Subquery);
      nullable = false;
      bool? boolConstantValue = DmSqlNullabilityProcessor.TryGetBoolConstantValue(selectExpression.Predicate);
      bool flag = false;
      return boolConstantValue.GetValueOrDefault() == flag & boolConstantValue.HasValue ? selectExpression.Predicate : (SqlExpression) existsExpression.Update(selectExpression);
    }

    protected virtual SqlExpression VisitIn(
      [NotNull] InExpression inExpression,
      bool allowOptimizedExpansion,
      out bool nullable)
    {
      Check.NotNull<InExpression>(inExpression, nameof (inExpression));
      bool nullable1;
      SqlExpression sqlExpression1 = this.Visit(inExpression.Item, out nullable1);
      if (inExpression.Subquery != null)
      {
        SelectExpression selectExpression = this.Visit(inExpression.Subquery);
        bool? boolConstantValue = DmSqlNullabilityProcessor.TryGetBoolConstantValue(selectExpression.Predicate);
        bool flag = false;
        if (boolConstantValue.GetValueOrDefault() == flag & boolConstantValue.HasValue)
        {
          nullable = false;
          return selectExpression.Predicate;
        }
        nullable = nullable1 || ((IReadOnlyCollection<ProjectionExpression>) selectExpression.Projection).Count != 1 || !(selectExpression.Projection[0].Expression is ColumnExpression expression) || expression.IsNullable;
        return (SqlExpression) inExpression.Update(sqlExpression1, (SqlExpression) null, selectExpression);
      }
      if (this.UseRelationalNulls || !(inExpression.Values is SqlConstantExpression) && !(inExpression.Values is SqlParameterExpression))
      {
        (SqlConstantExpression constantExpression, List<object> objectList, bool _) = ProcessInExpressionValues(inExpression.Values, false);
        nullable = false;
        return objectList.Count == 0 ? (SqlExpression) this._sqlExpressionFactory.Constant((object) false, ((SqlExpression) inExpression).TypeMapping) : SimplifyInExpression(inExpression.Update(sqlExpression1, (SqlExpression) constantExpression, (SelectExpression) null), constantExpression, objectList);
      }
      (SqlConstantExpression constantExpression1, List<object> objectList1, bool HasNullValue) = ProcessInExpressionValues(inExpression.Values, true);
      if (objectList1.Count == 0)
      {
        nullable = false;
        return !HasNullValue || !nullable1 ? (SqlExpression) this._sqlExpressionFactory.Equal((SqlExpression) this._sqlExpressionFactory.Constant((object) true, ((SqlExpression) inExpression).TypeMapping), (SqlExpression) this._sqlExpressionFactory.Constant((object) inExpression.IsNegated, ((SqlExpression) inExpression).TypeMapping)) : (inExpression.IsNegated ? (SqlExpression) this._sqlExpressionFactory.IsNotNull(sqlExpression1) : (SqlExpression) this._sqlExpressionFactory.IsNull(sqlExpression1));
      }
      SqlExpression sqlExpression2 = SimplifyInExpression(inExpression.Update(sqlExpression1, (SqlExpression) constantExpression1, (SelectExpression) null), constantExpression1, objectList1);
      if (!nullable1 || allowOptimizedExpansion && !inExpression.IsNegated && !HasNullValue)
      {
        nullable = false;
        return sqlExpression2;
      }
      nullable = false;
      return inExpression.IsNegated == HasNullValue ? (SqlExpression) this._sqlExpressionFactory.AndAlso(sqlExpression2, (SqlExpression) this._sqlExpressionFactory.IsNotNull(sqlExpression1)) : (SqlExpression) this._sqlExpressionFactory.OrElse(sqlExpression2, (SqlExpression) this._sqlExpressionFactory.IsNull(sqlExpression1));

      (SqlConstantExpression ProcessedValuesExpression, List<object?> ProcessedValuesList, bool HasNullValue) ProcessInExpressionValues(
        SqlExpression valuesExpression,
        bool extractNullValues)
      {
        List<object> objectList = new List<object>();
        bool flag = false;
        RelationalTypeMapping relationalTypeMapping = (RelationalTypeMapping) null;
        IEnumerable enumerable = (IEnumerable) null;
        switch (valuesExpression)
        {
          case SqlConstantExpression constantExpression2:
            relationalTypeMapping = ((SqlExpression) constantExpression2).TypeMapping;
            enumerable = (IEnumerable) constantExpression2.Value;
            break;
          case SqlParameterExpression parameterExpression:
            this.DoNotCache();
            relationalTypeMapping = ((SqlExpression) parameterExpression).TypeMapping;
            enumerable = (IEnumerable) this.ParameterValues[parameterExpression.Name];
            if (enumerable == null)
              throw new NullReferenceException();
            break;
        }
        foreach (object obj in enumerable)
        {
          if (obj == null & extractNullValues)
            flag = true;
          else
            objectList.Add(obj);
        }
        return (this._sqlExpressionFactory.Constant((object) objectList, relationalTypeMapping), objectList, flag);
      }

      SqlExpression SimplifyInExpression(
        InExpression inExpression,
        SqlConstantExpression inValuesExpression,
        List<object?> inValuesList)
      {
        return inValuesList.Count == 1 ? (inExpression.IsNegated ? (SqlExpression) this._sqlExpressionFactory.NotEqual(inExpression.Item, (SqlExpression) this._sqlExpressionFactory.Constant(inValuesList[0], ((SqlExpression) inValuesExpression).TypeMapping)) : (SqlExpression) this._sqlExpressionFactory.Equal(inExpression.Item, (SqlExpression) this._sqlExpressionFactory.Constant(inValuesList[0], inExpression.Values.TypeMapping))) : (SqlExpression) inExpression;
      }
    }

    protected virtual SqlExpression VisitLike(
      [NotNull] LikeExpression likeExpression,
      bool allowOptimizedExpansion,
      out bool nullable)
    {
      Check.NotNull<LikeExpression>(likeExpression, nameof (likeExpression));
      bool nullable1;
      SqlExpression sqlExpression1 = this.Visit(likeExpression.Match, out nullable1);
      bool nullable2;
      SqlExpression sqlExpression2 = this.Visit(likeExpression.Pattern, out nullable2);
      bool nullable3;
      SqlExpression sqlExpression3 = this.Visit(likeExpression.EscapeChar, out nullable3);
      nullable = nullable1 | nullable2 | nullable3;
      return (SqlExpression) likeExpression.Update(sqlExpression1, sqlExpression2, sqlExpression3);
    }

    protected virtual SqlExpression VisitRowNumber(
      [NotNull] RowNumberExpression rowNumberExpression,
      bool allowOptimizedExpansion,
      out bool nullable)
    {
      Check.NotNull<RowNumberExpression>(rowNumberExpression, nameof (rowNumberExpression));
      bool flag = false;
      List<SqlExpression> sqlExpressionList = new List<SqlExpression>();
      bool nullable1;
      foreach (SqlExpression partition in (IEnumerable<SqlExpression>) rowNumberExpression.Partitions)
      {
        SqlExpression sqlExpression = this.Visit(partition, out nullable1);
        flag |= sqlExpression != partition;
        sqlExpressionList.Add(sqlExpression);
      }
      List<OrderingExpression> orderingExpressionList = new List<OrderingExpression>();
      foreach (OrderingExpression ordering in (IEnumerable<OrderingExpression>) rowNumberExpression.Orderings)
      {
        OrderingExpression orderingExpression = ordering.Update(this.Visit(ordering.Expression, out nullable1));
        flag |= orderingExpression != ordering;
        orderingExpressionList.Add(orderingExpression);
      }
      nullable = false;
      return flag ? (SqlExpression) rowNumberExpression.Update((IReadOnlyList<SqlExpression>) sqlExpressionList, (IReadOnlyList<OrderingExpression>) orderingExpressionList) : (SqlExpression) rowNumberExpression;
    }

    protected virtual SqlExpression VisitScalarSubquery(
      [NotNull] ScalarSubqueryExpression scalarSubqueryExpression,
      bool allowOptimizedExpansion,
      out bool nullable)
    {
      Check.NotNull<ScalarSubqueryExpression>(scalarSubqueryExpression, nameof (scalarSubqueryExpression));
      nullable = true;
      return (SqlExpression) scalarSubqueryExpression.Update(this.Visit(scalarSubqueryExpression.Subquery));
    }

        protected virtual SqlExpression VisitSqlBinary([NotNull] SqlBinaryExpression sqlBinaryExpression, bool allowOptimizedExpansion, out bool nullable)
        {
            Check.NotNull<SqlBinaryExpression>(sqlBinaryExpression, "sqlBinaryExpression");
            bool optimize = allowOptimizedExpansion;
            allowOptimizedExpansion = allowOptimizedExpansion && (sqlBinaryExpression.OperatorType == ExpressionType.AndAlso || sqlBinaryExpression.OperatorType == ExpressionType.OrElse);
            int count = _nonNullableColumns.Count;
            bool nullable2;
            SqlExpression val = Visit(sqlBinaryExpression.Left, allowOptimizedExpansion, preserveNonNullableColumns: true, out nullable2);
            List<ColumnExpression> first = _nonNullableColumns.Skip(count).ToList();
            if (sqlBinaryExpression.OperatorType != ExpressionType.AndAlso)
            {
                RestoreNonNullableColumnsList(count);
            }
            bool nullable3;
            SqlExpression val2 = Visit(sqlBinaryExpression.Right, allowOptimizedExpansion, preserveNonNullableColumns: true, out nullable3);
            if (sqlBinaryExpression.OperatorType == ExpressionType.OrElse)
            {
                List<ColumnExpression> collection = first.Intersect(_nonNullableColumns.Skip(count)).ToList();
                RestoreNonNullableColumnsList(count);
                _nonNullableColumns.AddRange(collection);
            }
            else if (sqlBinaryExpression.OperatorType != ExpressionType.AndAlso)
            {
                RestoreNonNullableColumnsList(count);
            }
            if (sqlBinaryExpression.OperatorType == ExpressionType.Add && ((Expression)(object)sqlBinaryExpression).Type == typeof(string))
            {
                if (nullable2)
                {
                    val = AddNullConcatenationProtection(val, ((SqlExpression)sqlBinaryExpression).TypeMapping);
                }
                if (nullable3)
                {
                    val2 = AddNullConcatenationProtection(val2, ((SqlExpression)sqlBinaryExpression).TypeMapping);
                }
                nullable = false;
                return (SqlExpression)(object)sqlBinaryExpression.Update(val, val2);
            }
            if (sqlBinaryExpression.OperatorType == ExpressionType.Equal || sqlBinaryExpression.OperatorType == ExpressionType.NotEqual)
            {
                SqlBinaryExpression val3 = sqlBinaryExpression.Update(val, val2);
                SqlExpression val4 = OptimizeComparison(val3, val, val2, nullable2, nullable3, out nullable);
                SqlUnaryExpression val5 = (SqlUnaryExpression)(object)((val4 is SqlUnaryExpression) ? val4 : null);
                if (val5 != null && val5.OperatorType == ExpressionType.NotEqual)
                {
                    SqlExpression operand = val5.Operand;
                    ColumnExpression val6 = (ColumnExpression)(object)((operand is ColumnExpression) ? operand : null);
                    if (val6 != null)
                    {
                        _nonNullableColumns.Add(val6);
                    }
                }
                if (((object)val4).Equals((object?)val3) && (nullable2 || nullable3) && !UseRelationalNulls)
                {
                    return RewriteNullSemantics(val3, val3.Left, val3.Right, nullable2, nullable3, optimize, out nullable);
                }
                return val4;
            }
            nullable = nullable2 || nullable3;
            SqlBinaryExpression val7 = sqlBinaryExpression.Update(val, val2);
            object result;
            if (val7 != null)
            {
                SqlBinaryExpression sqlBinaryExpression2 = val7;
                if (sqlBinaryExpression.OperatorType == ExpressionType.AndAlso || sqlBinaryExpression.OperatorType == ExpressionType.OrElse)
                {
                    result = SimplifyLogicalSqlBinaryExpression(sqlBinaryExpression2);
                    goto IL_025c;
                }
            }
            result = val7;
            goto IL_025c;
        IL_025c:
            return (SqlExpression)result;
            SqlExpression AddNullConcatenationProtection(SqlExpression argument, RelationalTypeMapping typeMapping)
            {
                return (SqlExpression)((argument is SqlConstantExpression || argument is SqlParameterExpression) ? ((object)_sqlExpressionFactory.Constant((object)string.Empty, typeMapping)) : ((object)_sqlExpressionFactory.Coalesce(argument, (SqlExpression)(object)_sqlExpressionFactory.Constant((object)string.Empty, typeMapping), (RelationalTypeMapping)null)));
            }
        }

        protected virtual SqlExpression VisitSqlConstant(
      [NotNull] SqlConstantExpression sqlConstantExpression,
      bool allowOptimizedExpansion,
      out bool nullable)
    {
      Check.NotNull<SqlConstantExpression>(sqlConstantExpression, nameof (sqlConstantExpression));
      nullable = sqlConstantExpression.Value == null;
      return (SqlExpression) sqlConstantExpression;
    }

    protected virtual SqlExpression VisitSqlFragment(
      [NotNull] SqlFragmentExpression sqlFragmentExpression,
      bool allowOptimizedExpansion,
      out bool nullable)
    {
      Check.NotNull<SqlFragmentExpression>(sqlFragmentExpression, nameof (sqlFragmentExpression));
      nullable = false;
      return (SqlExpression) sqlFragmentExpression;
    }

    protected virtual SqlExpression VisitSqlFunction(
      [NotNull] SqlFunctionExpression sqlFunctionExpression,
      bool allowOptimizedExpansion,
      out bool nullable)
    {
      Check.NotNull<SqlFunctionExpression>(sqlFunctionExpression, nameof (sqlFunctionExpression));
      if (sqlFunctionExpression.IsBuiltIn && sqlFunctionExpression.Arguments != null && string.Equals(sqlFunctionExpression.Name, "COALESCE", StringComparison.OrdinalIgnoreCase))
      {
        bool nullable1;
        SqlExpression sqlExpression1 = this.Visit(sqlFunctionExpression.Arguments[0], out nullable1);
        bool nullable2;
        SqlExpression sqlExpression2 = this.Visit(sqlFunctionExpression.Arguments[1], out nullable2);
        nullable = nullable1 & nullable2;
        return (SqlExpression) sqlFunctionExpression.Update(sqlFunctionExpression.Instance, (IReadOnlyList<SqlExpression>) new SqlExpression[2]
        {
          sqlExpression1,
          sqlExpression2
        });
      }
      bool nullable3;
      SqlExpression sqlExpression = this.Visit(sqlFunctionExpression.Instance, out nullable3);
      nullable = sqlFunctionExpression.IsNullable;
      if (sqlFunctionExpression.IsNiladic)
        return (SqlExpression) sqlFunctionExpression.Update(sqlExpression, sqlFunctionExpression.Arguments);
      SqlExpression[] sqlExpressionArray = new SqlExpression[((IReadOnlyCollection<SqlExpression>) sqlFunctionExpression.Arguments).Count];
      for (int index = 0; index < sqlExpressionArray.Length; ++index)
        sqlExpressionArray[index] = this.Visit(sqlFunctionExpression.Arguments[index], out nullable3);
      return !sqlFunctionExpression.IsBuiltIn || !string.Equals(sqlFunctionExpression.Name, "SUM", StringComparison.OrdinalIgnoreCase) ? (SqlExpression) sqlFunctionExpression.Update(sqlExpression, (IReadOnlyList<SqlExpression>) sqlExpressionArray) : (SqlExpression) this._sqlExpressionFactory.Coalesce((SqlExpression) sqlFunctionExpression.Update(sqlExpression, (IReadOnlyList<SqlExpression>) sqlExpressionArray), (SqlExpression) this._sqlExpressionFactory.Constant((object) 0, ((SqlExpression) sqlFunctionExpression).TypeMapping), ((SqlExpression) sqlFunctionExpression).TypeMapping);
    }

    protected virtual SqlExpression VisitSqlParameter(
      [NotNull] SqlParameterExpression sqlParameterExpression,
      bool allowOptimizedExpansion,
      out bool nullable)
    {
      Check.NotNull<SqlParameterExpression>(sqlParameterExpression, nameof (sqlParameterExpression));
      nullable = this.ParameterValues[sqlParameterExpression.Name] == null;
      return nullable ? (SqlExpression) this._sqlExpressionFactory.Constant((object) null, ((SqlExpression) sqlParameterExpression).TypeMapping) : (SqlExpression) sqlParameterExpression;
    }

        protected virtual SqlExpression VisitSqlUnary([NotNull] SqlUnaryExpression sqlUnaryExpression, bool allowOptimizedExpansion, out bool nullable)
        {
            Check.NotNull<SqlUnaryExpression>(sqlUnaryExpression, "sqlUnaryExpression");
            bool nullable2;
            SqlExpression val = Visit(sqlUnaryExpression.Operand, out nullable2);
            SqlUnaryExpression val2 = sqlUnaryExpression.Update(val);
            if (sqlUnaryExpression.OperatorType == ExpressionType.Equal || sqlUnaryExpression.OperatorType == ExpressionType.NotEqual)
            {
                SqlExpression val3 = ProcessNullNotNull(val2, nullable2);
                nullable = false;
                SqlUnaryExpression val4 = (SqlUnaryExpression)(object)((val3 is SqlUnaryExpression) ? val3 : null);
                if (val4 != null && val4.OperatorType == ExpressionType.NotEqual)
                {
                    SqlExpression operand = val4.Operand;
                    ColumnExpression val5 = (ColumnExpression)(object)((operand is ColumnExpression) ? operand : null);
                    if (val5 != null)
                    {
                        _nonNullableColumns.Add(val5);
                    }
                }
                return val3;
            }
            nullable = nullable2;
            return (SqlExpression)((!nullable2 && sqlUnaryExpression.OperatorType == ExpressionType.Not) ? ((object)OptimizeNonNullableNotExpression(val2)) : ((object)val2));
        }

        private static bool? TryGetBoolConstantValue(SqlExpression? expression) => expression is SqlConstantExpression constantExpression && constantExpression.Value is bool flag ? new bool?(flag) : new bool?();

    private void RestoreNonNullableColumnsList(int counter)
    {
      if (counter >= this._nonNullableColumns.Count)
        return;
      this._nonNullableColumns.RemoveRange(counter, this._nonNullableColumns.Count - counter);
    }

    private SqlExpression ProcessJoinPredicate(SqlExpression predicate)
    {
      if (predicate is SqlBinaryExpression binaryExpression)
      {
        bool nullable1;
        if (binaryExpression.OperatorType == ExpressionType.Equal)
        {
          bool nullable2;
          SqlExpression left = this.Visit(binaryExpression.Left, true, out nullable2);
          bool nullable3;
          SqlExpression right = this.Visit(binaryExpression.Right, true, out nullable3);
          return this.OptimizeComparison(binaryExpression.Update(left, right), left, right, nullable2, nullable3, out nullable1);
        }
        if (binaryExpression.OperatorType == ExpressionType.AndAlso || binaryExpression.OperatorType == ExpressionType.NotEqual || binaryExpression.OperatorType == ExpressionType.GreaterThan || binaryExpression.OperatorType == ExpressionType.GreaterThanOrEqual || binaryExpression.OperatorType == ExpressionType.LessThan || binaryExpression.OperatorType == ExpressionType.LessThanOrEqual)
          return this.Visit((SqlExpression) binaryExpression, true, out nullable1);
      }
      throw new InvalidOperationException(RelationalStrings.UnhandledExpressionInVisitor((object) predicate, (object) ((object) predicate).GetType(), (object) "SqlNullabilityProcessor"));
    }

    private SqlExpression OptimizeComparison(
      SqlBinaryExpression sqlBinaryExpression,
      SqlExpression left,
      SqlExpression right,
      bool leftNullable,
      bool rightNullable,
      out bool nullable)
    {
      bool flag1 = leftNullable && (left is SqlConstantExpression || left is SqlParameterExpression);
      if (rightNullable && (right is SqlConstantExpression || right is SqlParameterExpression))
      {
        SqlExpression sqlExpression = sqlBinaryExpression.OperatorType == ExpressionType.Equal ? this.ProcessNullNotNull(this._sqlExpressionFactory.IsNull(left), leftNullable) : this.ProcessNullNotNull(this._sqlExpressionFactory.IsNotNull(left), leftNullable);
        nullable = false;
        return sqlExpression;
      }
      if (flag1)
      {
        SqlExpression sqlExpression = sqlBinaryExpression.OperatorType == ExpressionType.Equal ? this.ProcessNullNotNull(this._sqlExpressionFactory.IsNull(right), rightNullable) : this.ProcessNullNotNull(this._sqlExpressionFactory.IsNotNull(right), rightNullable);
        nullable = false;
        return sqlExpression;
      }
      if (!leftNullable && ((object) left).Equals((object) right))
      {
        nullable = false;
        return (SqlExpression) this._sqlExpressionFactory.Equal((SqlExpression) this._sqlExpressionFactory.Constant((object) true, (RelationalTypeMapping) null), (SqlExpression) this._sqlExpressionFactory.Constant((object) (sqlBinaryExpression.OperatorType == ExpressionType.Equal), ((SqlExpression) sqlBinaryExpression).TypeMapping));
      }
      if (!leftNullable && !rightNullable && (sqlBinaryExpression.OperatorType == ExpressionType.Equal || sqlBinaryExpression.OperatorType == ExpressionType.NotEqual))
      {
        SqlUnaryExpression sqlUnaryExpression1 = left as SqlUnaryExpression;
        SqlUnaryExpression sqlUnaryExpression2 = right as SqlUnaryExpression;
        bool flag2 = DmSqlNullabilityProcessor.IsLogicalNot(sqlUnaryExpression1);
        bool flag3 = DmSqlNullabilityProcessor.IsLogicalNot(sqlUnaryExpression2);
        if (flag2)
          left = sqlUnaryExpression1.Operand;
        if (flag3)
          right = sqlUnaryExpression2.Operand;
        nullable = false;
        return sqlBinaryExpression.OperatorType == ExpressionType.Equal ^ flag2 == flag3 ? (SqlExpression) this._sqlExpressionFactory.NotEqual(left, right) : (SqlExpression) this._sqlExpressionFactory.Equal(left, right);
      }
      nullable = false;
      return (SqlExpression) sqlBinaryExpression.Update(left, right);
    }

    private SqlExpression RewriteNullSemantics(
      SqlBinaryExpression sqlBinaryExpression,
      SqlExpression left,
      SqlExpression right,
      bool leftNullable,
      bool rightNullable,
      bool optimize,
      out bool nullable)
    {
      SqlUnaryExpression sqlUnaryExpression1 = left as SqlUnaryExpression;
      SqlUnaryExpression sqlUnaryExpression2 = right as SqlUnaryExpression;
      bool flag1 = DmSqlNullabilityProcessor.IsLogicalNot(sqlUnaryExpression1);
      bool flag2 = DmSqlNullabilityProcessor.IsLogicalNot(sqlUnaryExpression2);
      if (flag1)
        left = sqlUnaryExpression1.Operand;
      if (flag2)
        right = sqlUnaryExpression2.Operand;
      SqlExpression leftIsNull = this.ProcessNullNotNull(this._sqlExpressionFactory.IsNull(left), leftNullable);
      SqlExpression leftIsNotNull = this.OptimizeNonNullableNotExpression(this._sqlExpressionFactory.Not(leftIsNull));
      SqlExpression sqlExpression1 = this.ProcessNullNotNull(this._sqlExpressionFactory.IsNull(right), rightNullable);
      SqlExpression sqlExpression2 = this.OptimizeNonNullableNotExpression(this._sqlExpressionFactory.Not(sqlExpression1));
      if (optimize && sqlBinaryExpression.OperatorType == ExpressionType.Equal && !flag1 && !flag2)
      {
        if (leftNullable & rightNullable)
        {
          nullable = true;
          return this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.OrElse((SqlExpression) this._sqlExpressionFactory.Equal(left, right), this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.AndAlso(leftIsNull, sqlExpression1))));
        }
        if (leftNullable && !rightNullable || !leftNullable & rightNullable)
        {
          nullable = true;
          return (SqlExpression) this._sqlExpressionFactory.Equal(left, right);
        }
      }
      nullable = false;
      if (sqlBinaryExpression.OperatorType == ExpressionType.Equal)
      {
        if (leftNullable & rightNullable)
          return flag1 == flag2 ? this.ExpandNullableEqualNullable(left, right, leftIsNull, leftIsNotNull, sqlExpression1, sqlExpression2) : this.ExpandNegatedNullableEqualNullable(left, right, leftIsNull, leftIsNotNull, sqlExpression1, sqlExpression2);
        if (leftNullable && !rightNullable)
          return flag1 == flag2 ? this.ExpandNullableEqualNonNullable(left, right, leftIsNotNull) : this.ExpandNegatedNullableEqualNonNullable(left, right, leftIsNotNull);
        if (rightNullable && !leftNullable)
          return flag1 == flag2 ? this.ExpandNullableEqualNonNullable(left, right, sqlExpression2) : this.ExpandNegatedNullableEqualNonNullable(left, right, sqlExpression2);
      }
      if (sqlBinaryExpression.OperatorType == ExpressionType.NotEqual)
      {
        if (leftNullable & rightNullable)
          return flag1 == flag2 ? this.ExpandNullableNotEqualNullable(left, right, leftIsNull, leftIsNotNull, sqlExpression1, sqlExpression2) : this.ExpandNegatedNullableNotEqualNullable(left, right, leftIsNull, leftIsNotNull, sqlExpression1, sqlExpression2);
        if (leftNullable && !rightNullable)
          return flag1 == flag2 ? this.ExpandNullableNotEqualNonNullable(left, right, leftIsNull) : this.ExpandNegatedNullableNotEqualNonNullable(left, right, leftIsNull);
        if (rightNullable && !leftNullable)
          return flag1 == flag2 ? this.ExpandNullableNotEqualNonNullable(left, right, sqlExpression1) : this.ExpandNegatedNullableNotEqualNonNullable(left, right, sqlExpression1);
      }
      return (SqlExpression) sqlBinaryExpression.Update(left, right);
    }

    private SqlExpression SimplifyLogicalSqlBinaryExpression(
      SqlBinaryExpression sqlBinaryExpression)
    {
      if (sqlBinaryExpression.Left is SqlUnaryExpression left1 && sqlBinaryExpression.Right is SqlUnaryExpression right1 && (left1.OperatorType == ExpressionType.Equal || left1.OperatorType == ExpressionType.NotEqual) && (right1.OperatorType == ExpressionType.Equal || right1.OperatorType == ExpressionType.NotEqual) && ((object) left1.Operand).Equals((object) right1.Operand))
        return left1.OperatorType == right1.OperatorType ? (SqlExpression) left1 : (SqlExpression) this._sqlExpressionFactory.Constant((object) (sqlBinaryExpression.OperatorType == ExpressionType.OrElse), ((SqlExpression) sqlBinaryExpression).TypeMapping);
      if (sqlBinaryExpression.Left is SqlConstantExpression left2 && left2.Value is bool flag1)
        return sqlBinaryExpression.OperatorType == ExpressionType.AndAlso ? (flag1 ? sqlBinaryExpression.Right : (SqlExpression) left2) : (flag1 ? (SqlExpression) left2 : sqlBinaryExpression.Right);
      return sqlBinaryExpression.Right is SqlConstantExpression right2 && right2.Value is bool flag2 ? (sqlBinaryExpression.OperatorType == ExpressionType.AndAlso ? (flag2 ? sqlBinaryExpression.Left : (SqlExpression) right2) : (flag2 ? (SqlExpression) right2 : sqlBinaryExpression.Left)) : (SqlExpression) sqlBinaryExpression;
    }

    private SqlExpression OptimizeNonNullableNotExpression(
      SqlUnaryExpression sqlUnaryExpression)
    {
      if (sqlUnaryExpression.OperatorType != ExpressionType.Not)
        return (SqlExpression) sqlUnaryExpression;
      switch (sqlUnaryExpression.Operand)
      {
        case SqlConstantExpression constantExpression:
          if (constantExpression.Value is bool flag)
            return (SqlExpression) this._sqlExpressionFactory.Constant((object) !flag, ((SqlExpression) sqlUnaryExpression).TypeMapping);
          break;
        case InExpression inExpression:
          return (SqlExpression) inExpression.Negate();
        case SqlUnaryExpression sqlUnaryExpression1:
          switch (sqlUnaryExpression1.OperatorType)
          {
            case ExpressionType.Equal:
              return (SqlExpression) this._sqlExpressionFactory.IsNotNull(sqlUnaryExpression1.Operand);
            case ExpressionType.Not:
              return sqlUnaryExpression1.Operand;
            case ExpressionType.NotEqual:
              return (SqlExpression) this._sqlExpressionFactory.IsNull(sqlUnaryExpression1.Operand);
          }
          break;
        case SqlBinaryExpression binaryExpression:
          if (binaryExpression.OperatorType == ExpressionType.AndAlso || binaryExpression.OperatorType == ExpressionType.OrElse)
          {
            SqlExpression sqlExpression1 = this.OptimizeNonNullableNotExpression(this._sqlExpressionFactory.Not(binaryExpression.Left));
            SqlExpression sqlExpression2 = this.OptimizeNonNullableNotExpression(this._sqlExpressionFactory.Not(binaryExpression.Right));
            return this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.MakeBinary(binaryExpression.OperatorType == ExpressionType.AndAlso ? ExpressionType.OrElse : ExpressionType.AndAlso, sqlExpression1, sqlExpression2, ((SqlExpression) binaryExpression).TypeMapping));
          }
          if (binaryExpression.OperatorType == ExpressionType.Equal)
          {
            if (binaryExpression.Left is SqlConstantExpression left && ((Expression) left).Type == typeof (bool))
              return (SqlExpression) this._sqlExpressionFactory.MakeBinary(ExpressionType.Equal, (SqlExpression) this._sqlExpressionFactory.Constant((object) !(bool) left.Value, ((SqlExpression) left).TypeMapping), binaryExpression.Right, ((SqlExpression) binaryExpression).TypeMapping);
            if (binaryExpression.Right is SqlConstantExpression right && ((Expression) right).Type == typeof (bool))
              return (SqlExpression) this._sqlExpressionFactory.MakeBinary(ExpressionType.Equal, binaryExpression.Left, (SqlExpression) this._sqlExpressionFactory.Constant((object) !(bool) right.Value, ((SqlExpression) right).TypeMapping), ((SqlExpression) binaryExpression).TypeMapping);
          }
          ExpressionType result;
          if (TryNegate(binaryExpression.OperatorType, out result))
            return (SqlExpression) this._sqlExpressionFactory.MakeBinary(result, binaryExpression.Left, binaryExpression.Right, ((SqlExpression) binaryExpression).TypeMapping);
          break;
      }
      return (SqlExpression) sqlUnaryExpression;

      static bool TryNegate(ExpressionType expressionType, out ExpressionType result)
      {
        if (true)
          ;
        ExpressionType? nullable1;
        switch (expressionType)
        {
          case ExpressionType.Equal:
            nullable1 = new ExpressionType?(ExpressionType.NotEqual);
            break;
          case ExpressionType.GreaterThan:
            nullable1 = new ExpressionType?(ExpressionType.LessThanOrEqual);
            break;
          case ExpressionType.GreaterThanOrEqual:
            nullable1 = new ExpressionType?(ExpressionType.LessThan);
            break;
          case ExpressionType.LessThan:
            nullable1 = new ExpressionType?(ExpressionType.GreaterThanOrEqual);
            break;
          case ExpressionType.LessThanOrEqual:
            nullable1 = new ExpressionType?(ExpressionType.GreaterThan);
            break;
          case ExpressionType.NotEqual:
            nullable1 = new ExpressionType?(ExpressionType.Equal);
            break;
          default:
            nullable1 = new ExpressionType?();
            break;
        }
        if (true)
          ;
        ExpressionType? nullable2 = nullable1;
        result = nullable2.GetValueOrDefault();
        return nullable2.HasValue;
      }
    }

    private SqlExpression ProcessNullNotNull(
      SqlUnaryExpression sqlUnaryExpression,
      bool operandNullable)
    {
      if (!operandNullable)
        return (SqlExpression) this._sqlExpressionFactory.Equal((SqlExpression) this._sqlExpressionFactory.Constant((object) true, (RelationalTypeMapping) null), (SqlExpression) this._sqlExpressionFactory.Constant((object) (sqlUnaryExpression.OperatorType == ExpressionType.NotEqual), ((SqlExpression) sqlUnaryExpression).TypeMapping));
      switch (sqlUnaryExpression.Operand)
      {
        case SqlConstantExpression constantExpression:
          return (SqlExpression) this._sqlExpressionFactory.Equal((SqlExpression) this._sqlExpressionFactory.Constant((object) true, (RelationalTypeMapping) null), (SqlExpression) this._sqlExpressionFactory.Constant((object) (constantExpression.Value == null ^ sqlUnaryExpression.OperatorType == ExpressionType.NotEqual), ((SqlExpression) sqlUnaryExpression).TypeMapping));
        case SqlParameterExpression parameterExpression:
          return (SqlExpression) this._sqlExpressionFactory.Equal((SqlExpression) this._sqlExpressionFactory.Constant((object) true, (RelationalTypeMapping) null), (SqlExpression) this._sqlExpressionFactory.Constant((object) (this.ParameterValues[parameterExpression.Name] == null ^ sqlUnaryExpression.OperatorType == ExpressionType.NotEqual), ((SqlExpression) sqlUnaryExpression).TypeMapping));
        case ColumnExpression columnExpression:
          if (!columnExpression.IsNullable || this._nonNullableColumns.Contains(columnExpression))
            return (SqlExpression) this._sqlExpressionFactory.Equal((SqlExpression) this._sqlExpressionFactory.Constant((object) true, (RelationalTypeMapping) null), (SqlExpression) this._sqlExpressionFactory.Constant((object) (sqlUnaryExpression.OperatorType == ExpressionType.NotEqual), ((SqlExpression) sqlUnaryExpression).TypeMapping));
          break;
        case SqlUnaryExpression sqlUnaryExpression1:
          switch (sqlUnaryExpression1.OperatorType)
          {
            case ExpressionType.Convert:
            case ExpressionType.Negate:
            case ExpressionType.Not:
              return this.ProcessNullNotNull(sqlUnaryExpression.Update(sqlUnaryExpression1.Operand), operandNullable);
            case ExpressionType.Equal:
            case ExpressionType.NotEqual:
              return (SqlExpression) this._sqlExpressionFactory.Equal((SqlExpression) this._sqlExpressionFactory.Constant((object) true, (RelationalTypeMapping) null), (SqlExpression) this._sqlExpressionFactory.Constant((object) (sqlUnaryExpression1.OperatorType == ExpressionType.NotEqual), ((SqlExpression) sqlUnaryExpression1).TypeMapping));
          }
          break;
        case SqlBinaryExpression binaryExpression:
          if (binaryExpression.OperatorType != ExpressionType.AndAlso && binaryExpression.OperatorType != ExpressionType.OrElse)
          {
            SqlExpression sqlExpression1 = this.ProcessNullNotNull(this._sqlExpressionFactory.MakeUnary(sqlUnaryExpression.OperatorType, binaryExpression.Left, typeof (bool), ((SqlExpression) sqlUnaryExpression).TypeMapping), operandNullable);
            SqlExpression sqlExpression2 = this.ProcessNullNotNull(this._sqlExpressionFactory.MakeUnary(sqlUnaryExpression.OperatorType, binaryExpression.Right, typeof (bool), ((SqlExpression) sqlUnaryExpression).TypeMapping), operandNullable);
            return this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.MakeBinary(sqlUnaryExpression.OperatorType == ExpressionType.Equal ? ExpressionType.OrElse : ExpressionType.AndAlso, sqlExpression1, sqlExpression2, ((SqlExpression) sqlUnaryExpression).TypeMapping));
          }
          break;
        case SqlFunctionExpression functionExpression:
          if (functionExpression.IsBuiltIn && string.Equals("COALESCE", functionExpression.Name, StringComparison.OrdinalIgnoreCase))
          {
            SqlExpression sqlExpression3 = this.ProcessNullNotNull(this._sqlExpressionFactory.MakeUnary(sqlUnaryExpression.OperatorType, functionExpression.Arguments[0], typeof (bool), ((SqlExpression) sqlUnaryExpression).TypeMapping), operandNullable);
            SqlExpression sqlExpression4 = this.ProcessNullNotNull(this._sqlExpressionFactory.MakeUnary(sqlUnaryExpression.OperatorType, functionExpression.Arguments[1], typeof (bool), ((SqlExpression) sqlUnaryExpression).TypeMapping), operandNullable);
            return this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.MakeBinary(sqlUnaryExpression.OperatorType == ExpressionType.Equal ? ExpressionType.AndAlso : ExpressionType.OrElse, sqlExpression3, sqlExpression4, ((SqlExpression) sqlUnaryExpression).TypeMapping));
          }
          if (!functionExpression.IsNullable)
            return (SqlExpression) this._sqlExpressionFactory.Equal((SqlExpression) this._sqlExpressionFactory.Constant((object) true, (RelationalTypeMapping) null), (SqlExpression) this._sqlExpressionFactory.Constant((object) (sqlUnaryExpression.OperatorType == ExpressionType.NotEqual), ((SqlExpression) sqlUnaryExpression).TypeMapping));
          List<SqlExpression> source = new List<SqlExpression>();
          int num;
          if (functionExpression.Instance != null)
          {
            bool? propagatesNullability = functionExpression.InstancePropagatesNullability;
            bool flag = true;
            num = propagatesNullability.GetValueOrDefault() == flag & propagatesNullability.HasValue ? 1 : 0;
          }
          else
            num = 0;
          if (num != 0)
            source.Add(functionExpression.Instance);
          if (!functionExpression.IsNiladic)
          {
            for (int index = 0; index < ((IReadOnlyCollection<SqlExpression>) functionExpression.Arguments).Count; ++index)
            {
              if (functionExpression.ArgumentsPropagateNullability[index])
                source.Add(functionExpression.Arguments[index]);
            }
          }
          if (source.Count > 0)
            return ((IEnumerable<SqlExpression>) source).Select<SqlExpression, SqlExpression>((Func<SqlExpression, SqlExpression>) (e => this.ProcessNullNotNull(this._sqlExpressionFactory.MakeUnary(sqlUnaryExpression.OperatorType, e, ((Expression) sqlUnaryExpression).Type, ((SqlExpression) sqlUnaryExpression).TypeMapping), operandNullable))).Aggregate<SqlExpression>((Func<SqlExpression, SqlExpression, SqlExpression>) ((r, e) => this.SimplifyLogicalSqlBinaryExpression(sqlUnaryExpression.OperatorType == ExpressionType.Equal ? this._sqlExpressionFactory.OrElse(r, e) : this._sqlExpressionFactory.AndAlso(r, e))));
          break;
      }
      return (SqlExpression) sqlUnaryExpression;
    }

    private static bool IsLogicalNot(SqlUnaryExpression? sqlUnaryExpression) => sqlUnaryExpression != null && sqlUnaryExpression.OperatorType == ExpressionType.Not && ((Expression) sqlUnaryExpression).Type == typeof (bool);

    private SqlExpression ExpandNullableEqualNullable(
      SqlExpression left,
      SqlExpression right,
      SqlExpression leftIsNull,
      SqlExpression leftIsNotNull,
      SqlExpression rightIsNull,
      SqlExpression rightIsNotNull)
    {
      return this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.OrElse(this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.AndAlso((SqlExpression) this._sqlExpressionFactory.Equal(left, right), this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.AndAlso(leftIsNotNull, rightIsNotNull)))), this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.AndAlso(leftIsNull, rightIsNull))));
    }

    private SqlExpression ExpandNegatedNullableEqualNullable(
      SqlExpression left,
      SqlExpression right,
      SqlExpression leftIsNull,
      SqlExpression leftIsNotNull,
      SqlExpression rightIsNull,
      SqlExpression rightIsNotNull)
    {
      return this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.OrElse(this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.AndAlso((SqlExpression) this._sqlExpressionFactory.NotEqual(left, right), this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.AndAlso(leftIsNotNull, rightIsNotNull)))), this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.AndAlso(leftIsNull, rightIsNull))));
    }

    private SqlExpression ExpandNullableEqualNonNullable(
      SqlExpression left,
      SqlExpression right,
      SqlExpression leftIsNotNull)
    {
      return this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.AndAlso((SqlExpression) this._sqlExpressionFactory.Equal(left, right), leftIsNotNull));
    }

    private SqlExpression ExpandNegatedNullableEqualNonNullable(
      SqlExpression left,
      SqlExpression right,
      SqlExpression leftIsNotNull)
    {
      return this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.AndAlso((SqlExpression) this._sqlExpressionFactory.NotEqual(left, right), leftIsNotNull));
    }

    private SqlExpression ExpandNullableNotEqualNullable(
      SqlExpression left,
      SqlExpression right,
      SqlExpression leftIsNull,
      SqlExpression leftIsNotNull,
      SqlExpression rightIsNull,
      SqlExpression rightIsNotNull)
    {
      return this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.AndAlso(this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.OrElse((SqlExpression) this._sqlExpressionFactory.NotEqual(left, right), this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.OrElse(leftIsNull, rightIsNull)))), this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.OrElse(leftIsNotNull, rightIsNotNull))));
    }

    private SqlExpression ExpandNegatedNullableNotEqualNullable(
      SqlExpression left,
      SqlExpression right,
      SqlExpression leftIsNull,
      SqlExpression leftIsNotNull,
      SqlExpression rightIsNull,
      SqlExpression rightIsNotNull)
    {
      return this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.AndAlso(this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.OrElse((SqlExpression) this._sqlExpressionFactory.Equal(left, right), this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.OrElse(leftIsNull, rightIsNull)))), this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.OrElse(leftIsNotNull, rightIsNotNull))));
    }

    private SqlExpression ExpandNullableNotEqualNonNullable(
      SqlExpression left,
      SqlExpression right,
      SqlExpression leftIsNull)
    {
      return this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.OrElse((SqlExpression) this._sqlExpressionFactory.NotEqual(left, right), leftIsNull));
    }

    private SqlExpression ExpandNegatedNullableNotEqualNonNullable(
      SqlExpression left,
      SqlExpression right,
      SqlExpression leftIsNull)
    {
      return this.SimplifyLogicalSqlBinaryExpression(this._sqlExpressionFactory.OrElse((SqlExpression) this._sqlExpressionFactory.Equal(left, right), leftIsNull));
    }
  }
}
