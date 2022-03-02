using System;
using System.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmTimeSpanTypeMapping : TimeSpanTypeMapping
	{
		public DmTimeSpanTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
			: base(storeType, dbType)
		{
		}

		protected DmTimeSpanTypeMapping(RelationalTypeMappingParameters parameters)
			: base(parameters)
		{
		}

		public override RelationalTypeMapping Clone(string storeType, int? size)
		{
			return new DmTimeSpanTypeMapping(Parameters.WithStoreTypeAndSize(storeType, size));
		}

		public override CoreTypeMapping Clone(ValueConverter converter)
		{
			return new DmTimeSpanTypeMapping(Parameters.WithComposedConverter(converter));
		}

		protected override string GenerateNonNullSqlLiteral(object value)
		{
			TimeSpan timeSpan = (TimeSpan)value;
			string text = timeSpan.Milliseconds.ToString();
			text = text.PadLeft(4 - text.Length, '0');
			return $"INTERVAL '{timeSpan.Days} {timeSpan.Hours}:{timeSpan.Minutes}:{timeSpan.Seconds}.{text}' DAY TO SECOND";
		}
	}
}
