using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentData
{
	internal abstract class BaseMapper<T>
		where T : BaseMapper<T>
	{
		protected IDataReader _reader;
		protected List<DataReaderField> Fields;

		internal T Reader(IDataReader reader)
		{
			_reader = reader;
			Fields = GetDataReaderFields();
			return (T) this;
		}

		private List<DataReaderField> GetDataReaderFields()
		{
			var columns = new List<DataReaderField>();

			for (int i = 0; i < _reader.FieldCount; i++)
			{
				var column = new DataReaderField();
				column.Name = _reader.GetName(i);
				column.Type = _reader.GetFieldType(i);
				column.Index = i;

				if (columns.SingleOrDefault(x => x.Name == column.Name) == null)
					columns.Add(column);
			}

			return columns;
		}

		protected object GetDataReaderValue(int index, bool isNullable)
		{
			var value = _reader[index];
			var type = value.GetType();

			if (value == DBNull.Value)
			{
				if (isNullable)
					return null;
				
				if (type == typeof(DateTime))
					return DateTime.MinValue;
			}

			return value;
		}
	}
}
