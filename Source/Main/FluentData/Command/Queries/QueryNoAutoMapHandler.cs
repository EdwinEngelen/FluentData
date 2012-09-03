using System;
using System.Collections.Generic;

namespace FluentData
{
	internal class QueryNoAutoMapHandler<TEntity>
	{
		internal TList QueryNoAutoMap<TList>(DbCommandData data,
			Func<IDataReader, TEntity> customMapperReader)
			where TList : IList<TEntity>
		{
			var items = (TList) data.ContextData.EntityFactory.Create(typeof(TList));

			while (data.Reader.Read())
			{
				var item = default(TEntity);

				if (customMapperReader != null)
					item = customMapperReader(data.Reader);

				items.Add(item);
			}

			return items;
		}
	}
}
