using System;

namespace FluentData
{
	internal partial class DbCommand
	{
		public TEntity QueryComplexSingle<TEntity>(Func<IDataReader, TEntity> customMapper)
		{
			var item = default(TEntity);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				item = new QueryComplexSingleHandler<TEntity>().ExecuteSingleComplex(_data, customMapper);
			});

			return item;
		}
	}
}
