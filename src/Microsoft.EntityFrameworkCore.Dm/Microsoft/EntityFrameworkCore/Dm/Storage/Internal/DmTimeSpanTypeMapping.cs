// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmTimeSpanTypeMapping
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Data;
using System.Runtime.CompilerServices;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class DmTimeSpanTypeMapping : TimeSpanTypeMapping
  {
    public DmTimeSpanTypeMapping([NotNull] string storeType, DbType? dbType = null)
      : base(storeType, dbType)
    {
    }

    protected DmTimeSpanTypeMapping(
      RelationalTypeMapping.RelationalTypeMappingParameters parameters)
      : base(parameters)
    {
    }

    public override RelationalTypeMapping Clone(string storeType, int? size)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters = base.Parameters;
      return (RelationalTypeMapping) new DmTimeSpanTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).WithStoreTypeAndSize(storeType, size, new StoreTypePostfix?()));
    }

    public override CoreTypeMapping Clone(ValueConverter converter)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters = base.Parameters;
      return (CoreTypeMapping) new DmTimeSpanTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).WithComposedConverter(converter));
    }

    protected override string GenerateNonNullSqlLiteral(object value)
    {
      TimeSpan timeSpan = (TimeSpan) value;
      string str1 = timeSpan.Milliseconds.ToString();
      string str2 = str1.PadLeft(4 - str1.Length, '0');
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 5);
      interpolatedStringHandler.AppendLiteral("INTERVAL '");
      interpolatedStringHandler.AppendFormatted<int>(timeSpan.Days);
      interpolatedStringHandler.AppendLiteral(" ");
      interpolatedStringHandler.AppendFormatted<int>(timeSpan.Hours);
      interpolatedStringHandler.AppendLiteral(":");
      interpolatedStringHandler.AppendFormatted<int>(timeSpan.Minutes);
      interpolatedStringHandler.AppendLiteral(":");
      interpolatedStringHandler.AppendFormatted<int>(timeSpan.Seconds);
      interpolatedStringHandler.AppendLiteral(".");
      interpolatedStringHandler.AppendFormatted(str2);
      interpolatedStringHandler.AppendLiteral("' DAY TO SECOND");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
