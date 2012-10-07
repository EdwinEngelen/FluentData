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
		public IDeleteBuilder<T> Where(Expression<Func<T, object>> expression, DataTypes parameterType, int size)
		{
			Actions.ColumnValueAction(expression, parameterType, size);
			return this;
		}

		public IDeleteBuilder<T> Where(string columnName, object value, DataTypes parameterType, int size)
		{
			Actions.ColumnValueAction(columnName, value, parameterType, size);
			return this;
		}
	}
}
