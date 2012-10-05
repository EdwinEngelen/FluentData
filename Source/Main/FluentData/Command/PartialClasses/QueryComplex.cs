using System;
using System.Collections.Generic;

namespace FluentData
{
	internal partial class DbCommand
	{
		public void QueryComplex<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper)
		{
			Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				while(Data.Reader.Read())
					customMapper(list, Data.Reader);
			});
		}
	}
}
