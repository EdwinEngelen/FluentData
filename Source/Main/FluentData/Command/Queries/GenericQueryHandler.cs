using System;
using System.Collections.Generic;

namespace FluentData
{
	internal class GenericQueryHandler<TEntity> : BaseQueryHandler
	{
		public GenericQueryHandler(DbCommandData data)
			: base(data)
		{
		}

		internal TList ExecuteList<TList>(
									bool autoMap,
									Action<IDataReader, TEntity> customMapperReader,
									Action<dynamic, TEntity> customMapperDynamic
					)
			where TList : IList<TEntity>
		{
			var items = ResolveList<TList, TEntity>();

			AutoMapper<TEntity> autoMapper = null;
			if (autoMap)
			{
				autoMapper = new AutoMapper<TEntity>(Data.ContextData);
				autoMapper.EntityFactory = Data.ContextData.EntityFactory;

				autoMapper.Reader(Data.Reader);
			}

			DynamicTypAutoMapper dynamicAutoMapper = null;			

			while (Data.Reader.Read())
			{
				var item = (TEntity) Data.ContextData.EntityFactory.Create(typeof(TEntity));

				if (autoMap)
					autoMapper.AutoMap(item);

				if (customMapperReader != null)
					customMapperReader(Data.Reader, item);

				if (customMapperDynamic != null)
				{
					if (dynamicAutoMapper == null)
						dynamicAutoMapper = new DynamicTypAutoMapper().Reader(Data.Reader);
					var dynamicObject = dynamicAutoMapper.AutoMap();
					customMapperDynamic(dynamicObject, item);
				}

				items.Add(item);
			}

			return items;
		}

		internal TEntity ExecuteSingle(bool autoMap, Action<IDataReader, TEntity> customMapper)
		{
			AutoMapper<TEntity> autoMapper = null;
			if (autoMap)
			{
				autoMapper = new AutoMapper<TEntity>(Data.ContextData);
				autoMapper.EntityFactory = Data.ContextData.EntityFactory;
				autoMapper.Reader(Data.Reader);
			}

			TEntity item = default(TEntity);

			if (Data.Reader.Read())
			{
				item = (TEntity) Data.ContextData.EntityFactory.Create(typeof(TEntity));

				if (autoMap)
					autoMapper.AutoMap(item);

				if (customMapper != null)
					customMapper(Data.Reader, item);
			}

			return item;
		}
	}
}
