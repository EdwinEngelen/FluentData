using System;
namespace FluentData
{
	internal class QueryValueHandler<T>
	{
		public T Execute(DbCommandData data)
		{
			var value = default(T);

			if (data.Reader.Read())
			{
				if (data.Reader.GetFieldType(0) == typeof(T))
					value = (T) data.Reader.GetValue(0);
				else
					value = (T) Convert.ChangeType(data.Reader.GetValue(0), typeof(T));
			}
			
			return value;
		}
	}
}
