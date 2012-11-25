using System;

namespace FluentData
{
	internal partial class DbCommand
	{
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
