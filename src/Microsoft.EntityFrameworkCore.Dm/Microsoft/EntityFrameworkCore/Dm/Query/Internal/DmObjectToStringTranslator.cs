using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmObjectToStringTranslator : IMethodCallTranslator
	{
		private const int DefaultLength = 100;

		private static readonly Dictionary<Type, string> _typeMapping;

		private readonly ISqlExpressionFactory _sqlExpressionFactory;

		public DmObjectToStringTranslator(ISqlExpressionFactory sqlExpressionFactory)
		{
			_sqlExpressionFactory = sqlExpressionFactory;
		}

		public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
		{
			string value;
			return (SqlExpression)(object)((method.Name == "ToString" && arguments.Count == 0 && instance != null && _typeMapping.TryGetValue(((Expression)(object)instance).Type.UnwrapNullableType(), out value)) ? _sqlExpressionFactory.Function("CONVERT", (IEnumerable<SqlExpression>)(object)new SqlExpression[2]
			{
				(SqlExpression)_sqlExpressionFactory.Fragment(value),
				instance
			}, true, (IEnumerable<bool>)new bool[2] { false, true }, typeof(string), (RelationalTypeMapping)null) : null);
		}

		static DmObjectToStringTranslator()
		{
			Dictionary<Type, string> dictionary = new Dictionary<Type, string>
			{
				{
					typeof(int),
					"VARCHAR(11)"
				},
				{
					typeof(long),
					"VARCHAR(20)"
				}
			};
			Type typeFromHandle = typeof(DateTime);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
			defaultInterpolatedStringHandler.AppendLiteral("VARCHAR(");
			defaultInterpolatedStringHandler.AppendFormatted(100);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			dictionary.Add(typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
			dictionary.Add(typeof(Guid), "VARCHAR(36)");
			dictionary.Add(typeof(byte), "VARCHAR(3)");
			Type typeFromHandle2 = typeof(byte[]);
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
			defaultInterpolatedStringHandler.AppendLiteral("VARCHAR(");
			defaultInterpolatedStringHandler.AppendFormatted(100);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			dictionary.Add(typeFromHandle2, defaultInterpolatedStringHandler.ToStringAndClear());
			Type typeFromHandle3 = typeof(double);
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
			defaultInterpolatedStringHandler.AppendLiteral("VARCHAR(");
			defaultInterpolatedStringHandler.AppendFormatted(100);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			dictionary.Add(typeFromHandle3, defaultInterpolatedStringHandler.ToStringAndClear());
			Type typeFromHandle4 = typeof(DateTimeOffset);
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
			defaultInterpolatedStringHandler.AppendLiteral("VARCHAR(");
			defaultInterpolatedStringHandler.AppendFormatted(100);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			dictionary.Add(typeFromHandle4, defaultInterpolatedStringHandler.ToStringAndClear());
			dictionary.Add(typeof(char), "VARCHAR(1)");
			dictionary.Add(typeof(short), "VARCHAR(6)");
			Type typeFromHandle5 = typeof(float);
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
			defaultInterpolatedStringHandler.AppendLiteral("VARCHAR(");
			defaultInterpolatedStringHandler.AppendFormatted(100);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			dictionary.Add(typeFromHandle5, defaultInterpolatedStringHandler.ToStringAndClear());
			Type typeFromHandle6 = typeof(decimal);
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
			defaultInterpolatedStringHandler.AppendLiteral("VARCHAR(");
			defaultInterpolatedStringHandler.AppendFormatted(100);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			dictionary.Add(typeFromHandle6, defaultInterpolatedStringHandler.ToStringAndClear());
			Type typeFromHandle7 = typeof(TimeSpan);
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
			defaultInterpolatedStringHandler.AppendLiteral("VARCHAR(");
			defaultInterpolatedStringHandler.AppendFormatted(100);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			dictionary.Add(typeFromHandle7, defaultInterpolatedStringHandler.ToStringAndClear());
			dictionary.Add(typeof(uint), "VARCHAR(10)");
			dictionary.Add(typeof(ushort), "VARCHAR(5)");
			dictionary.Add(typeof(ulong), "VARCHAR(19)");
			dictionary.Add(typeof(sbyte), "VARCHAR(4)");
			_typeMapping = dictionary;
		}
	}
}
