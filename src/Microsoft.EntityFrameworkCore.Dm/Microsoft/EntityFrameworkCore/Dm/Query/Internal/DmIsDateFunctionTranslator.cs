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
	public class DmIsDateFunctionTranslator : IMethodCallTranslator
	{
		private readonly ISqlExpressionFactory _sqlExpressionFactory;

		private static readonly MethodInfo _methodInfo = typeof(DmDbFunctionsExtensions).GetRuntimeMethod("IsDate", new Type[2]
		{
			typeof(DbFunctions),
			typeof(string)
		});

		public DmIsDateFunctionTranslator(ISqlExpressionFactory sqlExpressionFactory)
		{
			_sqlExpressionFactory = sqlExpressionFactory;
		}

		public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
		{
			return (SqlExpression)(object)(_methodInfo.Equals(method) ? _sqlExpressionFactory.Convert((SqlExpression)(object)_sqlExpressionFactory.Function("ISDATE", (IEnumerable<SqlExpression>)(object)new SqlExpression[1] { arguments[1] }, true, (IEnumerable<bool>)new bool[1] { true }, _methodInfo.ReturnType, (RelationalTypeMapping)null), _methodInfo.ReturnType, (RelationalTypeMapping)null) : null);
		}
	}
}
