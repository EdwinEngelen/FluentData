using System.Collections.Generic;
using System.Dynamic;

namespace FluentData
{
	internal class DynamicTypAutoMapper : BaseMapper<DynamicTypAutoMapper>
	{
		public ExpandoObject AutoMap()
		{
			var item = new ExpandoObject();

			var itemDictionary = (IDictionary<string, object>) item;

			foreach (var column in Fields)
			{
				var value = GetDataReaderValue(column.Index, true);

				itemDictionary.Add(column.Name, value); 
			}

			return item;
		}
	}
}
