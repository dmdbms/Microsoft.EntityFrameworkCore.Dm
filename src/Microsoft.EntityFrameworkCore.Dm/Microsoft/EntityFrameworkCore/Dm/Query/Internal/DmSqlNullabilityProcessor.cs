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
			Check.NotNull<RelationalParameterBasedSqlProcessorDependencies>(dependencies, "dependencies");
			_sqlExpressionFactory = dependencies.SqlExpressionFactory;
			UseRelationalNulls = useRelationalNulls;
			_nonNullableColumns = new List<ColumnExpression>();
			ParameterValues = null;
		}

		public virtual SelectExpression Process([NotNull] SelectExpression selectExpression, [NotNull] IReadOnlyDictionary<string, object?> parameterValues, out bool canCache)
		{
			Check.NotNull<SelectExpression>(selectExpression, "selectExpression");
			Check.NotNull(parameterValues, "parameterValues");
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
			_nonNullableColumns.Add(Check.NotNull<ColumnExpression>(columnExpression, "columnExpression"));
		}

		protected virtual TableExpressionBase Visit([NotNull] TableExpressionBase tableExpressionBase)
		{
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			Check.NotNull<TableExpressionBase>(tableExpressionBase, "tableExpressionBase");
			CrossApplyExpression val = (CrossApplyExpression)(object)((tableExpressionBase is CrossApplyExpression) ? tableExpressionBase : null);
			if (val == null)
			{
				CrossJoinExpression val2 = (CrossJoinExpression)(object)((tableExpressionBase is CrossJoinExpression) ? tableExpressionBase : null);
				if (val2 == null)
				{
					ExceptExpression val3 = (ExceptExpression)(object)((tableExpressionBase is ExceptExpression) ? tableExpressionBase : null);
					if (val3 == null)
					{
						FromSqlExpression val4 = (FromSqlExpression)(object)((tableExpressionBase is FromSqlExpression) ? tableExpressionBase : null);
						if (val4 == null)
						{
							InnerJoinExpression val5 = (InnerJoinExpression)(object)((tableExpressionBase is InnerJoinExpression) ? tableExpressionBase : null);
							if (val5 == null)
							{
								IntersectExpression val6 = (IntersectExpression)(object)((tableExpressionBase is IntersectExpression) ? tableExpressionBase : null);
								if (val6 == null)
								{
									LeftJoinExpression val7 = (LeftJoinExpression)(object)((tableExpressionBase is LeftJoinExpression) ? tableExpressionBase : null);
									if (val7 == null)
									{
										OuterApplyExpression val8 = (OuterApplyExpression)(object)((tableExpressionBase is OuterApplyExpression) ? tableExpressionBase : null);
										if (val8 == null)
										{
											SelectExpression val9 = (SelectExpression)(object)((tableExpressionBase is SelectExpression) ? tableExpressionBase : null);
											if (val9 == null)
											{
												TableValuedFunctionExpression val10 = (TableValuedFunctionExpression)(object)((tableExpressionBase is TableValuedFunctionExpression) ? tableExpressionBase : null);
												if (val10 == null)
												{
													TableExpression val11 = (TableExpression)(object)((tableExpressionBase is TableExpression) ? tableExpressionBase : null);
													if (val11 == null)
													{
														UnionExpression val12 = (UnionExpression)(object)((tableExpressionBase is UnionExpression) ? tableExpressionBase : null);
														if (val12 != null)
														{
															SelectExpression val13 = Visit(((SetOperationBase)val12).Source1);
															SelectExpression val14 = Visit(((SetOperationBase)val12).Source2);
															return (TableExpressionBase)(object)val12.Update(val13, val14);
														}
														throw new InvalidOperationException(RelationalStrings.UnhandledExpressionInVisitor((object)tableExpressionBase, (object)((object)tableExpressionBase).GetType(), (object)"SqlNullabilityProcessor"));
													}
													return (TableExpressionBase)(object)val11;
												}
												List<SqlExpression> list = new List<SqlExpression>();
												foreach (SqlExpression argument in val10.Arguments)
												{
													list.Add(Visit(argument, out var _));
												}
												return (TableExpressionBase)(object)val10.Update((IReadOnlyList<SqlExpression>)list);
											}
											return (TableExpressionBase)(object)Visit(val9);
										}
										return (TableExpressionBase)(object)val8.Update(Visit(((JoinExpressionBase)val8).Table));
									}
									TableExpressionBase val15 = Visit(((JoinExpressionBase)val7).Table);
									SqlExpression val16 = ProcessJoinPredicate(((PredicateJoinExpressionBase)val7).JoinPredicate);
									return (TableExpressionBase)(object)val7.Update(val15, val16);
								}
								SelectExpression val17 = Visit(((SetOperationBase)val6).Source1);
								SelectExpression val18 = Visit(((SetOperationBase)val6).Source2);
								return (TableExpressionBase)(object)val6.Update(val17, val18);
							}
							TableExpressionBase val19 = Visit(((JoinExpressionBase)val5).Table);
							SqlExpression val20 = ProcessJoinPredicate(((PredicateJoinExpressionBase)val5).JoinPredicate);
							return (TableExpressionBase)((TryGetBoolConstantValue(val20) == true) ? ((object)new CrossJoinExpression(val19)) : ((object)val5.Update(val19, val20)));
						}
						return (TableExpressionBase)(object)val4;
					}
					SelectExpression val21 = Visit(((SetOperationBase)val3).Source1);
					SelectExpression val22 = Visit(((SetOperationBase)val3).Source2);
					return (TableExpressionBase)(object)val3.Update(val21, val22);
				}
				return (TableExpressionBase)(object)val2.Update(Visit(((JoinExpressionBase)val2).Table));
			}
			return (TableExpressionBase)(object)val.Update(Visit(((JoinExpressionBase)val).Table));
		}

		protected virtual SelectExpression Visit([NotNull] SelectExpression selectExpression)
		{
			Check.NotNull<SelectExpression>(selectExpression, "selectExpression");
			bool flag = false;
			List<ProjectionExpression> list = (List<ProjectionExpression>)selectExpression.Projection;
			bool nullable;
			for (int i = 0; i < selectExpression.Projection.Count; i++)
			{
				ProjectionExpression val = selectExpression.Projection[i];
				ProjectionExpression val2 = val.Update(Visit(val.Expression, out nullable));
				if (val2 != val && list == selectExpression.Projection)
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
					list.Add(val2);
				}
			}
			List<TableExpressionBase> list2 = (List<TableExpressionBase>)selectExpression.Tables;
			for (int k = 0; k < selectExpression.Tables.Count; k++)
			{
				TableExpressionBase val3 = selectExpression.Tables[k];
				TableExpressionBase val4 = Visit(val3);
				if (val4 != val3 && list2 == selectExpression.Tables)
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
					list2.Add(val4);
				}
			}
			SqlExpression val5 = Visit(selectExpression.Predicate, allowOptimizedExpansion: true, out nullable);
			flag |= val5 != selectExpression.Predicate;
			bool? flag2 = TryGetBoolConstantValue(val5);
			nullable = true;
			if (flag2 == nullable)
			{
				val5 = null;
				flag = true;
			}
			List<SqlExpression> list3 = (List<SqlExpression>)selectExpression.GroupBy;
			for (int m = 0; m < selectExpression.GroupBy.Count; m++)
			{
				SqlExpression val6 = selectExpression.GroupBy[m];
				SqlExpression val7 = Visit(val6, out nullable);
				if (val7 != val6 && list3 == selectExpression.GroupBy)
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
					list3.Add(val7);
				}
			}
			SqlExpression val8 = Visit(selectExpression.Having, allowOptimizedExpansion: true, out nullable);
			flag |= val8 != selectExpression.Having;
			flag2 = TryGetBoolConstantValue(val8);
			nullable = true;
			if (flag2 == nullable)
			{
				val8 = null;
				flag = true;
			}
			List<OrderingExpression> list4 = (List<OrderingExpression>)selectExpression.Orderings;
			for (int num = 0; num < selectExpression.Orderings.Count; num++)
			{
				OrderingExpression val9 = selectExpression.Orderings[num];
				OrderingExpression val10 = val9.Update(Visit(val9.Expression, out nullable));
				if (val10 != val9 && list4 == selectExpression.Orderings)
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
					list4.Add(val10);
				}
			}
			SqlExpression val11 = Visit(selectExpression.Offset, out nullable);
			flag |= val11 != selectExpression.Offset;
			SqlExpression val12 = Visit(selectExpression.Limit, out nullable);
			return (SelectExpression)((flag | (val12 != selectExpression.Limit)) ? ((object)selectExpression.Update((IReadOnlyList<ProjectionExpression>)list, (IReadOnlyList<TableExpressionBase>)list2, val5, (IReadOnlyList<SqlExpression>)list3, val8, (IReadOnlyList<OrderingExpression>)list4, val12, val11)) : ((object)selectExpression));
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
			CaseExpression val = (CaseExpression)(object)((sqlExpression is CaseExpression) ? sqlExpression : null);
			SqlExpression val16;
			if (val == null)
			{
				CollateExpression val2 = (CollateExpression)(object)((sqlExpression is CollateExpression) ? sqlExpression : null);
				if (val2 == null)
				{
					ColumnExpression val3 = (ColumnExpression)(object)((sqlExpression is ColumnExpression) ? sqlExpression : null);
					if (val3 == null)
					{
						DistinctExpression val4 = (DistinctExpression)(object)((sqlExpression is DistinctExpression) ? sqlExpression : null);
						if (val4 == null)
						{
							ExistsExpression val5 = (ExistsExpression)(object)((sqlExpression is ExistsExpression) ? sqlExpression : null);
							if (val5 == null)
							{
								InExpression val6 = (InExpression)(object)((sqlExpression is InExpression) ? sqlExpression : null);
								if (val6 == null)
								{
									LikeExpression val7 = (LikeExpression)(object)((sqlExpression is LikeExpression) ? sqlExpression : null);
									if (val7 == null)
									{
										RowNumberExpression val8 = (RowNumberExpression)(object)((sqlExpression is RowNumberExpression) ? sqlExpression : null);
										if (val8 == null)
										{
											ScalarSubqueryExpression val9 = (ScalarSubqueryExpression)(object)((sqlExpression is ScalarSubqueryExpression) ? sqlExpression : null);
											if (val9 == null)
											{
												SqlBinaryExpression val10 = (SqlBinaryExpression)(object)((sqlExpression is SqlBinaryExpression) ? sqlExpression : null);
												if (val10 == null)
												{
													SqlConstantExpression val11 = (SqlConstantExpression)(object)((sqlExpression is SqlConstantExpression) ? sqlExpression : null);
													if (val11 == null)
													{
														SqlFragmentExpression val12 = (SqlFragmentExpression)(object)((sqlExpression is SqlFragmentExpression) ? sqlExpression : null);
														if (val12 == null)
														{
															SqlFunctionExpression val13 = (SqlFunctionExpression)(object)((sqlExpression is SqlFunctionExpression) ? sqlExpression : null);
															if (val13 == null)
															{
																SqlParameterExpression val14 = (SqlParameterExpression)(object)((sqlExpression is SqlParameterExpression) ? sqlExpression : null);
																if (val14 == null)
																{
																	SqlUnaryExpression val15 = (SqlUnaryExpression)(object)((sqlExpression is SqlUnaryExpression) ? sqlExpression : null);
																	val16 = ((val15 == null) ? VisitCustomSqlExpression(sqlExpression, allowOptimizedExpansion, out nullable) : VisitSqlUnary(val15, allowOptimizedExpansion, out nullable));
																}
																else
																{
																	val16 = VisitSqlParameter(val14, allowOptimizedExpansion, out nullable);
																}
															}
															else
															{
																val16 = VisitSqlFunction(val13, allowOptimizedExpansion, out nullable);
															}
														}
														else
														{
															val16 = VisitSqlFragment(val12, allowOptimizedExpansion, out nullable);
														}
													}
													else
													{
														val16 = VisitSqlConstant(val11, allowOptimizedExpansion, out nullable);
													}
												}
												else
												{
													val16 = VisitSqlBinary(val10, allowOptimizedExpansion, out nullable);
												}
											}
											else
											{
												val16 = VisitScalarSubquery(val9, allowOptimizedExpansion, out nullable);
											}
										}
										else
										{
											val16 = VisitRowNumber(val8, allowOptimizedExpansion, out nullable);
										}
									}
									else
									{
										val16 = VisitLike(val7, allowOptimizedExpansion, out nullable);
									}
								}
								else
								{
									val16 = VisitIn(val6, allowOptimizedExpansion, out nullable);
								}
							}
							else
							{
								val16 = VisitExists(val5, allowOptimizedExpansion, out nullable);
							}
						}
						else
						{
							val16 = VisitDistinct(val4, allowOptimizedExpansion, out nullable);
						}
					}
					else
					{
						val16 = VisitColumn(val3, allowOptimizedExpansion, out nullable);
					}
				}
				else
				{
					val16 = VisitCollate(val2, allowOptimizedExpansion, out nullable);
				}
			}
			else
			{
				val16 = VisitCase(val, allowOptimizedExpansion, out nullable);
			}
			if (1 == 0)
			{
			}
			SqlExpression result = val16;
			if (!preserveNonNullableColumns)
			{
				RestoreNonNullableColumnsList(count);
			}
			return result;
		}

		protected virtual SqlExpression VisitCustomSqlExpression([NotNull] SqlExpression sqlExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			throw new InvalidOperationException(RelationalStrings.UnhandledExpressionInVisitor((object)sqlExpression, (object)((object)sqlExpression).GetType(), (object)"SqlNullabilityProcessor"));
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
			bool valueOrDefault = default(bool);
			foreach (CaseWhenClause whenClause in caseExpression.WhenClauses)
			{
				SqlExpression val2 = Visit(whenClause.Test, allowOptimizedExpansion2, preserveNonNullableColumns: true, out nullable2);
				bool? flag2 = TryGetBoolConstantValue(val2);
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

		protected virtual SqlExpression VisitCollate([NotNull] CollateExpression collateExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Check.NotNull<CollateExpression>(collateExpression, "collateExpression");
			return (SqlExpression)(object)collateExpression.Update(Visit(collateExpression.Operand, out nullable));
		}

		protected virtual SqlExpression VisitColumn([NotNull] ColumnExpression columnExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Check.NotNull<ColumnExpression>(columnExpression, "columnExpression");
			nullable = columnExpression.IsNullable && !_nonNullableColumns.Contains(columnExpression);
			return (SqlExpression)(object)columnExpression;
		}

		protected virtual SqlExpression VisitDistinct([NotNull] DistinctExpression distinctExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Check.NotNull<DistinctExpression>(distinctExpression, "distinctExpression");
			return (SqlExpression)(object)distinctExpression.Update(Visit(distinctExpression.Operand, out nullable));
		}

		protected virtual SqlExpression VisitExists([NotNull] ExistsExpression existsExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Check.NotNull<ExistsExpression>(existsExpression, "existsExpression");
			SelectExpression val = Visit(existsExpression.Subquery);
			nullable = false;
			return (SqlExpression)((TryGetBoolConstantValue(val.Predicate) == false) ? ((object)val.Predicate) : ((object)existsExpression.Update(val)));
		}

		protected virtual SqlExpression VisitIn([NotNull] InExpression inExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Check.NotNull<InExpression>(inExpression, "inExpression");
			bool nullable2;
			SqlExpression val = Visit(inExpression.Item, out nullable2);
			SelectExpression val2;
			int num;
			if (inExpression.Subquery != null)
			{
				val2 = Visit(inExpression.Subquery);
				if (TryGetBoolConstantValue(val2.Predicate) == false)
				{
					nullable = false;
					return val2.Predicate;
				}
				if (!nullable2 && val2.Projection.Count == 1)
				{
					SqlExpression expression = val2.Projection[0].Expression;
					ColumnExpression val3 = (ColumnExpression)(object)((expression is ColumnExpression) ? expression : null);
					if (val3 != null)
					{
						num = (val3.IsNullable ? 1 : 0);
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
				return (SqlExpression)((item2.Count == 0) ? ((object)_sqlExpressionFactory.Constant((object)false, ((SqlExpression)inExpression).TypeMapping)) : ((object)SimplifyInExpression(inExpression.Update(val, (SqlExpression)(object)item, (SelectExpression)null), item, item2)));
			}
			var (val4, list, flag) = ProcessInExpressionValues(inExpression.Values, extractNullValues: true);
			if (list.Count == 0)
			{
				nullable = false;
				return (SqlExpression)((!flag || !nullable2) ? ((object)_sqlExpressionFactory.Equal((SqlExpression)(object)_sqlExpressionFactory.Constant((object)true, ((SqlExpression)inExpression).TypeMapping), (SqlExpression)(object)_sqlExpressionFactory.Constant((object)inExpression.IsNegated, ((SqlExpression)inExpression).TypeMapping))) : ((object)(inExpression.IsNegated ? _sqlExpressionFactory.IsNotNull(val) : _sqlExpressionFactory.IsNull(val))));
			}
			SqlExpression val5 = SimplifyInExpression(inExpression.Update(val, (SqlExpression)(object)val4, (SelectExpression)null), val4, list);
			if (!nullable2 || (allowOptimizedExpansion && !inExpression.IsNegated && !flag))
			{
				nullable = false;
				return val5;
			}
			nullable = false;
			return (SqlExpression)(object)((inExpression.IsNegated == flag) ? _sqlExpressionFactory.AndAlso(val5, (SqlExpression)(object)_sqlExpressionFactory.IsNotNull(val)) : _sqlExpressionFactory.OrElse(val5, (SqlExpression)(object)_sqlExpressionFactory.IsNull(val)));
			IL_00b3:
			nullable = (byte)num != 0;
			return (SqlExpression)(object)inExpression.Update(val, (SqlExpression)null, val2);
			(SqlConstantExpression ProcessedValuesExpression, List<object?> ProcessedValuesList, bool HasNullValue) ProcessInExpressionValues(SqlExpression valuesExpression, bool extractNullValues)
			{
				List<object> list2 = new List<object>();
				bool item3 = false;
				RelationalTypeMapping val6 = null;
				IEnumerable enumerable = null;
				SqlConstantExpression val7 = (SqlConstantExpression)(object)((valuesExpression is SqlConstantExpression) ? valuesExpression : null);
				if (val7 != null)
				{
					val6 = ((SqlExpression)val7).TypeMapping;
					enumerable = (IEnumerable)val7.Value;
				}
				else
				{
					SqlParameterExpression val8 = (SqlParameterExpression)(object)((valuesExpression is SqlParameterExpression) ? valuesExpression : null);
					if (val8 != null)
					{
						DoNotCache();
						val6 = ((SqlExpression)val8).TypeMapping;
						enumerable = (IEnumerable)ParameterValues[val8.Name];
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
				SqlConstantExpression item4 = _sqlExpressionFactory.Constant((object)list2, val6);
				return (item4, list2, item3);
			}
			SqlExpression SimplifyInExpression(InExpression inExpression, SqlConstantExpression inValuesExpression, List<object?> inValuesList)
			{
				return (SqlExpression)((inValuesList.Count != 1) ? ((object)inExpression) : ((object)(inExpression.IsNegated ? _sqlExpressionFactory.NotEqual(inExpression.Item, (SqlExpression)(object)_sqlExpressionFactory.Constant(inValuesList[0], ((SqlExpression)inValuesExpression).TypeMapping)) : _sqlExpressionFactory.Equal(inExpression.Item, (SqlExpression)(object)_sqlExpressionFactory.Constant(inValuesList[0], inExpression.Values.TypeMapping)))));
			}
		}

		protected virtual SqlExpression VisitLike([NotNull] LikeExpression likeExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Check.NotNull<LikeExpression>(likeExpression, "likeExpression");
			bool nullable2;
			SqlExpression val = Visit(likeExpression.Match, out nullable2);
			bool nullable3;
			SqlExpression val2 = Visit(likeExpression.Pattern, out nullable3);
			bool nullable4;
			SqlExpression val3 = Visit(likeExpression.EscapeChar, out nullable4);
			nullable = nullable2 || nullable3 || nullable4;
			return (SqlExpression)(object)likeExpression.Update(val, val2, val3);
		}

		protected virtual SqlExpression VisitRowNumber([NotNull] RowNumberExpression rowNumberExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Check.NotNull<RowNumberExpression>(rowNumberExpression, "rowNumberExpression");
			bool flag = false;
			List<SqlExpression> list = new List<SqlExpression>();
			bool nullable2;
			foreach (SqlExpression partition in rowNumberExpression.Partitions)
			{
				SqlExpression val = Visit(partition, out nullable2);
				flag = flag || val != partition;
				list.Add(val);
			}
			List<OrderingExpression> list2 = new List<OrderingExpression>();
			foreach (OrderingExpression ordering in rowNumberExpression.Orderings)
			{
				OrderingExpression val2 = ordering.Update(Visit(ordering.Expression, out nullable2));
				flag = flag || val2 != ordering;
				list2.Add(val2);
			}
			nullable = false;
			return (SqlExpression)(flag ? ((object)rowNumberExpression.Update((IReadOnlyList<SqlExpression>)list, (IReadOnlyList<OrderingExpression>)list2)) : ((object)rowNumberExpression));
		}

		protected virtual SqlExpression VisitScalarSubquery([NotNull] ScalarSubqueryExpression scalarSubqueryExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Check.NotNull<ScalarSubqueryExpression>(scalarSubqueryExpression, "scalarSubqueryExpression");
			nullable = true;
			return (SqlExpression)(object)scalarSubqueryExpression.Update(Visit(scalarSubqueryExpression.Subquery));
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
				ColumnExpression val6 = default(ColumnExpression);
				int num;
				if (val5 != null && val5.OperatorType == ExpressionType.NotEqual)
				{
					SqlExpression operand = val5.Operand;
					val6 = (ColumnExpression)(object)((operand is ColumnExpression) ? operand : null);
					num = ((val6 != null) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				if (num != 0)
				{
					_nonNullableColumns.Add(val6);
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

		protected virtual SqlExpression VisitSqlConstant([NotNull] SqlConstantExpression sqlConstantExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Check.NotNull<SqlConstantExpression>(sqlConstantExpression, "sqlConstantExpression");
			nullable = sqlConstantExpression.Value == null;
			return (SqlExpression)(object)sqlConstantExpression;
		}

		protected virtual SqlExpression VisitSqlFragment([NotNull] SqlFragmentExpression sqlFragmentExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Check.NotNull<SqlFragmentExpression>(sqlFragmentExpression, "sqlFragmentExpression");
			nullable = false;
			return (SqlExpression)(object)sqlFragmentExpression;
		}

		protected virtual SqlExpression VisitSqlFunction([NotNull] SqlFunctionExpression sqlFunctionExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Check.NotNull<SqlFunctionExpression>(sqlFunctionExpression, "sqlFunctionExpression");
			if (sqlFunctionExpression.IsBuiltIn && sqlFunctionExpression.Arguments != null && string.Equals(sqlFunctionExpression.Name, "COALESCE", StringComparison.OrdinalIgnoreCase))
			{
				bool nullable2;
				SqlExpression val = Visit(sqlFunctionExpression.Arguments[0], out nullable2);
				bool nullable3;
				SqlExpression val2 = Visit(sqlFunctionExpression.Arguments[1], out nullable3);
				nullable = nullable2 && nullable3;
				return (SqlExpression)(object)sqlFunctionExpression.Update(sqlFunctionExpression.Instance, (IReadOnlyList<SqlExpression>)(object)new SqlExpression[2] { val, val2 });
			}
			bool nullable4;
			SqlExpression val3 = Visit(sqlFunctionExpression.Instance, out nullable4);
			nullable = sqlFunctionExpression.IsNullable;
			if (sqlFunctionExpression.IsNiladic)
			{
				return (SqlExpression)(object)sqlFunctionExpression.Update(val3, sqlFunctionExpression.Arguments);
			}
			SqlExpression[] array = (SqlExpression[])(object)new SqlExpression[sqlFunctionExpression.Arguments.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Visit(sqlFunctionExpression.Arguments[i], out nullable4);
			}
			return (SqlExpression)(object)((sqlFunctionExpression.IsBuiltIn && string.Equals(sqlFunctionExpression.Name, "SUM", StringComparison.OrdinalIgnoreCase)) ? _sqlExpressionFactory.Coalesce((SqlExpression)(object)sqlFunctionExpression.Update(val3, (IReadOnlyList<SqlExpression>)array), (SqlExpression)(object)_sqlExpressionFactory.Constant((object)0, ((SqlExpression)sqlFunctionExpression).TypeMapping), ((SqlExpression)sqlFunctionExpression).TypeMapping) : sqlFunctionExpression.Update(val3, (IReadOnlyList<SqlExpression>)array));
		}

		protected virtual SqlExpression VisitSqlParameter([NotNull] SqlParameterExpression sqlParameterExpression, bool allowOptimizedExpansion, out bool nullable)
		{
			Check.NotNull<SqlParameterExpression>(sqlParameterExpression, "sqlParameterExpression");
			nullable = ParameterValues[sqlParameterExpression.Name] == null;
			return (SqlExpression)(nullable ? ((object)_sqlExpressionFactory.Constant((object)null, ((SqlExpression)sqlParameterExpression).TypeMapping)) : ((object)sqlParameterExpression));
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
				ColumnExpression val5 = default(ColumnExpression);
				int num;
				if (val4 != null && val4.OperatorType == ExpressionType.NotEqual)
				{
					SqlExpression operand = val4.Operand;
					val5 = (ColumnExpression)(object)((operand is ColumnExpression) ? operand : null);
					num = ((val5 != null) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				if (num != 0)
				{
					_nonNullableColumns.Add(val5);
				}
				return val3;
			}
			nullable = nullable2;
			return (SqlExpression)((!nullable2 && sqlUnaryExpression.OperatorType == ExpressionType.Not) ? ((object)OptimizeNonNullableNotExpression(val2)) : ((object)val2));
		}

		private static bool? TryGetBoolConstantValue(SqlExpression? expression)
		{
			SqlConstantExpression val = (SqlConstantExpression)(object)((expression is SqlConstantExpression) ? expression : null);
			bool? result;
			if (val != null)
			{
				object value = val.Value;
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
			SqlBinaryExpression val = (SqlBinaryExpression)(object)((predicate is SqlBinaryExpression) ? predicate : null);
			if (val != null)
			{
				bool nullable3;
				if (val.OperatorType == ExpressionType.Equal)
				{
					bool nullable;
					SqlExpression val2 = Visit(val.Left, allowOptimizedExpansion: true, out nullable);
					bool nullable2;
					SqlExpression val3 = Visit(val.Right, allowOptimizedExpansion: true, out nullable2);
					return OptimizeComparison(val.Update(val2, val3), val2, val3, nullable, nullable2, out nullable3);
				}
				if (val.OperatorType == ExpressionType.AndAlso || val.OperatorType == ExpressionType.NotEqual || val.OperatorType == ExpressionType.GreaterThan || val.OperatorType == ExpressionType.GreaterThanOrEqual || val.OperatorType == ExpressionType.LessThan || val.OperatorType == ExpressionType.LessThanOrEqual)
				{
					return Visit((SqlExpression?)(object)val, allowOptimizedExpansion: true, out nullable3);
				}
			}
			throw new InvalidOperationException(RelationalStrings.UnhandledExpressionInVisitor((object)predicate, (object)((object)predicate).GetType(), (object)"SqlNullabilityProcessor"));
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
			if (!leftNullable && ((object)left).Equals((object?)right))
			{
				nullable = false;
				return (SqlExpression)(object)_sqlExpressionFactory.Equal((SqlExpression)(object)_sqlExpressionFactory.Constant((object)true, (RelationalTypeMapping)null), (SqlExpression)(object)_sqlExpressionFactory.Constant((object)(sqlBinaryExpression.OperatorType == ExpressionType.Equal), ((SqlExpression)sqlBinaryExpression).TypeMapping));
			}
			if (!leftNullable && !rightNullable && (sqlBinaryExpression.OperatorType == ExpressionType.Equal || sqlBinaryExpression.OperatorType == ExpressionType.NotEqual))
			{
				SqlUnaryExpression val = (SqlUnaryExpression)(object)((left is SqlUnaryExpression) ? left : null);
				SqlUnaryExpression val2 = (SqlUnaryExpression)(object)((right is SqlUnaryExpression) ? right : null);
				bool flag2 = IsLogicalNot(val);
				bool flag3 = IsLogicalNot(val2);
				if (flag2)
				{
					left = val.Operand;
				}
				if (flag3)
				{
					right = val2.Operand;
				}
				nullable = false;
				return (SqlExpression)(object)(((sqlBinaryExpression.OperatorType == ExpressionType.Equal) ^ (flag2 == flag3)) ? _sqlExpressionFactory.NotEqual(left, right) : _sqlExpressionFactory.Equal(left, right));
			}
			nullable = false;
			return (SqlExpression)(object)sqlBinaryExpression.Update(left, right);
		}

		private SqlExpression RewriteNullSemantics(SqlBinaryExpression sqlBinaryExpression, SqlExpression left, SqlExpression right, bool leftNullable, bool rightNullable, bool optimize, out bool nullable)
		{
			SqlUnaryExpression val = (SqlUnaryExpression)(object)((left is SqlUnaryExpression) ? left : null);
			SqlUnaryExpression val2 = (SqlUnaryExpression)(object)((right is SqlUnaryExpression) ? right : null);
			bool flag = IsLogicalNot(val);
			bool flag2 = IsLogicalNot(val2);
			if (flag)
			{
				left = val.Operand;
			}
			if (flag2)
			{
				right = val2.Operand;
			}
			SqlExpression val3 = ProcessNullNotNull(_sqlExpressionFactory.IsNull(left), leftNullable);
			SqlExpression leftIsNotNull = OptimizeNonNullableNotExpression(_sqlExpressionFactory.Not(val3));
			SqlExpression val4 = ProcessNullNotNull(_sqlExpressionFactory.IsNull(right), rightNullable);
			SqlExpression val5 = OptimizeNonNullableNotExpression(_sqlExpressionFactory.Not(val4));
			if (optimize && sqlBinaryExpression.OperatorType == ExpressionType.Equal && !flag && !flag2)
			{
				if (leftNullable && rightNullable)
				{
					nullable = true;
					return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse((SqlExpression)(object)_sqlExpressionFactory.Equal(left, right), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(val3, val4))));
				}
				if ((leftNullable && !rightNullable) || (!leftNullable && rightNullable))
				{
					nullable = true;
					return (SqlExpression)(object)_sqlExpressionFactory.Equal(left, right);
				}
			}
			nullable = false;
			if (sqlBinaryExpression.OperatorType == ExpressionType.Equal)
			{
				if (leftNullable && rightNullable)
				{
					return (flag == flag2) ? ExpandNullableEqualNullable(left, right, val3, leftIsNotNull, val4, val5) : ExpandNegatedNullableEqualNullable(left, right, val3, leftIsNotNull, val4, val5);
				}
				if (leftNullable && !rightNullable)
				{
					return (flag == flag2) ? ExpandNullableEqualNonNullable(left, right, leftIsNotNull) : ExpandNegatedNullableEqualNonNullable(left, right, leftIsNotNull);
				}
				if (rightNullable && !leftNullable)
				{
					return (flag == flag2) ? ExpandNullableEqualNonNullable(left, right, val5) : ExpandNegatedNullableEqualNonNullable(left, right, val5);
				}
			}
			if (sqlBinaryExpression.OperatorType == ExpressionType.NotEqual)
			{
				if (leftNullable && rightNullable)
				{
					return (flag == flag2) ? ExpandNullableNotEqualNullable(left, right, val3, leftIsNotNull, val4, val5) : ExpandNegatedNullableNotEqualNullable(left, right, val3, leftIsNotNull, val4, val5);
				}
				if (leftNullable && !rightNullable)
				{
					return (flag == flag2) ? ExpandNullableNotEqualNonNullable(left, right, val3) : ExpandNegatedNullableNotEqualNonNullable(left, right, val3);
				}
				if (rightNullable && !leftNullable)
				{
					return (flag == flag2) ? ExpandNullableNotEqualNonNullable(left, right, val4) : ExpandNegatedNullableNotEqualNonNullable(left, right, val4);
				}
			}
			return (SqlExpression)(object)sqlBinaryExpression.Update(left, right);
		}

		private SqlExpression SimplifyLogicalSqlBinaryExpression(SqlBinaryExpression sqlBinaryExpression)
		{
			SqlExpression left = sqlBinaryExpression.Left;
			SqlUnaryExpression val = (SqlUnaryExpression)(object)((left is SqlUnaryExpression) ? left : null);
			SqlUnaryExpression val2 = default(SqlUnaryExpression);
			int num;
			if (val != null)
			{
				SqlExpression right = sqlBinaryExpression.Right;
				val2 = (SqlUnaryExpression)(object)((right is SqlUnaryExpression) ? right : null);
				if (val2 != null && (val.OperatorType == ExpressionType.Equal || val.OperatorType == ExpressionType.NotEqual) && (val2.OperatorType == ExpressionType.Equal || val2.OperatorType == ExpressionType.NotEqual))
				{
					num = (((object)val.Operand).Equals((object?)val2.Operand) ? 1 : 0);
					goto IL_005b;
				}
			}
			num = 0;
			goto IL_005b;
			IL_005b:
			if (num != 0)
			{
				return (SqlExpression)((val.OperatorType == val2.OperatorType) ? ((object)val) : ((object)_sqlExpressionFactory.Constant((object)(sqlBinaryExpression.OperatorType == ExpressionType.OrElse), ((SqlExpression)sqlBinaryExpression).TypeMapping)));
			}
			SqlExpression left2 = sqlBinaryExpression.Left;
			SqlConstantExpression val3 = (SqlConstantExpression)(object)((left2 is SqlConstantExpression) ? left2 : null);
			bool flag = default(bool);
			int num2;
			if (val3 != null)
			{
				object value = val3.Value;
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
				return (SqlExpression)((sqlBinaryExpression.OperatorType != ExpressionType.AndAlso) ? (flag ? ((object)val3) : ((object)sqlBinaryExpression.Right)) : (flag ? ((object)sqlBinaryExpression.Right) : ((object)val3)));
			}
			SqlExpression right2 = sqlBinaryExpression.Right;
			SqlConstantExpression val4 = (SqlConstantExpression)(object)((right2 is SqlConstantExpression) ? right2 : null);
			bool flag2 = default(bool);
			int num3;
			if (val4 != null)
			{
				object value = val4.Value;
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
				return (SqlExpression)((sqlBinaryExpression.OperatorType != ExpressionType.AndAlso) ? (flag2 ? ((object)val4) : ((object)sqlBinaryExpression.Left)) : (flag2 ? ((object)sqlBinaryExpression.Left) : ((object)val4)));
			}
			return (SqlExpression)(object)sqlBinaryExpression;
		}

		private SqlExpression OptimizeNonNullableNotExpression(SqlUnaryExpression sqlUnaryExpression)
		{
			if (sqlUnaryExpression.OperatorType != ExpressionType.Not)
			{
				return (SqlExpression)(object)sqlUnaryExpression;
			}
			SqlExpression operand = sqlUnaryExpression.Operand;
			SqlExpression val = operand;
			SqlConstantExpression val2 = (SqlConstantExpression)(object)((val is SqlConstantExpression) ? val : null);
			if (val2 == null)
			{
				InExpression val3 = (InExpression)(object)((val is InExpression) ? val : null);
				if (val3 != null)
				{
					return (SqlExpression)(object)val3.Negate();
				}
				SqlUnaryExpression val4 = (SqlUnaryExpression)(object)((val is SqlUnaryExpression) ? val : null);
				if (val4 == null)
				{
					SqlBinaryExpression val5 = (SqlBinaryExpression)(object)((val is SqlBinaryExpression) ? val : null);
					if (val5 != null)
					{
						if (val5.OperatorType == ExpressionType.AndAlso || val5.OperatorType == ExpressionType.OrElse)
						{
							SqlExpression val6 = OptimizeNonNullableNotExpression(_sqlExpressionFactory.Not(val5.Left));
							SqlExpression val7 = OptimizeNonNullableNotExpression(_sqlExpressionFactory.Not(val5.Right));
							return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.MakeBinary((val5.OperatorType == ExpressionType.AndAlso) ? ExpressionType.OrElse : ExpressionType.AndAlso, val6, val7, ((SqlExpression)val5).TypeMapping));
						}
						if (val5.OperatorType == ExpressionType.Equal)
						{
							SqlExpression left = val5.Left;
							SqlConstantExpression val8 = (SqlConstantExpression)(object)((left is SqlConstantExpression) ? left : null);
							if (val8 != null && ((Expression)(object)val8).Type == typeof(bool))
							{
								return (SqlExpression)(object)_sqlExpressionFactory.MakeBinary(ExpressionType.Equal, (SqlExpression)(object)_sqlExpressionFactory.Constant((object)(!(bool)val8.Value), ((SqlExpression)val8).TypeMapping), val5.Right, ((SqlExpression)val5).TypeMapping);
							}
							SqlExpression right = val5.Right;
							SqlConstantExpression val9 = (SqlConstantExpression)(object)((right is SqlConstantExpression) ? right : null);
							if (val9 != null && ((Expression)(object)val9).Type == typeof(bool))
							{
								return (SqlExpression)(object)_sqlExpressionFactory.MakeBinary(ExpressionType.Equal, val5.Left, (SqlExpression)(object)_sqlExpressionFactory.Constant((object)(!(bool)val9.Value), ((SqlExpression)val9).TypeMapping), ((SqlExpression)val5).TypeMapping);
							}
						}
						if (TryNegate(val5.OperatorType, out var result2))
						{
							return (SqlExpression)(object)_sqlExpressionFactory.MakeBinary(result2, val5.Left, val5.Right, ((SqlExpression)val5).TypeMapping);
						}
					}
				}
				else
				{
					switch (val4.OperatorType)
					{
					case ExpressionType.Not:
						return val4.Operand;
					case ExpressionType.Equal:
						return (SqlExpression)(object)_sqlExpressionFactory.IsNotNull(val4.Operand);
					case ExpressionType.NotEqual:
						return (SqlExpression)(object)_sqlExpressionFactory.IsNull(val4.Operand);
					}
				}
			}
			else
			{
				object value = val2.Value;
				if (value is bool)
				{
					bool flag = (bool)value;
					return (SqlExpression)(object)_sqlExpressionFactory.Constant((object)(!flag), ((SqlExpression)sqlUnaryExpression).TypeMapping);
				}
			}
			return (SqlExpression)(object)sqlUnaryExpression;
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
			SqlUnaryExpression sqlUnaryExpression2 = sqlUnaryExpression;
			if (!operandNullable)
			{
				return (SqlExpression)(object)_sqlExpressionFactory.Equal((SqlExpression)(object)_sqlExpressionFactory.Constant((object)true, (RelationalTypeMapping)null), (SqlExpression)(object)_sqlExpressionFactory.Constant((object)(sqlUnaryExpression2.OperatorType == ExpressionType.NotEqual), ((SqlExpression)sqlUnaryExpression2).TypeMapping));
			}
			SqlExpression operand = sqlUnaryExpression2.Operand;
			SqlExpression val = operand;
			SqlConstantExpression val2 = (SqlConstantExpression)(object)((val is SqlConstantExpression) ? val : null);
			if (val2 == null)
			{
				SqlParameterExpression val3 = (SqlParameterExpression)(object)((val is SqlParameterExpression) ? val : null);
				if (val3 == null)
				{
					ColumnExpression val4 = (ColumnExpression)(object)((val is ColumnExpression) ? val : null);
					if (val4 == null)
					{
						SqlUnaryExpression val5 = (SqlUnaryExpression)(object)((val is SqlUnaryExpression) ? val : null);
						if (val5 == null)
						{
							SqlBinaryExpression val6 = (SqlBinaryExpression)(object)((val is SqlBinaryExpression) ? val : null);
							if (val6 == null)
							{
								SqlFunctionExpression val7 = (SqlFunctionExpression)(object)((val is SqlFunctionExpression) ? val : null);
								if (val7 != null)
								{
									if (val7.IsBuiltIn && string.Equals("COALESCE", val7.Name, StringComparison.OrdinalIgnoreCase))
									{
										SqlExpression val8 = ProcessNullNotNull(_sqlExpressionFactory.MakeUnary(sqlUnaryExpression2.OperatorType, val7.Arguments[0], typeof(bool), ((SqlExpression)sqlUnaryExpression2).TypeMapping), operandNullable);
										SqlExpression val9 = ProcessNullNotNull(_sqlExpressionFactory.MakeUnary(sqlUnaryExpression2.OperatorType, val7.Arguments[1], typeof(bool), ((SqlExpression)sqlUnaryExpression2).TypeMapping), operandNullable);
										return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.MakeBinary((sqlUnaryExpression2.OperatorType == ExpressionType.Equal) ? ExpressionType.AndAlso : ExpressionType.OrElse, val8, val9, ((SqlExpression)sqlUnaryExpression2).TypeMapping));
									}
									if (!val7.IsNullable)
									{
										return (SqlExpression)(object)_sqlExpressionFactory.Equal((SqlExpression)(object)_sqlExpressionFactory.Constant((object)true, (RelationalTypeMapping)null), (SqlExpression)(object)_sqlExpressionFactory.Constant((object)(sqlUnaryExpression2.OperatorType == ExpressionType.NotEqual), ((SqlExpression)sqlUnaryExpression2).TypeMapping));
									}
									List<SqlExpression> list = new List<SqlExpression>();
									if (val7.Instance != null && val7.InstancePropagatesNullability == true)
									{
										list.Add(val7.Instance);
									}
									if (!val7.IsNiladic)
									{
										for (int i = 0; i < val7.Arguments.Count; i++)
										{
											if (val7.ArgumentsPropagateNullability[i])
											{
												list.Add(val7.Arguments[i]);
											}
										}
									}
									if (list.Count > 0)
									{
										return list.Select((SqlExpression e) => ProcessNullNotNull(_sqlExpressionFactory.MakeUnary(sqlUnaryExpression2.OperatorType, e, ((Expression)(object)sqlUnaryExpression2).Type, ((SqlExpression)sqlUnaryExpression2).TypeMapping), operandNullable)).Aggregate((SqlExpression r, SqlExpression e) => SimplifyLogicalSqlBinaryExpression((sqlUnaryExpression2.OperatorType == ExpressionType.Equal) ? _sqlExpressionFactory.OrElse(r, e) : _sqlExpressionFactory.AndAlso(r, e)));
									}
								}
							}
							else if (val6.OperatorType != ExpressionType.AndAlso && val6.OperatorType != ExpressionType.OrElse)
							{
								SqlExpression val10 = ProcessNullNotNull(_sqlExpressionFactory.MakeUnary(sqlUnaryExpression2.OperatorType, val6.Left, typeof(bool), ((SqlExpression)sqlUnaryExpression2).TypeMapping), operandNullable);
								SqlExpression val11 = ProcessNullNotNull(_sqlExpressionFactory.MakeUnary(sqlUnaryExpression2.OperatorType, val6.Right, typeof(bool), ((SqlExpression)sqlUnaryExpression2).TypeMapping), operandNullable);
								return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.MakeBinary((sqlUnaryExpression2.OperatorType == ExpressionType.Equal) ? ExpressionType.OrElse : ExpressionType.AndAlso, val10, val11, ((SqlExpression)sqlUnaryExpression2).TypeMapping));
							}
						}
						else
						{
							switch (val5.OperatorType)
							{
							case ExpressionType.Convert:
							case ExpressionType.Negate:
							case ExpressionType.Not:
								return ProcessNullNotNull(sqlUnaryExpression2.Update(val5.Operand), operandNullable);
							case ExpressionType.Equal:
							case ExpressionType.NotEqual:
								return (SqlExpression)(object)_sqlExpressionFactory.Equal((SqlExpression)(object)_sqlExpressionFactory.Constant((object)true, (RelationalTypeMapping)null), (SqlExpression)(object)_sqlExpressionFactory.Constant((object)(val5.OperatorType == ExpressionType.NotEqual), ((SqlExpression)val5).TypeMapping));
							}
						}
					}
					else if (!val4.IsNullable || _nonNullableColumns.Contains(val4))
					{
						return (SqlExpression)(object)_sqlExpressionFactory.Equal((SqlExpression)(object)_sqlExpressionFactory.Constant((object)true, (RelationalTypeMapping)null), (SqlExpression)(object)_sqlExpressionFactory.Constant((object)(sqlUnaryExpression2.OperatorType == ExpressionType.NotEqual), ((SqlExpression)sqlUnaryExpression2).TypeMapping));
					}
					return (SqlExpression)(object)sqlUnaryExpression2;
				}
				return (SqlExpression)(object)_sqlExpressionFactory.Equal((SqlExpression)(object)_sqlExpressionFactory.Constant((object)true, (RelationalTypeMapping)null), (SqlExpression)(object)_sqlExpressionFactory.Constant((object)((ParameterValues[val3.Name] == null) ^ (sqlUnaryExpression2.OperatorType == ExpressionType.NotEqual)), ((SqlExpression)sqlUnaryExpression2).TypeMapping));
			}
			return (SqlExpression)(object)_sqlExpressionFactory.Equal((SqlExpression)(object)_sqlExpressionFactory.Constant((object)true, (RelationalTypeMapping)null), (SqlExpression)(object)_sqlExpressionFactory.Constant((object)((val2.Value == null) ^ (sqlUnaryExpression2.OperatorType == ExpressionType.NotEqual)), ((SqlExpression)sqlUnaryExpression2).TypeMapping));
		}

		private static bool IsLogicalNot(SqlUnaryExpression? sqlUnaryExpression)
		{
			return sqlUnaryExpression != null && sqlUnaryExpression!.OperatorType == ExpressionType.Not && ((Expression)(object)sqlUnaryExpression!).Type == typeof(bool);
		}

		private SqlExpression ExpandNullableEqualNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull, SqlExpression leftIsNotNull, SqlExpression rightIsNull, SqlExpression rightIsNotNull)
		{
			return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse(SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso((SqlExpression)(object)_sqlExpressionFactory.Equal(left, right), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(leftIsNotNull, rightIsNotNull)))), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(leftIsNull, rightIsNull))));
		}

		private SqlExpression ExpandNegatedNullableEqualNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull, SqlExpression leftIsNotNull, SqlExpression rightIsNull, SqlExpression rightIsNotNull)
		{
			return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse(SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso((SqlExpression)(object)_sqlExpressionFactory.NotEqual(left, right), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(leftIsNotNull, rightIsNotNull)))), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(leftIsNull, rightIsNull))));
		}

		private SqlExpression ExpandNullableEqualNonNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNotNull)
		{
			return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso((SqlExpression)(object)_sqlExpressionFactory.Equal(left, right), leftIsNotNull));
		}

		private SqlExpression ExpandNegatedNullableEqualNonNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNotNull)
		{
			return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso((SqlExpression)(object)_sqlExpressionFactory.NotEqual(left, right), leftIsNotNull));
		}

		private SqlExpression ExpandNullableNotEqualNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull, SqlExpression leftIsNotNull, SqlExpression rightIsNull, SqlExpression rightIsNotNull)
		{
			return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse((SqlExpression)(object)_sqlExpressionFactory.NotEqual(left, right), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse(leftIsNull, rightIsNull)))), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse(leftIsNotNull, rightIsNotNull))));
		}

		private SqlExpression ExpandNegatedNullableNotEqualNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull, SqlExpression leftIsNotNull, SqlExpression rightIsNull, SqlExpression rightIsNotNull)
		{
			return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.AndAlso(SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse((SqlExpression)(object)_sqlExpressionFactory.Equal(left, right), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse(leftIsNull, rightIsNull)))), SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse(leftIsNotNull, rightIsNotNull))));
		}

		private SqlExpression ExpandNullableNotEqualNonNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull)
		{
			return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse((SqlExpression)(object)_sqlExpressionFactory.NotEqual(left, right), leftIsNull));
		}

		private SqlExpression ExpandNegatedNullableNotEqualNonNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull)
		{
			return SimplifyLogicalSqlBinaryExpression(_sqlExpressionFactory.OrElse((SqlExpression)(object)_sqlExpressionFactory.Equal(left, right), leftIsNull));
		}
	}
}
