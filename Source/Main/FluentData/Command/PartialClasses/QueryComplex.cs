using System;
using System.Collections.Generic;

namespace FluentData
{
	internal partial class DbCommand
	{
		public TList QueryComplex<TEntity, TList>(Action<IDataReader, IList<TEntity>> customMapper)
			where TList : IList<TEntity>
		{
			var items = default(TList);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				items = new QueryComplexHandler<TEntity, TList>().Execute(_data, customMapper);
			});

			return items;
		}

		public List<TEntity> QueryComplex<TEntity>(Action<IDataReader, IList<TEntity>> customMapper)
		{
			return QueryComplex<TEntity, List<TEntity>>(customMapper);
		}
	}
}
