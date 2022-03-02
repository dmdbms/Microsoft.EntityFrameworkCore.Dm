using System.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmByteTypeMapping : ByteTypeMapping
	{
		public DmByteTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
			: base(storeType, dbType)
		{
		}

		protected DmByteTypeMapping(RelationalTypeMappingParameters parameters)
			: base(parameters)
		{
		}

		public override RelationalTypeMapping Clone(string storeType, int? size)
		{
			return new DmByteTypeMapping(Parameters.WithStoreTypeAndSize(storeType, size));
		}

		public override CoreTypeMapping Clone(ValueConverter converter)
		{
			return new DmByteTypeMapping(Parameters.WithComposedConverter(converter));
		}

		protected override string GenerateNonNullSqlLiteral(object value)
		{
			return "CAST(" + base.GenerateNonNullSqlLiteral(value) + " AS " + StoreType + ")";
		}
	}
}
