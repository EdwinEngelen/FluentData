using FluentData.Providers;

namespace FluentData
{
	internal class InsertBuilder : BaseInsertBuilder, IInsertBuilder, IInsertUpdateBuilder
	{
		internal InsertBuilder(IDbProvider provider, IDbCommand command, string name)
			: base(provider, command, name)
		{
		}

		public IInsertBuilder Column(string columnName, object value)
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
