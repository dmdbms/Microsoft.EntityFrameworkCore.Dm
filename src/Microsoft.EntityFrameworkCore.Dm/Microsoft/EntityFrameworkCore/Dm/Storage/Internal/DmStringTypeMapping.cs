// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmStringTypeMapping
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Data;
using System.Data.Common;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class DmStringTypeMapping : StringTypeMapping
  {
    private const int UnicodeMax = 8188;
    private const int AnsiMax = 8188;
    private readonly int _maxSpecificSize;
    private readonly StoreTypePostfix? _storeTypePostfix;

    public DmStringTypeMapping(
      [NotNull] string storeType,
      [CanBeNull] DbType? dbType,
      bool unicode = false,
      int? size = null,
      bool fixedLength = false,
      StoreTypePostfix? storeTypePostfix = null)
      : this(new RelationalTypeMapping.RelationalTypeMappingParameters(new CoreTypeMapping.CoreTypeMappingParameters(typeof (string), (ValueConverter) null, (ValueComparer) null, (ValueComparer) null, (Func<IProperty, IEntityType, ValueGenerator>) null), storeType, DmStringTypeMapping.GetStoreTypePostfix(storeTypePostfix, unicode, size), dbType, unicode, size, fixedLength, new int?(), new int?()))
    {
      this._storeTypePostfix = storeTypePostfix;
    }

    protected DmStringTypeMapping(
      RelationalTypeMapping.RelationalTypeMappingParameters parameters)
      : base(parameters)
    {
      this._maxSpecificSize = DmStringTypeMapping.CalculateSize(((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).Unicode, ((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).Size);
    }

    private static StoreTypePostfix GetStoreTypePostfix(
      StoreTypePostfix? storeTypePostfix,
      bool unicode,
      int? size)
    {
      StoreTypePostfix? nullable1 = storeTypePostfix;
      if (nullable1.HasValue)
        return nullable1.GetValueOrDefault();
      if (!unicode)
      {
        if (size.HasValue)
        {
          int? nullable2 = size;
          int num = 8188;
          if (nullable2.GetValueOrDefault() <= num & nullable2.HasValue)
            return (StoreTypePostfix) 1;
        }
        return (StoreTypePostfix) 0;
      }
      if (size.HasValue)
      {
        int? nullable3 = size;
        int num = 8188;
        if (nullable3.GetValueOrDefault() <= num & nullable3.HasValue)
          return (StoreTypePostfix) 1;
      }
      return (StoreTypePostfix) 0;
    }

    private static int CalculateSize(bool unicode, int? size)
    {
      if (!unicode)
      {
        if (size.HasValue)
        {
          int? nullable = size;
          int num = 8188;
          if (nullable.GetValueOrDefault() <= num & nullable.HasValue)
            return size.Value;
        }
        return 8188;
      }
      if (size.HasValue)
      {
        int? nullable = size;
        int num = 8188;
        if (nullable.GetValueOrDefault() <= num & nullable.HasValue)
          return size.Value;
      }
      return 8188;
    }

    public override RelationalTypeMapping Clone(string storeType, int? size)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters = base.Parameters;
      return (RelationalTypeMapping) new DmStringTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).WithStoreTypeAndSize(storeType, size, new StoreTypePostfix?(DmStringTypeMapping.GetStoreTypePostfix(this._storeTypePostfix, ((RelationalTypeMapping) this).IsUnicode, size))));
    }

    public override CoreTypeMapping Clone(ValueConverter converter)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters = base.Parameters;
      return (CoreTypeMapping) new DmStringTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).WithComposedConverter(converter));
    }

    protected override void ConfigureParameter(DbParameter parameter)
    {
      object obj = parameter.Value;
      int? nullable1 = obj is string str ? new int?(str.Length) : new int?();
      DbParameter dbParameter = parameter;
      int num;
      if (obj != null && obj != DBNull.Value)
      {
        if (nullable1.HasValue)
        {
          int? nullable2 = nullable1;
          int maxSpecificSize = this._maxSpecificSize;
          if (nullable2.GetValueOrDefault() <= maxSpecificSize & nullable2.HasValue)
            goto label_4;
        }
        num = 0;
        goto label_5;
      }
label_4:
      num = this._maxSpecificSize;
label_5:
      dbParameter.Size = num;
    }

    protected override string GenerateNonNullSqlLiteral(object value) => "'" + this.EscapeSqlLiteral((string) value) + "'";
  }
}
