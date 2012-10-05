using FluentData.Providers;

namespace FluentData
{
	internal class InsertBuilder : BaseInsertBuilder, IInsertBuilder, IInsertUpdateBuilder
	{
		internal InsertBuilder(IDbCommand command, string name)
			: base(command, name)
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
