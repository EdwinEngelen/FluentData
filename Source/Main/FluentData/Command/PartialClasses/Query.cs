using System;
using System.Collections.Generic;

namespace FluentData
{
	internal partial class DbCommand
	{
		public TList Query<TEntity, TList>()
			where TList : IList<TEntity>
		{
			return Query<TEntity, TList>(null);
		}

		public TList Query<TEntity, TList>(Action<TEntity, IDataReader> customMapper = null)
			where TList : IList<TEntity>
		{
			var items = default(TList);

			Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				items = new GenericQueryHandler<TEntity>().ExecuteListReader<TList>(Data, customMapper);
			});

			return items;
		}

		public List<TEntity> Query<TEntity>(Action<TEntity, IDataReader> customMapper)
		{
			return Query<TEntity, List<TEntity>>(customMapper);
		}
	}
}
