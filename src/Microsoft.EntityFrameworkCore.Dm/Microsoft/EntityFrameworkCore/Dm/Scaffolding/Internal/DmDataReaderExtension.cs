// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Scaffolding.Internal.DmDataReaderExtension
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using System.Data.Common;



namespace Microsoft.EntityFrameworkCore.Dm.Scaffolding.Internal
{
  public static class DmDataReaderExtension
  {
    public static T GetValueOrDefault<T>([NotNull] this DbDataReader reader, [NotNull] string name)
    {
      int ordinal = reader.GetOrdinal(name);
      return reader.IsDBNull(ordinal) ? default (T) : reader.GetFieldValue<T>(ordinal);
    }

    public static T GetValueOrDefault<T>([NotNull] this DbDataRecord record, [NotNull] string name)
    {
      int ordinal = record.GetOrdinal(name);
      return record.IsDBNull(ordinal) ? default (T) : (T) record.GetValue(ordinal);
    }
  }
}
