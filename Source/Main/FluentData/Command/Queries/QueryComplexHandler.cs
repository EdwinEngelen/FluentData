using System;
using System.Collections.Generic;

namespace FluentData
{
	internal class QueryComplexHandler<TEntity, TList>
		where TList : IList<TEntity>
	{
		public TList Execute(DbCommandData data, Action<IDataReader, IList<TEntity>> customMapper)
		{
			var items = (TList) data.ContextData.EntityFactory.Create(typeof(TList));

			while (data.Reader.Read())
				customMapper(data.Reader, items);

			return items;
		}
	}
}
