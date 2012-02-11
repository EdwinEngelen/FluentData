using System;
using System.Collections.Generic;

namespace FluentData
{
	internal partial class DbCommand
	{
		public TList QueryComplex<TEntity, TList>(Action<IDataReader, IList<TEntity>> customMapper)
			where TList : IList<TEntity>
		{
			TList items = default(TList);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				items = new QueryComplexHandler<TEntity, TList>(_data).Execute(customMapper);
			});

			return items;
		}

		public List<TEntity> QueryComplex<TEntity>(Action<IDataReader, IList<TEntity>> customMapper)
		{
			return QueryComplex<TEntity, List<TEntity>>(customMapper);
		}
	}
}
