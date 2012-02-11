using System;
using System.Collections.Generic;

namespace FluentData
{
	internal partial class DbCommand
	{
		private TList Query<TEntity, TList>(bool autoMap,
								Action<IDataReader, TEntity> customMapperReader,
								Action<dynamic, TEntity> customMapperDynamic)
			where TList : IList<TEntity>
		{
			TList items = default(TList);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				items = new GenericQueryHandler<TEntity>(_data).ExecuteList<TList>(autoMap, customMapperReader, customMapperDynamic);
			});

			return items;
		}

		public TList Query<TEntity, TList>()
			where TList : IList<TEntity>
		{
			return Query<TEntity, TList>(true, null, null);
		}

		public List<TEntity> Query<TEntity>()
		{
			return Query<TEntity, List<TEntity>>(true, null, null);
		}

		public TList Query<TEntity, TList>(Action<IDataReader, TEntity> customMapper)
			where TList : IList<TEntity>
		{
			return Query<TEntity, TList>(true, customMapper, null);
		}

		public TList Query<TEntity, TList>(Action<dynamic, TEntity> customMapper)
			where TList : IList<TEntity>
		{
			return Query<TEntity, TList>(true, null, customMapper);
		}

		public List<TEntity> Query<TEntity>(Action<IDataReader, TEntity> customMapper)
		{
			return Query<TEntity, List<TEntity>>(true, customMapper, null);
		}

		public List<TEntity> Query<TEntity>(Action<dynamic, TEntity> customMapper)
		{
			return Query<TEntity, List<TEntity>>(true, null, customMapper);
		}

		public TList QueryNoAutoMap<TEntity, TList>(Action<IDataReader, TEntity> customMapper)
			where TList : IList<TEntity>
		{
			return Query<TEntity, TList>(false, customMapper, null);
		}

		public TList QueryNoAutoMap<TEntity, TList>(Action<dynamic, TEntity> customMapper)
			where TList : IList<TEntity>
		{
			return Query<TEntity, TList>(false, null, customMapper);
		}

		public List<TEntity> QueryNoAutoMap<TEntity>(Action<IDataReader, TEntity> customMapper)
		{
			return Query<TEntity, List<TEntity>>(false, customMapper, null);
		}

		public List<TEntity> QueryNoAutoMap<TEntity>(Action<dynamic, TEntity> customMapper)
		{
			return Query<TEntity, List<TEntity>>(false, null, customMapper);
		}
	}
}
