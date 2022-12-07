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

		public DmDateTimeOffsetTypeMapping([NotNull] string storeType, DbType? dbType = System.Data.DbType.DateTimeOffset)
			: base(storeType, dbType)
		{
		}

		protected DmDateTimeOffsetTypeMapping(RelationalTypeMappingParameters parameters)
			: base(parameters)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


		public override RelationalTypeMapping Clone(string storeType, int? size)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			RelationalTypeMappingParameters parameters = this.Parameters;
			return (RelationalTypeMapping)(object)new DmDateTimeOffsetTypeMapping(((RelationalTypeMappingParameters)(parameters)).WithStoreTypeAndSize(storeType, size, (StoreTypePostfix?)null));
		}

		public override CoreTypeMapping Clone(ValueConverter converter)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			RelationalTypeMappingParameters parameters = this.Parameters;
			return (CoreTypeMapping)(object)new DmDateTimeOffsetTypeMapping(((RelationalTypeMappingParameters)(parameters)).WithComposedConverter(converter));
		}
	}
}
