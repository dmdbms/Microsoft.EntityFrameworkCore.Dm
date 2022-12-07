// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmStringMemberTranslator
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class DmStringMemberTranslator : IMemberTranslator
  {
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public DmStringMemberTranslator(ISqlExpressionFactory sqlExpressionFactory) => this._sqlExpressionFactory = sqlExpressionFactory;

    public virtual SqlExpression Translate(
      SqlExpression instance,
      MemberInfo member,
      Type returnType,
      IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
      if (!(member.Name == "Length") || !(((Expression) instance)?.Type == typeof (string)))
        return (SqlExpression) null;
      return (SqlExpression) this._sqlExpressionFactory.Convert((SqlExpression) this._sqlExpressionFactory.Function("LENGTH", (IEnumerable<SqlExpression>) new SqlExpression[1]
      {
        instance
      }, true, (IEnumerable<bool>) new bool[1]
      {
        true
      }, typeof (long), (RelationalTypeMapping) null), returnType, (RelationalTypeMapping) null);
    }
  }
}
