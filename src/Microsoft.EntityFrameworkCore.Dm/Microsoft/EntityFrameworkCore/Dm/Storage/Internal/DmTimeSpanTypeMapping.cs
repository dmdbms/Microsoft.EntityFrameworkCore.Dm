using System;
using System.Data;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmTimeSpanTypeMapping : TimeSpanTypeMapping
	{
		public DmTimeSpanTypeMapping([NotNull] string storeType, DbType? dbType = null)
			: base(storeType, dbType)
		{
		}

		protected DmTimeSpanTypeMapping(RelationalTypeMappingParameters parameters)
			: base(parameters)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


		public override RelationalTypeMapping Clone(string storeType, int? size)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			RelationalTypeMappingParameters parameters = this.Parameters;
			return (RelationalTypeMapping)(object)new DmTimeSpanTypeMapping(((RelationalTypeMappingParameters)(parameters)).WithStoreTypeAndSize(storeType, size, (StoreTypePostfix?)null));
		}

		public override CoreTypeMapping Clone(ValueConverter converter)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			RelationalTypeMappingParameters parameters = this.Parameters;
			return (CoreTypeMapping)(object)new DmTimeSpanTypeMapping(((RelationalTypeMappingParameters)(parameters)).WithComposedConverter(converter));
		}

		protected override string GenerateNonNullSqlLiteral(object value)
		{
			TimeSpan timeSpan = (TimeSpan)value;
			string text = timeSpan.Milliseconds.ToString();
			text = text.PadLeft(4 - text.Length, '0');
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 5);
			defaultInterpolatedStringHandler.AppendLiteral("INTERVAL '");
			defaultInterpolatedStringHandler.AppendFormatted(timeSpan.Days);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(timeSpan.Hours);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted(timeSpan.Minutes);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted(timeSpan.Seconds);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral("' DAY TO SECOND");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
