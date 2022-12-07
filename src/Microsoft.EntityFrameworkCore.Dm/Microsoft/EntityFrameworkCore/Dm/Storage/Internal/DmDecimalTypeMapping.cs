// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmDecimalTypeMapping
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
using System.Data.SqlTypes;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class DmDecimalTypeMapping : DecimalTypeMapping
    {
        public DmDecimalTypeMapping([NotNull] string storeType, DbType? dbType = null, int? precision = null, int? scale = null):base(storeType,dbType,precision,scale)
        {/*
            CoreTypeMapping.CoreTypeMappingParameters mappingParameters = new CoreTypeMapping.CoreTypeMappingParameters(typeof (Decimal), (ValueConverter) null, (ValueComparer) null, (ValueComparer) null, (Func<IProperty, IEntityType, ValueGenerator>) null);
            string str = storeType;
            DbType? nullable1 = dbType;
            int? nullable2 = precision;
            int? nullable3 = scale;
            int? nullable4 = new int?();
            int? nullable5 = nullable2;
            int? nullable6 = nullable3;
            // ISSUE: explicit constructor call
      base(new RelationalTypeMapping.RelationalTypeMappingParameters(mappingParameters, str, (StoreTypePostfix) 3, nullable1, false, nullable4, false, nullable5, nullable6));*/
        }
        protected DmDecimalTypeMapping(
        RelationalTypeMapping.RelationalTypeMappingParameters parameters)
      : base(parameters)
    {
    }

    public override RelationalTypeMapping Clone(string storeType, int? size)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters = base.Parameters;
      return (RelationalTypeMapping) new DmDecimalTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).WithStoreTypeAndSize(storeType, size, new StoreTypePostfix?()));
    }

    public override CoreTypeMapping Clone(ValueConverter converter)
    {
      RelationalTypeMapping.RelationalTypeMappingParameters parameters = base.Parameters;
      return (CoreTypeMapping) new DmDecimalTypeMapping(((RelationalTypeMapping.RelationalTypeMappingParameters)  parameters).WithComposedConverter(converter));
    }
  }
}
