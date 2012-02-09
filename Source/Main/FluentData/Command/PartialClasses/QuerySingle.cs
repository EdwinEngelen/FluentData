using System;

namespace FluentData
{
	internal partial class DbCommand
	{
		private TEntity QuerySingle<TEntity>(bool autoMap, Action<IDataReader, TEntity> customMapper)
		{
			TEntity item = default(TEntity);

			_data.QueryExecuter.ExecuteQueryHandler(true, () =>
			{
				item = new GenericQueryHandler<TEntity>(_data).ExecuteSingle(autoMap, customMapper);
			});

			return item;
		}

		public TEntity QuerySingle<TEntity>()
		{
			return QuerySingle<TEntity>(true, null);
		}

		public TEntity QuerySingle<TEntity>(Action<IDataReader, TEntity> customMapper)
		{
			return QuerySingle<TEntity>(true, customMapper);
		}
	}
}
