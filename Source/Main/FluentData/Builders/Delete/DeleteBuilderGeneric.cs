using System;
using System.Linq.Expressions;

namespace FluentData
{
	internal class DeleteBuilder<T> : BaseDeleteBuilder, IDeleteBuilder<T>
	{
		public DeleteBuilder(IDbCommand command, string tableName, T item)
			: base(command, tableName)
		{
			Data.Item = item;
		}
		public IDeleteBuilder<T> Where(Expression<Func<T, object>> expression)
		{
			Actions.ColumnValueAction(expression);
			return this;
		}

		public IDeleteBuilder<T> Where(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value);
			return this;
		}
	}
}
