using System;
using System.Data;

namespace FluentData
{
	internal class DataReader : IDataReader
	{
		public System.Data.IDataReader InnerReader { get; private set; }
		private DynamicDataReader _dynamicReader;

		public DataReader(System.Data.IDataReader reader)
		{
			InnerReader = reader;
		}

		public dynamic Value
		{
			get
			{
				if(_dynamicReader == null)
					_dynamicReader = new DynamicDataReader(InnerReader);
				return _dynamicReader;
			}
		}

		public void Close()
		{
			InnerReader.Close();
		}

		public int Depth
		{
			get { return InnerReader.Depth; }
		}

		public DataTable GetSchemaTable()
		{
			return InnerReader.GetSchemaTable();
		}

		public bool IsClosed
		{
			get { return InnerReader.IsClosed; }
		}

		public bool NextResult()
		{
			return InnerReader.NextResult();
		}

		public bool Read()
		{
			return InnerReader.Read();
		}

		public int RecordsAffected
		{
			get { return InnerReader.RecordsAffected; }
		}

		public void Dispose()
		{
			InnerReader.Dispose();
		}

		public int FieldCount
		{
			get { return InnerReader.FieldCount; }
		}

		public bool GetBoolean(int i)
		{
			return InnerReader.GetBoolean(i);
		}

		public bool GetBoolean(string name)
		{
			return InnerReader.GetBoolean(GetOrdinal(name));
		}

		public byte GetByte(int i)
		{
			return InnerReader.GetByte(i);
		}

		public byte GetByte(string name)
		{
			return InnerReader.GetByte(GetOrdinal(name));
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return InnerReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
		}

		public long GetBytes(string name, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return InnerReader.GetBytes(GetOrdinal(name), fieldOffset, buffer, bufferoffset, length);
		}

		public char GetChar(int i)
		{
			return InnerReader.GetChar(i);
		}

		public char GetChar(string name)
		{
			return InnerReader.GetChar(GetOrdinal(name));
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return InnerReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
		}

		public long GetChars(string name, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return InnerReader.GetChars(GetOrdinal(name), fieldoffset, buffer, bufferoffset, length);
		}

		public System.Data.IDataReader GetData(int i)
		{
			return InnerReader.GetData(i);
		}

		public System.Data.IDataReader GetData(string name)
		{
			return InnerReader.GetData(GetOrdinal(name));
		}

		public string GetDataTypeName(int i)
		{
			return InnerReader.GetDataTypeName(i);
		}

		public string GetDataTypeName(string name)
		{
			return InnerReader.GetDataTypeName(GetOrdinal(name));
		}

		public DateTime GetDateTime(int i)
		{
			return InnerReader.GetDateTime(i);
		}

		public DateTime GetDateTime(string name)
		{
			return InnerReader.GetDateTime(GetOrdinal(name));
		}

		public decimal GetDecimal(int i)
		{
			return InnerReader.GetDecimal(i);
		}

		public decimal GetDecimal(string name)
		{
			return InnerReader.GetDecimal(GetOrdinal(name));
		}

		public double GetDouble(int i)
		{
			return InnerReader.GetDouble(i);
		}

		public double GetDouble(string name)
		{
			return InnerReader.GetDouble(GetOrdinal(name));
		}

		public Type GetFieldType(int i)
		{
			return InnerReader.GetFieldType(i);
		}

		public Type GetFieldType(string name)
		{
			return InnerReader.GetFieldType(GetOrdinal(name));
		}

		public float GetFloat(int i)
		{
			return InnerReader.GetFloat(i);
		}

		public float GetFloat(string name)
		{
			return InnerReader.GetFloat(GetOrdinal(name));
		}

		public Guid GetGuid(int i)
		{
			return InnerReader.GetGuid(i);
		}

		public Guid GetGuid(string name)
		{
			return InnerReader.GetGuid(GetOrdinal(name));
		}

		public short GetInt16(int i)
		{
			return InnerReader.GetInt16(i);
		}

		public short GetInt16(string name)
		{
			return InnerReader.GetInt16(GetOrdinal(name));
		}

		public int GetInt32(int i)
		{
			return InnerReader.GetInt32(i);
		}

		public int GetInt32(string name)
		{
			return InnerReader.GetInt32(GetOrdinal(name));
		}

		public long GetInt64(int i)
		{
			return InnerReader.GetInt64(i);
		}

		public long GetInt64(string name)
		{
			return InnerReader.GetInt64(GetOrdinal(name));
		}

		public string GetName(int i)
		{
			return InnerReader.GetName(i);
		}

		public string GetName(string name)
		{
			return InnerReader.GetName(GetOrdinal(name));
		}

		public int GetOrdinal(string name)
		{
			return InnerReader.GetOrdinal(name);
		}

		public string GetString(int i)
		{
			return InnerReader.GetString(i);
		}

		public string GetString(string name)
		{
			return InnerReader.GetString(GetOrdinal(name));
		}

		public object GetValue(int i)
		{
			return InnerReader.GetValue(i);
		}

		public object GetValue(string name)
		{
			return InnerReader.GetValue(GetOrdinal(name));
		}

		public int GetValues(object[] values)
		{
			return InnerReader.GetValues(values);
		}

		public bool IsDBNull(int i)
		{
			return InnerReader.IsDBNull(i);
		}

		public bool IsDBNull(string name)
		{
			return InnerReader.IsDBNull(GetOrdinal(name));
		}

		public object this[string name]
		{
			get { return InnerReader[name]; }
		}

		public object this[int i]
		{
			get { return GetValue(i); }
		}
	}
}
