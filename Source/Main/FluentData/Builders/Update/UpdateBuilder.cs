namespace FluentData
{
	internal class UpdateBuilder : BaseUpdateBuilder, IUpdateBuilder, IInsertUpdateBuilder
	{
		internal UpdateBuilder(IDbProvider dbProvider, IDbCommand command, string name)
			: base(dbProvider, command, name)
		{
		}

		public virtual IUpdateBuilder Where(string columnName, object value)
		{
			Actions.WhereAction(columnName, value);
			return this;
		}

		public IUpdateBuilder Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value);
			return this;
		}

		IInsertUpdateBuilder IInsertUpdateBuilder.Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value);
			return this;
		}
	}
}
