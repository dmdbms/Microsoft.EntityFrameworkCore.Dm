// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmFullTextSearchFunctionsTranslator
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Extensions;
using Microsoft.EntityFrameworkCore.Dm.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class DmFullTextSearchFunctionsTranslator : IMethodCallTranslator
  {
    private const string FreeTextFunctionName = "FREETEXT";
    private const string ContainsFunctionName = "CONTAINS";
    private static readonly MethodInfo _freeTextMethodInfo = typeof (DmDbFunctionsExtensions).GetRuntimeMethod("FreeText", new Type[3]
    {
      typeof (DbFunctions),
      typeof (string),
      typeof (string)
    });
    private static readonly MethodInfo _freeTextMethodInfoWithLanguage = typeof (DmDbFunctionsExtensions).GetRuntimeMethod("FreeText", new Type[4]
    {
      typeof (DbFunctions),
      typeof (string),
      typeof (string),
      typeof (int)
    });
    private static readonly MethodInfo _containsMethodInfo = typeof (DmDbFunctionsExtensions).GetRuntimeMethod("Contains", new Type[3]
    {
      typeof (DbFunctions),
      typeof (string),
      typeof (string)
    });
    private static readonly MethodInfo _containsMethodInfoWithLanguage = typeof (DmDbFunctionsExtensions).GetRuntimeMethod("Contains", new Type[4]
    {
      typeof (DbFunctions),
      typeof (string),
      typeof (string),
      typeof (int)
    });
    private static readonly IDictionary<MethodInfo, string> _functionMapping = (IDictionary<MethodInfo, string>) new Dictionary<MethodInfo, string>()
    {
      {
        DmFullTextSearchFunctionsTranslator._freeTextMethodInfo,
        "FREETEXT"
      },
      {
        DmFullTextSearchFunctionsTranslator._freeTextMethodInfoWithLanguage,
        "FREETEXT"
      },
      {
        DmFullTextSearchFunctionsTranslator._containsMethodInfo,
        "CONTAINS"
      },
      {
        DmFullTextSearchFunctionsTranslator._containsMethodInfoWithLanguage,
        "CONTAINS"
      }
    };
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public DmFullTextSearchFunctionsTranslator(ISqlExpressionFactory sqlExpressionFactory) => this._sqlExpressionFactory = sqlExpressionFactory;

    public virtual SqlExpression Translate(
      SqlExpression instance,
      MethodInfo method,
      IReadOnlyList<SqlExpression> arguments,
      IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
      string str;
      if (!DmFullTextSearchFunctionsTranslator._functionMapping.TryGetValue(method, out str))
        return (SqlExpression) null;
      SqlExpression sqlExpression1 = arguments[1];
      RelationalTypeMapping relationalTypeMapping = sqlExpression1 is ColumnExpression ? sqlExpression1.TypeMapping : throw new InvalidOperationException(DmStrings.InvalidColumnNameForFreeText);
      SqlExpression sqlExpression2 = this._sqlExpressionFactory.ApplyTypeMapping(arguments[2], relationalTypeMapping);
      List<SqlExpression> sqlExpressionList1 = new List<SqlExpression>();
      sqlExpressionList1.Add(sqlExpression1);
      sqlExpressionList1.Add(sqlExpression2);
      List<SqlExpression> source = sqlExpressionList1;
      if (((IReadOnlyCollection<SqlExpression>) arguments).Count == 4)
      {
        List<SqlExpression> sqlExpressionList2 = source;
        ISqlExpressionFactory expressionFactory = this._sqlExpressionFactory;
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
        interpolatedStringHandler.AppendLiteral("LANGUAGE ");
        interpolatedStringHandler.AppendFormatted<object>(((SqlConstantExpression) arguments[3]).Value);
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        SqlFragmentExpression fragmentExpression = expressionFactory.Fragment(stringAndClear);
        sqlExpressionList2.Add((SqlExpression) fragmentExpression);
      }
      return (SqlExpression) this._sqlExpressionFactory.Function(str, (IEnumerable<SqlExpression>) source, true, (IEnumerable<bool>) ((IEnumerable<SqlExpression>) source).Select<SqlExpression, bool>((Func<SqlExpression, bool>) (a => false)).ToList<bool>(), typeof (bool), (RelationalTypeMapping) null);
    }
  }
}
