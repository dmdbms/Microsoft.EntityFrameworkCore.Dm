// Decompiled with JetBrains decompiler
// Type: System.Collections.Generic.EnumerableExtensions
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using System.Diagnostics;
using System.Linq;



namespace System.Collections.Generic
{
  [DebuggerStepThrough]
  internal static class EnumerableExtensions
  {
    public static string Join(this IEnumerable<object> source, string separator = ", ") => string.Join<object>(separator, source);

    public static IEnumerable<T> Distinct<T>(
      this IEnumerable<T> source,
      Func<T, T, bool> comparer)
      where T : class
    {
      return source.Distinct<T>((IEqualityComparer<T>) new EnumerableExtensions.DynamicEqualityComparer<T>(comparer));
    }

    private sealed class DynamicEqualityComparer<T> : IEqualityComparer<T> where T : class
    {
      private readonly Func<T, T, bool> _func;

      public DynamicEqualityComparer(Func<T, T, bool> func) => this._func = func;

      public bool Equals(T x, T y) => this._func(x, y);

      public int GetHashCode(T obj) => 0;
    }
  }
}
