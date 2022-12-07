using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmByteArrayTypeMapping : ByteArrayTypeMapping
	{
		private const int MaxSize = 8188;

		private readonly StoreTypePostfix? _storeTypePostfix;

		public DmByteArrayTypeMapping([NotNull] string storeType, DbType? dbType = System.Data.DbType.Binary, int? size = null, bool fixedLength = false, ValueComparer comparer = null, StoreTypePostfix? storeTypePostfix = null)
			: this(new RelationalTypeMappingParameters(new CoreTypeMappingParameters(typeof(byte[]), (ValueConverter)null, comparer, (ValueComparer)null, (Func<IProperty, IEntityType, ValueGenerator>)null), storeType, GetStoreTypePostfix(storeTypePostfix, size), dbType, false, size, fixedLength, (int?)null, (int?)null))
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			_storeTypePostfix = storeTypePostfix;
		}

		private static StoreTypePostfix GetStoreTypePostfix(StoreTypePostfix? storeTypePostfix, int? size)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			return (StoreTypePostfix)(((int?)storeTypePostfix) ?? ((size.HasValue && size <= 8188) ? 1 : 0));
		}

		protected DmByteArrayTypeMapping(RelationalTypeMappingParameters parameters)
			: base(parameters)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


		private static int CalculateSize(int? size)
		{
			return (size.HasValue && size < 8188) ? size.Value : 8188;
		}

		public override RelationalTypeMapping Clone(string storeType, int? size)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			RelationalTypeMappingParameters parameters = this.Parameters;
			return (RelationalTypeMapping)(object)new DmByteArrayTypeMapping(((RelationalTypeMappingParameters)(parameters)).WithStoreTypeAndSize(storeType, size, (StoreTypePostfix?)GetStoreTypePostfix(_storeTypePostfix, size)));
		}

		public override CoreTypeMapping Clone(ValueConverter converter)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			RelationalTypeMappingParameters parameters = this.Parameters;
			return (CoreTypeMapping)(object)new DmByteArrayTypeMapping(((RelationalTypeMappingParameters)(parameters)).WithComposedConverter(converter));
		}

		protected override void ConfigureParameter(DbParameter parameter)
		{
			object value = parameter.Value;
			int? num = (value as byte[])?.Length;
			int num2 = CalculateSize(Size);
			parameter.Size = ((value == null || value == DBNull.Value || (num.HasValue && num <= num2)) ? num2 : (-1));
		}

		protected override string GenerateNonNullSqlLiteral(object value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("0x");
			byte[] array = (byte[])value;
			foreach (byte b in array)
			{
				stringBuilder.Append(b.ToString("X2", CultureInfo.InvariantCulture));
			}
			return stringBuilder.ToString();
		}
	}
}
