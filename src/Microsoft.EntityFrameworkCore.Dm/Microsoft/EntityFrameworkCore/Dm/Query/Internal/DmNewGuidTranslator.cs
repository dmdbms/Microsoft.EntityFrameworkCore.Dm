using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmNewGuidTranslator : IMethodCallTranslator
	{
		private static readonly MethodInfo _methodInfo = typeof(Guid).GetRuntimeMethod("NewGuid", Array.Empty<Type>());

		private readonly ISqlExpressionFactory _sqlExpressionFactory;

		public DmNewGuidTranslator(ISqlExpressionFactory sqlExpressionFactory)
		{
			_sqlExpressionFactory = sqlExpressionFactory;
		}

		public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
		{
			return (SqlExpression)(object)(_methodInfo.Equals(method) ? _sqlExpressionFactory.Function("NEWID", (IEnumerable<SqlExpression>)Array.Empty<SqlExpression>(), false, (IEnumerable<bool>)Array.Empty<bool>(), method.ReturnType, (RelationalTypeMapping)null) : null);
		}
	}
}
