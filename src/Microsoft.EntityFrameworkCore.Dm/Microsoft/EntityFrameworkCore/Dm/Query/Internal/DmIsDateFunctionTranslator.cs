// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmIsDateFunctionTranslator
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Extensions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Reflection;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class DmIsDateFunctionTranslator : IMethodCallTranslator
  {
    private readonly ISqlExpressionFactory _sqlExpressionFactory;
    private static readonly MethodInfo _methodInfo = typeof (DmDbFunctionsExtensions).GetRuntimeMethod("IsDate", new Type[2]
    {
      typeof (DbFunctions),
      typeof (string)
    });

    public DmIsDateFunctionTranslator(ISqlExpressionFactory sqlExpressionFactory) => this._sqlExpressionFactory = sqlExpressionFactory;

    public virtual SqlExpression Translate(
      SqlExpression instance,
      MethodInfo method,
      IReadOnlyList<SqlExpression> arguments,
      IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
      SqlUnaryExpression sqlUnaryExpression;
      if (!DmIsDateFunctionTranslator._methodInfo.Equals((object) method))
        sqlUnaryExpression = (SqlUnaryExpression) null;
      else
        sqlUnaryExpression = this._sqlExpressionFactory.Convert((SqlExpression) this._sqlExpressionFactory.Function("ISDATE", (IEnumerable<SqlExpression>) new SqlExpression[1]
        {
          arguments[1]
        }, true, (IEnumerable<bool>) new bool[1]
        {
          true
        }, DmIsDateFunctionTranslator._methodInfo.ReturnType, (RelationalTypeMapping) null), DmIsDateFunctionTranslator._methodInfo.ReturnType, (RelationalTypeMapping) null);
      return (SqlExpression) sqlUnaryExpression;
    }
  }
}
