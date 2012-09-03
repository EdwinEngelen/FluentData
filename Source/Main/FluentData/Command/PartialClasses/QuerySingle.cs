using System;

namespace FluentData
{
	internal partial class DbCommand
	{
		public TEntity QuerySingle<TEntity>()
		{
			return QuerySingle<TEntity>(null);
		}

		public TEntity QuerySingle<TEntity>(Action<IDataReader, TEntity> customMapper)
		{
			var item = default(TEntity);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				item = new GenericQueryHandler<TEntity>().ExecuteSingle(_data, customMapper);
			});

			return item;
		}
	}
}
