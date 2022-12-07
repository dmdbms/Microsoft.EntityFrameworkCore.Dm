// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmDateTimeOffsetTypeMapping
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Data;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class DmDateTimeOffsetTypeMapping : DateTimeOffsetTypeMapping
  {
    private const string DateTimeOffsetFormatConst = "{0:yyyy-MM-dd HH:mm:ss.fffzzz}";

    public DmDateTimeOffsetTypeMapping([NotNull] string storeType, DbType? dbType = System.Data.DbType.DateTimeOffset)
      : base(storeType, dbType)
    {
    }

    protected DmDateTimeOffsetTypeMapping(
      RelationalTypeMapping.RelationalTypeMappingParameters parameters)
      : base(parameters)
    {
    }

    public override RelationalTypeMapping Clone(string storeType, int? size)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters = this.Parameters;
      return (RelationalTypeMapping) new DmDateTimeOffsetTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters) parameters).WithStoreTypeAndSize(storeType, size, new StoreTypePostfix?()));
    }

    public override CoreTypeMapping Clone(ValueConverter converter)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters = base.Parameters;
      return (CoreTypeMapping) new DmDateTimeOffsetTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters) parameters).WithComposedConverter(converter));
    }

    protected override string SqlLiteralFormatString => "'{0:yyyy-MM-dd HH:mm:ss.fffzzz}'";
  }
}
