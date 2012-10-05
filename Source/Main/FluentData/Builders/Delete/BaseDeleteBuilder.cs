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
				Data.Command.Sql(Data.Command.Data.Context.Data.Provider.GetSqlForDeleteBuilder(Data));
				return Data.Command;
			}
		}

		public BaseDeleteBuilder(IDbCommand command, string name)
		{
			Data =  new BuilderData(command, name);
			Actions = new ActionsHandler(Data);
		}

		public int Execute()
		{
			return Command.Execute();
		}
	}
}
