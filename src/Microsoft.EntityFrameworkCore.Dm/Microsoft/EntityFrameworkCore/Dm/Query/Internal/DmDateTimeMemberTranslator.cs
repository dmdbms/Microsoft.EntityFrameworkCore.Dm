using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmDateTimeMemberTranslator : IMemberTranslator
	{
		private static readonly Dictionary<string, string> _datePartMapping = new Dictionary<string, string>
		{
			{ "Year", "year" },
			{ "Month", "month" },
			{ "DayOfYear", "dayofyear" },
			{ "Day", "day" },
			{ "Hour", "hour" },
			{ "Minute", "minute" },
			{ "Second", "second" },
			{ "Millisecond", "millisecond" }
		};

		private readonly ISqlExpressionFactory _sqlExpressionFactory;

		public DmDateTimeMemberTranslator(ISqlExpressionFactory sqlExpressionFactory)
		{
			_sqlExpressionFactory = sqlExpressionFactory;
		}

		public virtual SqlExpression Translate(SqlExpression instance, MemberInfo member, Type returnType, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
		{
			Type declaringType = member.DeclaringType;
			if (declaringType == typeof(DateTime) || declaringType == typeof(DateTimeOffset))
			{
				string name = member.Name;
				if (_datePartMapping.TryGetValue(name, out var value))
				{
					return (SqlExpression)(object)_sqlExpressionFactory.Function("DATEPART", (IEnumerable<SqlExpression>)(object)new SqlExpression[2]
					{
						(SqlExpression)_sqlExpressionFactory.Fragment(value),
						instance
					}, true, (IEnumerable<bool>)new bool[2] { false, true }, returnType, (RelationalTypeMapping)null);
				}
				switch (name)
				{
				case "Date":
					return (SqlExpression)(object)_sqlExpressionFactory.Function("CONVERT", (IEnumerable<SqlExpression>)(object)new SqlExpression[2]
					{
						(SqlExpression)_sqlExpressionFactory.Fragment("date"),
						instance
					}, true, (IEnumerable<bool>)new bool[2] { false, true }, returnType, instance.TypeMapping);
				case "TimeOfDay":
					return (SqlExpression)(object)_sqlExpressionFactory.Convert(instance, returnType, (RelationalTypeMapping)null);
				case "Now":
					return (SqlExpression)(object)_sqlExpressionFactory.Function((declaringType == typeof(DateTime)) ? "GETDATE" : "SYSDATETIMEOFFSET", (IEnumerable<SqlExpression>)Array.Empty<SqlExpression>(), false, (IEnumerable<bool>)Array.Empty<bool>(), returnType, (RelationalTypeMapping)null);
				case "UtcNow":
				{
					SqlFunctionExpression val = _sqlExpressionFactory.Function((declaringType == typeof(DateTime)) ? "GETUTCDATE" : "SYSUTCDATETIME", (IEnumerable<SqlExpression>)Array.Empty<SqlExpression>(), false, (IEnumerable<bool>)Array.Empty<bool>(), returnType, (RelationalTypeMapping)null);
					return (SqlExpression)((declaringType == typeof(DateTime)) ? ((object)val) : ((object)_sqlExpressionFactory.Convert((SqlExpression)(object)val, returnType, (RelationalTypeMapping)null)));
				}
				case "Today":
					return (SqlExpression)(object)_sqlExpressionFactory.Function("CONVERT", (IEnumerable<SqlExpression>)(object)new SqlExpression[2]
					{
						(SqlExpression)_sqlExpressionFactory.Fragment("date"),
						(SqlExpression)_sqlExpressionFactory.Function("GETDATE", (IEnumerable<SqlExpression>)Array.Empty<SqlExpression>(), false, (IEnumerable<bool>)Array.Empty<bool>(), typeof(DateTime), (RelationalTypeMapping)null)
					}, true, (IEnumerable<bool>)new bool[2] { false, true }, returnType, (RelationalTypeMapping)null);
				}
			}
			return null;
		}
	}
}
