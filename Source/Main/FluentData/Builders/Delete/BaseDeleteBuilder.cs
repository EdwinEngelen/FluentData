namespace FluentData
{
	internal abstract class BaseDeleteBuilder
	{
		public BuilderData Data { get; set; }
		protected ActionsHandler Actions { get; set; }

		public BaseDeleteBuilder(IDbCommand command, string name)
		{
			Data =  new BuilderData(command, name);
			Actions = new ActionsHandler(Data);
		}

		public int Execute()
		{
			Data.Command.Sql(Data.Command.Data.Context.Data.Provider.GetSqlForDeleteBuilder(Data));			

			return Data.Command.Execute();
		}
	}
}
