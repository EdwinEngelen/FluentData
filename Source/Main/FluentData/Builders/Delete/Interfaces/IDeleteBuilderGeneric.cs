using System;
using System.Linq.Expressions;

namespace FluentData
{
	public interface IDeleteBuilder<T>
	{
		int Execute();
		IDeleteBuilder<T> Where<TProp>(Expression<Func<T, TProp>> expression);
		IDeleteBuilder<T> Where(string columnName, object value);
	}
}