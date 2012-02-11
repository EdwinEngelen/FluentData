using System;
using System.Linq.Expressions;

namespace FluentData
{
	public interface IInsertBuilder<T>
	{
		int Execute();
		int ExecuteReturnLastId();
		TReturn ExecuteReturnLastId<TReturn>();
		/// <summary>
		/// Only needed for Oracle.
		/// </summary>
		/// <param name="identityColumnName">Name of the column with the auto/sequence number.</param>
		int ExecuteReturnLastId(string identityColumnName);
		/// <summary>
		/// Only needed for Oracle.
		/// </summary>
		/// <param name="identityColumnName">Name of the column with the auto/sequence number.</param>
		TReturn ExecuteReturnLastId<TReturn>(string identityColumnName);
		IInsertBuilder<T> AutoMap();
		IInsertBuilder<T> IgnoreProperty(Expression<Func<T, object>> expression);
		IInsertBuilder<T> Column(string columnName, object value);
		IInsertBuilder<T> Column(Expression<Func<T, object>> expression);
	}
}