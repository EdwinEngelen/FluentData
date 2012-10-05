namespace FluentData
{
	internal abstract class BaseUpdateBuilder
	{
		protected BuilderData Data { get; set; }
		protected ActionsHandler Actions { get; set; }

		private IDbCommand Command
		{
			get
			{
				if (Data.Columns.Count == 0
					|| Data.Where.Count == 0)
					throw new FluentDataException("Columns or where filter have not yet been added.");

				Data.Command.Sql(Data.Command.Data.Context.Data.Provider.GetSqlForUpdateBuilder(Data));
				return Data.Command;
			}
		}

		public BaseUpdateBuilder(IDbProvider provider, IDbCommand command, string name)
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
