using System;
using System.Collections.Generic;

namespace FluentData
{
	internal class SelectBuilder<TEntity> : ISelectBuilder<TEntity>
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

				Data.Command.Data.InnerCommand.CommandText = Data.Command.Data.Context.Data.Provider.GetSqlForSelectBuilder(Data);
				return Data.Command;
			}
		}

		public SelectBuilder(IDbCommand command)
		{
			Data =  new BuilderData(command, "");
			Actions = new ActionsHandler(Data);
		}

		public ISelectBuilder<TEntity> Select(string sql)
		{
			Data.Select += sql;
			return this;
		}

		public ISelectBuilder<TEntity> From(string sql)
		{
			Data.From += sql;
			return this;
		}

		public ISelectBuilder<TEntity> Where(string sql)
		{
			Data.WhereSql += sql;
			return this;
		}

		public ISelectBuilder<TEntity> AndWhere(string sql)
		{
			if(Data.WhereSql.Length > 0)
				Data.WhereSql += " and ";
			Data.WhereSql += sql;
			return this;
		}

		public ISelectBuilder<TEntity> OrWhere(string sql)
		{
			if(Data.WhereSql.Length > 0)
				Data.WhereSql += " or ";
			Data.WhereSql += sql;
			return this;
		}

		public ISelectBuilder<TEntity> Where(Operators operators, string sql)
		{
			if(Data.WhereSql.Length > 0)
			{
				if(operators == Operators.And)
					Data.WhereSql += " and ";
				else if(operators == Operators.Or)
					Data.WhereSql += " or ";
			}
			Data.WhereSql += sql;
			return this;
		}

		public ISelectBuilder<TEntity> OrderBy(string sql)
		{
			Data.OrderBy += sql;
			return this;
		}

		public ISelectBuilder<TEntity> GroupBy(string sql)
		{
			Data.GroupBy += sql;
			return this;
		}

		public ISelectBuilder<TEntity> Having(string sql)
		{
			Data.Having += sql;
			return this;
		}

		public ISelectBuilder<TEntity> Paging(int currentPage, int itemsPerPage)
		{
			Data.PagingCurrentPage = currentPage;
			Data.PagingItemsPerPage = itemsPerPage;
			return this;
		}

		public ISelectBuilder<TEntity> Parameter(string name, object value)
		{
			Data.Command.Parameter(name, value);
			return this;
		}

		public TList QueryMany<TList>(Action<TEntity, IDataReader> customMapper = null) where TList : IList<TEntity>
		{
			return Command.QueryMany<TEntity, TList>(customMapper);
		}

		public List<TEntity> QueryMany(Action<TEntity, IDataReader> customMapper = null)
		{
			return Command.QueryMany(customMapper);
		}

		public void QueryComplexMany(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper)
		{
			Command.QueryComplexMany<TEntity>(list, customMapper);
		}

		public TEntity QuerySingle(Action<TEntity, IDataReader> customMapper = null)
		{
			return Command.QuerySingle(customMapper);
		}

		public TEntity QueryComplexSingle(Func<IDataReader, TEntity> customMapper)
		{
			return Command.QueryComplexSingle(customMapper);
		}
	}
}
