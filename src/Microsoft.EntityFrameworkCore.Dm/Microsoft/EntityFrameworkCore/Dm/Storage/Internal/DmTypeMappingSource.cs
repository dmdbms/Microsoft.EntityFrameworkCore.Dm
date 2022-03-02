using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmTypeMappingSource : RelationalTypeMappingSource
	{
		private readonly DmByteArrayTypeMapping _rowversion = new DmByteArrayTypeMapping("BINARY(8)", DbType.Binary, 8, fixedLength: false, new ValueComparer<byte[]>((byte[] v1, byte[] v2) => StructuralComparisons.StructuralEqualityComparer.Equals(v1, v2), (byte[] v) => StructuralComparisons.StructuralEqualityComparer.GetHashCode(v), (byte[] v) => (v == null) ? null : v.ToArray()), StoreTypePostfix.None);

		private readonly IntTypeMapping _int = new IntTypeMapping("INT", DbType.Int32);

		private readonly LongTypeMapping _long = new LongTypeMapping("BIGINT", DbType.Int64);

		private readonly ShortTypeMapping _short = new ShortTypeMapping("SMALLINT", DbType.Int16);

		private readonly ByteTypeMapping _byte = new DmByteTypeMapping("TINYINT", DbType.Byte);

		private readonly BoolTypeMapping _bool = new BoolTypeMapping("BIT", DbType.Boolean);

		private readonly DmStringTypeMapping _fixedLengthUnicodeString = new DmStringTypeMapping("NCHAR", DbType.String, unicode: true, null, fixedLength: true);

		private readonly DmStringTypeMapping _variableLengthUnicodeString = new DmStringTypeMapping("NVARCHAR", null, unicode: true);

		private readonly DmStringTypeMapping _fixedLengthAnsiString = new DmStringTypeMapping("CHAR", DbType.AnsiString, unicode: false, null, fixedLength: true);

		private readonly DmStringTypeMapping _variableLengthAnsiString = new DmStringTypeMapping("VARCHAR", DbType.AnsiString);

		private readonly DmByteArrayTypeMapping _variableLengthBinary = new DmByteArrayTypeMapping("VARBINARY", DbType.Binary);

		private readonly DmByteArrayTypeMapping _fixedLengthBinary = new DmByteArrayTypeMapping("BINARY", DbType.Binary, null, fixedLength: true);

		private readonly DmDateTimeTypeMapping _date = new DmDateTimeTypeMapping("DATE", DbType.Date);

		private readonly DmDateTimeTypeMapping _datetime = new DmDateTimeTypeMapping("TIMESTAMP", DbType.DateTime);

		private readonly DmDateTimeTypeMapping _datetime2 = new DmDateTimeTypeMapping("TIMESTAMP", DbType.DateTime2);

		private readonly DoubleTypeMapping _double = new DmDoubleTypeMapping("FLOAT", DbType.Double);

		private readonly DmDateTimeOffsetTypeMapping _datetimeoffset = new DmDateTimeOffsetTypeMapping("DATETIME WITH TIME ZONE", DbType.DateTimeOffset);

		private readonly DmDateTimeOffsetTypeMapping _datetimeoffset3 = new DmDateTimeOffsetTypeMapping("DATETIME(3) WITH TIME ZONE", DbType.DateTimeOffset);

		private readonly FloatTypeMapping _real = new DmFloatTypeMapping("REAL", DbType.Single);

		private readonly GuidTypeMapping _guid = new GuidTypeMapping("VARCHAR(36)", DbType.Guid);

		private readonly DecimalTypeMapping _decimal = new DmDecimalTypeMapping("DECIMAL(29, 4)", DbType.Decimal, 29, 4);

		private readonly TimeSpanTypeMapping _intervaldt = new DmTimeSpanTypeMapping("INTERVAL DAY TO SECOND");

		private readonly DmStringTypeMapping _xml = new DmStringTypeMapping("VARCHAR(8188)", DbType.String);

		private readonly Dictionary<string, RelationalTypeMapping> _storeTypeMappings;

		private readonly Dictionary<Type, RelationalTypeMapping> _clrTypeMappings;

		private readonly HashSet<string> _disallowedMappings;

		public DmTypeMappingSource([JetBrains.Annotations.NotNull] TypeMappingSourceDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalTypeMappingSourceDependencies relationalDependencies)
			: base(dependencies, relationalDependencies)
		{
			_storeTypeMappings = new Dictionary<string, RelationalTypeMapping>(StringComparer.OrdinalIgnoreCase)
			{
				{ "bigint", _long },
				{ "binary varying", _variableLengthBinary },
				{ "binary", _fixedLengthBinary },
				{ "bit", _bool },
				{ "char varying", _variableLengthAnsiString },
				{ "char", _fixedLengthAnsiString },
				{ "character varying", _variableLengthAnsiString },
				{ "character", _fixedLengthAnsiString },
				{ "date", _date },
				{ "datetime", _datetime },
				{ "datetime2", _datetime2 },
				{ "datetime with time zone", _datetimeoffset },
				{ "timestamp with time zone", _datetimeoffset },
				{ "datetime(3) with time zone", _datetimeoffset3 },
				{ "timestamp(3) with time zone", _datetimeoffset3 },
				{ "dec", _decimal },
				{ "decimal", _decimal },
				{ "float", _double },
				{ "image", _variableLengthBinary },
				{ "int", _int },
				{ "money", _decimal },
				{ "national char varying", _variableLengthUnicodeString },
				{ "national character varying", _variableLengthUnicodeString },
				{ "national character", _fixedLengthUnicodeString },
				{ "nchar", _fixedLengthUnicodeString },
				{ "ntext", _variableLengthUnicodeString },
				{ "numeric", _decimal },
				{ "nvarchar", _variableLengthUnicodeString },
				{ "real", _real },
				{ "rowversion", _rowversion },
				{ "smalldatetime", _datetime },
				{ "smallint", _short },
				{ "smallmoney", _decimal },
				{ "text", _variableLengthAnsiString },
				{ "clob", _variableLengthAnsiString },
				{ "interval day to second", _intervaldt },
				{ "timestamp", _datetime },
				{ "tinyint", _byte },
				{ "varchar(36)", _guid },
				{ "varbinary", _variableLengthBinary },
				{ "blob", _variableLengthBinary },
				{ "varchar", _variableLengthAnsiString },
				{ "xml", _xml }
			};
			_clrTypeMappings = new Dictionary<Type, RelationalTypeMapping>
			{
				{
					typeof(int),
					_int
				},
				{
					typeof(long),
					_long
				},
				{
					typeof(DateTime),
					_datetime2
				},
				{
					typeof(Guid),
					_guid
				},
				{
					typeof(bool),
					_bool
				},
				{
					typeof(byte),
					_byte
				},
				{
					typeof(double),
					_double
				},
				{
					typeof(DateTimeOffset),
					_datetimeoffset
				},
				{
					typeof(short),
					_short
				},
				{
					typeof(float),
					_real
				},
				{
					typeof(decimal),
					_decimal
				},
				{
					typeof(TimeSpan),
					_intervaldt
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
			return FindRawMapping(mappingInfo)?.Clone(in mappingInfo);
		}

		private RelationalTypeMapping FindRawMapping(RelationalTypeMappingInfo mappingInfo)
		{
			Type clrType = mappingInfo.ClrType;
			string storeTypeName = mappingInfo.StoreTypeName;
			string storeTypeNameBase = mappingInfo.StoreTypeNameBase;
			if (storeTypeName != null)
			{
				if (clrType == typeof(float) && mappingInfo.Size.HasValue && mappingInfo.Size <= 24 && (storeTypeNameBase.Equals("float", StringComparison.OrdinalIgnoreCase) || storeTypeNameBase.Equals("double precision", StringComparison.OrdinalIgnoreCase)))
				{
					return _real;
				}
				if (_storeTypeMappings.TryGetValue(storeTypeName, out var value) || _storeTypeMappings.TryGetValue(storeTypeNameBase, out value))
				{
					return (clrType == null || value.ClrType == clrType) ? value : null;
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
					bool flag = mappingInfo.IsUnicode == false;
					bool flag2 = mappingInfo.IsFixedLength == true;
					string text = (flag ? "" : "N") + (flag2 ? "CHAR" : "VARCHAR2");
					string text2 = (flag ? "CLOB" : "NCLOB");
					int num = (flag ? 8188 : 8188);
					StoreTypePostfix? storeTypePostfix = null;
					int? num2 = mappingInfo.Size ?? ((!mappingInfo.IsKeyOrIndex) ? num : (flag ? 900 : 450));
					if (num2 > num)
					{
						num2 = null;
						storeTypePostfix = StoreTypePostfix.None;
					}
					object storeType;
					if (storeTypePostfix != StoreTypePostfix.None)
					{
						int? num3 = num2;
						storeType = text + "(" + num3 + ")";
					}
					else
					{
						storeType = "VARCHAR(8188)";
					}
					return new DmStringTypeMapping((string)storeType, flag ? new DbType?(DbType.AnsiString) : null, !flag, num2, flag2, storeTypePostfix);
				}
				if (clrType == typeof(byte[]))
				{
					if (mappingInfo.IsRowVersion == true)
					{
						return _rowversion;
					}
					int? num4 = mappingInfo.Size ?? (mappingInfo.IsKeyOrIndex ? new int?(900) : null);
					StoreTypePostfix? storeTypePostfix2 = null;
					if (num4 > 8188)
					{
						num4 = null;
						storeTypePostfix2 = StoreTypePostfix.None;
					}
					object storeType2;
					if (mappingInfo.IsFixedLength != true)
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
					return new DmByteArrayTypeMapping((string)storeType2, DbType.Binary, num4, fixedLength: false, null, storeTypePostfix2);
				}
			}
			return null;
		}
	}
}
