using System.Data;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmFloatTypeMapping : FloatTypeMapping
	{
		public DmFloatTypeMapping([NotNull] string storeType, DbType? dbType = null)
			: base(storeType, dbType)
		{
		}

		protected DmFloatTypeMapping(RelationalTypeMappingParameters parameters)
			: base(parameters)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


		public override RelationalTypeMapping Clone(string storeType, int? size)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			RelationalTypeMappingParameters parameters = this.Parameters;
			return (RelationalTypeMapping)(object)new DmFloatTypeMapping(((RelationalTypeMappingParameters)(parameters)).WithStoreTypeAndSize(storeType, size, (StoreTypePostfix?)null));
		}

		public override CoreTypeMapping Clone(ValueConverter converter)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			RelationalTypeMappingParameters parameters = this.Parameters;
			return (CoreTypeMapping)(object)new DmFloatTypeMapping(((RelationalTypeMappingParameters)(parameters)).WithComposedConverter(converter));
		}

		protected override string GenerateNonNullSqlLiteral(object value)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 2);
			defaultInterpolatedStringHandler.AppendLiteral("CAST(");
			defaultInterpolatedStringHandler.AppendFormatted(base.GenerateNonNullSqlLiteral(value));
			defaultInterpolatedStringHandler.AppendLiteral(" AS ");
			defaultInterpolatedStringHandler.AppendFormatted(base.StoreType);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
