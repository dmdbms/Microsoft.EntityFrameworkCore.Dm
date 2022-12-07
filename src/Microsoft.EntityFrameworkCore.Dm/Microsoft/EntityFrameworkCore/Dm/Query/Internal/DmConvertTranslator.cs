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
	public class DmConvertTranslator : IMethodCallTranslator
	{
		private static readonly Dictionary<string, string> _typeMapping = new Dictionary<string, string>
		{
			["ToByte"] = "tinyint",
			["ToDecimal"] = "decimal(18, 2)",
			["ToDouble"] = "float",
			["ToInt16"] = "smallint",
			["ToInt32"] = "int",
			["ToInt64"] = "bigint",
			["ToString"] = "varchar"
		};

		private static readonly List<Type> _supportedTypes = new List<Type>
		{
			typeof(bool),
			typeof(byte),
			typeof(decimal),
			typeof(double),
			typeof(float),
			typeof(int),
			typeof(long),
			typeof(short),
			typeof(string)
		};

		private static readonly IEnumerable<MethodInfo> _supportedMethods = _typeMapping.Keys.SelectMany((string t) => from m in typeof(Convert).GetTypeInfo().GetDeclaredMethods(t)
			where m.GetParameters().Length == 1 && _supportedTypes.Contains(m.GetParameters().First().ParameterType)
			select m);

		private readonly ISqlExpressionFactory _sqlExpressionFactory;

		public DmConvertTranslator(ISqlExpressionFactory sqlExpressionFactory)
		{
			_sqlExpressionFactory = sqlExpressionFactory;
		}

		public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
		{
			return (SqlExpression)(object)(_supportedMethods.Contains(method) ? _sqlExpressionFactory.Function("CONVERT", (IEnumerable<SqlExpression>)(object)new SqlExpression[2]
			{
				(SqlExpression)_sqlExpressionFactory.Fragment(_typeMapping[method.Name]),
				arguments[0]
			}, true, (IEnumerable<bool>)new bool[2] { false, true }, method.ReturnType, (RelationalTypeMapping)null) : null);
		}
	}
}
