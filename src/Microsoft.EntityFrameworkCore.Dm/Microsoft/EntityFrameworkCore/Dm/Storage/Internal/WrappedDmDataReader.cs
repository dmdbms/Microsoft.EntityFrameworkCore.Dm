// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.WrappedDmDataReader
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using System;
using System.Collections;
using System.Data.Common;
using System.IO;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class WrappedDmDataReader : DbDataReader
  {
    private DbDataReader _reader;
    private bool _disposed;

    internal WrappedDmDataReader(DbDataReader reader) => this._reader = reader;

    public override bool GetBoolean(int ordinal) => this.GetReader().GetBoolean(ordinal);

    public override byte GetByte(int ordinal) => this.GetReader().GetByte(ordinal);

    public override long GetBytes(
      int ordinal,
      long dataOffset,
      byte[] buffer,
      int bufferOffset,
      int length)
    {
      return this.GetReader().GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
    }

    public override char GetChar(int ordinal) => Convert.ToChar(this.GetReader().GetByte(ordinal));

    public override long GetChars(
      int ordinal,
      long dataOffset,
      char[] buffer,
      int bufferOffset,
      int length)
    {
      return this.GetReader().GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
    }

    public override string GetDataTypeName(int ordinal) => this.GetReader().GetDataTypeName(ordinal);

    public override DateTime GetDateTime(int ordinal) => this.GetReader().GetDateTime(ordinal);

    public override Decimal GetDecimal(int ordinal) => this.GetReader().GetDecimal(ordinal);

    public override double GetDouble(int ordinal) => this.GetReader().GetDouble(ordinal);

    public override Type GetFieldType(int ordinal) => this.GetReader().GetFieldType(ordinal);

    public override float GetFloat(int ordinal) => this.GetReader().GetFloat(ordinal);

    public override Guid GetGuid(int ordinal) => this.GetReader().GetGuid(ordinal);

    public override short GetInt16(int ordinal) => this.GetReader().GetInt16(ordinal);

    public override int GetInt32(int ordinal) => this.GetReader().GetInt32(ordinal);

    public override long GetInt64(int ordinal) => this.GetReader().GetInt64(ordinal);

    public override string GetName(int ordinal) => this.GetReader().GetName(ordinal);

    public override int GetOrdinal(string name) => this.GetReader().GetOrdinal(name);

    public override string GetString(int ordinal) => this.GetReader().GetString(ordinal);

    public override object GetValue(int ordinal)
    {
      try
      {
        return this.GetReader().GetValue(ordinal);
      }
      catch (InvalidCastException ex)
      {
        return this.ConvertWithReflection<object>(ordinal, ex);
      }
    }

    public override int GetValues(object[] values) => this.GetReader().GetValues(values);

    public override bool IsDBNull(int ordinal) => this.GetReader().IsDBNull(ordinal);

    public override int FieldCount => this.GetReader().FieldCount;

    public override object this[int ordinal] => this.GetReader()[ordinal];

    public override object this[string name] => this.GetReader()[name];

    public override int RecordsAffected => this.GetReader().RecordsAffected;

    public override bool HasRows => this.GetReader().HasRows;

    public override bool IsClosed => this._reader == null || this._reader.IsClosed;

    public override int Depth => this.GetReader().Depth;

    public override IEnumerator GetEnumerator() => this.GetReader().GetEnumerator();

    public override Type GetProviderSpecificFieldType(int ordinal) => this.GetReader().GetProviderSpecificFieldType(ordinal);

    public override object GetProviderSpecificValue(int ordinal) => this.GetReader().GetProviderSpecificValue(ordinal);

    public override int GetProviderSpecificValues(object[] values) => this.GetReader().GetProviderSpecificValues(values);

    public override Stream GetStream(int ordinal) => this.GetReader().GetStream(ordinal);

    public override TextReader GetTextReader(int ordinal) => this.GetReader().GetTextReader(ordinal);

    public override Task<bool> IsDBNullAsync(int ordinal, CancellationToken cancellationToken) => this.GetReader().IsDBNullAsync(ordinal, cancellationToken);

    public override int VisibleFieldCount => this.GetReader().VisibleFieldCount;

    public override bool NextResult() => this.GetReader().NextResult();

    public override Task<bool> NextResultAsync(CancellationToken cancellationToken) => this.GetReader().NextResultAsync(cancellationToken);

    public override bool Read() => this.GetReader().Read();

    public override Task<bool> ReadAsync(CancellationToken cancellationToken) => this.GetReader().ReadAsync(cancellationToken);

    public override T GetFieldValue<T>(int ordinal)
    {
      try
      {
        return typeof (T) == typeof (char) ? (T) Convert.ChangeType((object) Convert.ToChar(this.GetReader().GetFieldValue<byte>(ordinal)), typeof (T)) : this.GetReader().GetFieldValue<T>(ordinal);
      }
      catch (InvalidCastException ex)
      {
        return this.ConvertWithReflection<T>(ordinal, ex);
      }
    }

    private T ConvertWithReflection<T>(int ordinal, InvalidCastException e)
    {
      try
      {
        ParameterExpression parameterExpression = Expression.Parameter(typeof (string), "data");
        return (T) Expression.Lambda((Expression) Expression.Block((Expression) Expression.Convert((Expression) parameterExpression, typeof (T))), parameterExpression).Compile().DynamicInvoke(this.GetReader().GetValue(ordinal));
      }
      catch (Exception ex)
      {
        throw e;
      }
    }

    private void CloseReader()
    {
      if (this._reader == null)
        return;
      this._reader.Dispose();
      this._reader = (DbDataReader) null;
    }

    protected override void Dispose(bool disposing)
    {
      if (!disposing || this._disposed)
        return;
      this.CloseReader();
      base.Dispose(disposing);
      this._disposed = true;
    }

    private DbDataReader GetReader() => this._reader != null ? this._reader : throw new ObjectDisposedException(nameof (WrappedDmDataReader));
  }
}
