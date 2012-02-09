using System;
using System.Linq.Expressions;

namespace FluentData
{
	internal class InsertBuilder<T> : BaseInsertBuilder, IInsertBuilder<T>, IInsertUpdateBuilder<T>
	{
		internal InsertBuilder(IDbProvider provider, IDbCommand command, string name, T item)
			: base(provider, command, name)
		{
			Data.Item = item;
		}

		public IInsertBuilder<T> Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}

		public IInsertBuilder<T> Column<TProp>(Expression<Func<T, TProp>> expression)
		{
			Actions.ColumnValueAction(expression, false);
			return this;
		}

		public IInsertBuilder<T> AutoMap()
		{
			Actions.AutoMapColumnsAction(false);
			return this;
		}

		public IInsertBuilder<T> IgnoreProperty<TProp>(Expression<Func<T, TProp>> expression)
		{
			Actions.AutoMapIgnorePropertyAction<T, TProp>(expression);
			return this;
		}

		IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.AutoMap()
		{
			Actions.AutoMapColumnsAction(false);
			return this;
		}

		IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.IgnoreProperty<TProp>(Expression<Func<T, TProp>> expression)
		{
			Actions.AutoMapIgnorePropertyAction<T, TProp>(expression);
			return this;
		}

		IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}

		IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.Column<TProp>(Expression<Func<T, TProp>> expression)
		{
			Actions.ColumnValueAction(expression, false);
			return this;
		}
	}
}
