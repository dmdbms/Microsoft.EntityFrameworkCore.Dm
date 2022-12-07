using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Extensions;
using Microsoft.EntityFrameworkCore.Dm.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmFullTextSearchFunctionsTranslator : IMethodCallTranslator
	{
		private const string FreeTextFunctionName = "FREETEXT";

		private const string ContainsFunctionName = "CONTAINS";

		private static readonly MethodInfo _freeTextMethodInfo = typeof(DmDbFunctionsExtensions).GetRuntimeMethod("FreeText", new Type[3]
		{
			typeof(DbFunctions),
			typeof(string),
			typeof(string)
		});

		private static readonly MethodInfo _freeTextMethodInfoWithLanguage = typeof(DmDbFunctionsExtensions).GetRuntimeMethod("FreeText", new Type[4]
		{
			typeof(DbFunctions),
			typeof(string),
			typeof(string),
			typeof(int)
		});

		private static readonly MethodInfo _containsMethodInfo = typeof(DmDbFunctionsExtensions).GetRuntimeMethod("Contains", new Type[3]
		{
			typeof(DbFunctions),
			typeof(string),
			typeof(string)
		});

		private static readonly MethodInfo _containsMethodInfoWithLanguage = typeof(DmDbFunctionsExtensions).GetRuntimeMethod("Contains", new Type[4]
		{
			typeof(DbFunctions),
			typeof(string),
			typeof(string),
			typeof(int)
		});

		private static readonly IDictionary<MethodInfo, string> _functionMapping = new Dictionary<MethodInfo, string>
		{
			{ _freeTextMethodInfo, "FREETEXT" },
			{ _freeTextMethodInfoWithLanguage, "FREETEXT" },
			{ _containsMethodInfo, "CONTAINS" },
			{ _containsMethodInfoWithLanguage, "CONTAINS" }
		};

		private readonly ISqlExpressionFactory _sqlExpressionFactory;

		public DmFullTextSearchFunctionsTranslator(ISqlExpressionFactory sqlExpressionFactory)
		{
			_sqlExpressionFactory = sqlExpressionFactory;
		}

		public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
		{
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			if (_functionMapping.TryGetValue(method, out var value))
			{
				SqlExpression val = arguments[1];
				if (!(val is ColumnExpression))
				{
					throw new InvalidOperationException(DmStrings.InvalidColumnNameForFreeText);
				}
				RelationalTypeMapping typeMapping = val.TypeMapping;
				SqlExpression item = _sqlExpressionFactory.ApplyTypeMapping(arguments[2], typeMapping);
				List<SqlExpression> list = new List<SqlExpression> { val, item };
				if (arguments.Count == 4)
				{
					ISqlExpressionFactory sqlExpressionFactory = _sqlExpressionFactory;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
					defaultInterpolatedStringHandler.AppendLiteral("LANGUAGE ");
					defaultInterpolatedStringHandler.AppendFormatted<object>(((SqlConstantExpression)arguments[3]).Value);
					list.Add((SqlExpression)(object)sqlExpressionFactory.Fragment(defaultInterpolatedStringHandler.ToStringAndClear()));
				}
				return (SqlExpression)(object)_sqlExpressionFactory.Function(value, (IEnumerable<SqlExpression>)list, true, (IEnumerable<bool>)list.Select((SqlExpression a) => false).ToList(), typeof(bool), (RelationalTypeMapping)null);
			}
			return null;
		}
	}
}
