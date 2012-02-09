namespace FluentData
{
	internal partial class DbCommand
	{
		public T QueryValue<T>()
		{
			T value = default(T);

			_data.QueryExecuter.ExecuteQueryHandler(true,
				() =>
				{
					value = new QueryValueHandler<T>(_data).Execute();
				});

			return value;
		}
	}
}
