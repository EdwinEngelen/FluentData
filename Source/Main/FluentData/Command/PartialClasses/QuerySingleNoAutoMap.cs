using System;

namespace FluentData
{
	internal partial class DbCommand
	{
		public TEntity QuerySingleNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper)
		{
			var item = default(TEntity);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				item = new QuerySingleNoAutoMapHandler<TEntity>().ExecuteSingleNoAutoMap(_data, customMapper, null);
			});

			return item;
		}

		public TEntity QuerySingleNoAutoMap<TEntity>(Func<dynamic, TEntity> customMapper)
		{
			var item = default(TEntity);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				item = new QuerySingleNoAutoMapHandler<TEntity>().ExecuteSingleNoAutoMap(_data, null, customMapper);
			});

			return item;
		}
	}
}
