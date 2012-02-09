using System;
using System.Collections.Generic;

namespace FluentData
{
	internal class BaseQueryHandler
	{
		protected DbCommandData Data;

		public BaseQueryHandler(
			DbCommandData data)
		{
			Data = data;
		}

		protected TList ResolveList<TList, TEntity>()
			where TList : IList<TEntity>
		{
			object item = null;

			var type = typeof(TList);

			if (type == typeof(List<TEntity>))
				item = Activator.CreateInstance(type);
			else
				item = Data.DbContextData.EntityFactory.Resolve(type);

			return (TList) item;
		}

		protected object Resolve<TEntity>(Type type)
		{
			object item = null;

			if (type == typeof(List<TEntity>))
				item = Activator.CreateInstance(type);
			else
				item = Data.DbContextData.EntityFactory.Resolve(type);

			return item;
		}
	}
}
