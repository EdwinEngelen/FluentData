using System;
using System.Collections.Generic;
using System.Data;

namespace FluentData
{
	internal abstract class BaseStoredProcedureBuilder
	{
		protected BuilderData Data { get; set; }
		protected ActionsHandler Actions { get; set; }

		private IDbCommand Command
		{
			get
			{
				Data.Command.CommandType(DbCommandTypes.StoredProcedure);
				Data.Command.Sql(Data.Command.Data.Context.Data.Provider.GetSqlForStoredProcedureBuilder(Data));
				return Data.Command;
			}
		}

		public BaseStoredProcedureBuilder(IDbCommand command, string name)
		{
			Data =  new BuilderData(command, name);
			Actions = new ActionsHandler(Data);
		}


		public void Dispose()
		{
			Command.Dispose();
		}

		public TParameterType ParameterValue<TParameterType>(string outputParameterName)
		{
			return Command.ParameterValue<TParameterType>(outputParameterName);
		}

		public int Execute()
		{
			return Command.Execute();
		}

		public TList Query<TEntity, TList>(Action<TEntity, IDataReader> customMapper = null) where TList : IList<TEntity>
		{
			return Command.Query<TEntity, TList>(customMapper);
		}

		public List<TEntity> Query<TEntity>(Action<TEntity, IDataReader> customMapper = null)
		{
			return Command.Query<TEntity>(customMapper);
		}

		public void QueryComplex<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper)
		{
			Command.QueryComplex<TEntity>(list, customMapper);
		}

		public TEntity QuerySingle<TEntity>(Action<TEntity, IDataReader> customMapper = null)
		{
			return Command.QuerySingle<TEntity>(customMapper);
		}

		public TEntity QueryComplexSingle<TEntity>(Func<IDataReader, TEntity> customMapper)
		{
			return Command.QueryComplexSingle(customMapper);
		}

		public DataTable QueryDataTable()
		{
			return Command.QueryDataTable();
		}
	}
}
