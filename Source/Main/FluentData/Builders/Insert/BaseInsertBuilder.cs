namespace FluentData
{
	internal abstract class BaseInsertBuilder
	{
		public BuilderData Data { get; set; }
		protected ActionsHandler Actions { get; set; }

		public BaseInsertBuilder(IDbCommand command, string name)
		{
			Data =  new BuilderData(command, name);
			Actions = new ActionsHandler(Data);
		}

		private IDbCommand GetPreparedCommand()
		{
			Data.Command.ClearSql.Sql(Data.Command.Data.Context.Data.Provider.GetSqlForInsertBuilder(Data));
			return Data.Command;
		}

		public int Execute()
		{
			return GetPreparedCommand().Execute();
		}

		public T ExecuteReturnLastId<T>(string identityColumnName = null)
		{
			return GetPreparedCommand().ExecuteReturnLastId<T>(identityColumnName);
		}
	}
}
