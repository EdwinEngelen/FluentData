using System;

namespace FluentData
{
	internal class QueryScalarHandler<TEntity> : IQueryTypeHandler<TEntity>
	{
		private readonly DbCommandData _data;
		private Type _fieldType;

		public QueryScalarHandler(DbCommandData data)
		{
			_data = data;
		}

		public TEntity HandleType(Action<TEntity, IDataReader> customMapperReader, Action<TEntity, dynamic> customMapperDynamic)
		{
			var value = _data.Reader.GetValue(0);
			if (_fieldType == null)
				_fieldType = _data.Reader.GetFieldType(0);

			if (value == null)
				value = default(TEntity);
			else if (_fieldType != typeof(TEntity))
				value = (Convert.ChangeType(value, typeof(TEntity)));
			return (TEntity)value;
		}
	}
}
