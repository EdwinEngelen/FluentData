using System;
using System.Collections.Generic;

namespace FluentData
{
	internal class QueryComplexHandler<TEntity, TList> : BaseQueryHandler
		where TList : IList<TEntity>
	{
		public QueryComplexHandler(DbCommandData data) : base(data)
		{
		}

		public TList Execute(Action<IDataReader, IList<TEntity>> customMapper)
		{
			var items = ResolveList<TList, TEntity>();

			while (Data.Reader.Read())
				customMapper(Data.Reader, items);

			return items;
		}
	}
}
