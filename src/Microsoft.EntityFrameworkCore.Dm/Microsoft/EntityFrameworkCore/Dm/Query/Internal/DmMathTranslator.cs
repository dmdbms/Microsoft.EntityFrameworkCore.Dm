// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmMathTranslator
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
  public class DmMathTranslator : IMethodCallTranslator
  {
    private static readonly Dictionary<MethodInfo, string> _supportedMethodTranslations = new Dictionary<MethodInfo, string>()
    {
      {
        typeof (Math).GetRuntimeMethod("Abs", new Type[1]
        {
          typeof (Decimal)
        }),
        "ABS"
      },
      {
        typeof (Math).GetRuntimeMethod("Abs", new Type[1]
        {
          typeof (double)
        }),
        "ABS"
      },
      {
        typeof (Math).GetRuntimeMethod("Abs", new Type[1]
        {
          typeof (float)
        }),
        "ABS"
      },
      {
        typeof (Math).GetRuntimeMethod("Abs", new Type[1]
        {
          typeof (int)
        }),
        "ABS"
      },
      {
        typeof (Math).GetRuntimeMethod("Abs", new Type[1]
        {
          typeof (long)
        }),
        "ABS"
      },
      {
        typeof (Math).GetRuntimeMethod("Abs", new Type[1]
        {
          typeof (sbyte)
        }),
        "ABS"
      },
      {
        typeof (Math).GetRuntimeMethod("Abs", new Type[1]
        {
          typeof (short)
        }),
        "ABS"
      },
      {
        typeof (Math).GetRuntimeMethod("Ceiling", new Type[1]
        {
          typeof (Decimal)
        }),
        "CEILING"
      },
      {
        typeof (Math).GetRuntimeMethod("Ceiling", new Type[1]
        {
          typeof (double)
        }),
        "CEILING"
      },
      {
        typeof (Math).GetRuntimeMethod("Floor", new Type[1]
        {
          typeof (Decimal)
        }),
        "FLOOR"
      },
      {
        typeof (Math).GetRuntimeMethod("Floor", new Type[1]
        {
          typeof (double)
        }),
        "FLOOR"
      },
      {
        typeof (Math).GetRuntimeMethod("Pow", new Type[2]
        {
          typeof (double),
          typeof (double)
        }),
        "POWER"
      },
      {
        typeof (Math).GetRuntimeMethod("Exp", new Type[1]
        {
          typeof (double)
        }),
        "EXP"
      },
      {
        typeof (Math).GetRuntimeMethod("Log10", new Type[1]
        {
          typeof (double)
        }),
        "LOG10"
      },
      {
        typeof (Math).GetRuntimeMethod("Log", new Type[1]
        {
          typeof (double)
        }),
        "LOG"
      },
      {
        typeof (Math).GetRuntimeMethod("Log", new Type[2]
        {
          typeof (double),
          typeof (double)
        }),
        "LOG"
      },
      {
        typeof (Math).GetRuntimeMethod("Sqrt", new Type[1]
        {
          typeof (double)
        }),
        "SQRT"
      },
      {
        typeof (Math).GetRuntimeMethod("Acos", new Type[1]
        {
          typeof (double)
        }),
        "ACOS"
      },
      {
        typeof (Math).GetRuntimeMethod("Asin", new Type[1]
        {
          typeof (double)
        }),
        "ASIN"
      },
      {
        typeof (Math).GetRuntimeMethod("Atan", new Type[1]
        {
          typeof (double)
        }),
        "ATAN"
      },
      {
        typeof (Math).GetRuntimeMethod("Atan2", new Type[2]
        {
          typeof (double),
          typeof (double)
        }),
        "ATN2"
      },
      {
        typeof (Math).GetRuntimeMethod("Cos", new Type[1]
        {
          typeof (double)
        }),
        "COS"
      },
      {
        typeof (Math).GetRuntimeMethod("Sin", new Type[1]
        {
          typeof (double)
        }),
        "SIN"
      },
      {
        typeof (Math).GetRuntimeMethod("Tan", new Type[1]
        {
          typeof (double)
        }),
        "TAN"
      },
      {
        typeof (Math).GetRuntimeMethod("Sign", new Type[1]
        {
          typeof (Decimal)
        }),
        "SIGN"
      },
      {
        typeof (Math).GetRuntimeMethod("Sign", new Type[1]
        {
          typeof (double)
        }),
        "SIGN"
      },
      {
        typeof (Math).GetRuntimeMethod("Sign", new Type[1]
        {
          typeof (float)
        }),
        "SIGN"
      },
      {
        typeof (Math).GetRuntimeMethod("Sign", new Type[1]
        {
          typeof (int)
        }),
        "SIGN"
      },
      {
        typeof (Math).GetRuntimeMethod("Sign", new Type[1]
        {
          typeof (long)
        }),
        "SIGN"
      },
      {
        typeof (Math).GetRuntimeMethod("Sign", new Type[1]
        {
          typeof (sbyte)
        }),
        "SIGN"
      },
      {
        typeof (Math).GetRuntimeMethod("Sign", new Type[1]
        {
          typeof (short)
        }),
        "SIGN"
      }
    };
    private static readonly IEnumerable<MethodInfo> _truncateMethodInfos = (IEnumerable<MethodInfo>) new MethodInfo[2]
    {
      typeof (Math).GetRuntimeMethod("Truncate", new Type[1]
      {
        typeof (Decimal)
      }),
      typeof (Math).GetRuntimeMethod("Truncate", new Type[1]
      {
        typeof (double)
      })
    };
    private static readonly IEnumerable<MethodInfo> _roundMethodInfos = (IEnumerable<MethodInfo>) new MethodInfo[4]
    {
      typeof (Math).GetRuntimeMethod("Round", new Type[1]
      {
        typeof (Decimal)
      }),
      typeof (Math).GetRuntimeMethod("Round", new Type[1]
      {
        typeof (double)
      }),
      typeof (Math).GetRuntimeMethod("Round", new Type[2]
      {
        typeof (Decimal),
        typeof (int)
      }),
      typeof (Math).GetRuntimeMethod("Round", new Type[2]
      {
        typeof (double),
        typeof (int)
      })
    };
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public DmMathTranslator(ISqlExpressionFactory sqlExpressionFactory) => this._sqlExpressionFactory = sqlExpressionFactory;

    public virtual SqlExpression Translate(
      SqlExpression instance,
      MethodInfo method,
      IReadOnlyList<SqlExpression> arguments,
      IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
      string str;
      if (DmMathTranslator._supportedMethodTranslations.TryGetValue(method, out str))
      {
        RelationalTypeMapping relationalTypeMapping1;
        if (((IReadOnlyCollection<SqlExpression>) arguments).Count != 1)
          relationalTypeMapping1 = ExpressionExtensions.InferTypeMapping(new SqlExpression[2]
          {
            arguments[0],
            arguments[1]
          });
        else
          relationalTypeMapping1 = ExpressionExtensions.InferTypeMapping(new SqlExpression[1]
          {
            arguments[0]
          });
        RelationalTypeMapping relationalTypeMapping2 = relationalTypeMapping1;
        SqlExpression[] source = new SqlExpression[((IReadOnlyCollection<SqlExpression>) arguments).Count];
        source[0] = this._sqlExpressionFactory.ApplyTypeMapping(arguments[0], relationalTypeMapping2);
        if (((IReadOnlyCollection<SqlExpression>) arguments).Count == 2)
          source[1] = this._sqlExpressionFactory.ApplyTypeMapping(arguments[1], relationalTypeMapping2);
        return (SqlExpression) this._sqlExpressionFactory.Function(str, (IEnumerable<SqlExpression>) source, true, (IEnumerable<bool>) ((IEnumerable<SqlExpression>) source).Select<SqlExpression, bool>((Func<SqlExpression, bool>) (a => true)).ToArray<bool>(), method.ReturnType, str == "SIGN" ? (RelationalTypeMapping) null : relationalTypeMapping2);
      }
      if (DmMathTranslator._truncateMethodInfos.Contains<MethodInfo>(method))
      {
        SqlExpression sqlExpression = arguments[0];
        return (SqlExpression) this._sqlExpressionFactory.Function("TRUNCATE", (IEnumerable<SqlExpression>) new SqlExpression[3]
        {
          sqlExpression,
          (SqlExpression) this._sqlExpressionFactory.Constant((object) 0, (RelationalTypeMapping) null),
          (SqlExpression) this._sqlExpressionFactory.Constant((object) 1, (RelationalTypeMapping) null)
        }, true, (IEnumerable<bool>) new bool[3]
        {
          true,
          false,
          false
        }, method.ReturnType, sqlExpression.TypeMapping);
      }
      if (!DmMathTranslator._roundMethodInfos.Contains<MethodInfo>(method))
        return (SqlExpression) null;
      SqlExpression sqlExpression1 = arguments[0];
      SqlExpression sqlExpression2 = ((IReadOnlyCollection<SqlExpression>) arguments).Count == 2 ? arguments[1] : (SqlExpression) this._sqlExpressionFactory.Constant((object) 0, (RelationalTypeMapping) null);
      return (SqlExpression) this._sqlExpressionFactory.Function("ROUND", (IEnumerable<SqlExpression>) new SqlExpression[2]
      {
        sqlExpression1,
        sqlExpression2
      }, true, (IEnumerable<bool>) new bool[2]
      {
        true,
        true
      }, method.ReturnType, sqlExpression1.TypeMapping);
    }
  }
}
