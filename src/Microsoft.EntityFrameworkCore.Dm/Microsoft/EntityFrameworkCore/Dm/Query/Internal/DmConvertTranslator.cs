// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmConvertTranslator
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class DmConvertTranslator : IMethodCallTranslator
  {
    private static readonly Dictionary<string, string> _typeMapping = new Dictionary<string, string>()
    {
      ["ToByte"] = "tinyint",
      ["ToDecimal"] = "decimal(18, 2)",
      ["ToDouble"] = "float",
      ["ToInt16"] = "smallint",
      ["ToInt32"] = "int",
      ["ToInt64"] = "bigint",
      ["ToString"] = "varchar"
    };
    private static readonly List<Type> _supportedTypes = new List<Type>()
    {
      typeof (bool),
      typeof (byte),
      typeof (Decimal),
      typeof (double),
      typeof (float),
      typeof (int),
      typeof (long),
      typeof (short),
      typeof (string)
    };
    private static readonly IEnumerable<MethodInfo> _supportedMethods = DmConvertTranslator._typeMapping.Keys.SelectMany<string, MethodInfo>((Func<string, IEnumerable<MethodInfo>>) (t => typeof (Convert).GetTypeInfo().GetDeclaredMethods(t).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.GetParameters().Length == 1 && DmConvertTranslator._supportedTypes.Contains(((IEnumerable<ParameterInfo>) m.GetParameters()).First<ParameterInfo>().ParameterType)))));
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public DmConvertTranslator(ISqlExpressionFactory sqlExpressionFactory) => this._sqlExpressionFactory = sqlExpressionFactory;

    public virtual SqlExpression Translate(
      SqlExpression instance,
      MethodInfo method,
      IReadOnlyList<SqlExpression> arguments,
      IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
      SqlFunctionExpression functionExpression;
      if (!DmConvertTranslator._supportedMethods.Contains<MethodInfo>(method))
        functionExpression = (SqlFunctionExpression) null;
      else
        functionExpression = this._sqlExpressionFactory.Function("CONVERT", (IEnumerable<SqlExpression>) new SqlExpression[2]
        {
          (SqlExpression) this._sqlExpressionFactory.Fragment(DmConvertTranslator._typeMapping[method.Name]),
          arguments[0]
        }, true, (IEnumerable<bool>) new bool[2]
        {
          false,
          true
        }, method.ReturnType, (RelationalTypeMapping) null);
      return (SqlExpression) functionExpression;
    }
  }
}
