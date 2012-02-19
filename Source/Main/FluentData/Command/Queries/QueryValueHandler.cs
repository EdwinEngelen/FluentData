using System;
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
			{
				if (Data.Reader.GetFieldType(0) == typeof(T))
					value = (T) Data.Reader.GetValue(0);
				else
					value = (T) Convert.ChangeType(Data.Reader.GetValue(0), typeof(T));
			}
			
			return value;
		}
	}
}
