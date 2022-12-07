// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmTypeMappingSource
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class DmTypeMappingSource : RelationalTypeMappingSource
  {
    private readonly DmByteArrayTypeMapping _rowversion = new DmByteArrayTypeMapping("BINARY(8)", size: new int?(8), comparer: ((ValueComparer) new ValueComparer<byte[]>((Expression<Func<byte[], byte[], bool>>) ((v1, v2) => StructuralComparisons.StructuralEqualityComparer.Equals(v1, v2)), (Expression<Func<byte[], int>>) (v => StructuralComparisons.StructuralEqualityComparer.GetHashCode(v)), (Expression<Func<byte[], byte[]>>) (v => v == default (object) ? default (byte[]) : v.ToArray<byte>()))), storeTypePostfix: new StoreTypePostfix?((StoreTypePostfix) 0));
    private readonly IntTypeMapping _int = new IntTypeMapping("INT", new DbType?(DbType.Int32));
    private readonly LongTypeMapping _long = new LongTypeMapping("BIGINT", new DbType?(DbType.Int64));
    private readonly ShortTypeMapping _short = new ShortTypeMapping("SMALLINT", new DbType?(DbType.Int16));
    private readonly ByteTypeMapping _byte = (ByteTypeMapping) new DmByteTypeMapping("TINYINT", new DbType?(DbType.Byte));
    private readonly BoolTypeMapping _bool = new BoolTypeMapping("BIT", new DbType?(DbType.Boolean));
    private readonly DmStringTypeMapping _fixedLengthUnicodeString = new DmStringTypeMapping("NCHAR", new DbType?(DbType.String), true, fixedLength: true);
    private readonly DmStringTypeMapping _variableLengthUnicodeString = new DmStringTypeMapping("NVARCHAR", new DbType?(), true);
    private readonly DmStringTypeMapping _fixedLengthAnsiString = new DmStringTypeMapping("CHAR", new DbType?(DbType.AnsiString), fixedLength: true);
    private readonly DmStringTypeMapping _variableLengthAnsiString = new DmStringTypeMapping("VARCHAR", new DbType?(DbType.AnsiString));
    private readonly DmByteArrayTypeMapping _variableLengthBinary = new DmByteArrayTypeMapping("VARBINARY");
    private readonly DmByteArrayTypeMapping _fixedLengthBinary = new DmByteArrayTypeMapping("BINARY", fixedLength: true);
    private readonly DmDateTimeTypeMapping _date = new DmDateTimeTypeMapping("DATE", new DbType?(DbType.Date));
    private readonly DmDateTimeTypeMapping _datetime = new DmDateTimeTypeMapping("TIMESTAMP", new DbType?(DbType.DateTime));
    private readonly DmDateTimeTypeMapping _datetime2 = new DmDateTimeTypeMapping("TIMESTAMP", new DbType?(DbType.DateTime2));
    private readonly DoubleTypeMapping _double = (DoubleTypeMapping) new DmDoubleTypeMapping("FLOAT", new DbType?(DbType.Double));
    private readonly DmDateTimeOffsetTypeMapping _datetimeoffset = new DmDateTimeOffsetTypeMapping("DATETIME WITH TIME ZONE");
    private readonly DmDateTimeOffsetTypeMapping _datetimeoffset3 = new DmDateTimeOffsetTypeMapping("DATETIME(3) WITH TIME ZONE");
    private readonly FloatTypeMapping _real = (FloatTypeMapping) new DmFloatTypeMapping("REAL", new DbType?(DbType.Single));
    private readonly GuidTypeMapping _guid = new GuidTypeMapping("VARCHAR(36)", new DbType?(DbType.Guid));
    private readonly DecimalTypeMapping _decimal = (DecimalTypeMapping) new DmDecimalTypeMapping("DECIMAL(29, 4)", new DbType?(DbType.Decimal), new int?(29), new int?(4));
    private readonly TimeSpanTypeMapping _intervaldt = (TimeSpanTypeMapping) new DmTimeSpanTypeMapping("INTERVAL DAY TO SECOND");
    private readonly DmStringTypeMapping _xml = new DmStringTypeMapping("VARCHAR(8188)", new DbType?(DbType.String));
    private readonly Dictionary<string, RelationalTypeMapping> _storeTypeMappings;
    private readonly Dictionary<Type, RelationalTypeMapping> _clrTypeMappings;
    private readonly HashSet<string> _disallowedMappings;

    public DmTypeMappingSource(
      [NotNull] TypeMappingSourceDependencies dependencies,
      [NotNull] RelationalTypeMappingSourceDependencies relationalDependencies)
      : base(dependencies, relationalDependencies)
    {
      Dictionary<string, RelationalTypeMapping> dictionary1 = new Dictionary<string, RelationalTypeMapping>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      dictionary1.Add("bigint", (RelationalTypeMapping) this._long);
      dictionary1.Add("binary varying", (RelationalTypeMapping) this._variableLengthBinary);
      dictionary1.Add("binary", (RelationalTypeMapping) this._fixedLengthBinary);
      dictionary1.Add("bit", (RelationalTypeMapping) this._bool);
      dictionary1.Add("char varying", (RelationalTypeMapping) this._variableLengthAnsiString);
      dictionary1.Add("char", (RelationalTypeMapping) this._fixedLengthAnsiString);
      dictionary1.Add("character varying", (RelationalTypeMapping) this._variableLengthAnsiString);
      dictionary1.Add("character", (RelationalTypeMapping) this._fixedLengthAnsiString);
      dictionary1.Add("date", (RelationalTypeMapping) this._date);
      dictionary1.Add("datetime", (RelationalTypeMapping) this._datetime);
      dictionary1.Add("datetime2", (RelationalTypeMapping) this._datetime2);
      dictionary1.Add("datetime with time zone", (RelationalTypeMapping) this._datetimeoffset);
      dictionary1.Add("timestamp with time zone", (RelationalTypeMapping) this._datetimeoffset);
      dictionary1.Add("datetime(3) with time zone", (RelationalTypeMapping) this._datetimeoffset3);
      dictionary1.Add("timestamp(3) with time zone", (RelationalTypeMapping) this._datetimeoffset3);
      dictionary1.Add("dec", (RelationalTypeMapping) this._decimal);
      dictionary1.Add("decimal", (RelationalTypeMapping) this._decimal);
      dictionary1.Add("float", (RelationalTypeMapping) this._double);
      dictionary1.Add("image", (RelationalTypeMapping) this._variableLengthBinary);
      dictionary1.Add("int", (RelationalTypeMapping) this._int);
      dictionary1.Add("money", (RelationalTypeMapping) this._decimal);
      dictionary1.Add("national char varying", (RelationalTypeMapping) this._variableLengthUnicodeString);
      dictionary1.Add("national character varying", (RelationalTypeMapping) this._variableLengthUnicodeString);
      dictionary1.Add("national character", (RelationalTypeMapping) this._fixedLengthUnicodeString);
      dictionary1.Add("nchar", (RelationalTypeMapping) this._fixedLengthUnicodeString);
      dictionary1.Add("ntext", (RelationalTypeMapping) this._variableLengthUnicodeString);
      dictionary1.Add("numeric", (RelationalTypeMapping) this._decimal);
      dictionary1.Add("nvarchar", (RelationalTypeMapping) this._variableLengthUnicodeString);
      dictionary1.Add("real", (RelationalTypeMapping) this._real);
      dictionary1.Add("rowversion", (RelationalTypeMapping) this._rowversion);
      dictionary1.Add("smalldatetime", (RelationalTypeMapping) this._datetime);
      dictionary1.Add("smallint", (RelationalTypeMapping) this._short);
      dictionary1.Add("smallmoney", (RelationalTypeMapping) this._decimal);
      dictionary1.Add("text", (RelationalTypeMapping) this._variableLengthAnsiString);
      dictionary1.Add("clob", (RelationalTypeMapping) this._variableLengthAnsiString);
      dictionary1.Add("interval day to second", (RelationalTypeMapping) this._intervaldt);
      dictionary1.Add("timestamp", (RelationalTypeMapping) this._datetime);
      dictionary1.Add("tinyint", (RelationalTypeMapping) this._byte);
      dictionary1.Add("varchar(36)", (RelationalTypeMapping) this._guid);
      dictionary1.Add("varbinary", (RelationalTypeMapping) this._variableLengthBinary);
      dictionary1.Add("blob", (RelationalTypeMapping) this._variableLengthBinary);
      dictionary1.Add("varchar", (RelationalTypeMapping) this._variableLengthAnsiString);
      dictionary1.Add("xml", (RelationalTypeMapping) this._xml);
      this._storeTypeMappings = dictionary1;
      Dictionary<Type, RelationalTypeMapping> dictionary2 = new Dictionary<Type, RelationalTypeMapping>();
      dictionary2.Add(typeof (int), (RelationalTypeMapping) this._int);
      dictionary2.Add(typeof (long), (RelationalTypeMapping) this._long);
      dictionary2.Add(typeof (DateTime), (RelationalTypeMapping) this._datetime2);
      dictionary2.Add(typeof (Guid), (RelationalTypeMapping) this._guid);
      dictionary2.Add(typeof (bool), (RelationalTypeMapping) this._bool);
      dictionary2.Add(typeof (byte), (RelationalTypeMapping) this._byte);
      dictionary2.Add(typeof (double), (RelationalTypeMapping) this._double);
      dictionary2.Add(typeof (DateTimeOffset), (RelationalTypeMapping) this._datetimeoffset);
      dictionary2.Add(typeof (short), (RelationalTypeMapping) this._short);
      dictionary2.Add(typeof (float), (RelationalTypeMapping) this._real);
      dictionary2.Add(typeof (Decimal), (RelationalTypeMapping) this._decimal);
      dictionary2.Add(typeof (TimeSpan), (RelationalTypeMapping) this._intervaldt);
      this._clrTypeMappings = dictionary2;
      this._disallowedMappings = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
      {
        "binary varying",
        "binary",
        "char varying",
        "char",
        "character varying",
        "character",
        "national char varying",
        "national character varying",
        "national character",
        "nchar",
        "nvarchar",
        "varbinary",
        "varchar"
      };
    }

    protected virtual RelationalTypeMapping FindMapping(
      in RelationalTypeMappingInfo mappingInfo)
    {
      return this.FindRawMapping(mappingInfo)?.Clone( mappingInfo);
    }

        private RelationalTypeMapping FindRawMapping(RelationalTypeMappingInfo mappingInfo)
        {
            //IL_0244: Unknown result type (might be due to invalid IL or missing references)
            //IL_0248: Unknown result type (might be due to invalid IL or missing references)
            //IL_024d: Unknown result type (might be due to invalid IL or missing references)
            Type clrType = ((RelationalTypeMappingInfo)( mappingInfo)).ClrType;
            string storeTypeName = ((RelationalTypeMappingInfo)( mappingInfo)).StoreTypeName;
            string storeTypeNameBase = ((RelationalTypeMappingInfo)( mappingInfo)).StoreTypeNameBase;
            if (storeTypeName != null)
            {
                if (clrType == typeof(float) && ((RelationalTypeMappingInfo)( mappingInfo)).Size.HasValue && ((RelationalTypeMappingInfo)( mappingInfo)).Size <= 24 && (storeTypeNameBase.Equals("float", StringComparison.OrdinalIgnoreCase) || storeTypeNameBase.Equals("double precision", StringComparison.OrdinalIgnoreCase)))
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
