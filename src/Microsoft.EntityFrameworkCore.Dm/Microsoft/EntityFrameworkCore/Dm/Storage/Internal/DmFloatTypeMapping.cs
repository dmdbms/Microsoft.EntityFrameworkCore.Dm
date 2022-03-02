using System.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmFloatTypeMapping : FloatTypeMapping
	{
		public DmFloatTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
			: base(storeType, dbType)
		{
		}

		protected DmFloatTypeMapping(RelationalTypeMappingParameters parameters)
			: base(parameters)
		{
		}

		public override RelationalTypeMapping Clone(string storeType, int? size)
		{
			return new DmFloatTypeMapping(Parameters.WithStoreTypeAndSize(storeType, size));
		}

		public override CoreTypeMapping Clone(ValueConverter converter)
		{
			return new DmFloatTypeMapping(Parameters.WithComposedConverter(converter));
		}

		protected override string GenerateNonNullSqlLiteral(object value)
		{
			return "CAST(" + base.GenerateNonNullSqlLiteral(value) + " AS " + StoreType + ")";
		}
	}
}
