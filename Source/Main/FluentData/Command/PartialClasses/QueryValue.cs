using System.Collections.Generic;

namespace FluentData
{
	internal partial class DbCommand
	{
		public T QueryValue<T>()
		{
			T value = default(T);

			_data.ExecuteQueryHandler.ExecuteQuery(true,
				() =>
				{
					value = new QueryValueHandler<T>().Execute(_data);
				});

			return value;
		}

		public List<T> QueryValues<T>()
		{
			List<T> values = null;

			_data.ExecuteQueryHandler.ExecuteQuery(true,
				() =>
				{
					values = new QueryValuesHandler<T>().Execute(_data);
				});

			return values;
		}
	}
}
