using System.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmDoubleTypeMapping : DoubleTypeMapping
	{
		public DmDoubleTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
			: base(storeType, dbType)
		{
		}

		protected DmDoubleTypeMapping(RelationalTypeMappingParameters parameters)
			: base(parameters)
		{
		}

		public override RelationalTypeMapping Clone(string storeType, int? size)
		{
			return new DmDoubleTypeMapping(Parameters.WithStoreTypeAndSize(storeType, size));
		}

		public override CoreTypeMapping Clone(ValueConverter converter)
		{
			return new DmDoubleTypeMapping(Parameters.WithComposedConverter(converter));
		}

		protected override string GenerateNonNullSqlLiteral(object value)
		{
			string text = base.GenerateNonNullSqlLiteral(value);
			if (!text.Contains("E") && !text.Contains("e"))
			{
				return text + "E0";
			}
			return text;
		}
	}
}
