using System;
using System.Linq.Expressions;

namespace FluentData
{
	internal class DeleteBuilder<T> : BaseDeleteBuilder, IDeleteBuilder<T>
	{
		public DeleteBuilder(IDbProvider provider, IDbCommand command, string tableName, T item)
			: base(provider, command, tableName)
		{
			Data.Item = item;
		}
		public IDeleteBuilder<T> Where<TProp>(Expression<Func<T, TProp>> expression)
		{
			Actions.ColumnValueAction(expression, false);
			return this;
		}

		public IDeleteBuilder<T> Where(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}
	}
}
