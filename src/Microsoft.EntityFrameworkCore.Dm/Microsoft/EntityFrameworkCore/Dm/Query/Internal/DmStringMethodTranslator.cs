using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmStringMethodTranslator : IMethodCallTranslator
	{
		private static readonly MethodInfo _indexOfMethodInfo = typeof(string).GetRuntimeMethod("IndexOf", new Type[1] { typeof(string) });

		private static readonly MethodInfo _replaceMethodInfo = typeof(string).GetRuntimeMethod("Replace", new Type[2]
		{
			typeof(string),
			typeof(string)
		});

		private static readonly MethodInfo _toLowerMethodInfo = typeof(string).GetRuntimeMethod("ToLower", Array.Empty<Type>());

		private static readonly MethodInfo _toUpperMethodInfo = typeof(string).GetRuntimeMethod("ToUpper", Array.Empty<Type>());

		private static readonly MethodInfo _substringMethodInfo = typeof(string).GetRuntimeMethod("Substring", new Type[2]
		{
			typeof(int),
			typeof(int)
		});

		private static readonly MethodInfo _isNullOrWhiteSpaceMethodInfo = typeof(string).GetRuntimeMethod("IsNullOrWhiteSpace", new Type[1] { typeof(string) });

		private static readonly MethodInfo _trimStartMethodInfoWithoutArgs = typeof(string).GetRuntimeMethod("TrimStart", Array.Empty<Type>());

		private static readonly MethodInfo _trimEndMethodInfoWithoutArgs = typeof(string).GetRuntimeMethod("TrimEnd", Array.Empty<Type>());

		private static readonly MethodInfo _trimMethodInfoWithoutArgs = typeof(string).GetRuntimeMethod("Trim", Array.Empty<Type>());

		private static readonly MethodInfo _trimStartMethodInfoWithCharArrayArg = typeof(string).GetRuntimeMethod("TrimStart", new Type[1] { typeof(char[]) });

		private static readonly MethodInfo _trimEndMethodInfoWithCharArrayArg = typeof(string).GetRuntimeMethod("TrimEnd", new Type[1] { typeof(char[]) });

		private static readonly MethodInfo _trimMethodInfoWithCharArrayArg = typeof(string).GetRuntimeMethod("Trim", new Type[1] { typeof(char[]) });

		private static readonly MethodInfo _startsWithMethodInfo = typeof(string).GetRuntimeMethod("StartsWith", new Type[1] { typeof(string) });

		private static readonly MethodInfo _containsMethodInfo = typeof(string).GetRuntimeMethod("Contains", new Type[1] { typeof(string) });

		private static readonly MethodInfo _endsWithMethodInfo = typeof(string).GetRuntimeMethod("EndsWith", new Type[1] { typeof(string) });

		private readonly ISqlExpressionFactory _sqlExpressionFactory;

		private const char LikeEscapeChar = '\\';

		public DmStringMethodTranslator(ISqlExpressionFactory sqlExpressionFactory)
		{
			_sqlExpressionFactory = sqlExpressionFactory;
		}

		public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
		{
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Expected O, but got Unknown
			if (_indexOfMethodInfo.Equals(method))
			{
				SqlExpression val = arguments[0];
				RelationalTypeMapping val2 = ExpressionExtensions.InferTypeMapping((SqlExpression[])(object)new SqlExpression[2] { instance, val });
				val = _sqlExpressionFactory.ApplyTypeMapping(val, val2);
				SqlBinaryExpression val3 = _sqlExpressionFactory.Subtract((SqlExpression)(object)_sqlExpressionFactory.Function("POSITION", (IEnumerable<SqlExpression>)(object)new SqlExpression[2]
				{
					val,
					_sqlExpressionFactory.ApplyTypeMapping(instance, val2)
				}, true, (IEnumerable<bool>)new bool[2] { true, true }, method.ReturnType, (RelationalTypeMapping)null), (SqlExpression)(object)_sqlExpressionFactory.Constant((object)1, (RelationalTypeMapping)null), (RelationalTypeMapping)null);
				return (SqlExpression)(object)_sqlExpressionFactory.Case((IReadOnlyList<CaseWhenClause>)(object)new CaseWhenClause[1]
				{
					new CaseWhenClause((SqlExpression)(object)_sqlExpressionFactory.Equal(val, (SqlExpression)(object)_sqlExpressionFactory.Constant((object)string.Empty, val2)), (SqlExpression)(object)_sqlExpressionFactory.Constant((object)0, (RelationalTypeMapping)null))
				}, (SqlExpression)(object)val3);
			}
			if (_replaceMethodInfo.Equals(method))
			{
				SqlExpression val4 = arguments[0];
				SqlExpression val5 = arguments[1];
				RelationalTypeMapping val6 = ExpressionExtensions.InferTypeMapping((SqlExpression[])(object)new SqlExpression[3] { instance, val4, val5 });
				instance = _sqlExpressionFactory.ApplyTypeMapping(instance, val6);
				val4 = _sqlExpressionFactory.ApplyTypeMapping(val4, val6);
				val5 = _sqlExpressionFactory.ApplyTypeMapping(val5, val6);
				return (SqlExpression)(object)_sqlExpressionFactory.Function("REPLACE", (IEnumerable<SqlExpression>)(object)new SqlExpression[3] { instance, val4, val5 }, true, (IEnumerable<bool>)new bool[3] { true, true, true }, method.ReturnType, val6);
			}
			if (_toLowerMethodInfo.Equals(method) || _toUpperMethodInfo.Equals(method))
			{
				return (SqlExpression)(object)_sqlExpressionFactory.Function(_toLowerMethodInfo.Equals(method) ? "LOWER" : "UPPER", (IEnumerable<SqlExpression>)(object)new SqlExpression[1] { instance }, true, (IEnumerable<bool>)new bool[1] { true }, method.ReturnType, instance.TypeMapping);
			}
			if (_substringMethodInfo.Equals(method))
			{
				return (SqlExpression)(object)_sqlExpressionFactory.Function("SUBSTRING", (IEnumerable<SqlExpression>)(object)new SqlExpression[3]
				{
					instance,
					(SqlExpression)_sqlExpressionFactory.Add(arguments[0], (SqlExpression)(object)_sqlExpressionFactory.Constant((object)1, (RelationalTypeMapping)null), (RelationalTypeMapping)null),
					arguments[1]
				}, true, (IEnumerable<bool>)new bool[3] { true, true, true }, method.ReturnType, instance.TypeMapping);
			}
			if (_isNullOrWhiteSpaceMethodInfo.Equals(method))
			{
				SqlExpression val7 = arguments[0];
				return (SqlExpression)(object)_sqlExpressionFactory.OrElse((SqlExpression)(object)_sqlExpressionFactory.IsNull(val7), (SqlExpression)(object)_sqlExpressionFactory.Equal((SqlExpression)(object)_sqlExpressionFactory.Function("LTRIM", (IEnumerable<SqlExpression>)(object)new SqlFunctionExpression[1] { _sqlExpressionFactory.Function("RTRIM", (IEnumerable<SqlExpression>)(object)new SqlExpression[1] { val7 }, true, (IEnumerable<bool>)new bool[1] { true }, ((Expression)(object)val7).Type, val7.TypeMapping) }, true, (IEnumerable<bool>)new bool[1] { true }, ((Expression)(object)val7).Type, val7.TypeMapping), (SqlExpression)(object)_sqlExpressionFactory.Constant((object)string.Empty, val7.TypeMapping)));
			}
			int num;
			if (!(_trimStartMethodInfoWithoutArgs?.Equals(method) ?? false))
			{
				if (_trimStartMethodInfoWithCharArrayArg.Equals(method))
				{
					SqlExpression obj = arguments[0];
					SqlExpression obj2 = ((obj is SqlConstantExpression) ? obj : null);
					Array obj3 = ((obj2 != null) ? ((SqlConstantExpression)obj2).Value : null) as Array;
					num = ((obj3 != null && obj3.Length == 0) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
			}
			else
			{
				num = 1;
			}
			if (num != 0)
			{
				return (SqlExpression)(object)_sqlExpressionFactory.Function("LTRIM", (IEnumerable<SqlExpression>)(object)new SqlExpression[1] { instance }, true, (IEnumerable<bool>)new bool[1] { true }, ((Expression)(object)instance).Type, instance.TypeMapping);
			}
			int num2;
			if (!(_trimEndMethodInfoWithoutArgs?.Equals(method) ?? false))
			{
				if (_trimEndMethodInfoWithCharArrayArg.Equals(method))
				{
					SqlExpression obj4 = arguments[0];
					SqlExpression obj5 = ((obj4 is SqlConstantExpression) ? obj4 : null);
					Array obj6 = ((obj5 != null) ? ((SqlConstantExpression)obj5).Value : null) as Array;
					num2 = ((obj6 != null && obj6.Length == 0) ? 1 : 0);
				}
				else
				{
					num2 = 0;
				}
			}
			else
			{
				num2 = 1;
			}
			if (num2 != 0)
			{
				return (SqlExpression)(object)_sqlExpressionFactory.Function("RTRIM", (IEnumerable<SqlExpression>)(object)new SqlExpression[1] { instance }, true, (IEnumerable<bool>)new bool[1] { true }, ((Expression)(object)instance).Type, instance.TypeMapping);
			}
			int num3;
			if (!(_trimMethodInfoWithoutArgs?.Equals(method) ?? false))
			{
				if (_trimMethodInfoWithCharArrayArg.Equals(method))
				{
					SqlExpression obj7 = arguments[0];
					SqlExpression obj8 = ((obj7 is SqlConstantExpression) ? obj7 : null);
					Array obj9 = ((obj8 != null) ? ((SqlConstantExpression)obj8).Value : null) as Array;
					num3 = ((obj9 != null && obj9.Length == 0) ? 1 : 0);
				}
				else
				{
					num3 = 0;
				}
			}
			else
			{
				num3 = 1;
			}
			if (num3 != 0)
			{
				return (SqlExpression)(object)_sqlExpressionFactory.Function("LTRIM", (IEnumerable<SqlExpression>)(object)new SqlFunctionExpression[1] { _sqlExpressionFactory.Function("RTRIM", (IEnumerable<SqlExpression>)(object)new SqlExpression[1] { instance }, true, (IEnumerable<bool>)new bool[1] { true }, ((Expression)(object)instance).Type, instance.TypeMapping) }, true, (IEnumerable<bool>)new bool[1] { true }, ((Expression)(object)instance).Type, instance.TypeMapping);
			}
			if (_containsMethodInfo.Equals(method))
			{
				SqlExpression val8 = arguments[0];
				RelationalTypeMapping val9 = ExpressionExtensions.InferTypeMapping((SqlExpression[])(object)new SqlExpression[2] { instance, val8 });
				instance = _sqlExpressionFactory.ApplyTypeMapping(instance, val9);
				val8 = _sqlExpressionFactory.ApplyTypeMapping(val8, val9);
				SqlConstantExpression val10 = (SqlConstantExpression)(object)((val8 is SqlConstantExpression) ? val8 : null);
				if (val10 != null)
				{
					if ((string)val10.Value == string.Empty)
					{
						return (SqlExpression)(object)_sqlExpressionFactory.Constant((object)true, (RelationalTypeMapping)null);
					}
					return (SqlExpression)(object)_sqlExpressionFactory.GreaterThan((SqlExpression)(object)_sqlExpressionFactory.Function("POSITION", (IEnumerable<SqlExpression>)(object)new SqlExpression[2] { val8, instance }, true, (IEnumerable<bool>)new bool[2] { true, true }, typeof(int), (RelationalTypeMapping)null), (SqlExpression)(object)_sqlExpressionFactory.Constant((object)0, (RelationalTypeMapping)null));
				}
				return (SqlExpression)(object)_sqlExpressionFactory.OrElse((SqlExpression)(object)_sqlExpressionFactory.Equal(val8, (SqlExpression)(object)_sqlExpressionFactory.Constant((object)string.Empty, val9)), (SqlExpression)(object)_sqlExpressionFactory.GreaterThan((SqlExpression)(object)_sqlExpressionFactory.Function("POSITION", (IEnumerable<SqlExpression>)(object)new SqlExpression[2] { val8, instance }, true, (IEnumerable<bool>)new bool[2] { true, true }, typeof(int), (RelationalTypeMapping)null), (SqlExpression)(object)_sqlExpressionFactory.Constant((object)0, (RelationalTypeMapping)null)));
			}
			if (_startsWithMethodInfo.Equals(method))
			{
				return TranslateStartsEndsWith(instance, arguments[0], startsWith: true);
			}
			if (_endsWithMethodInfo.Equals(method))
			{
				return TranslateStartsEndsWith(instance, arguments[0], startsWith: false);
			}
			return null;
		}

		private SqlExpression TranslateStartsEndsWith(SqlExpression instance, SqlExpression pattern, bool startsWith)
		{
			RelationalTypeMapping val = ExpressionExtensions.InferTypeMapping((SqlExpression[])(object)new SqlExpression[2] { instance, pattern });
			instance = _sqlExpressionFactory.ApplyTypeMapping(instance, val);
			pattern = _sqlExpressionFactory.ApplyTypeMapping(pattern, val);
			SqlConstantExpression val2 = (SqlConstantExpression)(object)((pattern is SqlConstantExpression) ? pattern : null);
			if (val2 != null)
			{
				string text = val2.Value as string;
				if (text == null)
				{
					return (SqlExpression)(object)_sqlExpressionFactory.Like(instance, (SqlExpression)(object)_sqlExpressionFactory.Constant((object)null, val), (SqlExpression)null);
				}
				return (SqlExpression)(object)(text.Any((char c) => IsLikeWildChar(c)) ? _sqlExpressionFactory.Like(instance, (SqlExpression)(object)_sqlExpressionFactory.Constant((object)(startsWith ? (EscapeLikePattern(text) + "%") : ("%" + EscapeLikePattern(text))), (RelationalTypeMapping)null), (SqlExpression)(object)_sqlExpressionFactory.Constant((object)'\\'.ToString(), (RelationalTypeMapping)null)) : _sqlExpressionFactory.Like(instance, (SqlExpression)(object)_sqlExpressionFactory.Constant((object)(startsWith ? (text + "%") : ("%" + text)), (RelationalTypeMapping)null), (SqlExpression)null));
			}
			if (startsWith)
			{
				return (SqlExpression)(object)_sqlExpressionFactory.Equal((SqlExpression)(object)_sqlExpressionFactory.Function("LEFT", (IEnumerable<SqlExpression>)(object)new SqlExpression[2]
				{
					instance,
					(SqlExpression)_sqlExpressionFactory.Function("LEN", (IEnumerable<SqlExpression>)(object)new SqlExpression[1] { pattern }, true, (IEnumerable<bool>)new bool[1] { true }, typeof(int), (RelationalTypeMapping)null)
				}, true, (IEnumerable<bool>)new bool[2] { true, true }, typeof(string), val), pattern);
			}
			return (SqlExpression)(object)_sqlExpressionFactory.Equal((SqlExpression)(object)_sqlExpressionFactory.Function("RIGHT", (IEnumerable<SqlExpression>)(object)new SqlExpression[2]
			{
				instance,
				(SqlExpression)_sqlExpressionFactory.Function("LEN", (IEnumerable<SqlExpression>)(object)new SqlExpression[1] { pattern }, true, (IEnumerable<bool>)new bool[1] { true }, typeof(int), (RelationalTypeMapping)null)
			}, true, (IEnumerable<bool>)new bool[2] { true, true }, typeof(string), val), pattern);
		}

		private bool IsLikeWildChar(char c)
		{
			return c == '%' || c == '_' || c == '[';
		}

		private string EscapeLikePattern(string pattern)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in pattern)
			{
				if (IsLikeWildChar(c) || c == '\\')
				{
					stringBuilder.Append('\\');
				}
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}
	}
}
