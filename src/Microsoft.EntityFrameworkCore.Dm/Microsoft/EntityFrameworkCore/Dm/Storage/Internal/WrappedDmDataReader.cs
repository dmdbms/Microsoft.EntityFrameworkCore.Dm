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

		public override int FieldCount => GetReader().FieldCount;

		public override object this[int ordinal] => GetReader()[ordinal];

		public override object this[string name] => GetReader()[name];

		public override int RecordsAffected => GetReader().RecordsAffected;

		public override bool HasRows => GetReader().HasRows;

		public override bool IsClosed => _reader == null || _reader.IsClosed;

		public override int Depth => GetReader().Depth;

		public override int VisibleFieldCount => GetReader().VisibleFieldCount;

		internal WrappedDmDataReader(DbDataReader reader)
		{
			_reader = reader;
		}

		public override bool GetBoolean(int ordinal)
		{
			return GetReader().GetBoolean(ordinal);
		}

		public override byte GetByte(int ordinal)
		{
			return GetReader().GetByte(ordinal);
		}

		public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
		{
			return GetReader().GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
		}

		public override char GetChar(int ordinal)
		{
			return Convert.ToChar(GetReader().GetByte(ordinal));
		}

		public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
		{
			return GetReader().GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
		}

		public override string GetDataTypeName(int ordinal)
		{
			return GetReader().GetDataTypeName(ordinal);
		}

		public override DateTime GetDateTime(int ordinal)
		{
			return GetReader().GetDateTime(ordinal);
		}

		public override decimal GetDecimal(int ordinal)
		{
			return GetReader().GetDecimal(ordinal);
		}

		public override double GetDouble(int ordinal)
		{
			return GetReader().GetDouble(ordinal);
		}

		public override Type GetFieldType(int ordinal)
		{
			return GetReader().GetFieldType(ordinal);
		}

		public override float GetFloat(int ordinal)
		{
			return GetReader().GetFloat(ordinal);
		}

		public override Guid GetGuid(int ordinal)
		{
			return GetReader().GetGuid(ordinal);
		}

		public override short GetInt16(int ordinal)
		{
			return GetReader().GetInt16(ordinal);
		}

		public override int GetInt32(int ordinal)
		{
			return GetReader().GetInt32(ordinal);
		}

		public override long GetInt64(int ordinal)
		{
			return GetReader().GetInt64(ordinal);
		}

		public override string GetName(int ordinal)
		{
			return GetReader().GetName(ordinal);
		}

		public override int GetOrdinal(string name)
		{
			return GetReader().GetOrdinal(name);
		}

		public override string GetString(int ordinal)
		{
			return GetReader().GetString(ordinal);
		}

		public override object GetValue(int ordinal)
		{
			try
			{
				return GetReader().GetValue(ordinal);
			}
			catch (InvalidCastException e)
			{
				return ConvertWithReflection<object>(ordinal, e);
			}
		}

		public override int GetValues(object[] values)
		{
			return GetReader().GetValues(values);
		}

		public override bool IsDBNull(int ordinal)
		{
			return GetReader().IsDBNull(ordinal);
		}

		public override IEnumerator GetEnumerator()
		{
			return GetReader().GetEnumerator();
		}

		public override Type GetProviderSpecificFieldType(int ordinal)
		{
			return GetReader().GetProviderSpecificFieldType(ordinal);
		}

		public override object GetProviderSpecificValue(int ordinal)
		{
			return GetReader().GetProviderSpecificValue(ordinal);
		}

		public override int GetProviderSpecificValues(object[] values)
		{
			return GetReader().GetProviderSpecificValues(values);
		}

		public override Stream GetStream(int ordinal)
		{
			return GetReader().GetStream(ordinal);
		}

		public override TextReader GetTextReader(int ordinal)
		{
			return GetReader().GetTextReader(ordinal);
		}

		public override Task<bool> IsDBNullAsync(int ordinal, CancellationToken cancellationToken)
		{
			return GetReader().IsDBNullAsync(ordinal, cancellationToken);
		}

		public override bool NextResult()
		{
			return GetReader().NextResult();
		}

		public override Task<bool> NextResultAsync(CancellationToken cancellationToken)
		{
			return GetReader().NextResultAsync(cancellationToken);
		}

		public override bool Read()
		{
			return GetReader().Read();
		}

		public override Task<bool> ReadAsync(CancellationToken cancellationToken)
		{
			return GetReader().ReadAsync(cancellationToken);
		}

		public override T GetFieldValue<T>(int ordinal)
		{
			try
			{
				if (typeof(T) == typeof(char))
				{
					return (T)Convert.ChangeType(Convert.ToChar(GetReader().GetFieldValue<byte>(ordinal)), typeof(T));
				}
				return GetReader().GetFieldValue<T>(ordinal);
			}
			catch (InvalidCastException e)
			{
				return ConvertWithReflection<T>(ordinal, e);
			}
		}

		private T ConvertWithReflection<T>(int ordinal, InvalidCastException e)
		{
			try
			{
				ParameterExpression parameterExpression = Expression.Parameter(typeof(string), "data");
				BlockExpression body = Expression.Block(Expression.Convert(parameterExpression, typeof(T)));
				Delegate @delegate = Expression.Lambda(body, parameterExpression).Compile();
				return (T)@delegate.DynamicInvoke(GetReader().GetValue(ordinal));
			}
			catch (Exception)
			{
				throw e;
			}
		}

		private void CloseReader()
		{
			if (_reader != null)
			{
				_reader.Dispose();
				_reader = null;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && !_disposed)
			{
				CloseReader();
				base.Dispose(disposing);
				_disposed = true;
			}
		}

		private DbDataReader GetReader()
		{
			if (_reader == null)
			{
				throw new ObjectDisposedException("WrappedDmDataReader");
			}
			return _reader;
		}
	}
}
