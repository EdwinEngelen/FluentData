using System;
using System.Collections.Generic;
using System.Dynamic;

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
					var item = MapEntity(data, customMapperReader, customMapperDynamic, autoMapper);
					items.Add(item);
				}
			}
			else
			{
				while (data.Reader.Read())
				{
					var value = MapScalarValue(data);
					items.Add((TEntity) value);
				}
			}

			return items;
		}

	    internal TEntity ExecuteSingle(DbCommandData data,
	                                   Action<TEntity, IDataReader> customMapperReader,
	                                   Action<TEntity, dynamic> customMapperDynamic)
	    {
		    if (typeof (TEntity) == typeof (object))
		    {
			    var autoMapper = new DynamicTypAutoMapper(data);

			    ExpandoObject item = null;

			    if (data.Reader.Read())
				    item = autoMapper.AutoMap();

			    return (dynamic) item;
		    }
		    else
		    {
			    var item = default(TEntity);

			    if (ReflectionHelper.IsCustomEntity<TEntity>())
			    {
				    AutoMapper autoMapper = null;

				    if (customMapperReader == null && customMapperDynamic == null)
					    autoMapper = new AutoMapper(data, typeof (TEntity));

				    if (data.Reader.Read())
					    item = MapEntity(data, customMapperReader, customMapperDynamic, autoMapper);
			    }
			    else
			    {
				    if (data.Reader.Read())
					   item = (TEntity)  MapScalarValue(data);
			    }

			    return item;
		    }
	    }

	    private static TEntity MapEntity(DbCommandData data,
			Action<TEntity, IDataReader> customMapperReader,
			Action<TEntity, dynamic> customMapperDynamic,
	        AutoMapper autoMapper)
	    {
		    var item = (TEntity) data.Context.Data.EntityFactory.Create(typeof (TEntity));

		    if (customMapperReader != null)
			    customMapperReader(item, data.Reader);
		    else if (customMapperDynamic != null)
			    customMapperDynamic(item, new DynamicDataReader(data.Reader));
		    else
			    autoMapper.AutoMap(item);
		    return item;
	    }

	    private static object MapScalarValue(DbCommandData data)
	    {
		    var value = data.Reader.GetValue(0);

		    if (value == null)
			    value = default(TEntity);
		    else if (data.Reader.GetFieldType(0) != typeof(TEntity))
			    value = (Convert.ChangeType(value, typeof(TEntity)));
		    return value;
	    }
    }
}
