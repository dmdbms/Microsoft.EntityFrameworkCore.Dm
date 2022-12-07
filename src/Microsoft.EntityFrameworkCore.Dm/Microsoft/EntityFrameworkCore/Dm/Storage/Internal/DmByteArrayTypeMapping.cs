// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmByteArrayTypeMapping
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
using System.Globalization;
using System.Text;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class DmByteArrayTypeMapping : ByteArrayTypeMapping
  {
    private const int MaxSize = 8188;
    private readonly StoreTypePostfix? _storeTypePostfix;

    public DmByteArrayTypeMapping(
      [NotNull] string storeType,
      DbType? dbType = System.Data.DbType.Binary,
      int? size = null,
      bool fixedLength = false,
      ValueComparer comparer = null,
      StoreTypePostfix? storeTypePostfix = null)
      : base(new RelationalTypeMapping.RelationalTypeMappingParameters(new CoreTypeMapping.CoreTypeMappingParameters(typeof (byte[]), (ValueConverter) null, comparer, (ValueComparer) null, (Func<IProperty, IEntityType, ValueGenerator>) null), storeType, DmByteArrayTypeMapping.GetStoreTypePostfix(storeTypePostfix, size), dbType, false, size, fixedLength, new int?(), new int?()))
    {
      this._storeTypePostfix = storeTypePostfix;
    }

    private static StoreTypePostfix GetStoreTypePostfix(
      StoreTypePostfix? storeTypePostfix,
      int? size)
    {
      StoreTypePostfix? nullable1 = storeTypePostfix;
      if (nullable1.HasValue)
        return nullable1.GetValueOrDefault();
      if (size.HasValue)
      {
        int? nullable2 = size;
        int num = 8188;
        if (nullable2.GetValueOrDefault() <= num & nullable2.HasValue)
          return (StoreTypePostfix) 1;
      }
      return (StoreTypePostfix) 0;
    }

    protected DmByteArrayTypeMapping(
      RelationalTypeMapping.RelationalTypeMappingParameters parameters)
      : base(parameters)
    {
    }

    private static int CalculateSize(int? size)
    {
      if (size.HasValue)
      {
        int? nullable = size;
        int num = 8188;
        if (nullable.GetValueOrDefault() < num & nullable.HasValue)
          return size.Value;
      }
      return 8188;
    }

    public virtual RelationalTypeMapping Clone(string storeType, int? size)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters = this.Parameters;
      return (RelationalTypeMapping) new DmByteArrayTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).WithStoreTypeAndSize(storeType, size, new StoreTypePostfix?(DmByteArrayTypeMapping.GetStoreTypePostfix(this._storeTypePostfix, size))));
    }

    public virtual CoreTypeMapping Clone(ValueConverter converter)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters =Parameters;
      return (CoreTypeMapping) new DmByteArrayTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).WithComposedConverter(converter));
    }

    protected virtual void ConfigureParameter(DbParameter parameter)
    {
      object obj = parameter.Value;
      int? nullable1 = obj is byte[] numArray ? new int?(numArray.Length) : new int?();
      int size = DmByteArrayTypeMapping.CalculateSize(((RelationalTypeMapping) this).Size);
      DbParameter dbParameter = parameter;
      int num1;
      if (obj != null && obj != DBNull.Value)
      {
        if (nullable1.HasValue)
        {
          int? nullable2 = nullable1;
          int num2 = size;
          if (nullable2.GetValueOrDefault() <= num2 & nullable2.HasValue)
            goto label_4;
        }
        num1 = -1;
        goto label_5;
      }
label_4:
      num1 = size;
label_5:
      dbParameter.Size = num1;
    }

    protected virtual string GenerateNonNullSqlLiteral(object value)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("0x");
      foreach (byte num in (byte[]) value)
        stringBuilder.Append(num.ToString("X2", (IFormatProvider) CultureInfo.InvariantCulture));
      return stringBuilder.ToString();
    }
  }
}
