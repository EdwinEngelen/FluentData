using System;

namespace FluentData
{
	public class QueryDynamicHandler<TEntity> : IQueryTypeHandler<TEntity>
	{
		private readonly DbCommandData _data;
		private readonly DynamicTypeAutoMapper _autoMapper;

		public QueryDynamicHandler(DbCommandData data)
		{
			_data = data;
			_autoMapper = new DynamicTypeAutoMapper(_data.Reader.InnerReader);
		}

		public TEntity HandleType(Action<TEntity, IDataReader> customMapperReader, Action<TEntity, dynamic> customMapperDynamic)
		{
			dynamic item = _autoMapper.AutoMap();
			return item;
		}
	}
}
