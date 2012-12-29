using System;

namespace FluentData
{
	internal interface IQueryTypeHandler<TEntity>
	{
		TEntity HandleType(Action<TEntity, IDataReader> customMapperReader, Action<TEntity, dynamic> customMapperDynamic);
	}
}