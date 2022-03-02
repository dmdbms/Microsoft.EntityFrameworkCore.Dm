using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmMathTranslator : IMethodCallTranslator
	{
		private static readonly Dictionary<MethodInfo, string> _supportedMethodTranslations = new Dictionary<MethodInfo, string>
		{
			{
				typeof(Math).GetRuntimeMethod("Abs", new Type[1] { typeof(decimal) }),
				"ABS"
			},
			{
				typeof(Math).GetRuntimeMethod("Abs", new Type[1] { typeof(double) }),
				"ABS"
			},
			{
				typeof(Math).GetRuntimeMethod("Abs", new Type[1] { typeof(float) }),
				"ABS"
			},
			{
				typeof(Math).GetRuntimeMethod("Abs", new Type[1] { typeof(int) }),
				"ABS"
			},
			{
				typeof(Math).GetRuntimeMethod("Abs", new Type[1] { typeof(long) }),
				"ABS"
			},
			{
				typeof(Math).GetRuntimeMethod("Abs", new Type[1] { typeof(sbyte) }),
				"ABS"
			},
			{
				typeof(Math).GetRuntimeMethod("Abs", new Type[1] { typeof(short) }),
				"ABS"
			},
			{
				typeof(Math).GetRuntimeMethod("Ceiling", new Type[1] { typeof(decimal) }),
				"CEILING"
			},
			{
				typeof(Math).GetRuntimeMethod("Ceiling", new Type[1] { typeof(double) }),
				"CEILING"
			},
			{
				typeof(Math).GetRuntimeMethod("Floor", new Type[1] { typeof(decimal) }),
				"FLOOR"
			},
			{
				typeof(Math).GetRuntimeMethod("Floor", new Type[1] { typeof(double) }),
				"FLOOR"
			},
			{
				typeof(Math).GetRuntimeMethod("Pow", new Type[2]
				{
					typeof(double),
					typeof(double)
				}),
				"POWER"
			},
			{
				typeof(Math).GetRuntimeMethod("Exp", new Type[1] { typeof(double) }),
				"EXP"
			},
			{
				typeof(Math).GetRuntimeMethod("Log10", new Type[1] { typeof(double) }),
				"LOG10"
			},
			{
				typeof(Math).GetRuntimeMethod("Log", new Type[1] { typeof(double) }),
				"LOG"
			},
			{
				typeof(Math).GetRuntimeMethod("Log", new Type[2]
				{
					typeof(double),
					typeof(double)
				}),
				"LOG"
			},
			{
				typeof(Math).GetRuntimeMethod("Sqrt", new Type[1] { typeof(double) }),
				"SQRT"
			},
			{
				typeof(Math).GetRuntimeMethod("Acos", new Type[1] { typeof(double) }),
				"ACOS"
			},
			{
				typeof(Math).GetRuntimeMethod("Asin", new Type[1] { typeof(double) }),
				"ASIN"
			},
			{
				typeof(Math).GetRuntimeMethod("Atan", new Type[1] { typeof(double) }),
				"ATAN"
			},
			{
				typeof(Math).GetRuntimeMethod("Atan2", new Type[2]
				{
					typeof(double),
					typeof(double)
				}),
				"ATN2"
			},
			{
				typeof(Math).GetRuntimeMethod("Cos", new Type[1] { typeof(double) }),
				"COS"
			},
			{
				typeof(Math).GetRuntimeMethod("Sin", new Type[1] { typeof(double) }),
				"SIN"
			},
			{
				typeof(Math).GetRuntimeMethod("Tan", new Type[1] { typeof(double) }),
				"TAN"
			},
			{
				typeof(Math).GetRuntimeMethod("Sign", new Type[1] { typeof(decimal) }),
				"SIGN"
			},
			{
				typeof(Math).GetRuntimeMethod("Sign", new Type[1] { typeof(double) }),
				"SIGN"
			},
			{
				typeof(Math).GetRuntimeMethod("Sign", new Type[1] { typeof(float) }),
				"SIGN"
			},
			{
				typeof(Math).GetRuntimeMethod("Sign", new Type[1] { typeof(int) }),
				"SIGN"
			},
			{
				typeof(Math).GetRuntimeMethod("Sign", new Type[1] { typeof(long) }),
				"SIGN"
			},
			{
				typeof(Math).GetRuntimeMethod("Sign", new Type[1] { typeof(sbyte) }),
				"SIGN"
			},
			{
				typeof(Math).GetRuntimeMethod("Sign", new Type[1] { typeof(short) }),
				"SIGN"
			}
		};

		private static readonly IEnumerable<MethodInfo> _truncateMethodInfos = new MethodInfo[2]
		{
			typeof(Math).GetRuntimeMethod("Truncate", new Type[1] { typeof(decimal) }),
			typeof(Math).GetRuntimeMethod("Truncate", new Type[1] { typeof(double) })
		};

		private static readonly IEnumerable<MethodInfo> _roundMethodInfos = new MethodInfo[4]
		{
			typeof(Math).GetRuntimeMethod("Round", new Type[1] { typeof(decimal) }),
			typeof(Math).GetRuntimeMethod("Round", new Type[1] { typeof(double) }),
			typeof(Math).GetRuntimeMethod("Round", new Type[2]
			{
				typeof(decimal),
				typeof(int)
			}),
			typeof(Math).GetRuntimeMethod("Round", new Type[2]
			{
				typeof(double),
				typeof(int)
			})
		};

		private readonly ISqlExpressionFactory _sqlExpressionFactory;

		public DmMathTranslator(ISqlExpressionFactory sqlExpressionFactory)
		{
			_sqlExpressionFactory = sqlExpressionFactory;
		}

		public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
		{
			if (_supportedMethodTranslations.TryGetValue(method, out var value))
			{
				RelationalTypeMapping relationalTypeMapping = ((arguments.Count == 1) ? ExpressionExtensions.InferTypeMapping(arguments[0]) : ExpressionExtensions.InferTypeMapping(arguments[0], arguments[1]));
				SqlExpression[] array = new SqlExpression[arguments.Count];
				array[0] = _sqlExpressionFactory.ApplyTypeMapping(arguments[0], relationalTypeMapping);
				if (arguments.Count == 2)
				{
					array[1] = _sqlExpressionFactory.ApplyTypeMapping(arguments[1], relationalTypeMapping);
				}
				return _sqlExpressionFactory.Function(value, array, nullable: true, array.Select((SqlExpression a) => true).ToArray(), method.ReturnType, (value == "SIGN") ? null : relationalTypeMapping);
			}
			if (_truncateMethodInfos.Contains(method))
			{
				SqlExpression sqlExpression = arguments[0];
				return _sqlExpressionFactory.Function("TRUNCATE", new SqlExpression[3]
				{
					sqlExpression,
					_sqlExpressionFactory.Constant(0),
					_sqlExpressionFactory.Constant(1)
				}, nullable: true, new bool[3] { true, false, false }, method.ReturnType, sqlExpression.TypeMapping);
			}
			if (_roundMethodInfos.Contains(method))
			{
				SqlExpression sqlExpression2 = arguments[0];
				SqlExpression sqlExpression3 = ((arguments.Count == 2) ? arguments[1] : _sqlExpressionFactory.Constant(0));
				return _sqlExpressionFactory.Function("ROUND", new SqlExpression[2] { sqlExpression2, sqlExpression3 }, nullable: true, new bool[2] { true, true }, method.ReturnType, sqlExpression2.TypeMapping);
			}
			return null;
		}
	}
}
