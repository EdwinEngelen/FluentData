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

		public List<TEntity> Query<TEntity>()
		{
			return Query<TEntity, List<TEntity>>(null);
		}

		public TList Query<TEntity, TList>(Action<IDataReader, TEntity> customMapper)
			where TList : IList<TEntity>
		{
			var items = default(TList);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				items = new GenericQueryHandler<TEntity>().ExecuteListReader<TList>(_data, customMapper);
			});

			return items;
		}

		public List<TEntity> Query<TEntity>(Action<IDataReader, TEntity> customMapper)
		{
			return Query<TEntity, List<TEntity>>(customMapper);
		}
	}
}
