using System;
using System.Collections.Generic;

namespace FluentData
{
	internal partial class DbCommand
	{
		public void QueryComplexMany<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper)
		{
			Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				while(Data.Reader.Read())
					customMapper(list, Data.Reader);
			});
		}

		public TEntity QueryComplexSingle<TEntity>(Func<IDataReader, TEntity> customMapper)
		{
			var item = default(TEntity);

			Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				item = new QueryComplexSingleHandler<TEntity>().ExecuteSingleComplex(Data, customMapper);
			});

			return item;
		}
	}
}
