using System;
using System.Linq.Expressions;

namespace FluentData
{
	public interface IUpdateBuilder<T>
	{
		int Execute();
		IUpdateBuilder<T> AutoMap();
		IUpdateBuilder<T> IgnoreProperty<TProp>(Expression<Func<T, TProp>> expression);
		IUpdateBuilder<T> Where<TProp>(Expression<Func<T, TProp>> expression);
		IUpdateBuilder<T> Where(string columnName, object value);
		IUpdateBuilder<T> Column(string columnName, object value);
		IUpdateBuilder<T> Column<TProp>(Expression<Func<T, TProp>> expression);
	}
}