using System;

namespace FluentData
{
	internal partial class DbCommand
	{
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
