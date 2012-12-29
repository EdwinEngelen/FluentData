using System;

namespace FluentData
{
	internal partial class DbCommand
	{
		public TEntity QuerySingle<TEntity>(Action<TEntity, IDataReader> customMapper)
		{
			var item = default(TEntity);

			Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				item = new QueryManyHandler<TEntity>().ExecuteSingle(Data, customMapper, null);
			});

			return item;
		}

		public TEntity QuerySingle<TEntity>(Action<TEntity, dynamic> customMapper)
		{
			var item = default(TEntity);

			Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				item = new QueryManyHandler<TEntity>().ExecuteSingle(Data, null, customMapper);
			});

			return item;
		}
	}
}
