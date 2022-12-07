// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmByteTypeMapping
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Data;
using System.Runtime.CompilerServices;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class DmByteTypeMapping : ByteTypeMapping
  {
    public DmByteTypeMapping([NotNull] string storeType, DbType? dbType = null)
      : base(storeType, dbType)
    {
    }

    protected DmByteTypeMapping(
      RelationalTypeMapping.RelationalTypeMappingParameters parameters)
      : base(parameters)
    {
    }

    public virtual RelationalTypeMapping Clone(string storeType, int? size)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters = this.Parameters;
      return (RelationalTypeMapping) new DmByteTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).WithStoreTypeAndSize(storeType, size, new StoreTypePostfix?()));
    }

    public virtual CoreTypeMapping Clone(ValueConverter converter)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters = this.Parameters;
      return (CoreTypeMapping) new DmByteTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).WithComposedConverter(converter));
    }

    protected virtual string GenerateNonNullSqlLiteral(object value)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 2);
      interpolatedStringHandler.AppendLiteral("CAST(");
      interpolatedStringHandler.AppendFormatted(base.GenerateNonNullSqlLiteral(value));
      interpolatedStringHandler.AppendLiteral(" AS ");
      interpolatedStringHandler.AppendFormatted(this.StoreType);
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
