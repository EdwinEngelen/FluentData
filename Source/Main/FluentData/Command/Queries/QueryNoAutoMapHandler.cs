using System;
using System.Collections.Generic;

namespace FluentData
{
	internal class QueryNoAutoMapHandler<TEntity>
	{
		internal TList QueryNoAutoMap<TList>(DbCommandData data,
			Func<IDataReader, TEntity> customMapperReader,
			Func<dynamic, TEntity> customMapperDynamic)
			where TList : IList<TEntity>
		{
			var items = (TList) data.ContextData.EntityFactory.Create(typeof(TList));

			while (data.Reader.Read())
			{
				var item = default(TEntity);

				if (customMapperReader != null)
					item = customMapperReader(data.Reader);
				else if (customMapperDynamic != null)
					item = customMapperDynamic(new DynamicDataReader(data.InnerReader));

				items.Add(item);
			}

			return items;
		}
	}
}
