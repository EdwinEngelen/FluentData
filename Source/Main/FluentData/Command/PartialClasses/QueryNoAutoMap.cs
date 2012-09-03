using System;
using System.Collections.Generic;

namespace FluentData
{
	internal partial class DbCommand
	{
		public TList QueryNoAutoMap<TEntity, TList>(Func<IDataReader, TEntity> customMapper) where TList : IList<TEntity>
		{
			var items = default(TList);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				items = new QueryNoAutoMapHandler<TEntity>().QueryNoAutoMap<TList>(_data, customMapper);
			});

			return items;
		}

		public List<TEntity> QueryNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper)
		{
			return QueryNoAutoMap<TEntity, List<TEntity>>(customMapper);
		}
	}
}
