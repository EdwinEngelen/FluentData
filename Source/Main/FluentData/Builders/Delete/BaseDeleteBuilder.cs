namespace FluentData
{
	internal abstract class BaseDeleteBuilder
	{
		protected BuilderData Data { get; set; }
		protected ActionsHandler Actions { get; set; }

		private IDbCommand Command
		{
			get
			{
				Data.DbCommand.Sql(Data.DbProvider.GetSqlForDeleteBuilder(Data));
				return Data.DbCommand;
			}
		}

		public BaseDeleteBuilder(IDbProvider provider, IDbCommand command, string name)
		{
			Data =  new BuilderData(provider, command, name);
			Actions = new ActionsHandler(Data);
		}

		public int Execute()
		{
			return Command.Execute();
		}
	}
}
