using System;
using System.Linq.Expressions;

namespace FluentData
{
	public interface IInsertUpdateBuilder<T>
	{
		IInsertUpdateBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties);
		IInsertUpdateBuilder<T> Column(string columnName, object value);
		IInsertUpdateBuilder<T> Column(Expression<Func<T, object>> expression);
	}
}
