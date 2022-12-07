using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Extensions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmDateDiffFunctionsTranslator : IMethodCallTranslator
	{
		private readonly Dictionary<MethodInfo, string> _methodInfoDateDiffMapping = new Dictionary<MethodInfo, string>
		{
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffYear", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime),
					typeof(DateTime)
				}),
				"YEAR"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffYear", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime?),
					typeof(DateTime?)
				}),
				"YEAR"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffYear", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset),
					typeof(DateTimeOffset)
				}),
				"YEAR"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffYear", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset?),
					typeof(DateTimeOffset?)
				}),
				"YEAR"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMonth", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime),
					typeof(DateTime)
				}),
				"MONTH"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMonth", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime?),
					typeof(DateTime?)
				}),
				"MONTH"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMonth", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset),
					typeof(DateTimeOffset)
				}),
				"MONTH"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMonth", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset?),
					typeof(DateTimeOffset?)
				}),
				"MONTH"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffDay", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime),
					typeof(DateTime)
				}),
				"DAY"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffDay", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime?),
					typeof(DateTime?)
				}),
				"DAY"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffDay", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset),
					typeof(DateTimeOffset)
				}),
				"DAY"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffDay", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset?),
					typeof(DateTimeOffset?)
				}),
				"DAY"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime),
					typeof(DateTime)
				}),
				"HOUR"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime?),
					typeof(DateTime?)
				}),
				"HOUR"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset),
					typeof(DateTimeOffset)
				}),
				"HOUR"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset?),
					typeof(DateTimeOffset?)
				}),
				"HOUR"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
				{
					typeof(DbFunctions),
					typeof(TimeSpan),
					typeof(TimeSpan)
				}),
				"HOUR"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
				{
					typeof(DbFunctions),
					typeof(TimeSpan?),
					typeof(TimeSpan?)
				}),
				"HOUR"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime),
					typeof(DateTime)
				}),
				"MINUTE"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime?),
					typeof(DateTime?)
				}),
				"MINUTE"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset),
					typeof(DateTimeOffset)
				}),
				"MINUTE"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset?),
					typeof(DateTimeOffset?)
				}),
				"MINUTE"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
				{
					typeof(DbFunctions),
					typeof(TimeSpan),
					typeof(TimeSpan)
				}),
				"MINUTE"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
				{
					typeof(DbFunctions),
					typeof(TimeSpan?),
					typeof(TimeSpan?)
				}),
				"MINUTE"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime),
					typeof(DateTime)
				}),
				"SECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime?),
					typeof(DateTime?)
				}),
				"SECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset),
					typeof(DateTimeOffset)
				}),
				"SECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset?),
					typeof(DateTimeOffset?)
				}),
				"SECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(TimeSpan),
					typeof(TimeSpan)
				}),
				"SECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(TimeSpan?),
					typeof(TimeSpan?)
				}),
				"SECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime),
					typeof(DateTime)
				}),
				"MILLISECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime?),
					typeof(DateTime?)
				}),
				"MILLISECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset),
					typeof(DateTimeOffset)
				}),
				"MILLISECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset?),
					typeof(DateTimeOffset?)
				}),
				"MILLISECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(TimeSpan),
					typeof(TimeSpan)
				}),
				"MILLISECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(TimeSpan?),
					typeof(TimeSpan?)
				}),
				"MILLISECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime),
					typeof(DateTime)
				}),
				"MICROSECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime?),
					typeof(DateTime?)
				}),
				"MICROSECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset),
					typeof(DateTimeOffset)
				}),
				"MICROSECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset?),
					typeof(DateTimeOffset?)
				}),
				"MICROSECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(TimeSpan),
					typeof(TimeSpan)
				}),
				"MICROSECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(TimeSpan?),
					typeof(TimeSpan?)
				}),
				"MICROSECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime),
					typeof(DateTime)
				}),
				"NANOSECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTime?),
					typeof(DateTime?)
				}),
				"NANOSECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset),
					typeof(DateTimeOffset)
				}),
				"NANOSECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(DateTimeOffset?),
					typeof(DateTimeOffset?)
				}),
				"NANOSECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(TimeSpan),
					typeof(TimeSpan)
				}),
				"NANOSECOND"
			},
			{
				typeof(DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
				{
					typeof(DbFunctions),
					typeof(TimeSpan?),
					typeof(TimeSpan?)
				}),
				"NANOSECOND"
			}
		};

		private readonly ISqlExpressionFactory _sqlExpressionFactory;

		public DmDateDiffFunctionsTranslator(ISqlExpressionFactory sqlExpressionFactory)
		{
			_sqlExpressionFactory = sqlExpressionFactory;
		}

		public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
		{
			if (_methodInfoDateDiffMapping.TryGetValue(method, out var value))
			{
				SqlExpression val = arguments[1];
				SqlExpression val2 = arguments[2];
				RelationalTypeMapping val3 = ExpressionExtensions.InferTypeMapping((SqlExpression[])(object)new SqlExpression[2] { val, val2 });
				val = _sqlExpressionFactory.ApplyTypeMapping(val, val3);
				val2 = _sqlExpressionFactory.ApplyTypeMapping(val2, val3);
				return (SqlExpression)(object)_sqlExpressionFactory.Function("DATEDIFF", (IEnumerable<SqlExpression>)(object)new SqlExpression[3]
				{
					(SqlExpression)_sqlExpressionFactory.Fragment(value),
					val,
					val2
				}, true, (IEnumerable<bool>)new bool[3] { false, true, true }, typeof(int), (RelationalTypeMapping)null);
			}
			return null;
		}
	}
}
