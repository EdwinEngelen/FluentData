using System.Collections.Generic;

namespace FluentData
{
	internal partial class DbCommand
	{
		public List<dynamic> Query()
		{
			List<dynamic> items = null;

			Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				items = new DynamicQueryHandler().ExecuteList(Data);
			});

			return items;
		}

		public dynamic QuerySingle()
		{
			dynamic item = null;

			Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				item = new DynamicQueryHandler().ExecuteSingle(Data);
			});

			return item;
		}
	}
}
