using System;
using System.Data;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmStringTypeMapping : StringTypeMapping
	{
		private const int UnicodeMax = 8188;

		private const int AnsiMax = 8188;

		private readonly int _maxSpecificSize;

		private readonly StoreTypePostfix? _storeTypePostfix;

		public DmStringTypeMapping([JetBrains.Annotations.NotNull] string storeType, [JetBrains.Annotations.CanBeNull] DbType? dbType, bool unicode = false, int? size = null, bool fixedLength = false, StoreTypePostfix? storeTypePostfix = null)
			: this(new RelationalTypeMappingParameters(new CoreTypeMappingParameters(typeof(string)), storeType, GetStoreTypePostfix(storeTypePostfix, unicode, size), dbType, unicode, size, fixedLength))
		{
			_storeTypePostfix = storeTypePostfix;
		}

		protected DmStringTypeMapping(RelationalTypeMappingParameters parameters)
			: base(parameters)
		{
			_maxSpecificSize = CalculateSize(parameters.Unicode, parameters.Size);
		}

		private static StoreTypePostfix GetStoreTypePostfix(StoreTypePostfix? storeTypePostfix, bool unicode, int? size)
		{
			return (StoreTypePostfix)(((int?)storeTypePostfix) ?? ((!unicode) ? ((size.HasValue && size <= 8188) ? 1 : 0) : ((size.HasValue && size <= 8188) ? 1 : 0)));
		}

		private static int CalculateSize(bool unicode, int? size)
		{
			return (!unicode) ? ((size.HasValue && size <= 8188) ? size.Value : 8188) : ((size.HasValue && size <= 8188) ? size.Value : 8188);
		}

		public override RelationalTypeMapping Clone(string storeType, int? size)
		{
			return new DmStringTypeMapping(Parameters.WithStoreTypeAndSize(storeType, size, GetStoreTypePostfix(_storeTypePostfix, IsUnicode, size)));
		}

		public override CoreTypeMapping Clone(ValueConverter converter)
		{
			return new DmStringTypeMapping(Parameters.WithComposedConverter(converter));
		}

		protected override void ConfigureParameter(DbParameter parameter)
		{
			object value = parameter.Value;
			int? num = (value as string)?.Length;
			parameter.Size = ((value == null || value == DBNull.Value || (num.HasValue && num <= _maxSpecificSize)) ? _maxSpecificSize : 0);
		}

		protected override string GenerateNonNullSqlLiteral(object value)
		{
			return "'" + EscapeSqlLiteral((string)value) + "'";
		}
	}
}
