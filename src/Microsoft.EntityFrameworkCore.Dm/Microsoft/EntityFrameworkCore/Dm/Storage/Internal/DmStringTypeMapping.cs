using System;
using System.Data;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmStringTypeMapping : StringTypeMapping
	{
		private const int UnicodeMax = 8188;

		private const int AnsiMax = 8188;

		private readonly int _maxSpecificSize;

		private readonly StoreTypePostfix? _storeTypePostfix;

		public DmStringTypeMapping([NotNull] string storeType, [CanBeNull] DbType? dbType, bool unicode = false, int? size = null, bool fixedLength = false, StoreTypePostfix? storeTypePostfix = null)
			: this(new RelationalTypeMappingParameters(new CoreTypeMappingParameters(typeof(string), (ValueConverter)null, (ValueComparer)null, (ValueComparer)null, (Func<IProperty, IEntityType, ValueGenerator>)null), storeType, GetStoreTypePostfix(storeTypePostfix, unicode, size), dbType, unicode, size, fixedLength, (int?)null, (int?)null))
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			_storeTypePostfix = storeTypePostfix;
		}

		protected DmStringTypeMapping(RelationalTypeMappingParameters parameters)
			: base(parameters)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			_maxSpecificSize = CalculateSize(parameters.Unicode, parameters.Size);
		}

		private static StoreTypePostfix GetStoreTypePostfix(StoreTypePostfix? storeTypePostfix, bool unicode, int? size)
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			return (StoreTypePostfix)(((int?)storeTypePostfix) ?? ((!unicode) ? ((size.HasValue && size <= 8188) ? 1 : 0) : ((size.HasValue && size <= 8188) ? 1 : 0)));
		}

		private static int CalculateSize(bool unicode, int? size)
		{
			return (!unicode) ? ((size.HasValue && size <= 8188) ? size.Value : 8188) : ((size.HasValue && size <= 8188) ? size.Value : 8188);
		}

		public override RelationalTypeMapping Clone(string storeType, int? size)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			RelationalTypeMappingParameters parameters = this.Parameters;
			return (RelationalTypeMapping)(object)new DmStringTypeMapping(((RelationalTypeMappingParameters)(parameters)).WithStoreTypeAndSize(storeType, size, (StoreTypePostfix?)GetStoreTypePostfix(_storeTypePostfix, ((RelationalTypeMapping)this).IsUnicode, size)));
		}

		public override CoreTypeMapping Clone(ValueConverter converter)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			RelationalTypeMappingParameters parameters = this.Parameters;
			return (CoreTypeMapping)(object)new DmStringTypeMapping(((RelationalTypeMappingParameters)(parameters)).WithComposedConverter(converter));
		}

		protected override void ConfigureParameter(DbParameter parameter)
		{
			object value = parameter.Value;
			int? num = (value as string)?.Length;
			parameter.Size = ((value == null || value == DBNull.Value || (num.HasValue && num <= _maxSpecificSize)) ? _maxSpecificSize : 0);
		}

		protected override string GenerateNonNullSqlLiteral(object value)
		{
			return "'" + this.EscapeSqlLiteral((string)value) + "'";
		}
	}
}
