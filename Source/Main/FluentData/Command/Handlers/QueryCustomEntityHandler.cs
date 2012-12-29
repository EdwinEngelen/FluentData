using System;

namespace FluentData
{
	internal class QueryCustomEntityHandler<TEntity> : IQueryTypeHandler<TEntity>
	{
		private readonly AutoMapper _autoMapper;
		private readonly DbCommandData _data;

		public QueryCustomEntityHandler(DbCommandData data)
		{
			_data = data;
			_autoMapper = new AutoMapper(_data, typeof(TEntity));
		}

		public TEntity HandleType(Action<TEntity, IDataReader> customMapperReader, Action<TEntity, dynamic> customMapperDynamic)
		{
			var item = (TEntity)_data.Context.Data.EntityFactory.Create(typeof(TEntity));

			if (customMapperReader != null)
				customMapperReader(item, _data.Reader);
			else if (customMapperDynamic != null)
				customMapperDynamic(item, new DynamicDataReader(_data.Reader.InnerReader));
			else
				_autoMapper.AutoMap(item);
			return item;
		}
	}
}
