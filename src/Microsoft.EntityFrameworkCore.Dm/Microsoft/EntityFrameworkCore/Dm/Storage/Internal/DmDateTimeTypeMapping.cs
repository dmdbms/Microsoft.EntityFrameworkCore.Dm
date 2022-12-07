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

		protected override string SqlLiteralFormatString => (((RelationalTypeMapping)this).StoreType == "date") ? "'{0:yyyy-MM-dd}'" : "'{0:yyyy-MM-dd HH:mm:ss.ffffff}'";

		public DmDateTimeTypeMapping([NotNull] string storeType, DbType? dbType = null)
			: base(storeType, dbType)
		{
		}

		protected DmDateTimeTypeMapping(RelationalTypeMappingParameters parameters)
			: base(parameters)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


		protected override void ConfigureParameter(DbParameter parameter)
		{
			base.ConfigureParameter(parameter);
		}

		public override RelationalTypeMapping Clone(string storeType, int? size)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			RelationalTypeMappingParameters parameters =this.Parameters;
			return (RelationalTypeMapping)(object)new DmDateTimeTypeMapping(((RelationalTypeMappingParameters)( parameters)).WithStoreTypeAndSize(storeType, size, (StoreTypePostfix?)null));
		}

		public override CoreTypeMapping Clone(ValueConverter converter)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			RelationalTypeMappingParameters parameters = this.Parameters;
			return (CoreTypeMapping)(object)new DmDateTimeTypeMapping(((RelationalTypeMappingParameters)( parameters)).WithComposedConverter(converter));
		}
	}
}
