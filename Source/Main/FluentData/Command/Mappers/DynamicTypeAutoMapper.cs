using System.Collections.Generic;
using System.Dynamic;

namespace FluentData
{
	internal class DynamicTypAutoMapper
	{
		private readonly List<DataReaderField> _fields;
		private readonly System.Data.IDataReader _reader;

		public DynamicTypAutoMapper(DbCommandData dbCommandData)
		{
			_reader = dbCommandData.Reader.InnerReader;
			_fields = DataReaderHelper.GetDataReaderFields(_reader);
		}

		public ExpandoObject AutoMap()
		{
			var item = new ExpandoObject();

			var itemDictionary = (IDictionary<string, object>) item;

			foreach (var column in _fields)
			{
				if (_reader.IsDBNull(column.Index))
					itemDictionary.Add(column.Name, null);
				else
					itemDictionary.Add(column.Name, _reader[column.Index]);
			}

			return item;
		}
	}
}
