using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace FluentData
{
	internal class DynamicTypAutoMapper
	{
		private readonly DbCommandData _dbCommandData;

		public DynamicTypAutoMapper(DbCommandData dbCommandData)
		{
			_dbCommandData = dbCommandData;
		}

		public ExpandoObject AutoMap()
		{
			var item = new ExpandoObject();

			var fields = DataReaderHelper.GetDataReaderFields(_dbCommandData.Reader);

			var itemDictionary = (IDictionary<string, object>) item;

			foreach (var column in fields)
			{
				var value = DataReaderHelper.GetDataReaderValue(_dbCommandData.Reader, column.Index, true);

				itemDictionary.Add(column.Name, value); 
			}

			return item;
		}
	}
}
