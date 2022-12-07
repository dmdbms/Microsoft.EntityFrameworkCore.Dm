// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmNewGuidTranslator
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Reflection;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class DmNewGuidTranslator : IMethodCallTranslator
  {
    private static readonly MethodInfo _methodInfo = typeof (Guid).GetRuntimeMethod("NewGuid", Array.Empty<Type>());
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public DmNewGuidTranslator(ISqlExpressionFactory sqlExpressionFactory) => this._sqlExpressionFactory = sqlExpressionFactory;

    public virtual SqlExpression Translate(
      SqlExpression instance,
      MethodInfo method,
      IReadOnlyList<SqlExpression> arguments,
      IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
      return DmNewGuidTranslator._methodInfo.Equals((object) method) ? (SqlExpression) this._sqlExpressionFactory.Function("NEWID", (IEnumerable<SqlExpression>) Array.Empty<SqlExpression>(), false, (IEnumerable<bool>) Array.Empty<bool>(), method.ReturnType, (RelationalTypeMapping) null) : (SqlExpression) null;
    }
  }
}
