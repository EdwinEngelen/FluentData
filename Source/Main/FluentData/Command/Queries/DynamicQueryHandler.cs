using System.Collections.Generic;
using System.Dynamic;

namespace FluentData
{
	internal class DynamicQueryHandler : BaseQueryHandler
	{
		public DynamicQueryHandler(DbCommandData data) : base(data)
		{
		}

		public List<dynamic> ExecuteList()
		{
			var items = new List<dynamic>();

			var autoMapper = new DynamicTypAutoMapper()
									.Reader(Data.Reader);

			while (Data.Reader.Read())
			{
				var item = autoMapper.AutoMap();

				items.Add(item);
			}

			return items;
		}

		public dynamic ExecuteSingle()
		{
			var autoMapper = new DynamicTypAutoMapper()
									.Reader(Data.Reader);

			ExpandoObject item = null;

			if (Data.Reader.Read())
				item = autoMapper.AutoMap();

			return item;
		}
	}
}
