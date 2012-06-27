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

		public TList Query<TList>() where TList : IList<TEntity>
		{
			return Command.Query<TEntity, TList>();
		}

		public TList Query<TList>(Action<dynamic, TEntity> customMapper) where TList : IList<TEntity>
		{
			return Command.Query<TEntity, TList>(customMapper);
		}

		public TList Query<TList>(Action<IDataReader, TEntity> customMapper) where TList : IList<TEntity>
		{
			return Command.Query<TEntity, TList>(customMapper);
		}

		public List<TEntity> Query()
		{
			return Command.Query<TEntity>();
		}

		public List<TEntity> Query(Action<dynamic, TEntity> customMapper)
		{
			return Command.Query(customMapper);
		}

		public List<TEntity> Query(Action<IDataReader, TEntity> customMapper)
		{
			return Command.Query(customMapper);
		}

		public TList QueryComplex<TList>(Action<IDataReader, IList<TEntity>> customMapper) where TList : IList<TEntity>
		{
			return Command.QueryComplex<TEntity, TList>(customMapper);
		}

		public List<TEntity> QueryComplex(Action<IDataReader, IList<TEntity>> customMapper)
		{
			return Command.QueryComplex(customMapper);
		}

		public TList QueryNoAutoMap<TList>(Func<dynamic, TEntity> customMapper) where TList : IList<TEntity>
		{
			return Command.QueryNoAutoMap<TEntity, TList>(customMapper);
		}

		public TList QueryNoAutoMap<TList>(Func<IDataReader, TEntity> customMapper) where TList : IList<TEntity>
		{
			return Command.QueryNoAutoMap<TEntity, TList>(customMapper);
		}

		public List<TEntity> QueryNoAutoMap(Func<dynamic, TEntity> customMapper)
		{
			return Command.QueryNoAutoMap(customMapper);
		}

		public List<TEntity> QueryNoAutoMap(Func<IDataReader, TEntity> customMapper)
		{
			return Command.QueryNoAutoMap(customMapper);
		}

		public TEntity QuerySingle()
		{
			return Command.QuerySingle<TEntity>();
		}

		public TEntity QuerySingle(Action<IDataReader, TEntity> customMapper)
		{
			return Command.QuerySingle(customMapper);
		}

		public TEntity QuerySingle(Action<dynamic, TEntity> customMapper)
		{
			return Command.QuerySingle(customMapper);
		}

		public TEntity QuerySingleNoAutoMap(Func<IDataReader, TEntity> customMapper)
		{
			return Command.QuerySingleNoAutoMap(customMapper);
		}

		public TEntity QuerySingleNoAutoMap(Func<dynamic, TEntity> customMapper)
		{
			return Command.QuerySingleNoAutoMap(customMapper);
		}

		public TValue QueryValue<TValue>()
		{
			return Command.QueryValue<TValue>();

		}
	}
}
