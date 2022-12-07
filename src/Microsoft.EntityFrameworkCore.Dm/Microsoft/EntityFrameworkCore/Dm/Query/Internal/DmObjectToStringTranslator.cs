// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmObjectToStringTranslator
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
using System.Runtime.CompilerServices;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class DmObjectToStringTranslator : IMethodCallTranslator
  {
    private const int DefaultLength = 100;
    private static readonly Dictionary<Type, string> _typeMapping;
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public DmObjectToStringTranslator(ISqlExpressionFactory sqlExpressionFactory) => this._sqlExpressionFactory = sqlExpressionFactory;

    public virtual SqlExpression Translate(
      SqlExpression instance,
      MethodInfo method,
      IReadOnlyList<SqlExpression> arguments,
      IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
      string str;
      SqlFunctionExpression functionExpression;
      if (!(method.Name == "ToString") || ((IReadOnlyCollection<SqlExpression>) arguments).Count != 0 || instance == null || !DmObjectToStringTranslator._typeMapping.TryGetValue(((Expression) instance).Type.UnwrapNullableType(), out str))
        functionExpression = (SqlFunctionExpression) null;
      else
        functionExpression = this._sqlExpressionFactory.Function("CONVERT", (IEnumerable<SqlExpression>) new SqlExpression[2]
        {
          (SqlExpression) this._sqlExpressionFactory.Fragment(str),
          instance
        }, true, (IEnumerable<bool>) new bool[2]
        {
          false,
          true
        }, typeof (string), (RelationalTypeMapping) null);
      return (SqlExpression) functionExpression;
    }

    static DmObjectToStringTranslator()
    {
      Dictionary<Type, string> dictionary1 = new Dictionary<Type, string>();
      dictionary1.Add(typeof (int), "VARCHAR(11)");
      dictionary1.Add(typeof (long), "VARCHAR(20)");
      Dictionary<Type, string> dictionary2 = dictionary1;
      Type key1 = typeof (DateTime);
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
      interpolatedStringHandler.AppendLiteral("VARCHAR(");
      interpolatedStringHandler.AppendFormatted<int>(100);
      interpolatedStringHandler.AppendLiteral(")");
      string stringAndClear1 = interpolatedStringHandler.ToStringAndClear();
      dictionary2.Add(key1, stringAndClear1);
      dictionary1.Add(typeof (Guid), "VARCHAR(36)");
      dictionary1.Add(typeof (byte), "VARCHAR(3)");
      Dictionary<Type, string> dictionary3 = dictionary1;
      Type key2 = typeof (byte[]);
      interpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
      interpolatedStringHandler.AppendLiteral("VARCHAR(");
      interpolatedStringHandler.AppendFormatted<int>(100);
      interpolatedStringHandler.AppendLiteral(")");
      string stringAndClear2 = interpolatedStringHandler.ToStringAndClear();
      dictionary3.Add(key2, stringAndClear2);
      Dictionary<Type, string> dictionary4 = dictionary1;
      Type key3 = typeof (double);
      interpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
      interpolatedStringHandler.AppendLiteral("VARCHAR(");
      interpolatedStringHandler.AppendFormatted<int>(100);
      interpolatedStringHandler.AppendLiteral(")");
      string stringAndClear3 = interpolatedStringHandler.ToStringAndClear();
      dictionary4.Add(key3, stringAndClear3);
      Dictionary<Type, string> dictionary5 = dictionary1;
      Type key4 = typeof (DateTimeOffset);
      interpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
      interpolatedStringHandler.AppendLiteral("VARCHAR(");
      interpolatedStringHandler.AppendFormatted<int>(100);
      interpolatedStringHandler.AppendLiteral(")");
      string stringAndClear4 = interpolatedStringHandler.ToStringAndClear();
      dictionary5.Add(key4, stringAndClear4);
      dictionary1.Add(typeof (char), "VARCHAR(1)");
      dictionary1.Add(typeof (short), "VARCHAR(6)");
      Dictionary<Type, string> dictionary6 = dictionary1;
      Type key5 = typeof (float);
      interpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
      interpolatedStringHandler.AppendLiteral("VARCHAR(");
      interpolatedStringHandler.AppendFormatted<int>(100);
      interpolatedStringHandler.AppendLiteral(")");
      string stringAndClear5 = interpolatedStringHandler.ToStringAndClear();
      dictionary6.Add(key5, stringAndClear5);
      Dictionary<Type, string> dictionary7 = dictionary1;
      Type key6 = typeof (Decimal);
      interpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
      interpolatedStringHandler.AppendLiteral("VARCHAR(");
      interpolatedStringHandler.AppendFormatted<int>(100);
      interpolatedStringHandler.AppendLiteral(")");
      string stringAndClear6 = interpolatedStringHandler.ToStringAndClear();
      dictionary7.Add(key6, stringAndClear6);
      Dictionary<Type, string> dictionary8 = dictionary1;
      Type key7 = typeof (TimeSpan);
      interpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
      interpolatedStringHandler.AppendLiteral("VARCHAR(");
      interpolatedStringHandler.AppendFormatted<int>(100);
      interpolatedStringHandler.AppendLiteral(")");
      string stringAndClear7 = interpolatedStringHandler.ToStringAndClear();
      dictionary8.Add(key7, stringAndClear7);
      dictionary1.Add(typeof (uint), "VARCHAR(10)");
      dictionary1.Add(typeof (ushort), "VARCHAR(5)");
      dictionary1.Add(typeof (ulong), "VARCHAR(19)");
      dictionary1.Add(typeof (sbyte), "VARCHAR(4)");
      DmObjectToStringTranslator._typeMapping = dictionary1;
    }
  }
}
