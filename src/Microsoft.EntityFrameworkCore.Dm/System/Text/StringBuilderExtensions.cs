// Decompiled with JetBrains decompiler
// Type: System.Text.StringBuilderExtensions
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using System.Collections.Generic;



namespace System.Text
{
  internal static class StringBuilderExtensions
  {
    public static StringBuilder AppendJoin(
      this StringBuilder stringBuilder,
      IEnumerable<string> values,
      string separator = ", ")
    {
      return stringBuilder.AppendJoin<string>(values, (Action<StringBuilder, string>) ((sb, value) => sb.Append(value)), separator);
    }

    public static StringBuilder AppendJoin(
      this StringBuilder stringBuilder,
      string separator,
      params string[] values)
    {
      return stringBuilder.AppendJoin<string>((IEnumerable<string>) values, (Action<StringBuilder, string>) ((sb, value) => sb.Append(value)), separator);
    }

    public static StringBuilder AppendJoin<T>(
      this StringBuilder stringBuilder,
      IEnumerable<T> values,
      Action<StringBuilder, T> joinAction,
      string separator = ", ")
    {
      bool flag = false;
      foreach (T obj in values)
      {
        joinAction(stringBuilder, obj);
        stringBuilder.Append(separator);
        flag = true;
      }
      if (flag)
        stringBuilder.Length -= separator.Length;
      return stringBuilder;
    }

    public static StringBuilder AppendJoin<T, TParam>(
      this StringBuilder stringBuilder,
      IEnumerable<T> values,
      TParam param,
      Action<StringBuilder, T, TParam> joinAction,
      string separator = ", ")
    {
      bool flag = false;
      foreach (T obj in values)
      {
        joinAction(stringBuilder, obj, param);
        stringBuilder.Append(separator);
        flag = true;
      }
      if (flag)
        stringBuilder.Length -= separator.Length;
      return stringBuilder;
    }

    public static StringBuilder AppendJoin<T, TParam1, TParam2>(
      this StringBuilder stringBuilder,
      IEnumerable<T> values,
      TParam1 param1,
      TParam2 param2,
      Action<StringBuilder, T, TParam1, TParam2> joinAction,
      string separator = ", ")
    {
      bool flag = false;
      foreach (T obj in values)
      {
        joinAction(stringBuilder, obj, param1, param2);
        stringBuilder.Append(separator);
        flag = true;
      }
      if (flag)
        stringBuilder.Length -= separator.Length;
      return stringBuilder;
    }
  }
}
