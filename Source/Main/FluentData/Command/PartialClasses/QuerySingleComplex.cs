using System;

namespace FluentData
{
	internal partial class DbCommand
	{
		public TEntity QuerySingleComplex<TEntity>(Func<IDataReader, TEntity> customMapper)
		{
			var item = default(TEntity);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				item = new QuerySingleComplexHandler<TEntity>().ExecuteSingleComplex(_data, customMapper);
			});

			return item;
		}
	}
}
