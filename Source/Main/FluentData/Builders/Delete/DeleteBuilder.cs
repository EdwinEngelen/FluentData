namespace FluentData
{
	internal class DeleteBuilder : BaseDeleteBuilder, IDeleteBuilder
	{
		public DeleteBuilder(IDbProvider provider, IDbCommand command, string tableName)
			: base(provider, command, tableName)
		{
		}

		public IDeleteBuilder Where(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value);
			return this;
		}
	}
}
