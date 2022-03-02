using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

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
			return _methodInfo.Equals(method) ? _sqlExpressionFactory.Function("NEWID", Array.Empty<SqlExpression>(), nullable: false, Array.Empty<bool>(), method.ReturnType) : null;
		}
	}
}
