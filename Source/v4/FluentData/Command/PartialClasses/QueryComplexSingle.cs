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
				if (Data.Reader.Read())
					item = customMapper(Data.Reader);
			});

			return item;
		}

		public TEntity QueryComplexSingle<TEntity>(Func<dynamic, TEntity> customMapper)
		{
			var item = default(TEntity);

			Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				if (Data.Reader.Read())
					item = customMapper(new DynamicDataReader(Data.Reader));
			});

			return item;
		}
	}
}
