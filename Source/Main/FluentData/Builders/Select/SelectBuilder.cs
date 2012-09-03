using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentData
{
	internal class SelectBuilder<TEntity> : ISelectBuilder<TEntity>
	{
		protected BuilderData Data { get; set; }
		protected ActionsHandler Actions { get; set; }

		private IDbCommand Command
		{
			get
			{
				if (Data.PagingItemsPerPage > 0
					&& string.IsNullOrEmpty(Data.OrderBy))
					throw new FluentDataException("Order by must defined when using Paging.");

				Data.Command.Sql(Data.Provider.GetSqlForSelectBuilder(Data));
				return Data.Command;
			}
		}

		public SelectBuilder(IDbProvider provider, IDbCommand command)
		{
			Data =  new BuilderData(provider, command, "");
			Actions = new ActionsHandler(Data);
		}

		public ISelectBuilder<TEntity> Select(string sql)
		{
			Data.Select += sql;
			return this;
		}

		public ISelectBuilder<TEntity> Select(string sql, Expression<Func<TEntity, object>> mapToProperty)
		{
			var alias = Data.Provider.GetSelectBuilderAlias(sql, ReflectionHelper.GetPropertyNameFromExpression(mapToProperty).Replace(".", "_"));
			if (Data.Select.Length > 0)
				Data.Select += ",";

			Data.Select += alias;
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

		public ISelectBuilder<TEntity> Parameters(params object[] parameters)
		{
			Data.Command.Parameters(parameters);
			return this;
		}

		public TList Query<TList>(Action<IDataReader, TEntity> customMapper = null) where TList : IList<TEntity>
		{
			return Command.Query<TEntity, TList>(customMapper);
		}

		public List<TEntity> Query(Action<IDataReader, TEntity> customMapper = null)
		{
			return Command.Query(customMapper);
		}

		public void QueryComplex(IList<TEntity> list, Action<IDataReader, IList<TEntity>> customMapper)
		{
			Command.QueryComplex<TEntity>(list, customMapper);
		}

		public TEntity QuerySingle(Action<IDataReader, TEntity> customMapper = null)
		{
			return Command.QuerySingle(customMapper);
		}

		public TEntity QuerySingleComplex(Func<IDataReader, TEntity> customMapper)
		{
			return Command.QuerySingleComplex(customMapper);
		}

		public TValue QueryValue<TValue>()
		{
			return Command.QueryValue<TValue>();

		}
	}
}
