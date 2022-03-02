using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

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
					return _sqlExpressionFactory.Function("DATEPART", new SqlExpression[2]
					{
						_sqlExpressionFactory.Fragment(value),
						instance
					}, nullable: true, new bool[2] { false, true }, returnType);
				}
				switch (name)
				{
				case "Date":
					return _sqlExpressionFactory.Function("CONVERT", new SqlExpression[2]
					{
						_sqlExpressionFactory.Fragment("date"),
						instance
					}, nullable: true, new bool[2] { false, true }, returnType, instance.TypeMapping);
				case "TimeOfDay":
					return _sqlExpressionFactory.Convert(instance, returnType);
				case "Now":
					return _sqlExpressionFactory.Function((declaringType == typeof(DateTime)) ? "GETDATE" : "SYSDATETIMEOFFSET", Array.Empty<SqlExpression>(), nullable: false, Array.Empty<bool>(), returnType);
				case "UtcNow":
				{
					SqlFunctionExpression sqlFunctionExpression = _sqlExpressionFactory.Function((declaringType == typeof(DateTime)) ? "GETUTCDATE" : "SYSUTCDATETIME", Array.Empty<SqlExpression>(), nullable: false, Array.Empty<bool>(), returnType);
					return (declaringType == typeof(DateTime)) ? ((SqlExpression)sqlFunctionExpression) : ((SqlExpression)_sqlExpressionFactory.Convert(sqlFunctionExpression, returnType));
				}
				case "Today":
					return _sqlExpressionFactory.Function("CONVERT", new SqlExpression[2]
					{
						_sqlExpressionFactory.Fragment("date"),
						_sqlExpressionFactory.Function("GETDATE", Array.Empty<SqlExpression>(), nullable: false, Array.Empty<bool>(), typeof(DateTime))
					}, nullable: true, new bool[2] { false, true }, returnType);
				}
			}
			return null;
		}
	}
}
