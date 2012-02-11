using System;
using System.Linq.Expressions;

namespace FluentData
{
	public interface IUpdateBuilder<T>
	{
		int Execute();
		IUpdateBuilder<T> AutoMap();
		IUpdateBuilder<T> IgnoreProperty(Expression<Func<T, object>> expression);
		IUpdateBuilder<T> Where(Expression<Func<T, object>> expression);
		IUpdateBuilder<T> Where(string columnName, object value);
		IUpdateBuilder<T> Column(string columnName, object value);
		IUpdateBuilder<T> Column(Expression<Func<T, object>> expression);
	}
}