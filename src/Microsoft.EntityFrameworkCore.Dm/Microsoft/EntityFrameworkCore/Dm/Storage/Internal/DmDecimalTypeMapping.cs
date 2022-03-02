using System.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmDecimalTypeMapping : DecimalTypeMapping
	{
		public DmDecimalTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null, int? precision = null, int? scale = null)
			: base(new RelationalTypeMappingParameters(new CoreTypeMappingParameters(typeof(decimal)), storeType, StoreTypePostfix.PrecisionAndScale, dbType, unicode: false, null, fixedLength: false, precision, scale))
		{
		}

		protected DmDecimalTypeMapping(RelationalTypeMappingParameters parameters)
			: base(parameters)
		{
		}

		public override RelationalTypeMapping Clone(string storeType, int? size)
		{
			return new DmDecimalTypeMapping(Parameters.WithStoreTypeAndSize(storeType, size));
		}

		public override CoreTypeMapping Clone(ValueConverter converter)
		{
			return new DmDecimalTypeMapping(Parameters.WithComposedConverter(converter));
		}
	}
}
