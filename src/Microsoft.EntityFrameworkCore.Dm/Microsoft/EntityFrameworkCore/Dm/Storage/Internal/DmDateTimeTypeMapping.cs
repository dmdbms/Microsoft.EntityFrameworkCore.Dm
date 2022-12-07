// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmDateTimeTypeMapping
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Data;
using System.Data.Common;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class DmDateTimeTypeMapping : DateTimeTypeMapping
  {
    private const string DateFormatConst = "{0:yyyy-MM-dd}";
    private const string DateTimeFormatConst = "{0:yyyy-MM-dd HH:mm:ss.ffffff}";

    public DmDateTimeTypeMapping([NotNull] string storeType, DbType? dbType = null)
      : base(storeType, dbType)
    {
    }

    protected DmDateTimeTypeMapping(
      RelationalTypeMapping.RelationalTypeMappingParameters parameters)
      : base(parameters)
    {
    }

    protected override void ConfigureParameter(DbParameter parameter) => base.ConfigureParameter(parameter);

    public override RelationalTypeMapping Clone(string storeType, int? size)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters = base.Parameters;
      return (RelationalTypeMapping) new DmDateTimeTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).WithStoreTypeAndSize(storeType, size, new StoreTypePostfix?()));
    }

    public override CoreTypeMapping Clone(ValueConverter converter)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters = base.Parameters;
      return (CoreTypeMapping) new DmDateTimeTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).WithComposedConverter(converter));
    }

    protected override string SqlLiteralFormatString => !(((RelationalTypeMapping) this).StoreType == "date") ? "'{0:yyyy-MM-dd HH:mm:ss.ffffff}'" : "'{0:yyyy-MM-dd}'";
  }
}
