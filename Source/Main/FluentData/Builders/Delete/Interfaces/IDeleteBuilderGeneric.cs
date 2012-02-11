using System;
using System.Linq.Expressions;

namespace FluentData
{
	public interface IDeleteBuilder<T>
	{
		int Execute();
		IDeleteBuilder<T> Where(Expression<Func<T, object>> expression);
		IDeleteBuilder<T> Where(string columnName, object value);
	}
}