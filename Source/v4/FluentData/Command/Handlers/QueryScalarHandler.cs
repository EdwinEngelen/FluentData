using System;

namespace FluentData
{
	internal class QueryScalarHandler<TEntity> : IQueryTypeHandler<TEntity>
	{
		private readonly DbCommandData _data;
		private Type _fieldType;

		public bool IterateDataReader { get { return true; } }

		public QueryScalarHandler(DbCommandData data)
		{
			_data = data;
		}

		public object HandleType(Action<TEntity, IDataReader> customMapperReader, Action<TEntity, dynamic> customMapperDynamic)
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
