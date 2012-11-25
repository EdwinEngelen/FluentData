using System;
using System.Collections.Generic;

namespace FluentData
{
	internal partial class DbCommand
	{
		public TList QueryMany<TEntity, TList>(Action<TEntity, IDataReader> customMapper = null)
			where TList : IList<TEntity>
		{
			var items = default(TList);

			Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				items = new GenericQueryHandler<TEntity>().ExecuteListReader<TList>(Data, customMapper);
			});

			return items;
		}

		public List<TEntity> QueryMany<TEntity>(Action<TEntity, IDataReader> customMapper)
		{
			return QueryMany<TEntity, List<TEntity>>(customMapper);
		}

		public TEntity QuerySingle<TEntity>(Action<TEntity, IDataReader> customMapper)
		{
			var item = default(TEntity);

			Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				item = new GenericQueryHandler<TEntity>().ExecuteSingle(Data, customMapper);
			});

			return item;
		}
	}
}
