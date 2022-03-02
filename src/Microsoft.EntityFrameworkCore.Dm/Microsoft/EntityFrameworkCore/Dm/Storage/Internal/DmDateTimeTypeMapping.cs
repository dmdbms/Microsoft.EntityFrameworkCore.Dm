using System.Data;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmDateTimeTypeMapping : DateTimeTypeMapping
	{
		private const string DateFormatConst = "{0:yyyy-MM-dd}";

		private const string DateTimeFormatConst = "{0:yyyy-MM-dd HH:mm:ss.ffffff}";

		protected override string SqlLiteralFormatString => (StoreType == "date") ? "'{0:yyyy-MM-dd}'" : "'{0:yyyy-MM-dd HH:mm:ss.ffffff}'";

		public DmDateTimeTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
			: base(storeType, dbType)
		{
		}

		protected DmDateTimeTypeMapping(RelationalTypeMappingParameters parameters)
			: base(parameters)
		{
		}

		protected override void ConfigureParameter(DbParameter parameter)
		{
			base.ConfigureParameter(parameter);
		}

		public override RelationalTypeMapping Clone(string storeType, int? size)
		{
			return new DmDateTimeTypeMapping(Parameters.WithStoreTypeAndSize(storeType, size));
		}

		public override CoreTypeMapping Clone(ValueConverter converter)
		{
			return new DmDateTimeTypeMapping(Parameters.WithComposedConverter(converter));
		}
	}
}
