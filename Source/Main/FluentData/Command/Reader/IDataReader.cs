using System;

namespace FluentData
{
	public interface IDataReader : System.Data.IDataReader
	{
		System.Data.IDataReader InnerReader { get; }
		bool GetBoolean(string name);
		byte GetByte(string name);
		long GetBytes(string name, long fieldOffset, byte[] buffer, int bufferoffset, int length);
		char GetChar(string name);
		long GetChars(string name, long fieldoffset, char[] buffer, int bufferoffset, int length);
		global::System.Data.IDataReader GetData(string name);
		string GetDataTypeName(string name);
		DateTime GetDateTime(string name);
		decimal GetDecimal(string name);
		double GetDouble(string name);
		Type GetFieldType(string name);
		float GetFloat(string name);
		Guid GetGuid(string name);
		short GetInt16(string name);
		int GetInt32(string name);
		long GetInt64(string name);
		string GetName(string name);
		string GetString(string name);
		object GetValue(string name);
	}
}
