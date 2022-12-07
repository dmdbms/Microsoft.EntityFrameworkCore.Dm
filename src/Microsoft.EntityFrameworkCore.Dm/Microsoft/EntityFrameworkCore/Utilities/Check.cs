// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Utilities.Check
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;



namespace Microsoft.EntityFrameworkCore.Utilities
{
  [DebuggerStepThrough]
  internal static class Check
  {
    [ContractAnnotation("value:null => halt")]
    public static T NotNull<T>([NoEnumeration] T value, [InvokerParameterName, JetBrains.Annotations.NotNull] string parameterName)
    {
      if ((object) value == null)
      {
        Check.NotEmpty(parameterName, nameof (parameterName));
        throw new ArgumentNullException(parameterName);
      }
      return value;
    }

    [ContractAnnotation("value:null => halt")]
    public static IReadOnlyList<T> NotEmpty<T>(
      IReadOnlyList<T> value,
      [InvokerParameterName, JetBrains.Annotations.NotNull] string parameterName)
    {
      Check.NotNull<IReadOnlyList<T>>(value, parameterName);
      if (value.Count == 0)
      {
        Check.NotEmpty(parameterName, nameof (parameterName));
        throw new ArgumentException(AbstractionsStrings.CollectionArgumentIsEmpty((object) parameterName));
      }
      return value;
    }

    [ContractAnnotation("value:null => halt")]
    public static string NotEmpty(string value, [InvokerParameterName, JetBrains.Annotations.NotNull] string parameterName)
    {
      Exception exception = (Exception) null;
      if (value == null)
        exception = (Exception) new ArgumentNullException(parameterName);
      else if (value.Trim().Length == 0)
        exception = (Exception) new ArgumentException(AbstractionsStrings.ArgumentIsEmpty((object) parameterName));
      if (exception != null)
      {
        Check.NotEmpty(parameterName, nameof (parameterName));
        throw exception;
      }
      return value;
    }

    public static string NullButNotEmpty(string value, [InvokerParameterName, JetBrains.Annotations.NotNull] string parameterName)
    {
      if (value != null && value.Length == 0)
      {
        Check.NotEmpty(parameterName, nameof (parameterName));
        throw new ArgumentException(AbstractionsStrings.ArgumentIsEmpty((object) parameterName));
      }
      return value;
    }

    public static IReadOnlyList<T> HasNoNulls<T>(
      IReadOnlyList<T> value,
      [InvokerParameterName, JetBrains.Annotations.NotNull] string parameterName)
      where T : class
    {
      Check.NotNull<IReadOnlyList<T>>(value, parameterName);
      if (value.Any<T>((Func<T, bool>) (e => (object) e == null)))
      {
        Check.NotEmpty(parameterName, nameof (parameterName));
        throw new ArgumentException(parameterName);
      }
      return value;
    }
  }
}
