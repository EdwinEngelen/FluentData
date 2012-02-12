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
					|| Data.Wheres.Count == 0)
					throw new FluentDataException("Columns or where filter have not yet been added.");

				Data.DbCommand.Sql(Data.DbProvider.GetSqlForUpdateBuilder(Data));
				return Data.DbCommand;
			}
		}

		public BaseUpdateBuilder(IDbProvider provider, IDbCommand command, string name)
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
