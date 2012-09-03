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
				Data.Command.Sql(Data.Provider.GetSqlForInsertBuilder(Data));
				return Data.Command;
			}
		}

		public BaseInsertBuilder(IDbProvider provider, IDbCommand command, string name)
		{
			Data =  new BuilderData(provider, command, name);
			Actions = new ActionsHandler(Data);
		}

		public int Execute()
		{
			return Command.Execute();
		}

		public int ExecuteReturnLastId(string identityColumnName = null)
		{
			return Command.ExecuteReturnLastId(identityColumnName);
		}

		public T ExecuteReturnLastId<T>(string identityColumnName = null)
		{
			return Command.ExecuteReturnLastId<T>(identityColumnName);
		}
	}
}
