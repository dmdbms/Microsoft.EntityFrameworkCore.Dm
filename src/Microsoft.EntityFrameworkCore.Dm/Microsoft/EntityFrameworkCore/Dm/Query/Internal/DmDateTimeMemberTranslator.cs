// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmDateTimeMemberTranslator
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
  public class DmDateTimeMemberTranslator : IMemberTranslator
  {
    private static readonly Dictionary<string, string> _datePartMapping = new Dictionary<string, string>()
    {
      {
        "Year",
        "year"
      },
      {
        "Month",
        "month"
      },
      {
        "DayOfYear",
        "dayofyear"
      },
      {
        "Day",
        "day"
      },
      {
        "Hour",
        "hour"
      },
      {
        "Minute",
        "minute"
      },
      {
        "Second",
        "second"
      },
      {
        "Millisecond",
        "millisecond"
      }
    };
    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public DmDateTimeMemberTranslator(ISqlExpressionFactory sqlExpressionFactory) => this._sqlExpressionFactory = sqlExpressionFactory;

    public virtual SqlExpression Translate(
      SqlExpression instance,
      MemberInfo member,
      Type returnType,
      IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
      Type declaringType = member.DeclaringType;
      if (declaringType == typeof (DateTime) || declaringType == typeof (DateTimeOffset))
      {
        string name = member.Name;
        string str1;
        if (DmDateTimeMemberTranslator._datePartMapping.TryGetValue(name, out str1))
          return (SqlExpression) this._sqlExpressionFactory.Function("DATEPART", (IEnumerable<SqlExpression>) new SqlExpression[2]
          {
            (SqlExpression) this._sqlExpressionFactory.Fragment(str1),
            instance
          }, true, (IEnumerable<bool>) new bool[2]
          {
            false,
            true
          }, returnType, (RelationalTypeMapping) null);
        string str2 = name;
        if (!(str2 == "Date"))
        {
          if (str2 == "TimeOfDay")
            return (SqlExpression) this._sqlExpressionFactory.Convert(instance, returnType, (RelationalTypeMapping) null);
          if (str2 == "Now")
            return (SqlExpression) this._sqlExpressionFactory.Function(declaringType == typeof (DateTime) ? "GETDATE" : "SYSDATETIMEOFFSET", (IEnumerable<SqlExpression>) Array.Empty<SqlExpression>(), false, (IEnumerable<bool>) Array.Empty<bool>(), returnType, (RelationalTypeMapping) null);
          if (!(str2 == "UtcNow"))
          {
            if (str2 == "Today")
              return (SqlExpression) this._sqlExpressionFactory.Function("CONVERT", (IEnumerable<SqlExpression>) new SqlExpression[2]
              {
                (SqlExpression) this._sqlExpressionFactory.Fragment("date"),
                (SqlExpression) this._sqlExpressionFactory.Function("GETDATE", (IEnumerable<SqlExpression>) Array.Empty<SqlExpression>(), false, (IEnumerable<bool>) Array.Empty<bool>(), typeof (DateTime), (RelationalTypeMapping) null)
              }, true, (IEnumerable<bool>) new bool[2]
              {
                false,
                true
              }, returnType, (RelationalTypeMapping) null);
          }
          else
          {
            SqlFunctionExpression functionExpression = this._sqlExpressionFactory.Function(declaringType == typeof (DateTime) ? "GETUTCDATE" : "SYSUTCDATETIME", (IEnumerable<SqlExpression>) Array.Empty<SqlExpression>(), false, (IEnumerable<bool>) Array.Empty<bool>(), returnType, (RelationalTypeMapping) null);
            return declaringType == typeof (DateTime) ? (SqlExpression) functionExpression : (SqlExpression) this._sqlExpressionFactory.Convert((SqlExpression) functionExpression, returnType, (RelationalTypeMapping) null);
          }
        }
        else
          return (SqlExpression) this._sqlExpressionFactory.Function("CONVERT", (IEnumerable<SqlExpression>) new SqlExpression[2]
          {
            (SqlExpression) this._sqlExpressionFactory.Fragment("date"),
            instance
          }, true, (IEnumerable<bool>) new bool[2]
          {
            false,
            true
          }, returnType, instance.TypeMapping);
      }
      return (SqlExpression) null;
    }
  }
}
