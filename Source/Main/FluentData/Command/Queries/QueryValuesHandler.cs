using System;
using System.Collections.Generic;

namespace FluentData
{
	internal class QueryValuesHandler<T>
	{
		public List<T> Execute(DbCommandData data)
		{
			var items = new List<T>();

			while (data.Reader.Read())
			{
				T value;

				if (data.Reader.GetFieldType(0) == typeof(T))
					value = (T) data.Reader.GetValue(0);
				else
					value = (T) Convert.ChangeType(data.Reader.GetValue(0), typeof(T));

				items.Add(value);
			}

			return items;
		}
	}
}
