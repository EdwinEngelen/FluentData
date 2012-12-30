using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;

namespace FluentData
{
    internal class QueryHandler<TEntity>
    {
	    private readonly DbCommandData _data;
	    private readonly IQueryTypeHandler<TEntity> _typeHandler;

	    public QueryHandler(DbCommandData data)
	    {
		    _data = data;
			if (typeof(TEntity) == typeof(object) || typeof(TEntity) == typeof(ExpandoObject))
				_typeHandler = new QueryDynamicHandler<TEntity>(data);
			else if (typeof(TEntity) == typeof(DataTable))
				_typeHandler = new QueryDataTableHandler<TEntity>(data);
			else if (ReflectionHelper.IsCustomEntity<TEntity>())
				_typeHandler = new QueryCustomEntityHandler<TEntity>(data);
			else
				_typeHandler = new QueryScalarHandler<TEntity>(data);
		}

	    internal TList ExecuteMany<TList>(Action<TEntity, IDataReader> customMapperReader,
									Action<TEntity, dynamic> customMapperDynamic
					)
			where TList : IList<TEntity>
		{
			var items = (TList)_data.Context.Data.EntityFactory.Create(typeof(TList));
		    var reader = _data.Reader.InnerReader;
			
			if (_typeHandler.IterateDataReader)
			{
				while (reader.Read())
				{
					var item = (TEntity) _typeHandler.HandleType(customMapperReader, customMapperDynamic);
					items.Add(item);
				}
			}
			else
			{
				var item = (TEntity)_typeHandler.HandleType(customMapperReader, customMapperDynamic);
				items.Add(item);
			}

			return items;
		}

		internal TEntity ExecuteSingle(Action<TEntity, IDataReader> customMapperReader,
								   Action<TEntity, dynamic> customMapperDynamic)
		{
			var item = default(TEntity);
			if (!_typeHandler.IterateDataReader || _data.Reader.InnerReader.Read())
				item = (TEntity) _typeHandler.HandleType(customMapperReader, customMapperDynamic);
				
			return item;
		}
    }
}
