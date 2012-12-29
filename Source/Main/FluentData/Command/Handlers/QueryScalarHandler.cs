using System;

namespace FluentData
{
	public class QueryScalarHandler<TEntity> : IQueryTypeHandler<TEntity>
	{
		private readonly DbCommandData _data;

		public QueryScalarHandler(DbCommandData data)
		{
			_data = data;
		}

		public TEntity HandleType(Action<TEntity, IDataReader> customMapperReader, Action<TEntity, dynamic> customMapperDynamic)
		{
			var value = _data.Reader.GetValue(0);

			if (value == null)
				value = default(TEntity);
			else if (_data.Reader.GetFieldType(0) != typeof(TEntity))
				value = (Convert.ChangeType(value, typeof(TEntity)));
			return (TEntity)value;
		}
	}

	public interface IQueryTypeHandler<TEntity>
	{
		TEntity HandleType(Action<TEntity, IDataReader> customMapperReader, Action<TEntity, dynamic> customMapperDynamic);
	}
}
