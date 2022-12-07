// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Query.Internal.DmStringMethodTranslator
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
using System.Linq.Expressions;
using System.Reflection;
using System.Text;



namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
  public class DmStringMethodTranslator : IMethodCallTranslator
  {
    private static readonly MethodInfo _indexOfMethodInfo = typeof (string).GetRuntimeMethod("IndexOf", new Type[1]
    {
      typeof (string)
    });
    private static readonly MethodInfo _replaceMethodInfo = typeof (string).GetRuntimeMethod("Replace", new Type[2]
    {
      typeof (string),
      typeof (string)
    });
    private static readonly MethodInfo _toLowerMethodInfo = typeof (string).GetRuntimeMethod("ToLower", Array.Empty<Type>());
    private static readonly MethodInfo _toUpperMethodInfo = typeof (string).GetRuntimeMethod("ToUpper", Array.Empty<Type>());
    private static readonly MethodInfo _substringMethodInfo = typeof (string).GetRuntimeMethod("Substring", new Type[2]
    {
      typeof (int),
      typeof (int)
    });
    private static readonly MethodInfo _isNullOrWhiteSpaceMethodInfo = typeof (string).GetRuntimeMethod("IsNullOrWhiteSpace", new Type[1]
    {
      typeof (string)
    });
    private static readonly MethodInfo _trimStartMethodInfoWithoutArgs = typeof (string).GetRuntimeMethod("TrimStart", Array.Empty<Type>());
    private static readonly MethodInfo _trimEndMethodInfoWithoutArgs = typeof (string).GetRuntimeMethod("TrimEnd", Array.Empty<Type>());
    private static readonly MethodInfo _trimMethodInfoWithoutArgs = typeof (string).GetRuntimeMethod("Trim", Array.Empty<Type>());
    private static readonly MethodInfo _trimStartMethodInfoWithCharArrayArg = typeof (string).GetRuntimeMethod("TrimStart", new Type[1]
    {
      typeof (char[])
    });
    private static readonly MethodInfo _trimEndMethodInfoWithCharArrayArg = typeof (string).GetRuntimeMethod("TrimEnd", new Type[1]
    {
      typeof (char[])
    });
    private static readonly MethodInfo _trimMethodInfoWithCharArrayArg = typeof (string).GetRuntimeMethod("Trim", new Type[1]
    {
      typeof (char[])
    });
    private static readonly MethodInfo _startsWithMethodInfo = typeof (string).GetRuntimeMethod("StartsWith", new Type[1]
    {
      typeof (string)
    });
    private static readonly MethodInfo _containsMethodInfo = typeof (string).GetRuntimeMethod("Contains", new Type[1]
    {
      typeof (string)
    });
    private static readonly MethodInfo _endsWithMethodInfo = typeof (string).GetRuntimeMethod("EndsWith", new Type[1]
    {
      typeof (string)
    });
    private readonly ISqlExpressionFactory _sqlExpressionFactory;
    private const char LikeEscapeChar = '\\';

    public DmStringMethodTranslator(ISqlExpressionFactory sqlExpressionFactory) => this._sqlExpressionFactory = sqlExpressionFactory;

    public virtual SqlExpression Translate(
      SqlExpression instance,
      MethodInfo method,
      IReadOnlyList<SqlExpression> arguments,
      IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
      if (DmStringMethodTranslator._indexOfMethodInfo.Equals((object) method))
      {
        SqlExpression sqlExpression1 = arguments[0];
        RelationalTypeMapping relationalTypeMapping = ExpressionExtensions.InferTypeMapping(new SqlExpression[2]
        {
          instance,
          sqlExpression1
        });
        SqlExpression sqlExpression2 = this._sqlExpressionFactory.ApplyTypeMapping(sqlExpression1, relationalTypeMapping);
        SqlBinaryExpression binaryExpression = this._sqlExpressionFactory.Subtract((SqlExpression) this._sqlExpressionFactory.Function("POSITION", (IEnumerable<SqlExpression>) new SqlExpression[2]
        {
          sqlExpression2,
          this._sqlExpressionFactory.ApplyTypeMapping(instance, relationalTypeMapping)
        }, true, (IEnumerable<bool>) new bool[2]
        {
          true,
          true
        }, method.ReturnType, (RelationalTypeMapping) null), (SqlExpression) this._sqlExpressionFactory.Constant((object) 1, (RelationalTypeMapping) null), (RelationalTypeMapping) null);
        return (SqlExpression) this._sqlExpressionFactory.Case((IReadOnlyList<CaseWhenClause>) new CaseWhenClause[1]
        {
          new CaseWhenClause((SqlExpression) this._sqlExpressionFactory.Equal(sqlExpression2, (SqlExpression) this._sqlExpressionFactory.Constant((object) string.Empty, relationalTypeMapping)), (SqlExpression) this._sqlExpressionFactory.Constant((object) 0, (RelationalTypeMapping) null))
        }, (SqlExpression) binaryExpression);
      }
      if (DmStringMethodTranslator._replaceMethodInfo.Equals((object) method))
      {
        SqlExpression sqlExpression3 = arguments[0];
        SqlExpression sqlExpression4 = arguments[1];
        RelationalTypeMapping relationalTypeMapping = ExpressionExtensions.InferTypeMapping(new SqlExpression[3]
        {
          instance,
          sqlExpression3,
          sqlExpression4
        });
        instance = this._sqlExpressionFactory.ApplyTypeMapping(instance, relationalTypeMapping);
        SqlExpression sqlExpression5 = this._sqlExpressionFactory.ApplyTypeMapping(sqlExpression3, relationalTypeMapping);
        SqlExpression sqlExpression6 = this._sqlExpressionFactory.ApplyTypeMapping(sqlExpression4, relationalTypeMapping);
        return (SqlExpression) this._sqlExpressionFactory.Function("REPLACE", (IEnumerable<SqlExpression>) new SqlExpression[3]
        {
          instance,
          sqlExpression5,
          sqlExpression6
        }, true, (IEnumerable<bool>) new bool[3]
        {
          true,
          true,
          true
        }, method.ReturnType, relationalTypeMapping);
      }
      if (DmStringMethodTranslator._toLowerMethodInfo.Equals((object) method) || DmStringMethodTranslator._toUpperMethodInfo.Equals((object) method))
        return (SqlExpression) this._sqlExpressionFactory.Function(DmStringMethodTranslator._toLowerMethodInfo.Equals((object) method) ? "LOWER" : "UPPER", (IEnumerable<SqlExpression>) new SqlExpression[1]
        {
          instance
        }, true, (IEnumerable<bool>) new bool[1]
        {
          true
        }, method.ReturnType, instance.TypeMapping);
      if (DmStringMethodTranslator._substringMethodInfo.Equals((object) method))
        return (SqlExpression) this._sqlExpressionFactory.Function("SUBSTRING", (IEnumerable<SqlExpression>) new SqlExpression[3]
        {
          instance,
          (SqlExpression) this._sqlExpressionFactory.Add(arguments[0], (SqlExpression) this._sqlExpressionFactory.Constant((object) 1, (RelationalTypeMapping) null), (RelationalTypeMapping) null),
          arguments[1]
        }, true, (IEnumerable<bool>) new bool[3]
        {
          true,
          true,
          true
        }, method.ReturnType, instance.TypeMapping);
      if (DmStringMethodTranslator._isNullOrWhiteSpaceMethodInfo.Equals((object) method))
      {
        SqlExpression sqlExpression = arguments[0];
        return (SqlExpression) this._sqlExpressionFactory.OrElse((SqlExpression) this._sqlExpressionFactory.IsNull(sqlExpression), (SqlExpression) this._sqlExpressionFactory.Equal((SqlExpression) this._sqlExpressionFactory.Function("LTRIM", (IEnumerable<SqlExpression>) new SqlFunctionExpression[1]
        {
          this._sqlExpressionFactory.Function("RTRIM", (IEnumerable<SqlExpression>) new SqlExpression[1]
          {
            sqlExpression
          }, true, (IEnumerable<bool>) new bool[1]
          {
            true
          }, ((Expression) sqlExpression).Type, sqlExpression.TypeMapping)
        }, true, (IEnumerable<bool>) new bool[1]
        {
          true
        }, ((Expression) sqlExpression).Type, sqlExpression.TypeMapping), (SqlExpression) this._sqlExpressionFactory.Constant((object) string.Empty, sqlExpression.TypeMapping)));
      }
      MethodInfo methodInfoWithoutArgs1 = DmStringMethodTranslator._trimStartMethodInfoWithoutArgs;
      if (((object) methodInfoWithoutArgs1 != null ? (methodInfoWithoutArgs1.Equals((object) method) ? 1 : 0) : 0) != 0 || DmStringMethodTranslator._trimStartMethodInfoWithCharArrayArg.Equals((object) method) && (arguments[0] is SqlConstantExpression constantExpression1 ? constantExpression1.Value : (object) null) is Array array1 && array1.Length == 0)
        return (SqlExpression) this._sqlExpressionFactory.Function("LTRIM", (IEnumerable<SqlExpression>) new SqlExpression[1]
        {
          instance
        }, true, (IEnumerable<bool>) new bool[1]
        {
          true
        }, ((Expression) instance).Type, instance.TypeMapping);
      MethodInfo methodInfoWithoutArgs2 = DmStringMethodTranslator._trimEndMethodInfoWithoutArgs;
      if (((object) methodInfoWithoutArgs2 != null ? (methodInfoWithoutArgs2.Equals((object) method) ? 1 : 0) : 0) != 0 || DmStringMethodTranslator._trimEndMethodInfoWithCharArrayArg.Equals((object) method) && (arguments[0] is SqlConstantExpression constantExpression2 ? constantExpression2.Value : (object) null) is Array array2 && array2.Length == 0)
        return (SqlExpression) this._sqlExpressionFactory.Function("RTRIM", (IEnumerable<SqlExpression>) new SqlExpression[1]
        {
          instance
        }, true, (IEnumerable<bool>) new bool[1]
        {
          true
        }, ((Expression) instance).Type, instance.TypeMapping);
      MethodInfo methodInfoWithoutArgs3 = DmStringMethodTranslator._trimMethodInfoWithoutArgs;
      if (((object) methodInfoWithoutArgs3 != null ? (methodInfoWithoutArgs3.Equals((object) method) ? 1 : 0) : 0) != 0 || DmStringMethodTranslator._trimMethodInfoWithCharArrayArg.Equals((object) method) && (arguments[0] is SqlConstantExpression constantExpression3 ? constantExpression3.Value : (object) null) is Array array3 && array3.Length == 0)
        return (SqlExpression) this._sqlExpressionFactory.Function("LTRIM", (IEnumerable<SqlExpression>) new SqlFunctionExpression[1]
        {
          this._sqlExpressionFactory.Function("RTRIM", (IEnumerable<SqlExpression>) new SqlExpression[1]
          {
            instance
          }, true, (IEnumerable<bool>) new bool[1]
          {
            true
          }, ((Expression) instance).Type, instance.TypeMapping)
        }, true, (IEnumerable<bool>) new bool[1]
        {
          true
        }, ((Expression) instance).Type, instance.TypeMapping);
      if (DmStringMethodTranslator._containsMethodInfo.Equals((object) method))
      {
        SqlExpression sqlExpression7 = arguments[0];
        RelationalTypeMapping relationalTypeMapping = ExpressionExtensions.InferTypeMapping(new SqlExpression[2]
        {
          instance,
          sqlExpression7
        });
        instance = this._sqlExpressionFactory.ApplyTypeMapping(instance, relationalTypeMapping);
        SqlExpression sqlExpression8 = this._sqlExpressionFactory.ApplyTypeMapping(sqlExpression7, relationalTypeMapping);
        if (sqlExpression8 is SqlConstantExpression constantExpression4)
        {
          if ((string) constantExpression4.Value == string.Empty)
            return (SqlExpression) this._sqlExpressionFactory.Constant((object) true, (RelationalTypeMapping) null);
          return (SqlExpression) this._sqlExpressionFactory.GreaterThan((SqlExpression) this._sqlExpressionFactory.Function("POSITION", (IEnumerable<SqlExpression>) new SqlExpression[2]
          {
            sqlExpression8,
            instance
          }, true, (IEnumerable<bool>) new bool[2]
          {
            true,
            true
          }, typeof (int), (RelationalTypeMapping) null), (SqlExpression) this._sqlExpressionFactory.Constant((object) 0, (RelationalTypeMapping) null));
        }
        return (SqlExpression) this._sqlExpressionFactory.OrElse((SqlExpression) this._sqlExpressionFactory.Equal(sqlExpression8, (SqlExpression) this._sqlExpressionFactory.Constant((object) string.Empty, relationalTypeMapping)), (SqlExpression) this._sqlExpressionFactory.GreaterThan((SqlExpression) this._sqlExpressionFactory.Function("POSITION", (IEnumerable<SqlExpression>) new SqlExpression[2]
        {
          sqlExpression8,
          instance
        }, true, (IEnumerable<bool>) new bool[2]
        {
          true,
          true
        }, typeof (int), (RelationalTypeMapping) null), (SqlExpression) this._sqlExpressionFactory.Constant((object) 0, (RelationalTypeMapping) null)));
      }
      if (DmStringMethodTranslator._startsWithMethodInfo.Equals((object) method))
        return this.TranslateStartsEndsWith(instance, arguments[0], true);
      return DmStringMethodTranslator._endsWithMethodInfo.Equals((object) method) ? this.TranslateStartsEndsWith(instance, arguments[0], false) : (SqlExpression) null;
    }

    private SqlExpression TranslateStartsEndsWith(
      SqlExpression instance,
      SqlExpression pattern,
      bool startsWith)
    {
      RelationalTypeMapping relationalTypeMapping = ExpressionExtensions.InferTypeMapping(new SqlExpression[2]
      {
        instance,
        pattern
      });
      instance = this._sqlExpressionFactory.ApplyTypeMapping(instance, relationalTypeMapping);
      pattern = this._sqlExpressionFactory.ApplyTypeMapping(pattern, relationalTypeMapping);
      return pattern is SqlConstantExpression constantExpression ? (!(constantExpression.Value is string str) ? (SqlExpression) this._sqlExpressionFactory.Like(instance, (SqlExpression) this._sqlExpressionFactory.Constant((object) null, relationalTypeMapping), (SqlExpression) null) : (str.Any<char>((Func<char, bool>) (c => this.IsLikeWildChar(c))) ? (SqlExpression) this._sqlExpressionFactory.Like(instance, (SqlExpression) this._sqlExpressionFactory.Constant(startsWith ? (object) (this.EscapeLikePattern(str) + "%") : (object) ("%" + this.EscapeLikePattern(str)), (RelationalTypeMapping) null), (SqlExpression) this._sqlExpressionFactory.Constant((object) '\\'.ToString(), (RelationalTypeMapping) null)) : (SqlExpression) this._sqlExpressionFactory.Like(instance, (SqlExpression) this._sqlExpressionFactory.Constant(startsWith ? (object) (str + "%") : (object) ("%" + str), (RelationalTypeMapping) null), (SqlExpression) null))) : (startsWith ? (SqlExpression) this._sqlExpressionFactory.Equal((SqlExpression) this._sqlExpressionFactory.Function("LEFT", (IEnumerable<SqlExpression>) new SqlExpression[2]
      {
        instance,
        (SqlExpression) this._sqlExpressionFactory.Function("LEN", (IEnumerable<SqlExpression>) new SqlExpression[1]
        {
          pattern
        }, true, (IEnumerable<bool>) new bool[1]
        {
          true
        }, typeof (int), (RelationalTypeMapping) null)
      }, true, (IEnumerable<bool>) new bool[2]
      {
        true,
        true
      }, typeof (string), relationalTypeMapping), pattern) : (SqlExpression) this._sqlExpressionFactory.Equal((SqlExpression) this._sqlExpressionFactory.Function("RIGHT", (IEnumerable<SqlExpression>) new SqlExpression[2]
      {
        instance,
        (SqlExpression) this._sqlExpressionFactory.Function("LEN", (IEnumerable<SqlExpression>) new SqlExpression[1]
        {
          pattern
        }, true, (IEnumerable<bool>) new bool[1]
        {
          true
        }, typeof (int), (RelationalTypeMapping) null)
      }, true, (IEnumerable<bool>) new bool[2]
      {
        true,
        true
      }, typeof (string), relationalTypeMapping), pattern));
    }

    private bool IsLikeWildChar(char c) => c == '%' || c == '_' || c == '[';

    private string EscapeLikePattern(string pattern)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < pattern.Length; ++index)
      {
        char c = pattern[index];
        if (this.IsLikeWildChar(c) || c == '\\')
          stringBuilder.Append('\\');
        stringBuilder.Append(c);
      }
      return stringBuilder.ToString();
    }
  }
}
