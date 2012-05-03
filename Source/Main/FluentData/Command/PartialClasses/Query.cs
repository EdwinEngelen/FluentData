using System;
using System.Collections.Generic;

namespace FluentData
{
	internal partial class DbCommand
	{
		private TList Query<TEntity, TList>(
								Action<IDataReader, TEntity> customMapperReader,
								Action<dynamic, TEntity> customMapperDynamic)
			where TList : IList<TEntity>
		{
			var items = default(TList);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				items = new GenericQueryHandler<TEntity>().ExecuteListReader<TList>(_data, customMapperReader, customMapperDynamic);
			});

			return items;
		}

		public TList Query<TEntity, TList>()
			where TList : IList<TEntity>
		{
			return Query<TEntity, TList>(null, null);
		}

		public List<TEntity> Query<TEntity>()
		{
			return Query<TEntity, List<TEntity>>(null, null);
		}

		public TList Query<TEntity, TList>(Action<IDataReader, TEntity> customMapper)
			where TList : IList<TEntity>
		{
			return Query<TEntity, TList>(customMapper, null);
		}

		public TList Query<TEntity, TList>(Action<dynamic, TEntity> customMapper)
			where TList : IList<TEntity>
		{
			return Query<TEntity, TList>(null, customMapper);
		}

		public List<TEntity> Query<TEntity>(Action<IDataReader, TEntity> customMapper)
		{
			return Query<TEntity, List<TEntity>>(customMapper, null);
		}

		public List<TEntity> Query<TEntity>(Action<dynamic, TEntity> customMapper)
		{
			return Query<TEntity, List<TEntity>>(null, customMapper);
		}
	}
}
