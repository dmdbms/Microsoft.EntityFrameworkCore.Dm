using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmStringMemberTranslator : IMemberTranslator
	{
		private readonly ISqlExpressionFactory _sqlExpressionFactory;

		public DmStringMemberTranslator(ISqlExpressionFactory sqlExpressionFactory)
		{
			_sqlExpressionFactory = sqlExpressionFactory;
		}

		public virtual SqlExpression Translate(SqlExpression instance, MemberInfo member, Type returnType, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
		{
			if (member.Name == "Length" && ((Expression)(object)instance)?.Type == typeof(string))
			{
				return (SqlExpression)(object)_sqlExpressionFactory.Convert((SqlExpression)(object)_sqlExpressionFactory.Function("LENGTH", (IEnumerable<SqlExpression>)(object)new SqlExpression[1] { instance }, true, (IEnumerable<bool>)new bool[1] { true }, typeof(long), (RelationalTypeMapping)null), returnType, (RelationalTypeMapping)null);
			}
			return null;
		}
	}
}
