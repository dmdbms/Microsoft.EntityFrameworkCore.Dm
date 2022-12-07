// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmDoubleTypeMapping
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Data;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class DmDoubleTypeMapping : DoubleTypeMapping
  {
    public DmDoubleTypeMapping([NotNull] string storeType, DbType? dbType = null)
      : base(storeType, dbType)
    {
    }

    protected DmDoubleTypeMapping(
      RelationalTypeMapping.RelationalTypeMappingParameters parameters)
      : base(parameters)
    {
    }

    public override RelationalTypeMapping Clone(string storeType, int? size)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters = base.Parameters;
      return (RelationalTypeMapping) new DmDoubleTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).WithStoreTypeAndSize(storeType, size, new StoreTypePostfix?()));
    }

    public override CoreTypeMapping Clone(ValueConverter converter)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters = base.Parameters;
      return (CoreTypeMapping) new DmDoubleTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).WithComposedConverter(converter));
    }

    protected override string GenerateNonNullSqlLiteral(object value)
    {
      string nonNullSqlLiteral = base.GenerateNonNullSqlLiteral(value);
      return !nonNullSqlLiteral.Contains("E") && !nonNullSqlLiteral.Contains("e") ? nonNullSqlLiteral + "E0" : nonNullSqlLiteral;
    }
  }
}
