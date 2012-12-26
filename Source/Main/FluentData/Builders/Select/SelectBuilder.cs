using System;
using System.Collections.Generic;
using System.Data;

namespace FluentData
{
	internal class SelectBuilder : ISelectBuilder
	{
		public BuilderData Data { get; set; }
		protected ActionsHandler Actions { get; set; }

		private IDbCommand Command
		{
			get
			{
				if (Data.PagingItemsPerPage > 0
					&& string.IsNullOrEmpty(Data.OrderBy))
					throw new FluentDataException("Order by must defined when using Paging.");

				Data.Command.ClearSql.Sql(Data.Command.Data.Context.Data.Provider.GetSqlForSelectBuilder(Data));
				return Data.Command;
			}
		}

		public SelectBuilder(IDbCommand command)
		{
			Data =  new BuilderData(command, "");
			Actions = new ActionsHandler(Data);
		}

		public ISelectBuilder Select(string sql)
		{
			Data.Select += sql;
			return this;
		}

		public ISelectBuilder From(string sql)
		{
			Data.From += sql;
			return this;
		}

		public ISelectBuilder Where(string sql)
		{
			Data.WhereSql += sql;
			return this;
		}

		public ISelectBuilder AndWhere(string sql)
		{
			if(Data.WhereSql.Length > 0)
				Data.WhereSql += " and ";
			Data.WhereSql += sql;
			return this;
		}

		public ISelectBuilder OrWhere(string sql)
		{
			if(Data.WhereSql.Length > 0)
				Data.WhereSql += " or ";
			Data.WhereSql += sql;
			return this;
		}

		public ISelectBuilder OrderBy(string sql)
		{
			Data.OrderBy += sql;
			return this;
		}

		public ISelectBuilder GroupBy(string sql)
		{
			Data.GroupBy += sql;
			return this;
		}

		public ISelectBuilder Having(string sql)
		{
			Data.Having += sql;
			return this;
		}

		public ISelectBuilder Paging(int currentPage, int itemsPerPage)
		{
			Data.PagingCurrentPage = currentPage;
			Data.PagingItemsPerPage = itemsPerPage;
			return this;
		}

		public ISelectBuilder Parameter(string name, object value, DataTypes parameterType, ParameterDirection direction, int size)
		{
			Data.Command.Parameter(name, value, parameterType, direction, size);
			return this;
		}

		public ISelectBuilder Parameters(params object[] parameters)
		{
			Data.Command.Parameters(parameters);
			return this;
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
