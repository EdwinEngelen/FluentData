using System;
using System.Collections.Generic;

namespace FluentData
{
	internal class QuerySingleNoAutoMapHandler<TEntity>
	{
		internal TEntity ExecuteSingleNoAutoMap(DbCommandData data,
			Func<IDataReader, TEntity> customMapperReader,
			Func<dynamic, TEntity> customMapperDynamic)
		{
			var item = default(TEntity);

			if (data.Reader.Read())
			{
				if (customMapperReader != null)
					item = customMapperReader(data.Reader);
				else if (customMapperDynamic != null)
					item = customMapperDynamic(new DynamicDataReader(data.InnerReader));
			}

			return item;
		}
	}
}
