using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmSqlNullabilityProcessor
	{
		private readonly List<ColumnExpression> _nonNullableColumns;

		private readonly ISqlExpressionFactory _sqlExpressionFactory;

		private bool _canCache;

		protected virtual bool UseRelationalNulls { get; }

		protected virtual IReadOnlyDictionary<string, object?> ParameterValues { get; private set; }

		public DmSqlNullabilityProcessor([NotNull] RelationalParameterBasedSqlProcessorDependencies dependencies, bool useRelationalNulls)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
			_sqlExpressionFactory = dependencies.SqlExpressionFactory;
			UseRelationalNulls = useRelationalNulls;
			_nonNullableColumns = new List<ColumnExpression>();
			ParameterValues = null;
		}

		public virtual SelectExpression Process([NotNull] SelectExpression selectExpression, [NotNull] IReadOnlyDictionary<string, object?> parameterValues, out bool canCache)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selectExpression, "selectExpression");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameterValues, "parameterValues");
			_canCache = true;
			_nonNullableColumns.Clear();
			ParameterValues = parameterValues;
			SelectExpression result = Visit(selectExpression);
			canCache = _canCache;
			return result;
		}

		protected virtual void DoNotCache()
		{
			_canCache = false;
		}

		protected virtual void AddNonNullableColumn([NotNull] ColumnExpression columnExpression)
		{
			_nonNullableColumns.Add(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(columnExpression, "columnExpression"));
		}

		protected virtual TableExpressionBase Visit([NotNull] TableExpressionBase tableExpressionBase)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(tableExpressionBase, "tableExpressionBase");
			CrossApplyExpression crossApplyExpression = tableExpressionBase as CrossApplyExpression;
			if (crossApplyExpression == null)
			{
				CrossJoinExpression crossJoinExpression = tableExpressionBase as CrossJoinExpression;
				if (crossJoinExpression == null)
				{
					ExceptExpression exceptExpression = tableExpressionBase as ExceptExpression;
					if (exceptExpression == null)
					{
						FromSqlExpression fromSqlExpression = tableExpressionBase as FromSqlExpression;
						if (fromSqlExpression == null)
						{
							InnerJoinExpression innerJoinExpression = tableExpressionBase as InnerJoinExpression;
							if (innerJoinExpression == null)
							{
								IntersectExpression intersectExpression = tableExpressionBase as IntersectExpression;
								if (intersectExpression == null)
								{
									LeftJoinExpression leftJoinExpression = tableExpressionBase as LeftJoinExpression;
									if (leftJoinExpression == null)
									{
										OuterApplyExpression outerApplyExpression = tableExpressionBase as OuterApplyExpression;
										if (outerApplyExpression == null)
										{
											SelectExpression selectExpression = tableExpressionBase as SelectExpression;
											if (selectExpression == null)
											{
												TableValuedFunctionExpression tableValuedFunctionExpression = tableExpressionBase as TableValuedFunctionExpression;
												if (tableValuedFunctionExpression == null)
												{
													TableExpression tableExpression = tableExpressionBase as TableExpression;
													if (tableExpression == null)
													{
														UnionExpression unionExpression = tableExpressionBase as UnionExpression;
														if (unionExpression != null)
														{
															SelectExpression source = Visit(unionExpression.Source1);
															SelectExpression source2 = Visit(unionExpression.Source2);
															return unionExpression.Update(source, source2);
														}
														throw new InvalidOperationException(RelationalStrings.UnhandledExpressionInVisitor(tableExpressionBase, tableExpressionBase.GetType(), "SqlNullabilityProcessor"));
													}
													return tableExpression;
												}
												List<SqlExpression> list = new List<SqlExpression>();
												foreach (SqlExpression argument in tableValuedFunctionExpression.Arguments)
												{
													list.Add(Visit(argument, out var _));
												}
												return tableValuedFunctionExpression.Update(list);
											}
											return Visit(selectExpression);
										}
										return outerApplyExpression.Update(Visit(outerApplyExpression.Table));
									}
									TableExpressionBase table = Visit(leftJoinExpression.Table);
									SqlExpression joinPredicate = ProcessJoinPredicate(leftJoinExpression.JoinPredicate);
									return leftJoinExpression.Update(table, joinPredicate);
								}
								SelectExpression source3 = Visit(intersectExpression.Source1);
								SelectExpression source4 = Visit(intersectExpression.Source2);
								return intersectExpression.Update(source3, source4);
							}
							TableExpressionBase table2 = Visit(innerJoinExpression.Table);
							SqlExpression sqlExpression = ProcessJoinPredicate(innerJoinExpression.JoinPredicate);
							return (TryGetBoolConstantValue(sqlExpression) == true) ? ((JoinExpressionBase)new CrossJoinExpression(table2)) : ((JoinExpressionBase)innerJoinExpression.Update(table2, sqlExpression));
						}
						return fromSqlExpression;
					}
					SelectExpression source5 = Visit(exceptExpression.Source1);
					SelectExpression source6 = Visit(exceptExpression.Source2);
					return exceptExpression.Update(source5, source6);
				}
				return crossJoinExpression.Update(Visit(crossJoinExpression.Table));
			}
			return crossApplyExpression.Update(Visit(crossApplyExpression.Table));
		}

		protected virtual SelectExpression Visit([NotNull] SelectExpression selectExpression)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(selectExpression, "selectExpression");
			bool flag = false;
			List<ProjectionExpression> list = (List<ProjectionExpression>)selectExpression.Projection;
			bool nullable;
			for (int i = 0; i < selectExpression.Projection.Count; i++)
			{
				ProjectionExpression projectionExpression = selectExpression.Projection[i];
				ProjectionExpression projectionExpression2 = projectionExpression.Update(Visit(projectionExpression.Expression, out nullable));
				if (projectionExpression2 != projectionExpression && list == selectExpression.Projection)
				{
					list = new List<ProjectionExpression>();
					for (int j = 0; j < i; j++)
					{
						list.Add(selectExpression.Projection[j]);
					}
					flag = true;
				}
				if (list != selectExpression.Projection)
				{
					list.Add(projectionExpression2);
				}
			}
			List<TableExpressionBase> list2 = (List<TableExpressionBase>)selectExpression.Tables;
			for (int k = 0; k < selectExpression.Tables.Count; k++)
			{
				TableExpressionBase tableExpressionBase = selectExpression.Tables[k];
				TableExpressionBase tableExpressionBase2 = Visit(tableExpressionBase);
				if (tableExpressionBase2 != tableExpressionBase && list2 == selectExpression.Tables)
				{
					list2 = new List<TableExpressionBase>();
					for (int l = 0; l < k; l++)
					{
						list2.Add(selectExpression.Tables[l]);
					}
					flag = true;
				}
				if (list2 != selectExpression.Tables)
				{
					list2.Add(tableExpressionBase2);
				}
			}
			SqlExpression sqlExpression = Visit(selectExpression.Predicate, allowOptimizedExpansion: true, out nullable);
			flag |= sqlExpression != selectExpression.Predicate;
			bool? flag2 = TryGetBoolConstantValue(sqlExpression);
			nullable = true;
			if (flag2 == nullable)
			{
				sqlExpression = null;
				flag = true;
			}
			List<SqlExpression> list3 = (List<SqlExpression>)selectExpression.GroupBy;
			for (int m = 0; m < selectExpression.GroupBy.Count; m++)
			{
				SqlExpression sqlExpression2 = selectExpression.GroupBy[m];
				SqlExpression sqlExpression3 = Visit(sqlExpression2, out nullable);
				if (sqlExpression3 != sqlExpression2 && list3 == selectExpression.GroupBy)
				{
					list3 = new List<SqlExpression>();
					for (int n = 0; n < m; n++)
					{
						list3.Add(selectExpression.GroupBy[n]);
					}
					flag = true;
				}
				if (list3 != selectExpression.GroupBy)
				{
					list3.Add(sqlExpression3);
				}
			}
			SqlExpression sqlExpression4 = Visit(selectExpression.Having, allowOptimizedExpansion: true, out nullable);
			flag |= sqlExpression4 != selectExpression.Having;
			flag2 = TryGetBoolConstantValue(sqlExpression4);
			nullable = true;
			if (flag2 == nullable)
			{
				sqlExpression4 = null;
				flag = true;
			}
			List<OrderingExpression> list4 = (List<OrderingExpression>)selectExpression.Orderings;
			for (int num = 0; num < selectExpression.Orderings.Count; num++)
			{
				OrderingExpression orderingExpression = selectExpression.Orderings[num];
				OrderingExpression orderingExpression2 = orderingExpression.Update(Visit(orderingExpression.Expression, out nullable));
				if (orderingExpression2 != orderingExpression && list4 == selectExpression.Orderings)
				{
					list4 = new List<OrderingExpression>();
					for (int num2 = 0; num2 < num; num2++)
					{
						list4.Add(selectExpression.Orderings[num2]);
					}
					flag = true;
				}
				if (list4 != selectExpression.Orderings)
				{
					list4.Add(orderingExpression2);
				}
			}
			SqlExpression sqlExpression5 = Visit(selectExpression.Offset, out nullable);
			flag |= sqlExpression5 != selectExpression.Offset;
			SqlExpression sqlExpression6 = Visit(selectExpression.Limit, out nullable);
			return (flag | (sqlExpression6 != selectExpression.Limit)) ? selectExpression.Update(list, list2, sqlExpression, list3, sqlExpression4, list4, sqlExpression6, sqlExpression5) : selectExpression;
		}

		protected virtual SqlExpression? Visit(SqlExpression? sqlExpression, out bool nullable)
		{
			return Visit(sqlExpression, allowOptimizedExpansion: false, out nullable);
		}

		protected virtual SqlExpression? Visit(SqlExpression? sqlExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			return Visit(sqlExpression, allowOptimizedExpansion, preserveNonNullableColumns: false, out nullable);
		}

		private SqlExpression? Visit(SqlExpression? sqlExpression, bool allowOptimizedExpansion, bool preserveNonNullableColumns, out bool nullable)
		{
			if (sqlExpression == null)
			{
				nullable = false;
				return sqlExpression;
			}
			int count = _nonNullableColumns.Count;
			if (1 == 0)
			{
			}
			CaseExpression caseExpression = sqlExpression as CaseExpression;
			SqlExpression sqlExpression2;
			if (caseExpression == null)
			{
				CollateExpression collateExpression = sqlExpression as CollateExpression;
				if (collateExpression == null)
				{
					ColumnExpression columnExpression = sqlExpression as ColumnExpression;
					if (columnExpression == null)
					{
						DistinctExpression distinctExpression = sqlExpression as DistinctExpression;
						if (distinctExpression == null)
						{
							ExistsExpression existsExpression = sqlExpression as ExistsExpression;
							if (existsExpression == null)
							{
								InExpression inExpression = sqlExpression as InExpression;
								if (inExpression == null)
								{
									LikeExpression likeExpression = sqlExpression as LikeExpression;
									if (likeExpression == null)
									{
										RowNumberExpression rowNumberExpression = sqlExpression as RowNumberExpression;
										if (rowNumberExpression == null)
										{
											ScalarSubqueryExpression scalarSubqueryExpression = sqlExpression as ScalarSubqueryExpression;
											if (scalarSubqueryExpression == null)
											{
												SqlBinaryExpression sqlBinaryExpression = sqlExpression as SqlBinaryExpression;
												if (sqlBinaryExpression == null)
												{
													SqlConstantExpression sqlConstantExpression = sqlExpression as SqlConstantExpression;
													if (sqlConstantExpression == null)
													{
														SqlFragmentExpression sqlFragmentExpression = sqlExpression as SqlFragmentExpression;
														if (sqlFragmentExpression == null)
														{
															SqlFunctionExpression sqlFunctionExpression = sqlExpression as SqlFunctionExpression;
															if (sqlFunctionExpression == null)
															{
																SqlParameterExpression sqlParameterExpression = sqlExpression as SqlParameterExpression;
																if (sqlParameterExpression == null)
																{
																	SqlUnaryExpression sqlUnaryExpression = sqlExpression as SqlUnaryExpression;
																	sqlExpression2 = ((sqlUnaryExpression == null) ? VisitCustomSqlExpression(sqlExpression, allowOptimizedExpansion, out nullable) : VisitSqlUnary(sqlUnaryExpression, allowOptimizedExpansion, out nullable));
																}
																else
																{
																	sqlExpression2 = VisitSqlParameter(sqlParameterExpression, allowOptimizedExpansion, out nullable);
																}
															}
															else
															{
																sqlExpression2 = VisitSqlFunction(sqlFunctionExpression, allowOptimizedExpansion, out nullable);
															}
														}
														else
														{
															sqlExpression2 = VisitSqlFragment(sqlFragmentExpression, allowOptimizedExpansion, out nullable);
														}
													}
													else
													{
														sqlExpression2 = VisitSqlConstant(sqlConstantExpression, allowOptimizedExpansion, out nullable);
													}
												}
												else
												{
													sqlExpression2 = VisitSqlBinary(sqlBinaryExpression, allowOptimizedExpansion, out nullable);
												}
											}
											else
											{
												sqlExpression2 = VisitScalarSubquery(scalarSubqueryExpression, allowOptimizedExpansion, out nullable);
											}
										}
										else
										{
											sqlExpression2 = VisitRowNumber(rowNumberExpression, allowOptimizedExpansion, out nullable);
										}
									}
									else
									{
										sqlExpression2 = VisitLike(likeExpression, allowOptimizedExpansion, out nullable);
									}
								}
								else
								{
									sqlExpression2 = VisitIn(inExpression, allowOptimizedExpansion, out nullable);
								}
							}
							else
							{
								sqlExpression2 = VisitExists(existsExpression, allowOptimizedExpansion, out nullable);
							}
						}
						else
						{
							sqlExpression2 = VisitDistinct(distinctExpression, allowOptimizedExpansion, out nullable);
						}
					}
					else
					{
						sqlExpression2 = VisitColumn(columnExpression, allowOptimizedExpansion, out nullable);
					}
				}
				else
				{
					sqlExpression2 = VisitCollate(collateExpression, allowOptimizedExpansion, out nullable);
				}
			}
			else
			{
				sqlExpression2 = VisitCase(caseExpression, allowOptimizedExpansion, out nullable);
			}
			if (1 == 0)
			{
			}
			SqlExpression result = sqlExpression2;
			if (!preserveNonNullableColumns)
			{
				RestoreNonNullableColumnsList(count);
			}
			return result;
		}

		protected virtual SqlExpression VisitCustomSqlExpression([NotNull] SqlExpression sqlExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			throw new InvalidOperationException(RelationalStrings.UnhandledExpressionInVisitor(sqlExpression, sqlExpression.GetType(), "SqlNullabilityProcessor"));
		}

		protected virtual SqlExpression VisitCase([NotNull] CaseExpression caseExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(caseExpression, "caseExpression");
			nullable = caseExpression.ElseResult == null;
			int count = _nonNullableColumns.Count;
			bool nullable2;
			SqlExpression operand = Visit(caseExpression.Operand, out nullable2);
			List<CaseWhenClause> list = new List<CaseWhenClause>();
			bool allowOptimizedExpansion2 = caseExpression.Operand == null;
			bool flag = false;
			bool valueOrDefault = default(bool);
			foreach (CaseWhenClause whenClause in caseExpression.WhenClauses)
			{
				SqlExpression sqlExpression = Visit(whenClause.Test, allowOptimizedExpansion2, preserveNonNullableColumns: true, out nullable2);
				bool? flag2 = TryGetBoolConstantValue(sqlExpression);
				int num;
				if (flag2.HasValue)
				{
					valueOrDefault = flag2.GetValueOrDefault();
					num = 1;
				}
				else
				{
					num = 0;
				}
				if (num != 0)
				{
					if (!valueOrDefault)
					{
						RestoreNonNullableColumnsList(count);
						continue;
					}
					flag = true;
				}
				bool nullable3;
				SqlExpression result = Visit(whenClause.Result, out nullable3);
				nullable |= nullable3;
				list.Add(new CaseWhenClause(sqlExpression, result));
				RestoreNonNullableColumnsList(count);
				if (flag)
				{
					break;
				}
			}
			SqlExpression sqlExpression2 = null;
			if (!flag)
			{
				sqlExpression2 = Visit(caseExpression.ElseResult, out var nullable4);
				nullable |= nullable4;
			}
			if (list.Count == 0)
			{
				return sqlExpression2 ?? _sqlExpressionFactory.Constant(null, caseExpression.TypeMapping);
			}
			return (sqlExpression2 == null && list.Count == 1 && TryGetBoolConstantValue(list[0].Test) == true) ? list[0].Result : caseExpression.Update(operand, list, sqlExpression2);
		}

		protected virtual SqlExpression VisitCollate([NotNull] CollateExpression collateExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(collateExpression, "collateExpression");
			return collateExpression.Update(Visit(collateExpression.Operand, out nullable));
		}

		protected virtual SqlExpression VisitColumn([NotNull] ColumnExpression columnExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(columnExpression, "columnExpression");
			nullable = columnExpression.IsNullable && !_nonNullableColumns.Contains(columnExpression);
			return columnExpression;
		}

		protected virtual SqlExpression VisitDistinct([NotNull] DistinctExpression distinctExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(distinctExpression, "distinctExpression");
			return distinctExpression.Update(Visit(distinctExpression.Operand, out nullable));
		}

		protected virtual SqlExpression VisitExists([NotNull] ExistsExpression existsExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(existsExpression, "existsExpression");
			SelectExpression selectExpression = Visit(existsExpression.Subquery);
			nullable = false;
			return (TryGetBoolConstantValue(selectExpression.Predicate) == false) ? selectExpression.Predicate : existsExpression.Update(selectExpression);
		}

		protected virtual SqlExpression VisitIn([NotNull] InExpression inExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(inExpression, "inExpression");
			bool nullable2;
			SqlExpression sqlExpression = Visit(inExpression.Item, out nullable2);
			SelectExpression selectExpression;
			int num;
			if (inExpression.Subquery != null)
			{
				selectExpression = Visit(inExpression.Subquery);
				if (TryGetBoolConstantValue(selectExpression.Predicate) == false)
				{
					nullable = false;
					return selectExpression.Predicate;
				}
				if (!nullable2 && selectExpression.Projection.Count == 1)
				{
					ColumnExpression columnExpression = selectExpression.Projection[0].Expression as ColumnExpression;
					if (columnExpression != null)
					{
						num = (columnExpression.IsNullable ? 1 : 0);
						goto IL_00b3;
					}
				}
				num = 1;
				goto IL_00b3;
			}
			if (UseRelationalNulls || (!(inExpression.Values is SqlConstantExpression) && !(inExpression.Values is SqlParameterExpression)))
			{
				(SqlConstantExpression ProcessedValuesExpression, List<object?> ProcessedValuesList, bool HasNullValue) tuple = ProcessInExpressionValues(inExpression.Values, extractNullValues: false);
				SqlConstantExpression item = tuple.ProcessedValuesExpression;
				List<object> item2 = tuple.ProcessedValuesList;
				nullable = false;
				return (item2.Count == 0) ? _sqlExpressionFactory.Constant(false, inExpression.TypeMapping) : SimplifyInExpression(inExpression.Update(sqlExpression, item, null), item, item2);
			}
			var (sqlConstantExpression, list, flag) = ProcessInExpressionValues(inExpression.Values, extractNullValues: true);
			if (list.Count == 0)
			{
				nullable = false;
				return (!flag || !nullable2) ? ((SqlExpression)_sqlExpressionFactory.Equal(_sqlExpressionFactory.Constant(true, inExpression.TypeMapping), _sqlExpressionFactory.Constant(inExpression.IsNegated, inExpression.TypeMapping))) : ((SqlExpression)(inExpression.IsNegated ? _sqlExpressionFactory.IsNotNull(sqlExpression) : _sqlExpressionFactory.IsNull(sqlExpression)));
			}
			SqlExpression sqlExpression2 = SimplifyInExpression(inExpression.Update(sqlExpression, sqlConstantExpression, null), sqlConstantExpression, list);
			if (!nullable2 || (allowOptimizedExpansion && !inExpression.IsNegated && !flag))
			{
				nullable = false;
				return sqlExpression2;
			}
			nullable = false;
			return (inExpression.IsNegated == flag) ? _sqlExpressionFactory.AndAlso(sqlExpression2, _sqlExpressionFactory.IsNotNull(sqlExpression)) : _sqlExpressionFactory.OrElse(sqlExpression2, _sqlExpressionFactory.IsNull(sqlExpression));
			IL_00b3:
			nullable = (byte)num != 0;
			return inExpression.Update(sqlExpression, null, selectExpression);
			(SqlConstantExpression ProcessedValuesExpression, List<object?> ProcessedValuesList, bool HasNullValue) ProcessInExpressionValues(SqlExpression valuesExpression, bool extractNullValues)
			{
				List<object> list2 = new List<object>();
				bool item3 = false;
				RelationalTypeMapping typeMapping = null;
				IEnumerable enumerable = null;
				SqlConstantExpression sqlConstantExpression2 = valuesExpression as SqlConstantExpression;
				if (sqlConstantExpression2 != null)
				{
					typeMapping = sqlConstantExpression2.TypeMapping;
					enumerable = (IEnumerable)sqlConstantExpression2.Value;
				}
				else
				{
					SqlParameterExpression sqlParameterExpression = valuesExpression as SqlParameterExpression;
					if (sqlParameterExpression != null)
					{
						DoNotCache();
						typeMapping = sqlParameterExpression.TypeMapping;
						enumerable = (IEnumerable)ParameterValues[sqlParameterExpression.Name];
						if (enumerable == null)
						{
							throw new NullReferenceException();
						}
					}
				}
				foreach (object item5 in enumerable)
				{
					if (item5 == null && extractNullValues)
					{
						item3 = true;
					}
					else
					{
						list2.Add(item5);
					}
				}
				SqlConstantExpression item4 = _sqlExpressionFactory.Constant(list2, typeMapping);
				return (item4, list2, item3);
			}
			SqlExpression SimplifyInExpression(InExpression inExpression, SqlConstantExpression inValuesExpression, List<object?> inValuesList)
			{
				return (inValuesList.Count != 1) ? ((SqlExpression)inExpression) : ((SqlExpression)(inExpression.IsNegated ? _sqlExpressionFactory.NotEqual(inExpression.Item, _sqlExpressionFactory.Constant(inValuesList[0], inValuesExpression.TypeMapping)) : _sqlExpressionFactory.Equal(inExpression.Item, _sqlExpressionFactory.Constant(inValuesList[0], inExpression.Values.TypeMapping))));
			}
		}

		protected virtual SqlExpression VisitLike([NotNull] LikeExpression likeExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(likeExpression, "likeExpression");
			bool nullable2;
			SqlExpression match = Visit(likeExpression.Match, out nullable2);
			bool nullable3;
			SqlExpression pattern = Visit(likeExpression.Pattern, out nullable3);
			bool nullable4;
			SqlExpression escapeChar = Visit(likeExpression.EscapeChar, out nullable4);
			nullable = nullable2 || nullable3 || nullable4;
			return likeExpression.Update(match, pattern, escapeChar);
		}

		protected virtual SqlExpression VisitRowNumber([NotNull] RowNumberExpression rowNumberExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(rowNumberExpression, "rowNumberExpression");
			bool flag = false;
			List<SqlExpression> list = new List<SqlExpression>();
			bool nullable2;
			foreach (SqlExpression partition in rowNumberExpression.Partitions)
			{
				SqlExpression sqlExpression = Visit(partition, out nullable2);
				flag = flag || sqlExpression != partition;
				list.Add(sqlExpression);
			}
			List<OrderingExpression> list2 = new List<OrderingExpression>();
			foreach (OrderingExpression ordering in rowNumberExpression.Orderings)
			{
				OrderingExpression orderingExpression = ordering.Update(Visit(ordering.Expression, out nullable2));
				flag = flag || orderingExpression != ordering;
				list2.Add(orderingExpression);
			}
			nullable = false;
			return flag ? rowNumberExpression.Update(list, list2) : rowNumberExpression;
		}

		protected virtual SqlExpression VisitScalarSubquery([NotNull] ScalarSubqueryExpression scalarSubqueryExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(scalarSubqueryExpression, "scalarSubqueryExpression");
			nullable = true;
			return scalarSubqueryExpression.Update(Visit(scalarSubqueryExpression.Subquery));
		}

		protected virtual SqlExpression VisitSqlBinary([NotNull] SqlBinaryExpression sqlBinaryExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlBinaryExpression, "sqlBinaryExpression");
			bool optimize = allowOptimizedExpansion;
			allowOptimizedExpansion = allowOptimizedExpansion && (sqlBinaryExpression.OperatorType == ExpressionType.AndAlso || sqlBinaryExpression.OperatorType == ExpressionType.OrElse);
			int count = _nonNullableColumns.Count;
			bool nullable2;
			SqlExpression sqlExpression = Visit(sqlBinaryExpression.Left, allowOptimizedExpansion, preserveNonNullableColumns: true, out nullable2);
			List<ColumnExpression> first = _nonNullableColumns.Skip(count).ToList();
			if (sqlBinaryExpression.OperatorType != ExpressionType.AndAlso)
			{
				RestoreNonNullableColumnsList(count);
			}
			bool nullable3;
			SqlExpression sqlExpression2 = Visit(sqlBinaryExpression.Right, allowOptimizedExpansion, preserveNonNullableColumns: true, out nullable3);
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
			if (sqlBinaryExpression.OperatorType == ExpressionType.Add && sqlBinaryExpression.Type == typeof(string))
			{
				if (nullable2)
				{
					sqlExpression = AddNullConcatenationProtection(sqlExpression, sqlBinaryExpression.TypeMapping);
				}
				if (nullable3)
				{
					sqlExpression2 = AddNullConcatenationProtection(sqlExpression2, sqlBinaryExpression.TypeMapping);
				}
				nullable = false;
				return sqlBinaryExpression.Update(sqlExpression, sqlExpression2);
			}
			if (sqlBinaryExpression.OperatorType == ExpressionType.Equal || sqlBinaryExpression.OperatorType == ExpressionType.NotEqual)
			{
				SqlBinaryExpression sqlBinaryExpression2 = sqlBinaryExpression.Update(sqlExpression, sqlExpression2);
				SqlExpression sqlExpression3 = OptimizeComparison(sqlBinaryExpression2, sqlExpression, sqlExpression2, nullable2, nullable3, out nullable);
				SqlUnaryExpression sqlUnaryExpression = sqlExpression3 as SqlUnaryExpression;
				ColumnExpression columnExpression = default(ColumnExpression);
				int num;
				if (sqlUnaryExpression != null && sqlUnaryExpression.OperatorType == ExpressionType.NotEqual)
				{
					columnExpression = sqlUnaryExpression.Operand as ColumnExpression;
					num = ((columnExpression != null) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				if (num != 0)
				{
					_nonNullableColumns.Add(columnExpression);
				}
				if (sqlExpression3.Equals(sqlBinaryExpression2) && (nullable2 || nullable3) && !UseRelationalNulls)
				{
					return RewriteNullSemantics(sqlBinaryExpression2, sqlBinaryExpression2.Left, sqlBinaryExpression2.Right, nullable2, nullable3, optimize, out nullable);
				}
				return sqlExpression3;
			}
			nullable = nullable2 || nullable3;
			SqlBinaryExpression sqlBinaryExpression3 = sqlBinaryExpression.Update(sqlExpression, sqlExpression2);
			object result;
			if (sqlBinaryExpression3 != null)
			{
				SqlBinaryExpression sqlBinaryExpression4 = sqlBinaryExpression3;
				if (sqlBinaryExpression.OperatorType == ExpressionType.AndAlso || sqlBinaryExpression.OperatorType == ExpressionType.OrElse)
				{
					result = SimplifyLogicalSqlBinaryExpression(sqlBinaryExpression4);
					goto IL_025c;
				}
			}
			result = sqlBinaryExpression3;
			goto IL_025c;
			IL_025c:
			return (SqlExpression)result;
			SqlExpression AddNullConcatenationProtection(SqlExpression argument, RelationalTypeMapping typeMapping)
			{
				return (argument is SqlConstantExpression || argument is SqlParameterExpression) ? ((SqlExpression)_sqlExpressionFactory.Constant(string.Empty, typeMapping)) : ((SqlExpression)_sqlExpressionFactory.Coalesce(argument, _sqlExpressionFactory.Constant(string.Empty, typeMapping)));
			}
		}

		protected virtual SqlExpression VisitSqlConstant([NotNull] SqlConstantExpression sqlConstantExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlConstantExpression, "sqlConstantExpression");
			nullable = sqlConstantExpression.Value == null;
			return sqlConstantExpression;
		}

		protected virtual SqlExpression VisitSqlFragment([NotNull] SqlFragmentExpression sqlFragmentExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlFragmentExpression, "sqlFragmentExpression");
			nullable = false;
			return sqlFragmentExpression;
		}

		protected virtual SqlExpression VisitSqlFunction([NotNull] SqlFunctionExpression sqlFunctionExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlFunctionExpression, "sqlFunctionExpression");
			if (sqlFunctionExpression.IsBuiltIn && sqlFunctionExpression.Arguments != null && string.Equals(sqlFunctionExpression.Name, "COALESCE", StringComparison.OrdinalIgnoreCase))
			{
				bool nullable2;
				SqlExpression sqlExpression = Visit(sqlFunctionExpression.Arguments[0], out nullable2);
				bool nullable3;
				SqlExpression sqlExpression2 = Visit(sqlFunctionExpression.Arguments[1], out nullable3);
				nullable = nullable2 && nullable3;
				return sqlFunctionExpression.Update(sqlFunctionExpression.Instance, new SqlExpression[2] { sqlExpression, sqlExpression2 });
			}
			bool nullable4;
			SqlExpression instance = Visit(sqlFunctionExpression.Instance, out nullable4);
			nullable = sqlFunctionExpression.IsNullable;
			if (sqlFunctionExpression.IsNiladic)
			{
				return sqlFunctionExpression.Update(instance, sqlFunctionExpression.Arguments);
			}
			SqlExpression[] array = new SqlExpression[sqlFunctionExpression.Arguments.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Visit(sqlFunctionExpression.Arguments[i], out nullable4);
			}
			return (sqlFunctionExpression.IsBuiltIn && string.Equals(sqlFunctionExpression.Name, "SUM", StringComparison.OrdinalIgnoreCase)) ? _sqlExpressionFactory.Coalesce(sqlFunctionExpression.Update(instance, array), _sqlExpressionFactory.Constant(0, sqlFunctionExpression.TypeMapping), sqlFunctionExpression.TypeMapping) : sqlFunctionExpression.Update(instance, array);
		}

		protected virtual SqlExpression VisitSqlParameter([NotNull] SqlParameterExpression sqlParameterExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlParameterExpression, "sqlParameterExpression");
			nullable = ParameterValues[sqlParameterExpression.Name] == null;
			return nullable ? ((SqlExpression)_sqlExpressionFactory.Constant(null, sqlParameterExpression.TypeMapping)) : ((SqlExpression)sqlParameterExpression);
		}

		protected virtual SqlExpression VisitSqlUnary([NotNull] SqlUnaryExpression sqlUnaryExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlUnaryExpression, "sqlUnaryExpression");
			bool nullable2;
			SqlExpression operand = Visit(sqlUnaryExpression.Operand, out nullable2);
			SqlUnaryExpression sqlUnaryExpression2 = sqlUnaryExpression.Update(operand);
			if (sqlUnaryExpression.OperatorType == ExpressionType.Equal || sqlUnaryExpression.OperatorType == ExpressionType.NotEqual)
			{
				SqlExpression sqlExpression = ProcessNullNotNull(sqlUnaryExpression2, nullable2);
				nullable = false;
				SqlUnaryExpression sqlUnaryExpression3 = sqlExpression as SqlUnaryExpression;
				ColumnExpression columnExpression = default(ColumnExpression);
				int num;
				if (sqlUnaryExpression3 != null && sqlUnaryExpression3.OperatorType == ExpressionType.NotEqual)
				{
					columnExpression = sqlUnaryExpression3.Operand as ColumnExpression;
					num = ((columnExpression != null) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				if (num != 0)
				{
					_nonNullableColumns.Add(columnExpression);
				}
				return sqlExpression;
			}
			nullable = nullable2;
			return (!nullable2 && sqlUnaryExpression.OperatorType == ExpressionType.Not) ? OptimizeNonNullableNotExpression(sqlUnaryExpression2) : sqlUnaryExpression2;
		}

		private static bool? TryGetBoolConstantValue(SqlExpression? expression)
		{
			SqlConstantExpression sqlConstantExpression = expression as SqlConstantExpression;
			bool? result;
			if (sqlConstantExpression != null)
			{
				object value = sqlConstantExpression.Value;
				if (value is bool)
				{
					bool value2 = (bool)value;
					result = value2;
					goto IL_0033;
				}
			}
			result = null;
			goto IL_0033;
			IL_0033:
			return result;
		}

		private void RestoreNonNullableColumnsList(int counter)
		{
			if (counter < _nonNullableColumns.Count)
			{
				_nonNullableColumns.RemoveRange(counter, _nonNullableColumns.Count - counter);
			}
		}

		private SqlExpression ProcessJoinPredicate(SqlExpression predicate)
		{
			SqlBinaryExpression sqlBinaryExpression = predicate as SqlBinaryExpression;
			if (sqlBinaryExpression != null)
			{
				bool nullable3;
				if (sqlBinaryExpression.OperatorType == ExpressionType.Equal)
				{
					bool nullable;
					SqlExpression left = Visit(sqlBinaryExpression.Left, allowOptimizedExpansion: true, out nullable);
					bool nullable2;
					SqlExpression right = Visit(sqlBinaryExpression.Right, allowOptimizedExpansion: true, out nullable2);
					return OptimizeComparison(sqlBinaryExpression.Update(left, right), left, right, nullable, nullable2, out nullable3);
				}
				if (sqlBinaryExpression.OperatorType == ExpressionType.AndAlso || sqlBinaryExpression.OperatorType == ExpressionType.NotEqual || sqlBinaryExpression.OperatorType == ExpressionType.GreaterThan || sqlBinaryExpression.OperatorType == ExpressionType.GreaterThanOrEqual || sqlBinaryExpression.OperatorType == ExpressionType.LessThan || sqlBinaryExpression.OperatorType == ExpressionType.LessThanOrEqual)
				{
					return Visit(sqlBinaryExpression, allowOptimizedExpansion: true, out nullable3);
				}
			}
			throw new InvalidOperationException(RelationalStrings.UnhandledExpressionInVisitor(predicate, predicate.GetType(), "SqlNullabilityProcessor"));
		}

		private SqlExpression OptimizeComparison(SqlBinaryExpression sqlBinaryExpression, SqlExpression left, SqlExpression right, bool leftNullable, bool rightNullable, out bool nullable)
		{
			bool flag = leftNullable && (left is SqlConstantExpression || left is SqlParameterExpression);
			if (rightNullable && (right is SqlConstantExpression || right is SqlParameterExpression))
			{
				SqlExpression result = ((sqlBinaryExpression.OperatorType == ExpressionType.Equal) ? ProcessNullNotNull(_sqlExpressionFactory.IsNull(left), leftNullable) : ProcessNullNotNull(_sqlExpressionFactory.IsNotNull(left), leftNullable));
				nullable = false;
				return result;
			}
			if (flag)
			{
				SqlExpression result2 = ((sqlBinaryExpression.OperatorType == ExpressionType.Equal) ? ProcessNullNotNull(_sqlExpressionFactory.IsNull(right), rightNullable) : ProcessNullNotNull(_sqlExpressionFactory.IsNotNull(right), rightNullable));
				nullable = false;
				return result2;
			}
			if (!leftNullable && left.Equals(right))
			{
				nullable = false;
				return _sqlExpressionFactory.Constant(sqlBinaryExpression.OperatorType == ExpressionType.Equal, sqlBinaryExpression.TypeMapping);
			}
			if (!leftNullable && !rightNullable && (sqlBinaryExpression.OperatorType == ExpressionType.Equal || sqlBinaryExpression.OperatorType == ExpressionType.NotEqual))
			{
				SqlUnaryExpression sqlUnaryExpression = left as SqlUnaryExpression;
				SqlUnaryExpression sqlUnaryExpression2 = right as SqlUnaryExpression;
				bool flag2 = IsLogicalNot(sqlUnaryExpression);
				bool flag3 = IsLogicalNot(sqlUnaryExpression2);
				if (flag2)
				{
					left = sqlUnaryExpression.Operand;
				}
				if (flag3)
				{
					right = sqlUnaryExpression2.Operand;
				}
				nullable = false;
				return ((sqlBinaryExpression.OperatorType == ExpressionType.Equal) ^ (flag2 == flag3)) ? _sqlExpressionFactory.NotEqual(left, right) : _sqlExpressionFactory.Equal(left, right);
			}
			nullable = false;
			return sqlBinaryExpression.Update(left, right);
		}

		private SqlExpression RewriteNullSemantics(SqlBinaryExpression sqlBinaryExpression, SqlExpression left, SqlExpression right, bool leftNullable, bool rightNullable, bool optimize, out bool nullable)
		{
			SqlUnaryExpression sqlUnaryExpression = left as SqlUnaryExpression;
			SqlUnaryExpression sqlUnaryExpression2 = right as SqlUnaryExpression;
			bool flag = IsLogicalNot(sqlUnaryExpression);
			bool flag2 = IsLogicalNot(sqlUnaryExpression2);
			if (flag)
			{
				left = sqlUnaryExpression.Operand;
			}
			if (flag2)
			{
				right = sqlUnaryExpression2.Operand;
			}
			SqlExpression sqlExpression = ProcessNullNotNull(_sqlExpressionFactory.IsNull(left), leftNullable);
			SqlExpression leftIsNotNull = OptimizeNonNullableNotExpression(_sqlExpressionFactory.Not(sqlExpression));
			SqlExpression sqlExpression2 = ProcessNullNotNull(_sqlExpressionFactory.IsNull(right), rightNullable);
			SqlExpression sqlExpression3 = OptimizeNonNullableNotExpression(_sqlExpressionFactory.Not(sqlExpression2));
			if (optimize && sqlBinaryExpression.OperatorType == ExpressionType.Equal && !flag && !flag2)
			{
				if (leftNullable && rightNullable)
				{
					nullable = true;
					return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse(_sqlExpressionFactory.Equal(left, right), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(sqlExpression, sqlExpression2))));
				}
				if ((leftNullable && !rightNullable) || (!leftNullable && rightNullable))
				{
					nullable = true;
					return _sqlExpressionFactory.Equal(left, right);
				}
			}
			nullable = false;
			if (sqlBinaryExpression.OperatorType == ExpressionType.Equal)
			{
				if (leftNullable && rightNullable)
				{
					return (flag == flag2) ? ExpandNullableEqualNullable(left, right, sqlExpression, leftIsNotNull, sqlExpression2, sqlExpression3) : ExpandNegatedNullableEqualNullable(left, right, sqlExpression, leftIsNotNull, sqlExpression2, sqlExpression3);
				}
				if (leftNullable && !rightNullable)
				{
					return (flag == flag2) ? ExpandNullableEqualNonNullable(left, right, leftIsNotNull) : ExpandNegatedNullableEqualNonNullable(left, right, leftIsNotNull);
				}
				if (rightNullable && !leftNullable)
				{
					return (flag == flag2) ? ExpandNullableEqualNonNullable(left, right, sqlExpression3) : ExpandNegatedNullableEqualNonNullable(left, right, sqlExpression3);
				}
			}
			if (sqlBinaryExpression.OperatorType == ExpressionType.NotEqual)
			{
				if (leftNullable && rightNullable)
				{
					return (flag == flag2) ? ExpandNullableNotEqualNullable(left, right, sqlExpression, leftIsNotNull, sqlExpression2, sqlExpression3) : ExpandNegatedNullableNotEqualNullable(left, right, sqlExpression, leftIsNotNull, sqlExpression2, sqlExpression3);
				}
				if (leftNullable && !rightNullable)
				{
					return (flag == flag2) ? ExpandNullableNotEqualNonNullable(left, right, sqlExpression) : ExpandNegatedNullableNotEqualNonNullable(left, right, sqlExpression);
				}
				if (rightNullable && !leftNullable)
				{
					return (flag == flag2) ? ExpandNullableNotEqualNonNullable(left, right, sqlExpression2) : ExpandNegatedNullableNotEqualNonNullable(left, right, sqlExpression2);
				}
			}
			return sqlBinaryExpression.Update(left, right);
		}

		private SqlExpression SimplifyLogicalSqlBinaryExpression(SqlBinaryExpression sqlBinaryExpression)
		{
			SqlUnaryExpression sqlUnaryExpression = sqlBinaryExpression.Left as SqlUnaryExpression;
			SqlUnaryExpression sqlUnaryExpression2 = default(SqlUnaryExpression);
			int num;
			if (sqlUnaryExpression != null)
			{
				sqlUnaryExpression2 = sqlBinaryExpression.Right as SqlUnaryExpression;
				if (sqlUnaryExpression2 != null && (sqlUnaryExpression.OperatorType == ExpressionType.Equal || sqlUnaryExpression.OperatorType == ExpressionType.NotEqual) && (sqlUnaryExpression2.OperatorType == ExpressionType.Equal || sqlUnaryExpression2.OperatorType == ExpressionType.NotEqual))
				{
					num = (sqlUnaryExpression.Operand.Equals(sqlUnaryExpression2.Operand) ? 1 : 0);
					goto IL_005b;
				}
			}
			num = 0;
			goto IL_005b;
			IL_005b:
			if (num != 0)
			{
				return (sqlUnaryExpression.OperatorType == sqlUnaryExpression2.OperatorType) ? ((SqlExpression)sqlUnaryExpression) : ((SqlExpression)_sqlExpressionFactory.Constant(sqlBinaryExpression.OperatorType == ExpressionType.OrElse, sqlBinaryExpression.TypeMapping));
			}
			SqlConstantExpression sqlConstantExpression = sqlBinaryExpression.Left as SqlConstantExpression;
			bool flag = default(bool);
			int num2;
			if (sqlConstantExpression != null)
			{
				object value = sqlConstantExpression.Value;
				if (value is bool)
				{
					flag = (bool)value;
					num2 = 1;
				}
				else
				{
					num2 = 0;
				}
			}
			else
			{
				num2 = 0;
			}
			if (num2 != 0)
			{
				return (sqlBinaryExpression.OperatorType != ExpressionType.AndAlso) ? (flag ? sqlConstantExpression : sqlBinaryExpression.Right) : (flag ? sqlBinaryExpression.Right : sqlConstantExpression);
			}
			SqlConstantExpression sqlConstantExpression2 = sqlBinaryExpression.Right as SqlConstantExpression;
			bool flag2 = default(bool);
			int num3;
			if (sqlConstantExpression2 != null)
			{
				object value = sqlConstantExpression2.Value;
				if (value is bool)
				{
					flag2 = (bool)value;
					num3 = 1;
				}
				else
				{
					num3 = 0;
				}
			}
			else
			{
				num3 = 0;
			}
			if (num3 != 0)
			{
				return (sqlBinaryExpression.OperatorType != ExpressionType.AndAlso) ? (flag2 ? sqlConstantExpression2 : sqlBinaryExpression.Left) : (flag2 ? sqlBinaryExpression.Left : sqlConstantExpression2);
			}
			return sqlBinaryExpression;
		}

		private SqlExpression OptimizeNonNullableNotExpression(SqlUnaryExpression sqlUnaryExpression)
		{
			if (sqlUnaryExpression.OperatorType != ExpressionType.Not)
			{
				return sqlUnaryExpression;
			}
			SqlExpression operand = sqlUnaryExpression.Operand;
			SqlExpression sqlExpression = operand;
			SqlConstantExpression sqlConstantExpression = sqlExpression as SqlConstantExpression;
			if (sqlConstantExpression == null)
			{
				InExpression inExpression = sqlExpression as InExpression;
				if (inExpression != null)
				{
					return inExpression.Negate();
				}
				SqlUnaryExpression sqlUnaryExpression2 = sqlExpression as SqlUnaryExpression;
				if (sqlUnaryExpression2 == null)
				{
					SqlBinaryExpression sqlBinaryExpression = sqlExpression as SqlBinaryExpression;
					if (sqlBinaryExpression != null)
					{
						if (sqlBinaryExpression.OperatorType == ExpressionType.AndAlso || sqlBinaryExpression.OperatorType == ExpressionType.OrElse)
						{
							SqlExpression left = OptimizeNonNullableNotExpression(_sqlExpressionFactory.Not(sqlBinaryExpression.Left));
							SqlExpression right = OptimizeNonNullableNotExpression(_sqlExpressionFactory.Not(sqlBinaryExpression.Right));
							return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.MakeBinary((sqlBinaryExpression.OperatorType == ExpressionType.AndAlso) ? ExpressionType.OrElse : ExpressionType.AndAlso, left, right, sqlBinaryExpression.TypeMapping));
						}
						if (sqlBinaryExpression.OperatorType == ExpressionType.Equal)
						{
							SqlConstantExpression sqlConstantExpression2 = sqlBinaryExpression.Left as SqlConstantExpression;
							if (sqlConstantExpression2 != null && sqlConstantExpression2.Type == typeof(bool))
							{
								return _sqlExpressionFactory.MakeBinary(ExpressionType.Equal, _sqlExpressionFactory.Constant(!(bool)sqlConstantExpression2.Value, sqlConstantExpression2.TypeMapping), sqlBinaryExpression.Right, sqlBinaryExpression.TypeMapping);
							}
							SqlConstantExpression sqlConstantExpression3 = sqlBinaryExpression.Right as SqlConstantExpression;
							if (sqlConstantExpression3 != null && sqlConstantExpression3.Type == typeof(bool))
							{
								return _sqlExpressionFactory.MakeBinary(ExpressionType.Equal, sqlBinaryExpression.Left, _sqlExpressionFactory.Constant(!(bool)sqlConstantExpression3.Value, sqlConstantExpression3.TypeMapping), sqlBinaryExpression.TypeMapping);
							}
						}
						if (TryNegate(sqlBinaryExpression.OperatorType, out var result2))
						{
							return _sqlExpressionFactory.MakeBinary(result2, sqlBinaryExpression.Left, sqlBinaryExpression.Right, sqlBinaryExpression.TypeMapping);
						}
					}
				}
				else
				{
					switch (sqlUnaryExpression2.OperatorType)
					{
					case ExpressionType.Not:
						return sqlUnaryExpression2.Operand;
					case ExpressionType.Equal:
						return _sqlExpressionFactory.IsNotNull(sqlUnaryExpression2.Operand);
					case ExpressionType.NotEqual:
						return _sqlExpressionFactory.IsNull(sqlUnaryExpression2.Operand);
					}
				}
			}
			else
			{
				object value = sqlConstantExpression.Value;
				if (value is bool)
				{
					bool flag = (bool)value;
					return _sqlExpressionFactory.Constant(!flag, sqlUnaryExpression.TypeMapping);
				}
			}
			return sqlUnaryExpression;
			static bool TryNegate(ExpressionType expressionType, out ExpressionType result)
			{
				if (1 == 0)
				{
				}
				ExpressionType? expressionType2 = expressionType switch
				{
					ExpressionType.Equal => ExpressionType.NotEqual, 
					ExpressionType.NotEqual => ExpressionType.Equal, 
					ExpressionType.GreaterThan => ExpressionType.LessThanOrEqual, 
					ExpressionType.GreaterThanOrEqual => ExpressionType.LessThan, 
					ExpressionType.LessThan => ExpressionType.GreaterThanOrEqual, 
					ExpressionType.LessThanOrEqual => ExpressionType.GreaterThan, 
					_ => null, 
				};
				if (1 == 0)
				{
				}
				ExpressionType? expressionType3 = expressionType2;
				result = expressionType3.GetValueOrDefault();
				return expressionType3.HasValue;
			}
		}

		private SqlExpression ProcessNullNotNull(SqlUnaryExpression sqlUnaryExpression, bool operandNullable)
		{
			if (!operandNullable)
			{
				return _sqlExpressionFactory.Constant(sqlUnaryExpression.OperatorType == ExpressionType.NotEqual, sqlUnaryExpression.TypeMapping);
			}
			SqlExpression operand = sqlUnaryExpression.Operand;
			SqlExpression sqlExpression = operand;
			SqlConstantExpression sqlConstantExpression = sqlExpression as SqlConstantExpression;
			if (sqlConstantExpression == null)
			{
				SqlParameterExpression sqlParameterExpression = sqlExpression as SqlParameterExpression;
				if (sqlParameterExpression == null)
				{
					ColumnExpression columnExpression = sqlExpression as ColumnExpression;
					if (columnExpression == null)
					{
						SqlUnaryExpression sqlUnaryExpression2 = sqlExpression as SqlUnaryExpression;
						if (sqlUnaryExpression2 == null)
						{
							SqlBinaryExpression sqlBinaryExpression = sqlExpression as SqlBinaryExpression;
							if (sqlBinaryExpression == null)
							{
								SqlFunctionExpression sqlFunctionExpression = sqlExpression as SqlFunctionExpression;
								if (sqlFunctionExpression != null)
								{
									if (sqlFunctionExpression.IsBuiltIn && string.Equals("COALESCE", sqlFunctionExpression.Name, StringComparison.OrdinalIgnoreCase))
									{
										SqlExpression left = ProcessNullNotNull(_sqlExpressionFactory.MakeUnary(sqlUnaryExpression.OperatorType, sqlFunctionExpression.Arguments[0], typeof(bool), sqlUnaryExpression.TypeMapping), operandNullable);
										SqlExpression right = ProcessNullNotNull(_sqlExpressionFactory.MakeUnary(sqlUnaryExpression.OperatorType, sqlFunctionExpression.Arguments[1], typeof(bool), sqlUnaryExpression.TypeMapping), operandNullable);
										return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.MakeBinary((sqlUnaryExpression.OperatorType == ExpressionType.Equal) ? ExpressionType.AndAlso : ExpressionType.OrElse, left, right, sqlUnaryExpression.TypeMapping));
									}
									if (!sqlFunctionExpression.IsNullable)
									{
										return _sqlExpressionFactory.Constant(sqlUnaryExpression.OperatorType == ExpressionType.NotEqual, sqlUnaryExpression.TypeMapping);
									}
									List<SqlExpression> list = new List<SqlExpression>();
									if (sqlFunctionExpression.Instance != null && sqlFunctionExpression.InstancePropagatesNullability == true)
									{
										list.Add(sqlFunctionExpression.Instance);
									}
									if (!sqlFunctionExpression.IsNiladic)
									{
										for (int i = 0; i < sqlFunctionExpression.Arguments.Count; i++)
										{
											if (sqlFunctionExpression.ArgumentsPropagateNullability[i])
											{
												list.Add(sqlFunctionExpression.Arguments[i]);
											}
										}
									}
									if (list.Count > 0)
									{
										return list.Select((SqlExpression e) => ProcessNullNotNull(_sqlExpressionFactory.MakeUnary(sqlUnaryExpression.OperatorType, e, sqlUnaryExpression.Type, sqlUnaryExpression.TypeMapping), operandNullable)).Aggregate((SqlExpression r, SqlExpression e) => SimplifyLogicalSqlBinaryExpression((sqlUnaryExpression.OperatorType == ExpressionType.Equal) ? _sqlExpressionFactory.OrElse(r, e) : _sqlExpressionFactory.AndAlso(r, e)));
									}
								}
							}
							else if (sqlBinaryExpression.OperatorType != ExpressionType.AndAlso && sqlBinaryExpression.OperatorType != ExpressionType.OrElse)
							{
								SqlExpression left2 = ProcessNullNotNull(_sqlExpressionFactory.MakeUnary(sqlUnaryExpression.OperatorType, sqlBinaryExpression.Left, typeof(bool), sqlUnaryExpression.TypeMapping), operandNullable);
								SqlExpression right2 = ProcessNullNotNull(_sqlExpressionFactory.MakeUnary(sqlUnaryExpression.OperatorType, sqlBinaryExpression.Right, typeof(bool), sqlUnaryExpression.TypeMapping), operandNullable);
								return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.MakeBinary((sqlUnaryExpression.OperatorType == ExpressionType.Equal) ? ExpressionType.OrElse : ExpressionType.AndAlso, left2, right2, sqlUnaryExpression.TypeMapping));
							}
						}
						else
						{
							switch (sqlUnaryExpression2.OperatorType)
							{
							case ExpressionType.Convert:
							case ExpressionType.Negate:
							case ExpressionType.Not:
								return ProcessNullNotNull(sqlUnaryExpression.Update(sqlUnaryExpression2.Operand), operandNullable);
							case ExpressionType.Equal:
							case ExpressionType.NotEqual:
								return _sqlExpressionFactory.Constant(sqlUnaryExpression2.OperatorType == ExpressionType.NotEqual, sqlUnaryExpression2.TypeMapping);
							}
						}
					}
					else if (!columnExpression.IsNullable || _nonNullableColumns.Contains(columnExpression))
					{
						return _sqlExpressionFactory.Constant(sqlUnaryExpression.OperatorType == ExpressionType.NotEqual, sqlUnaryExpression.TypeMapping);
					}
					return sqlUnaryExpression;
				}
				return _sqlExpressionFactory.Constant((ParameterValues[sqlParameterExpression.Name] == null) ^ (sqlUnaryExpression.OperatorType == ExpressionType.NotEqual), sqlUnaryExpression.TypeMapping);
			}
			return _sqlExpressionFactory.Constant((sqlConstantExpression.Value == null) ^ (sqlUnaryExpression.OperatorType == ExpressionType.NotEqual), sqlUnaryExpression.TypeMapping);
		}

		private static bool IsLogicalNot(SqlUnaryExpression? sqlUnaryExpression)
		{
			return sqlUnaryExpression != null && sqlUnaryExpression!.OperatorType == ExpressionType.Not && sqlUnaryExpression!.Type == typeof(bool);
		}

		private SqlExpression ExpandNullableEqualNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull, SqlExpression leftIsNotNull, SqlExpression rightIsNull, SqlExpression rightIsNotNull)
		{
			return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse(SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(_sqlExpressionFactory.Equal(left, right), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(leftIsNotNull, rightIsNotNull)))), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(leftIsNull, rightIsNull))));
		}

		private SqlExpression ExpandNegatedNullableEqualNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull, SqlExpression leftIsNotNull, SqlExpression rightIsNull, SqlExpression rightIsNotNull)
		{
			return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse(SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(_sqlExpressionFactory.NotEqual(left, right), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(leftIsNotNull, rightIsNotNull)))), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(leftIsNull, rightIsNull))));
		}

		private SqlExpression ExpandNullableEqualNonNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNotNull)
		{
			return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(_sqlExpressionFactory.Equal(left, right), leftIsNotNull));
		}

		private SqlExpression ExpandNegatedNullableEqualNonNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNotNull)
		{
			return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(_sqlExpressionFactory.NotEqual(left, right), leftIsNotNull));
		}

		private SqlExpression ExpandNullableNotEqualNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull, SqlExpression leftIsNotNull, SqlExpression rightIsNull, SqlExpression rightIsNotNull)
		{
			return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse(_sqlExpressionFactory.NotEqual(left, right), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse(leftIsNull, rightIsNull)))), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse(leftIsNotNull, rightIsNotNull))));
		}

		private SqlExpression ExpandNegatedNullableNotEqualNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull, SqlExpression leftIsNotNull, SqlExpression rightIsNull, SqlExpression rightIsNotNull)
		{
			return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse(_sqlExpressionFactory.Equal(left, right), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse(leftIsNull, rightIsNull)))), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse(leftIsNotNull, rightIsNotNull))));
		}

		private SqlExpression ExpandNullableNotEqualNonNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull)
		{
			return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse(_sqlExpressionFactory.NotEqual(left, right), leftIsNull));
		}

		private SqlExpression ExpandNegatedNullableNotEqualNonNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull)
		{
			return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse(_sqlExpressionFactory.Equal(left, right), leftIsNull));
		}
	}
}
