using System;
using System.Linq.Expressions;

namespace FluentData
{
	public interface IInsertUpdateBuilder<T>
	{
		IInsertUpdateBuilder<T> AutoMap();
		IInsertUpdateBuilder<T> IgnoreProperty<TProp>(Expression<Func<T, TProp>> expression);
		IInsertUpdateBuilder<T> Column(string columnName, object value);
		IInsertUpdateBuilder<T> Column<TProp>(Expression<Func<T, TProp>> expression);
	}
}
