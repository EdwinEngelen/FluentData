using System;
using System.Linq.Expressions;

namespace FluentData
{
	public interface IInsertBuilder<T>
	{
		int Execute();
		TReturn ExecuteReturnLastId<TReturn>(string identityColumnName = null);
		IInsertBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties);
		IInsertBuilder<T> Column(string columnName, object value);
		IInsertBuilder<T> Column(Expression<Func<T, object>> expression);
	}
}