using System;
using System.Data;

namespace FluentData
{
	internal class DataReader : System.Data.IDataReader, IDataReader
	{
		private readonly System.Data.IDataReader _innerReader;
		private DynamicDataReader _dynamicReader;

		public DataReader(System.Data.IDataReader reader)
		{
			_innerReader = reader;
		}

		public dynamic Value {
			get
			{
				if(_dynamicReader == null)
					_dynamicReader = new DynamicDataReader(_innerReader);
				return _dynamicReader;
			}
		}

		public void Close()
		{
			_innerReader.Close();
		}

		public int Depth
		{
			get { return _innerReader.Depth; }
		}

		public DataTable GetSchemaTable()
		{
			return _innerReader.GetSchemaTable();
		}

		public bool IsClosed
		{
			get { return _innerReader.IsClosed; }
		}

		public bool NextResult()
		{
			return _innerReader.NextResult();
		}

		public bool Read()
		{
			return _innerReader.Read();
		}

		public int RecordsAffected
		{
			get { return _innerReader.RecordsAffected; }
		}

		public void Dispose()
		{
			_innerReader.Dispose();
		}

		public int FieldCount
		{
			get { return _innerReader.FieldCount; }
		}

		public bool GetBoolean(int i)
		{
			return _innerReader.GetBoolean(i);
		}

		public bool GetBoolean(string name)
		{
			return _innerReader.GetBoolean(GetOrdinal(name));
		}

		public byte GetByte(int i)
		{
			return _innerReader.GetByte(i);
		}

		public byte GetByte(string name)
		{
			return _innerReader.GetByte(GetOrdinal(name));
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return _innerReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
		}

		public long GetBytes(string name, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return _innerReader.GetBytes(GetOrdinal(name), fieldOffset, buffer, bufferoffset, length);
		}

		public char GetChar(int i)
		{
			return _innerReader.GetChar(i);
		}

		public char GetChar(string name)
		{
			return _innerReader.GetChar(GetOrdinal(name));
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return _innerReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
		}

		public long GetChars(string name, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return _innerReader.GetChars(GetOrdinal(name), fieldoffset, buffer, bufferoffset, length);
		}

		public System.Data.IDataReader GetData(int i)
		{
			return _innerReader.GetData(i);
		}

		public System.Data.IDataReader GetData(string name)
		{
			return _innerReader.GetData(GetOrdinal(name));
		}

		public string GetDataTypeName(int i)
		{
			return _innerReader.GetDataTypeName(i);
		}

		public string GetDataTypeName(string name)
		{
			return _innerReader.GetDataTypeName(GetOrdinal(name));
		}

		public DateTime GetDateTime(int i)
		{
			return _innerReader.GetDateTime(i);
		}

		public DateTime GetDateTime(string name)
		{
			return _innerReader.GetDateTime(GetOrdinal(name));
		}

		public decimal GetDecimal(int i)
		{
			return _innerReader.GetDecimal(i);
		}

		public decimal GetDecimal(string name)
		{
			return _innerReader.GetDecimal(GetOrdinal(name));
		}

		public double GetDouble(int i)
		{
			return _innerReader.GetDouble(i);
		}

		public double GetDouble(string name)
		{
			return _innerReader.GetDouble(GetOrdinal(name));
		}

		public Type GetFieldType(int i)
		{
			return _innerReader.GetFieldType(i);
		}

		public Type GetFieldType(string name)
		{
			return _innerReader.GetFieldType(GetOrdinal(name));
		}

		public float GetFloat(int i)
		{
			return _innerReader.GetFloat(i);
		}

		public float GetFloat(string name)
		{
			return _innerReader.GetFloat(GetOrdinal(name));
		}

		public Guid GetGuid(int i)
		{
			return _innerReader.GetGuid(i);
		}

		public Guid GetGuid(string name)
		{
			return _innerReader.GetGuid(GetOrdinal(name));
		}

		public short GetInt16(int i)
		{
			return _innerReader.GetInt16(i);
		}

		public short GetInt16(string name)
		{
			return _innerReader.GetInt16(GetOrdinal(name));
		}

		public int GetInt32(int i)
		{
			return _innerReader.GetInt32(i);
		}

		public int GetInt32(string name)
		{
			return _innerReader.GetInt32(GetOrdinal(name));
		}

		public long GetInt64(int i)
		{
			return _innerReader.GetInt64(i);
		}

		public long GetInt64(string name)
		{
			return _innerReader.GetInt64(GetOrdinal(name));
		}

		public string GetName(int i)
		{
			return _innerReader.GetName(i);
		}

		public string GetName(string name)
		{
			return _innerReader.GetName(GetOrdinal(name));
		}

		public int GetOrdinal(string name)
		{
			return _innerReader.GetOrdinal(name);
		}

		public string GetString(int i)
		{
			return _innerReader.GetString(i);
		}

		public string GetString(string name)
		{
			return _innerReader.GetString(GetOrdinal(name));
		}

		public object GetValue(int i)
		{
			return _innerReader.GetValue(i);
		}

		public object GetValue(string name)
		{
			return _innerReader.GetValue(GetOrdinal(name));
		}

		public int GetValues(object[] values)
		{
			return _innerReader.GetValues(values);
		}

		public bool IsDBNull(int i)
		{
			return _innerReader.IsDBNull(i);
		}

		public bool IsDBNull(string name)
		{
			return _innerReader.IsDBNull(GetOrdinal(name));
		}

		public object this[string name]
		{
			get { return _innerReader[name]; }
		}

		public object this[int i]
		{
			get { return GetValue(i); }
		}
	}
}
