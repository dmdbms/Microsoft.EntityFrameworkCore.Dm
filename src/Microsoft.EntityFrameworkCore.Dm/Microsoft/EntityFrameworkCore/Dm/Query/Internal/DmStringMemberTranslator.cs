using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

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
			if (member.Name == "Length" && instance?.Type == typeof(string))
			{
				return _sqlExpressionFactory.Convert(_sqlExpressionFactory.Function("LENGTH", new SqlExpression[1] { instance }, nullable: true, new bool[1] { true }, typeof(long)), returnType);
			}
			return null;
		}
	}
}
