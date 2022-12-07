// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmDateTimeMethodTranslator
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
  public class DmDateTimeMethodTranslator : IMethodCallTranslator
  {
    private readonly Dictionary<MethodInfo, string> _methodInfoDatePartMapping = new Dictionary<MethodInfo, string>()
    {
      {
        typeof (DateTime).GetRuntimeMethod("AddYears", new Type[1]
        {
          typeof (int)
        }),
        "year"
      },
      {
        typeof (DateTime).GetRuntimeMethod("AddMonths", new Type[1]
        {
          typeof (int)
        }),
        "month"
      },
      {
        typeof (DateTime).GetRuntimeMethod("AddDays", new Type[1]
        {
          typeof (double)
        }),
        "day"
      },
      {
        typeof (DateTime).GetRuntimeMethod("AddHours", new Type[1]
        {
          typeof (double)
        }),
        "hour"
      },
      {
        typeof (DateTime).GetRuntimeMethod("AddMinutes", new Type[1]
        {
          typeof (double)
        }),
        "minute"
      },
      {
        typeof (DateTime).GetRuntimeMethod("AddSeconds", new Type[1]
        {
          typeof (double)
        }),
        "second"
      },
      {
        typeof (DateTime).GetRuntimeMethod("AddMilliseconds", new Type[1]
        {
          typeof (double)
        }),
        "millisecond"
      },
      {
        typeof (DateTimeOffset).GetRuntimeMethod("AddYears", new Type[1]
        {
          typeof (int)
        }),
        "year"
      },
      {
        typeof (DateTimeOffset).GetRuntimeMethod("AddMonths", new Type[1]
        {
          typeof (int)
        }),
        "month"
      },
      {
        typeof (DateTimeOffset).GetRuntimeMethod("AddDays", new Type[1]
        {
          typeof (double)
        }),
        "day"
      },
      {
        typeof (DateTimeOffset).GetRuntimeMethod("AddHours", new Type[1]
        {
          typeof (double)
        }),
        "hour"
      },
      {
        typeof (DateTimeOffset).GetRuntimeMethod("AddMinutes", new Type[1]
        {
          typeof (double)
        }),
        "minute"
      },
      {
        typeof (DateTimeOffset).GetRuntimeMethod("AddSeconds", new Type[1]
        {
          typeof (double)
        }),
        "second"
      },
      {
        typeof (DateTimeOffset).GetRuntimeMethod("AddMilliseconds", new Type[1]
        {
          typeof (double)
        }),
        "millisecond"
      }
    };
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public DmDateTimeMethodTranslator(ISqlExpressionFactory sqlExpressionFactory) => this._sqlExpressionFactory = sqlExpressionFactory;

    public virtual SqlExpression Translate(
      SqlExpression instance,
      MethodInfo method,
      IReadOnlyList<SqlExpression> arguments,
      IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
      string str;
      if (!this._methodInfoDatePartMapping.TryGetValue(method, out str))
        return (SqlExpression) null;
      SqlFunctionExpression functionExpression;
      if (str.Equals("year") || str.Equals("month") || !(arguments[0] is SqlConstantExpression constantExpression) || (double) constantExpression.Value < (double) int.MaxValue && (double) constantExpression.Value > (double) int.MinValue)
        functionExpression = this._sqlExpressionFactory.Function("DATEADD", (IEnumerable<SqlExpression>) new SqlExpression[3]
        {
          (SqlExpression) this._sqlExpressionFactory.Fragment(str),
          (SqlExpression) this._sqlExpressionFactory.Convert(arguments[0], typeof (int), (RelationalTypeMapping) null),
          instance
        }, true, (IEnumerable<bool>) new bool[3]
        {
          false,
          true,
          true
        }, ((Expression) instance).Type, instance.TypeMapping);
      else
        functionExpression = (SqlFunctionExpression) null;
      return (SqlExpression) functionExpression;
    }
  }
}
