using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmObjectToStringTranslator : IMethodCallTranslator
	{
		private const int DefaultLength = 100;

		private static readonly Dictionary<Type, string> _typeMapping = new Dictionary<Type, string>
		{
			{
				typeof(int),
				"VARCHAR(11)"
			},
			{
				typeof(long),
				"VARCHAR(20)"
			},
			{
				typeof(DateTime),
				$"VARCHAR({100})"
			},
			{
				typeof(Guid),
				"VARCHAR(36)"
			},
			{
				typeof(byte),
				"VARCHAR(3)"
			},
			{
				typeof(byte[]),
				$"VARCHAR({100})"
			},
			{
				typeof(double),
				$"VARCHAR({100})"
			},
			{
				typeof(DateTimeOffset),
				$"VARCHAR({100})"
			},
			{
				typeof(char),
				"VARCHAR(1)"
			},
			{
				typeof(short),
				"VARCHAR(6)"
			},
			{
				typeof(float),
				$"VARCHAR({100})"
			},
			{
				typeof(decimal),
				$"VARCHAR({100})"
			},
			{
				typeof(TimeSpan),
				$"VARCHAR({100})"
			},
			{
				typeof(uint),
				"VARCHAR(10)"
			},
			{
				typeof(ushort),
				"VARCHAR(5)"
			},
			{
				typeof(ulong),
				"VARCHAR(19)"
			},
			{
				typeof(sbyte),
				"VARCHAR(4)"
			}
		};

		private readonly ISqlExpressionFactory _sqlExpressionFactory;

		public DmObjectToStringTranslator(ISqlExpressionFactory sqlExpressionFactory)
		{
			_sqlExpressionFactory = sqlExpressionFactory;
		}

		public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
		{
			string value;
			return (method.Name == "ToString" && arguments.Count == 0 && instance != null && _typeMapping.TryGetValue(instance.Type.UnwrapNullableType(), out value)) ? _sqlExpressionFactory.Function("CONVERT", new SqlExpression[2]
			{
				_sqlExpressionFactory.Fragment(value),
				instance
			}, nullable: true, new bool[2] { false, true }, typeof(string)) : null;
		}
	}
}
