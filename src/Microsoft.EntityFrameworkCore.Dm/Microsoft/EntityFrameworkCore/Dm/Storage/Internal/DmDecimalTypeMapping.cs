using System;
using System.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmDecimalTypeMapping : DecimalTypeMapping
	{
		public DmDecimalTypeMapping([NotNull] string storeType, DbType? dbType = null, int? precision = null, int? scale = null)
			: this(new RelationalTypeMappingParameters(new CoreTypeMappingParameters(typeof(decimal), (ValueConverter)null, (ValueComparer)null, (ValueComparer)null, (Func<IProperty, IEntityType, ValueGenerator>)null), storeType, (StoreTypePostfix)3, dbType, false, (int?)null, false, precision, scale))
		{
		}//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)


		protected DmDecimalTypeMapping(RelationalTypeMappingParameters parameters)
			: base(parameters)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


		public override RelationalTypeMapping Clone(string storeType, int? size)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			RelationalTypeMappingParameters parameters = this.Parameters;
			return (RelationalTypeMapping)(object)new DmDecimalTypeMapping(((RelationalTypeMappingParameters)(parameters)).WithStoreTypeAndSize(storeType, size, (StoreTypePostfix?)null));
		}

		public override CoreTypeMapping Clone(ValueConverter converter)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			RelationalTypeMappingParameters parameters = this.Parameters;
			return (CoreTypeMapping)(object)new DmDecimalTypeMapping(((RelationalTypeMappingParameters)(parameters)).WithComposedConverter(converter));
		}
	}
}
