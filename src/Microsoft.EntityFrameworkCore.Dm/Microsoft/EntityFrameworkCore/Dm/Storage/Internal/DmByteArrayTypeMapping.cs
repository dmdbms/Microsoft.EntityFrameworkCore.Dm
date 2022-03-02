using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmByteArrayTypeMapping : ByteArrayTypeMapping
	{
		private const int MaxSize = 8188;

		private readonly StoreTypePostfix? _storeTypePostfix;

		public DmByteArrayTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = System.Data.DbType.Binary, int? size = null, bool fixedLength = false, ValueComparer comparer = null, StoreTypePostfix? storeTypePostfix = null)
			: base(new RelationalTypeMappingParameters(new CoreTypeMappingParameters(typeof(byte[]), null, comparer), storeType, GetStoreTypePostfix(storeTypePostfix, size), dbType, unicode: false, size, fixedLength))
		{
			_storeTypePostfix = storeTypePostfix;
		}

		private static StoreTypePostfix GetStoreTypePostfix(StoreTypePostfix? storeTypePostfix, int? size)
		{
			return (StoreTypePostfix)(((int?)storeTypePostfix) ?? ((size.HasValue && size <= 8188) ? 1 : 0));
		}

		protected DmByteArrayTypeMapping(RelationalTypeMappingParameters parameters)
			: base(parameters)
		{
		}

		private static int CalculateSize(int? size)
		{
			return (size.HasValue && size < 8188) ? size.Value : 8188;
		}

		public override RelationalTypeMapping Clone(string storeType, int? size)
		{
			return new DmByteArrayTypeMapping(Parameters.WithStoreTypeAndSize(storeType, size, GetStoreTypePostfix(_storeTypePostfix, size)));
		}

		public override CoreTypeMapping Clone(ValueConverter converter)
		{
			return new DmByteArrayTypeMapping(Parameters.WithComposedConverter(converter));
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
