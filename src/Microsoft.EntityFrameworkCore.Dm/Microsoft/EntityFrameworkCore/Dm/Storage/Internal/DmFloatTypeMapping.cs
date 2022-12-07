// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmFloatTypeMapping
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
  public class DmFloatTypeMapping : FloatTypeMapping
  {
    public DmFloatTypeMapping([NotNull] string storeType, DbType? dbType = null)
      : base(storeType, dbType)
    {
    }

    protected DmFloatTypeMapping(
      RelationalTypeMapping.RelationalTypeMappingParameters parameters)
      : base(parameters)
    {
    }

    public virtual RelationalTypeMapping Clone(string storeType, int? size)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters = base.Parameters;
      return (RelationalTypeMapping) new DmFloatTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).WithStoreTypeAndSize(storeType, size, new StoreTypePostfix?()));
    }

    public virtual CoreTypeMapping Clone(ValueConverter converter)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters = base.Parameters;
      return (CoreTypeMapping) new DmFloatTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).WithComposedConverter(converter));
    }

    protected virtual string GenerateNonNullSqlLiteral(object value)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 2);
      interpolatedStringHandler.AppendLiteral("CAST(");
      interpolatedStringHandler.AppendFormatted(base.GenerateNonNullSqlLiteral(value));
      interpolatedStringHandler.AppendLiteral(" AS ");
      interpolatedStringHandler.AppendFormatted(((RelationalTypeMapping) this).StoreType);
      interpolatedStringHandler.AppendLiteral(")");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
