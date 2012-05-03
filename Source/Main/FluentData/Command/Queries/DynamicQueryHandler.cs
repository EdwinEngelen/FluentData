using System.Collections.Generic;
using System.Dynamic;

namespace FluentData
{
	internal class DynamicQueryHandler
	{
		public List<dynamic> ExecuteList(DbCommandData data)
		{
			var items = new List<dynamic>();

			var autoMapper = new DynamicTypAutoMapper(data);

			while (data.Reader.Read())
			{
				var item = autoMapper.AutoMap();

				items.Add(item);
			}

			return items;
		}

		public dynamic ExecuteSingle(DbCommandData data)
		{
			var autoMapper = new DynamicTypAutoMapper(data);

			ExpandoObject item = null;

			if (data.Reader.Read())
				item = autoMapper.AutoMap();

			return item;
		}
	}
}
