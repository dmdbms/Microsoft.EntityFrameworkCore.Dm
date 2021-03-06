using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmDateTimeMethodTranslator : IMethodCallTranslator
	{
		private readonly Dictionary<MethodInfo, string> _methodInfoDatePartMapping = new Dictionary<MethodInfo, string>
		{
			{
				typeof(DateTime).GetRuntimeMethod("AddYears", new Type[1] { typeof(int) }),
				"year"
			},
			{
				typeof(DateTime).GetRuntimeMethod("AddMonths", new Type[1] { typeof(int) }),
				"month"
			},
			{
				typeof(DateTime).GetRuntimeMethod("AddDays", new Type[1] { typeof(double) }),
				"day"
			},
			{
				typeof(DateTime).GetRuntimeMethod("AddHours", new Type[1] { typeof(double) }),
				"hour"
			},
			{
				typeof(DateTime).GetRuntimeMethod("AddMinutes", new Type[1] { typeof(double) }),
				"minute"
			},
			{
				typeof(DateTime).GetRuntimeMethod("AddSeconds", new Type[1] { typeof(double) }),
				"second"
			},
			{
				typeof(DateTime).GetRuntimeMethod("AddMilliseconds", new Type[1] { typeof(double) }),
				"millisecond"
			},
			{
				typeof(DateTimeOffset).GetRuntimeMethod("AddYears", new Type[1] { typeof(int) }),
				"year"
			},
			{
				typeof(DateTimeOffset).GetRuntimeMethod("AddMonths", new Type[1] { typeof(int) }),
				"month"
			},
			{
				typeof(DateTimeOffset).GetRuntimeMethod("AddDays", new Type[1] { typeof(double) }),
				"day"
			},
			{
				typeof(DateTimeOffset).GetRuntimeMethod("AddHours", new Type[1] { typeof(double) }),
				"hour"
			},
			{
				typeof(DateTimeOffset).GetRuntimeMethod("AddMinutes", new Type[1] { typeof(double) }),
				"minute"
			},
			{
				typeof(DateTimeOffset).GetRuntimeMethod("AddSeconds", new Type[1] { typeof(double) }),
				"second"
			},
			{
				typeof(DateTimeOffset).GetRuntimeMethod("AddMilliseconds", new Type[1] { typeof(double) }),
				"millisecond"
			}
		};

		private readonly ISqlExpressionFactory _sqlExpressionFactory;

		public DmDateTimeMethodTranslator(ISqlExpressionFactory sqlExpressionFactory)
		{
			_sqlExpressionFactory = sqlExpressionFactory;
		}

		public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
		{
			object result;
			if (_methodInfoDatePartMapping.TryGetValue(method, out var value))
			{
				if (!value.Equals("year") && !value.Equals("month"))
				{
					SqlConstantExpression sqlConstantExpression = arguments[0] as SqlConstantExpression;
					if (sqlConstantExpression != null && ((double)sqlConstantExpression.Value >= 2147483647.0 || (double)sqlConstantExpression.Value <= -2147483648.0))
					{
						result = null;
						goto IL_00d4;
					}
				}
				result = _sqlExpressionFactory.Function("DATEADD", new SqlExpression[3]
				{
					_sqlExpressionFactory.Fragment(value),
					_sqlExpressionFactory.Convert(arguments[0], typeof(int)),
					instance
				}, nullable: true, new bool[3] { false, true, true }, instance.Type, instance.TypeMapping);
				goto IL_00d4;
			}
			return null;
			IL_00d4:
			return (SqlExpression)result;
		}
	}
}
