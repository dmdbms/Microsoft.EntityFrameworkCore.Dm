// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmDateDiffFunctionsTranslator
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
  public class DmDateDiffFunctionsTranslator : IMethodCallTranslator
  {
    private readonly Dictionary<MethodInfo, string> _methodInfoDateDiffMapping = new Dictionary<MethodInfo, string>()
    {
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffYear", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime),
          typeof (DateTime)
        }),
        "YEAR"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffYear", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime?),
          typeof (DateTime?)
        }),
        "YEAR"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffYear", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset),
          typeof (DateTimeOffset)
        }),
        "YEAR"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffYear", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset?),
          typeof (DateTimeOffset?)
        }),
        "YEAR"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMonth", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime),
          typeof (DateTime)
        }),
        "MONTH"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMonth", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime?),
          typeof (DateTime?)
        }),
        "MONTH"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMonth", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset),
          typeof (DateTimeOffset)
        }),
        "MONTH"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMonth", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset?),
          typeof (DateTimeOffset?)
        }),
        "MONTH"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffDay", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime),
          typeof (DateTime)
        }),
        "DAY"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffDay", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime?),
          typeof (DateTime?)
        }),
        "DAY"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffDay", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset),
          typeof (DateTimeOffset)
        }),
        "DAY"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffDay", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset?),
          typeof (DateTimeOffset?)
        }),
        "DAY"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime),
          typeof (DateTime)
        }),
        "HOUR"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime?),
          typeof (DateTime?)
        }),
        "HOUR"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset),
          typeof (DateTimeOffset)
        }),
        "HOUR"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset?),
          typeof (DateTimeOffset?)
        }),
        "HOUR"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
        {
          typeof (DbFunctions),
          typeof (TimeSpan),
          typeof (TimeSpan)
        }),
        "HOUR"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
        {
          typeof (DbFunctions),
          typeof (TimeSpan?),
          typeof (TimeSpan?)
        }),
        "HOUR"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime),
          typeof (DateTime)
        }),
        "MINUTE"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime?),
          typeof (DateTime?)
        }),
        "MINUTE"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset),
          typeof (DateTimeOffset)
        }),
        "MINUTE"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset?),
          typeof (DateTimeOffset?)
        }),
        "MINUTE"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
        {
          typeof (DbFunctions),
          typeof (TimeSpan),
          typeof (TimeSpan)
        }),
        "MINUTE"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
        {
          typeof (DbFunctions),
          typeof (TimeSpan?),
          typeof (TimeSpan?)
        }),
        "MINUTE"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime),
          typeof (DateTime)
        }),
        "SECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime?),
          typeof (DateTime?)
        }),
        "SECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset),
          typeof (DateTimeOffset)
        }),
        "SECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset?),
          typeof (DateTimeOffset?)
        }),
        "SECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (TimeSpan),
          typeof (TimeSpan)
        }),
        "SECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (TimeSpan?),
          typeof (TimeSpan?)
        }),
        "SECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime),
          typeof (DateTime)
        }),
        "MILLISECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime?),
          typeof (DateTime?)
        }),
        "MILLISECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset),
          typeof (DateTimeOffset)
        }),
        "MILLISECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset?),
          typeof (DateTimeOffset?)
        }),
        "MILLISECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (TimeSpan),
          typeof (TimeSpan)
        }),
        "MILLISECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (TimeSpan?),
          typeof (TimeSpan?)
        }),
        "MILLISECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime),
          typeof (DateTime)
        }),
        "MICROSECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime?),
          typeof (DateTime?)
        }),
        "MICROSECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset),
          typeof (DateTimeOffset)
        }),
        "MICROSECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset?),
          typeof (DateTimeOffset?)
        }),
        "MICROSECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (TimeSpan),
          typeof (TimeSpan)
        }),
        "MICROSECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (TimeSpan?),
          typeof (TimeSpan?)
        }),
        "MICROSECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime),
          typeof (DateTime)
        }),
        "NANOSECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTime?),
          typeof (DateTime?)
        }),
        "NANOSECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset),
          typeof (DateTimeOffset)
        }),
        "NANOSECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (DateTimeOffset?),
          typeof (DateTimeOffset?)
        }),
        "NANOSECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (TimeSpan),
          typeof (TimeSpan)
        }),
        "NANOSECOND"
      },
      {
        typeof (DmDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
        {
          typeof (DbFunctions),
          typeof (TimeSpan?),
          typeof (TimeSpan?)
        }),
        "NANOSECOND"
      }
    };
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public DmDateDiffFunctionsTranslator(ISqlExpressionFactory sqlExpressionFactory) => this._sqlExpressionFactory = sqlExpressionFactory;

    public virtual SqlExpression Translate(
      SqlExpression instance,
      MethodInfo method,
      IReadOnlyList<SqlExpression> arguments,
      IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
      string str;
      if (!this._methodInfoDateDiffMapping.TryGetValue(method, out str))
        return (SqlExpression) null;
      SqlExpression sqlExpression1 = arguments[1];
      SqlExpression sqlExpression2 = arguments[2];
      RelationalTypeMapping relationalTypeMapping = ExpressionExtensions.InferTypeMapping(new SqlExpression[2]
      {
        sqlExpression1,
        sqlExpression2
      });
      SqlExpression sqlExpression3 = this._sqlExpressionFactory.ApplyTypeMapping(sqlExpression1, relationalTypeMapping);
      SqlExpression sqlExpression4 = this._sqlExpressionFactory.ApplyTypeMapping(sqlExpression2, relationalTypeMapping);
      return (SqlExpression) this._sqlExpressionFactory.Function("DATEDIFF", (IEnumerable<SqlExpression>) new SqlExpression[3]
      {
        (SqlExpression) this._sqlExpressionFactory.Fragment(str),
        sqlExpression3,
        sqlExpression4
      }, true, (IEnumerable<bool>) new bool[3]
      {
        false,
        true,
        true
      }, typeof (int), (RelationalTypeMapping) null);
    }
  }
}
