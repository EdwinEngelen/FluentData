using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentData
{
    internal class QueryManyHandler<TEntity>
    {
		internal TList Execute<TList>(
									DbCommandData data,
									Action<TEntity, IDataReader> customMapperReader,
									Action<TEntity, dynamic> customMapperDynamic
					)
			where TList : IList<TEntity>
		{
			var items = (TList)data.Context.Data.EntityFactory.Create(typeof(TList));

			if (typeof(TEntity) == typeof(object))
			{
				var autoMapper = new DynamicTypAutoMapper(data);

				while (data.Reader.Read())
				{
					dynamic item = autoMapper.AutoMap();

					items.Add(item);
				}

				return (TList)items;
			}
			else if (ReflectionHelper.IsCustomEntity<TEntity>())
			{
				var autoMapper = new AutoMapper(data, typeof(TEntity));

				while (data.Reader.Read())
				{
					var item = (TEntity)data.Context.Data.EntityFactory.Create(typeof(TEntity));

					if (customMapperReader != null)
						customMapperReader(item, data.Reader);
					else if (customMapperDynamic != null)
						customMapperDynamic(item, new DynamicDataReader(data.Reader));
					else
						autoMapper.AutoMap(item);
					items.Add(item);
				}
			}
			else
			{
				while (data.Reader.Read())
				{
					var value = (TEntity)data.Reader.GetValue(0);

					items.Add(value);
				}
			}

			return items;
		}
    }
}
