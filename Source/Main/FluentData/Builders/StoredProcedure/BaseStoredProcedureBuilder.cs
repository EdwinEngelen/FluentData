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
				Data.Command.Sql(Data.Provider.GetSqlForStoredProcedureBuilder(Data));
				return Data.Command;
			}
		}

		public BaseStoredProcedureBuilder(IDbProvider provider, IDbCommand command, string name)
		{
			Data =  new BuilderData(provider, command, name);
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

		public List<dynamic> Query()
		{
			return Command.Query();
		}

		public TList Query<TEntity, TList>() where TList : IList<TEntity>
		{
			return Command.Query<TEntity, TList>();
		}

		public TList Query<TEntity, TList>(Action<dynamic, TEntity> customMapper) where TList : IList<TEntity>
		{
			return Command.Query<TEntity, TList>(customMapper);
		}

		public TList Query<TEntity, TList>(Action<IDataReader, TEntity> customMapper) where TList : IList<TEntity>
		{
			return Command.Query<TEntity, TList>(customMapper);
		}

		public List<TEntity> Query<TEntity>()
		{
			return Command.Query<TEntity>();
		}

		public List<TEntity> Query<TEntity>(Action<dynamic, TEntity> customMapper)
		{
			return Command.Query<TEntity>(customMapper);
		}

		public List<TEntity> Query<TEntity>(Action<IDataReader, TEntity> customMapper)
		{
			return Command.Query<TEntity>(customMapper);
		}

		public TList QueryComplex<TEntity, TList>(Action<IDataReader, IList<TEntity>> customMapper) where TList : IList<TEntity>
		{
			return Command.QueryComplex<TEntity, TList>(customMapper);
		}

		public List<TEntity> QueryComplex<TEntity>(Action<IDataReader, IList<TEntity>> customMapper)
		{
			return Command.QueryComplex<TEntity>(customMapper);
		}

		public TList QueryNoAutoMap<TEntity, TList>(Func<dynamic, TEntity> customMapper) where TList : IList<TEntity>
		{
			return Command.QueryNoAutoMap<TEntity, TList>(customMapper);
		}

		public TList QueryNoAutoMap<TEntity, TList>(Func<IDataReader, TEntity> customMapper) where TList : IList<TEntity>
		{
			return Command.QueryNoAutoMap<TEntity, TList>(customMapper);
		}

		public List<TEntity> QueryNoAutoMap<TEntity>(Func<dynamic, TEntity> customMapper)
		{
			return Command.QueryNoAutoMap<TEntity>(customMapper);
		}

		public List<TEntity> QueryNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper)
		{
			return Command.QueryNoAutoMap<TEntity>(customMapper);
		}

		public dynamic QuerySingle()
		{
			return Command.QuerySingle();
		}

		public TEntity QuerySingle<TEntity>()
		{
			return Command.QuerySingle<TEntity>();
		}

		public TEntity QuerySingle<TEntity>(Action<IDataReader, TEntity> customMapper)
		{
			return Command.QuerySingle<TEntity>(customMapper);
		}

		public TEntity QuerySingle<TEntity>(Action<dynamic, TEntity> customMapper)
		{
			return Command.QuerySingle<TEntity>(customMapper);
		}

		public TEntity QuerySingleNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper)
		{
			return Command.QuerySingleNoAutoMap(customMapper);
		}

		public TEntity QuerySingleNoAutoMap<TEntity>(Func<dynamic, TEntity> customMapper)
		{
			return Command.QuerySingleNoAutoMap(customMapper);
		}

		public T QueryValue<T>()
		{
			return Command.QueryValue<T>();
		}

		public List<T> QueryValues<T>()
		{
			return Command.QueryValues<T>();
		}

		public DataTable QueryDataTable()
		{
			return Command.QueryDataTable();
		}
	}
}
