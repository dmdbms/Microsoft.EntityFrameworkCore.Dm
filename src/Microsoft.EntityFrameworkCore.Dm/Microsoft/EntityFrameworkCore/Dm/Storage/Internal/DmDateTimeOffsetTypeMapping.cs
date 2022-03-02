using System.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmDateTimeOffsetTypeMapping : DateTimeOffsetTypeMapping
	{
		private const string DateTimeOffsetFormatConst = "{0:yyyy-MM-dd HH:mm:ss.fffzzz}";

		protected override string SqlLiteralFormatString => "'{0:yyyy-MM-dd HH:mm:ss.fffzzz}'";

		public DmDateTimeOffsetTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = System.Data.DbType.DateTimeOffset)
			: base(storeType, dbType)
		{
		}

		protected DmDateTimeOffsetTypeMapping(RelationalTypeMappingParameters parameters)
			: base(parameters)
		{
		}

		public override RelationalTypeMapping Clone(string storeType, int? size)
		{
			return new DmDateTimeOffsetTypeMapping(Parameters.WithStoreTypeAndSize(storeType, size));
		}

		public override CoreTypeMapping Clone(ValueConverter converter)
		{
			return new DmDateTimeOffsetTypeMapping(Parameters.WithComposedConverter(converter));
		}
	}
}
