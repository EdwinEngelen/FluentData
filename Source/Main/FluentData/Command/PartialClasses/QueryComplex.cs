using System;
using System.Collections.Generic;

namespace FluentData
{
	internal partial class DbCommand
	{
		public void QueryComplex<TEntity>(IList<TEntity> list, Action<IDataReader, IList<TEntity>> customMapper)
		{
			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				while(_data.Reader.Read())
					customMapper(_data.Reader, list);
			});
		}
	}
}
