using System;
using System.Linq.Expressions;

namespace FluentData
{
	internal class InsertBuilder<T> : BaseInsertBuilder, IInsertBuilder<T>, IInsertUpdateBuilder<T>
	{
		internal InsertBuilder(IDbCommand command, string name, T item)
			: base(command, name)
		{
			Data.Item = item;
		}

		public IInsertBuilder<T> Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value);
			return this;
		}

		public IInsertBuilder<T> Column(Expression<Func<T, object>> expression)
		{
			Actions.ColumnValueAction(expression);
			return this;
		}

		public IInsertBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties)
		{
			Actions.AutoMapColumnsAction(ignoreProperties);
			return this;
		}

		IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.AutoMap(params Expression<Func<T, object>>[] ignoreProperties)
		{
			Actions.AutoMapColumnsAction(ignoreProperties);
			return this;
		}

		IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value);
			return this;
		}

		IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.Column(Expression<Func<T, object>> expression)
		{
			Actions.ColumnValueAction(expression);
			return this;
		}
	}
}
