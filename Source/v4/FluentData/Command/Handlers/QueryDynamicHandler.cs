using System;

namespace FluentData
{
	internal class QueryDynamicHandler<TEntity> : IQueryTypeHandler<TEntity>
	{
		private readonly DbCommandData _data;
		private readonly DynamicTypeAutoMapper _autoMapper;

		public bool IterateDataReader { get { return true; } }

		public QueryDynamicHandler(DbCommandData data)
		{
			_data = data;
			_autoMapper = new DynamicTypeAutoMapper(_data.Reader.InnerReader);
		}

		public object HandleType(Action<TEntity, IDataReader> customMapperReader, Action<TEntity, dynamic> customMapperDynamic)
		{
			var item = _autoMapper.AutoMap();
			return item;
		}
	}
}
