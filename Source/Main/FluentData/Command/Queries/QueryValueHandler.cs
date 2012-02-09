namespace FluentData
{
	internal class QueryValueHandler<T> : BaseQueryHandler
	{
		public QueryValueHandler(DbCommandData data)
			: base(data)
		{
		}

		public T Execute()
		{
			T value = default(T);

			if (Data.Reader.Read())
				value = (T) Data.Reader.GetValue(0);

			return value;
		}
	}
}
