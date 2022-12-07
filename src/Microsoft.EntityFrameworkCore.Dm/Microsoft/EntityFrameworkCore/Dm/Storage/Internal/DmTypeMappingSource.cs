using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmTypeMappingSource : RelationalTypeMappingSource
	{
		private readonly DmByteArrayTypeMapping _rowversion = new DmByteArrayTypeMapping("BINARY(8)", DbType.Binary, 8, fixedLength: false, (ValueComparer)(object)new ValueComparer<byte[]>((Expression<Func<byte[], byte[], bool>>)((byte[] v1, byte[] v2) => StructuralComparisons.StructuralEqualityComparer.Equals(v1, v2)), (Expression<Func<byte[], int>>)((byte[] v) => StructuralComparisons.StructuralEqualityComparer.GetHashCode(v)), (Expression<Func<byte[], byte[]>>)((byte[] v) => (v == null) ? null : v.ToArray())), (StoreTypePostfix)0);

		private readonly IntTypeMapping _int = new IntTypeMapping("INT", (DbType?)DbType.Int32);

		private readonly LongTypeMapping _long = new LongTypeMapping("BIGINT", (DbType?)DbType.Int64);

		private readonly ShortTypeMapping _short = new ShortTypeMapping("SMALLINT", (DbType?)DbType.Int16);

		private readonly ByteTypeMapping _byte = (ByteTypeMapping)(object)new DmByteTypeMapping("TINYINT", DbType.Byte);

		private readonly BoolTypeMapping _bool = new BoolTypeMapping("BIT", (DbType?)DbType.Boolean);

		private readonly DmStringTypeMapping _fixedLengthUnicodeString = new DmStringTypeMapping("NCHAR", DbType.String, unicode: true, null, fixedLength: true);

		private readonly DmStringTypeMapping _variableLengthUnicodeString = new DmStringTypeMapping("NVARCHAR", null, unicode: true);

		private readonly DmStringTypeMapping _fixedLengthAnsiString = new DmStringTypeMapping("CHAR", DbType.AnsiString, unicode: false, null, fixedLength: true);

		private readonly DmStringTypeMapping _variableLengthAnsiString = new DmStringTypeMapping("VARCHAR", DbType.AnsiString);

		private readonly DmByteArrayTypeMapping _variableLengthBinary = new DmByteArrayTypeMapping("VARBINARY", DbType.Binary);

		private readonly DmByteArrayTypeMapping _fixedLengthBinary = new DmByteArrayTypeMapping("BINARY", DbType.Binary, null, fixedLength: true);

		private readonly DmDateTimeTypeMapping _date = new DmDateTimeTypeMapping("DATE", DbType.Date);

		private readonly DmDateTimeTypeMapping _datetime = new DmDateTimeTypeMapping("TIMESTAMP", DbType.DateTime);

		private readonly DmDateTimeTypeMapping _datetime2 = new DmDateTimeTypeMapping("TIMESTAMP", DbType.DateTime2);

		private readonly DoubleTypeMapping _double = (DoubleTypeMapping)(object)new DmDoubleTypeMapping("FLOAT", DbType.Double);

		private readonly DmDateTimeOffsetTypeMapping _datetimeoffset = new DmDateTimeOffsetTypeMapping("DATETIME WITH TIME ZONE", DbType.DateTimeOffset);

		private readonly DmDateTimeOffsetTypeMapping _datetimeoffset3 = new DmDateTimeOffsetTypeMapping("DATETIME(3) WITH TIME ZONE", DbType.DateTimeOffset);

		private readonly FloatTypeMapping _real = (FloatTypeMapping)(object)new DmFloatTypeMapping("REAL", DbType.Single);

		private readonly GuidTypeMapping _guid = new GuidTypeMapping("VARCHAR(36)", (DbType?)DbType.Guid);

		private readonly DecimalTypeMapping _decimal = (DecimalTypeMapping)(object)new DmDecimalTypeMapping("DECIMAL(29, 4)", DbType.Decimal, 29, 4);

		private readonly TimeSpanTypeMapping _intervaldt = (TimeSpanTypeMapping)(object)new DmTimeSpanTypeMapping("INTERVAL DAY TO SECOND");

		private readonly DmStringTypeMapping _xml = new DmStringTypeMapping("VARCHAR(8188)", DbType.String);

		private readonly Dictionary<string, RelationalTypeMapping> _storeTypeMappings;

		private readonly Dictionary<Type, RelationalTypeMapping> _clrTypeMappings;

		private readonly HashSet<string> _disallowedMappings;

		public DmTypeMappingSource([NotNull] TypeMappingSourceDependencies dependencies, [NotNull] RelationalTypeMappingSourceDependencies relationalDependencies)
			: base(dependencies, relationalDependencies)
		{
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Expected O, but got Unknown
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Expected O, but got Unknown
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Expected O, but got Unknown
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Expected O, but got Unknown
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Expected O, but got Unknown
			_storeTypeMappings = new Dictionary<string, RelationalTypeMapping>(StringComparer.OrdinalIgnoreCase)
			{
				{
					"bigint",
					(RelationalTypeMapping)(object)_long
				},
				{
					"binary varying",
					(RelationalTypeMapping)(object)_variableLengthBinary
				},
				{
					"binary",
					(RelationalTypeMapping)(object)_fixedLengthBinary
				},
				{
					"bit",
					(RelationalTypeMapping)(object)_bool
				},
				{
					"char varying",
					(RelationalTypeMapping)(object)_variableLengthAnsiString
				},
				{
					"char",
					(RelationalTypeMapping)(object)_fixedLengthAnsiString
				},
				{
					"character varying",
					(RelationalTypeMapping)(object)_variableLengthAnsiString
				},
				{
					"character",
					(RelationalTypeMapping)(object)_fixedLengthAnsiString
				},
				{
					"date",
					(RelationalTypeMapping)(object)_date
				},
				{
					"datetime",
					(RelationalTypeMapping)(object)_datetime
				},
				{
					"datetime2",
					(RelationalTypeMapping)(object)_datetime2
				},
				{
					"datetime with time zone",
					(RelationalTypeMapping)(object)_datetimeoffset
				},
				{
					"timestamp with time zone",
					(RelationalTypeMapping)(object)_datetimeoffset
				},
				{
					"datetime(3) with time zone",
					(RelationalTypeMapping)(object)_datetimeoffset3
				},
				{
					"timestamp(3) with time zone",
					(RelationalTypeMapping)(object)_datetimeoffset3
				},
				{
					"dec",
					(RelationalTypeMapping)(object)_decimal
				},
				{
					"decimal",
					(RelationalTypeMapping)(object)_decimal
				},
				{
					"float",
					(RelationalTypeMapping)(object)_double
				},
				{
					"image",
					(RelationalTypeMapping)(object)_variableLengthBinary
				},
				{
					"int",
					(RelationalTypeMapping)(object)_int
				},
				{
					"money",
					(RelationalTypeMapping)(object)_decimal
				},
				{
					"national char varying",
					(RelationalTypeMapping)(object)_variableLengthUnicodeString
				},
				{
					"national character varying",
					(RelationalTypeMapping)(object)_variableLengthUnicodeString
				},
				{
					"national character",
					(RelationalTypeMapping)(object)_fixedLengthUnicodeString
				},
				{
					"nchar",
					(RelationalTypeMapping)(object)_fixedLengthUnicodeString
				},
				{
					"ntext",
					(RelationalTypeMapping)(object)_variableLengthUnicodeString
				},
				{
					"numeric",
					(RelationalTypeMapping)(object)_decimal
				},
				{
					"nvarchar",
					(RelationalTypeMapping)(object)_variableLengthUnicodeString
				},
				{
					"real",
					(RelationalTypeMapping)(object)_real
				},
				{
					"rowversion",
					(RelationalTypeMapping)(object)_rowversion
				},
				{
					"smalldatetime",
					(RelationalTypeMapping)(object)_datetime
				},
				{
					"smallint",
					(RelationalTypeMapping)(object)_short
				},
				{
					"smallmoney",
					(RelationalTypeMapping)(object)_decimal
				},
				{
					"text",
					(RelationalTypeMapping)(object)_variableLengthAnsiString
				},
				{
					"clob",
					(RelationalTypeMapping)(object)_variableLengthAnsiString
				},
				{
					"interval day to second",
					(RelationalTypeMapping)(object)_intervaldt
				},
				{
					"timestamp",
					(RelationalTypeMapping)(object)_datetime
				},
				{
					"tinyint",
					(RelationalTypeMapping)(object)_byte
				},
				{
					"varchar(36)",
					(RelationalTypeMapping)(object)_guid
				},
				{
					"varbinary",
					(RelationalTypeMapping)(object)_variableLengthBinary
				},
				{
					"blob",
					(RelationalTypeMapping)(object)_variableLengthBinary
				},
				{
					"varchar",
					(RelationalTypeMapping)(object)_variableLengthAnsiString
				},
				{
					"xml",
					(RelationalTypeMapping)(object)_xml
				}
			};
			_clrTypeMappings = new Dictionary<Type, RelationalTypeMapping>
			{
				{
					typeof(int),
					(RelationalTypeMapping)(object)_int
				},
				{
					typeof(long),
					(RelationalTypeMapping)(object)_long
				},
				{
					typeof(DateTime),
					(RelationalTypeMapping)(object)_datetime2
				},
				{
					typeof(Guid),
					(RelationalTypeMapping)(object)_guid
				},
				{
					typeof(bool),
					(RelationalTypeMapping)(object)_bool
				},
				{
					typeof(byte),
					(RelationalTypeMapping)(object)_byte
				},
				{
					typeof(double),
					(RelationalTypeMapping)(object)_double
				},
				{
					typeof(DateTimeOffset),
					(RelationalTypeMapping)(object)_datetimeoffset
				},
				{
					typeof(short),
					(RelationalTypeMapping)(object)_short
				},
				{
					typeof(float),
					(RelationalTypeMapping)(object)_real
				},
				{
					typeof(decimal),
					(RelationalTypeMapping)(object)_decimal
				},
				{
					typeof(TimeSpan),
					(RelationalTypeMapping)(object)_intervaldt
				}
			};
			_disallowedMappings = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
			{
				"binary varying", "binary", "char varying", "char", "character varying", "character", "national char varying", "national character varying", "national character", "nchar",
				"nvarchar", "varbinary", "varchar"
			};
		}

		protected override RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			RelationalTypeMapping obj = FindRawMapping(mappingInfo);
			return (obj != null) ? obj.Clone(mappingInfo) : null;
		}

		private RelationalTypeMapping FindRawMapping(RelationalTypeMappingInfo mappingInfo)
		{
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			Type clrType = ((RelationalTypeMappingInfo)(mappingInfo)).ClrType;
			string storeTypeName = ((RelationalTypeMappingInfo)(mappingInfo)).StoreTypeName;
			string storeTypeNameBase = ((RelationalTypeMappingInfo)(mappingInfo)).StoreTypeNameBase;
			if (storeTypeName != null)
			{
				if (clrType == typeof(float) && ((RelationalTypeMappingInfo)(mappingInfo)).Size.HasValue && ((RelationalTypeMappingInfo)(mappingInfo)).Size <= 24 && (storeTypeNameBase.Equals("float", StringComparison.OrdinalIgnoreCase) || storeTypeNameBase.Equals("double precision", StringComparison.OrdinalIgnoreCase)))
				{
					return (RelationalTypeMapping)(object)_real;
				}
				if (_storeTypeMappings.TryGetValue(storeTypeName, out var value) || _storeTypeMappings.TryGetValue(storeTypeNameBase, out value))
				{
					return (clrType == null || ((CoreTypeMapping)value).ClrType == clrType) ? value : null;
				}
			}
			if (clrType != null)
			{
				if (_clrTypeMappings.TryGetValue(clrType, out var value2))
				{
					return value2;
				}
				if (clrType == typeof(string))
				{
					bool flag = ((RelationalTypeMappingInfo)(mappingInfo)).IsUnicode == false;
					bool flag2 = ((RelationalTypeMappingInfo)(mappingInfo)).IsFixedLength == true;
					string text = (flag ? "" : "N") + (flag2 ? "CHAR" : "VARCHAR2");
					string text2 = (flag ? "CLOB" : "NCLOB");
					int num = (flag ? 8188 : 8188);
					StoreTypePostfix? val = null;
					int? num2 = ((RelationalTypeMappingInfo)(mappingInfo)).Size ?? ((!((RelationalTypeMappingInfo)(mappingInfo)).IsKeyOrIndex) ? num : (flag ? 900 : 450));
					if (num2 > num)
					{
						num2 = null;
						val = (StoreTypePostfix)0;
					}
					object storeType;
					if (val != (StoreTypePostfix?)0)
					{
						int? num3 = num2;
						storeType = text + "(" + num3 + ")";
					}
					else
					{
						storeType = "VARCHAR(8188)";
					}
					return (RelationalTypeMapping)(object)new DmStringTypeMapping((string)storeType, flag ? new DbType?(DbType.AnsiString) : null, !flag, num2, flag2, val);
				}
				if (clrType == typeof(byte[]))
				{
					if (((RelationalTypeMappingInfo)(mappingInfo)).IsRowVersion == true)
					{
						return (RelationalTypeMapping)(object)_rowversion;
					}
					int? num4 = ((RelationalTypeMappingInfo)(mappingInfo)).Size ?? (((RelationalTypeMappingInfo)(mappingInfo)).IsKeyOrIndex ? new int?(900) : null);
					StoreTypePostfix? storeTypePostfix = null;
					if (num4 > 8188)
					{
						num4 = null;
						storeTypePostfix = (StoreTypePostfix)0;
					}
					object storeType2;
					if (((RelationalTypeMappingInfo)(mappingInfo)).IsFixedLength != true)
					{
						object obj;
						if (num4 != -1 && num4.HasValue)
						{
							int? num3 = num4;
							num3 = num3;
							obj = num3 + ")";
						}
						else
						{
							obj = "8188)";
						}
						storeType2 = "VARBINARY(" + (string?)obj;
					}
					else
					{
						storeType2 = "BINARY(";
					}
					return (RelationalTypeMapping)(object)new DmByteArrayTypeMapping((string)storeType2, DbType.Binary, num4, fixedLength: false, null, storeTypePostfix);
				}
			}
			return null;
		}
	}
}
