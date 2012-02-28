using System;
using System.Collections.Generic;

namespace FluentData
{
	internal class QueryValuesHandler<T> : BaseQueryHandler
	{
		public QueryValuesHandler(DbCommandData data)
			: base(data)
		{
		}

		public List<T> Execute()
		{
			var items = new List<T>();

			while (Data.Reader.Read())
			{
				T value;

				if (Data.Reader.GetFieldType(0) == typeof(T))
					value = (T) Data.Reader.GetValue(0);
				else
					value = (T) Convert.ChangeType(Data.Reader.GetValue(0), typeof(T));

				items.Add(value);
			}

			return items;
		}
	}
}
