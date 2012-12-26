using System;
using System.Collections.Generic;
using System.Data;

namespace FluentData
{
	internal abstract class BaseStoredProcedureBuilder : IQuery
	{
		protected BuilderData Data { get; set; }
		protected ActionsHandler Actions { get; set; }

		private IDbCommand Command
		{
			get
			{
				Data.Command.CommandType(DbCommandTypes.StoredProcedure);
				Data.Command.ClearSql.Sql(Data.Command.Data.Context.Data.Provider.GetSqlForStoredProcedureBuilder(Data));
				return Data.Command;
			}
		}

		public BaseStoredProcedureBuilder(IDbCommand command, string name)
		{
			Data = new BuilderData(command, name);
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

		public List<TEntity> QueryMany<TEntity>(Action<TEntity, IDataReader> customMapper = null)
		{
			return Command.QueryMany<TEntity>(customMapper);
		}

		public List<TEntity> QueryMany<TEntity>(Action<TEntity, dynamic> customMapper)
		{
			return Command.QueryMany<TEntity>(customMapper);
		}

		public TList QueryMany<TEntity, TList>(Action<TEntity, IDataReader> customMapper = null) where TList : IList<TEntity>
		{
			return Command.QueryMany<TEntity, TList>(customMapper);
		}

		public TList QueryMany<TEntity, TList>(Action<TEntity, dynamic> customMapper) where TList : IList<TEntity>
		{
			return Command.QueryMany<TEntity, TList>(customMapper);
		}

		public void QueryComplexMany<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper)
		{
			Command.QueryComplexMany<TEntity>(list, customMapper);
		}

		public void QueryComplexMany<TEntity>(IList<TEntity> list, Action<IList<TEntity>, dynamic> customMapper)
		{
			Command.QueryComplexMany<TEntity>(list, customMapper);
		}

		public TEntity QuerySingle<TEntity>(Action<TEntity, IDataReader> customMapper = null)
		{
			return Command.QuerySingle<TEntity>(customMapper);
		}

		public TEntity QuerySingle<TEntity>(Action<TEntity, dynamic> customMapper)
		{
			return Command.QuerySingle<TEntity>(customMapper);
		}

		public TEntity QueryComplexSingle<TEntity>(Func<IDataReader, TEntity> customMapper)
		{
			return Command.QueryComplexSingle(customMapper);
		}

		public TEntity QueryComplexSingle<TEntity>(Func<dynamic, TEntity> customMapper)
		{
			return Command.QueryComplexSingle(customMapper);
		}

		public DataTable QueryManyDataTable()
		{
			return Command.QueryManyDataTable();
		}
	}
}
