using System.Collections.Generic;
using System.Dynamic;

namespace FluentData
{
	internal class DynamicTypAutoMapper
	{
		private readonly DbCommandData _dbCommandData;
		private readonly List<DataReaderField> _fields;

		public DynamicTypAutoMapper(DbCommandData dbCommandData)
		{
			_dbCommandData = dbCommandData;
			_fields = DataReaderHelper.GetDataReaderFields(_dbCommandData.Reader);
		}

		public ExpandoObject AutoMap()
		{
			var item = new ExpandoObject();

			var itemDictionary = (IDictionary<string, object>) item;

			foreach (var column in _fields)
			{
				var value = DataReaderHelper.GetDataReaderValue(_dbCommandData.Reader, column.Index, true);

				itemDictionary.Add(column.Name, value); 
			}

			return item;
		}
	}
}
