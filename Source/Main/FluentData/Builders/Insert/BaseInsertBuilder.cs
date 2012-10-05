namespace FluentData
{
	internal abstract class BaseInsertBuilder
	{
		protected BuilderData Data { get; set; }
		protected ActionsHandler Actions { get; set; }

		private IDbCommand Command
		{
			get
			{
				Data.Command.Sql(Data.Command.Data.Context.Data.Provider.GetSqlForInsertBuilder(Data));
				return Data.Command;
			}
		}

		public BaseInsertBuilder(IDbCommand command, string name)
		{
			Data =  new BuilderData(command, name);
			Actions = new ActionsHandler(Data);
		}

		public int Execute()
		{
			return Command.Execute();
		}

		public T ExecuteReturnLastId<T>(string identityColumnName = null)
		{
			return Command.ExecuteReturnLastId<T>(identityColumnName);
		}
	}
}
