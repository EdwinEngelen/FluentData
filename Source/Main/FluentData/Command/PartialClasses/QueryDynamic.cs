using System.Collections.Generic;

namespace FluentData
{
	internal partial class DbCommand
	{
		public List<dynamic> Query()
		{
			List<dynamic> items = null;

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				items = new DynamicQueryHandler().ExecuteList(_data);
			});

			return items;
		}

		public dynamic QuerySingle()
		{
			dynamic item = null;

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				item = new DynamicQueryHandler().ExecuteSingle(_data);
			});

			return item;
		}
	}
}
