using System;
using System.Collections.Generic;

namespace FluentData
{
	internal class SelectBuilder<TEntity> : ISelectBuilder<TEntity>
	{
		public SelectBuilderData Data { get; set; }
		protected ActionsHandler Actions { get; set; }

		public SelectBuilder(IDbCommand command)
		{
			Data =  new SelectBuilderData(command, "");
			Actions = new ActionsHandler(Data);
		}

		private IDbCommand GetPreparedDbCommand()
		{
			if (Data.PagingItemsPerPage > 0
					&& string.IsNullOrEmpty(Data.OrderBy))
				throw new FluentDataException("Order by must defined when using Paging.");

			Data.Command.ClearSql.Sql(Data.Command.Data.Context.Data.Provider.GetSqlForSelectBuilder(Data));
			return Data.Command;
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

		public ISelectBuilder<TEntity> Parameter(string name, object value, DataTypes parameterType, ParameterDirection direction, int size)
		{
			Data.Command.Parameter(name, value, parameterType, direction, size);
			return this;
		}

		public ISelectBuilder<TEntity> Parameters(params object[] parameters)
		{
			Data.Command.Parameters(parameters);
			return this;
		}
		public List<TEntity> QueryMany(Action<TEntity, IDataReader> customMapper = null)
		{
			return GetPreparedDbCommand().QueryMany<TEntity>(customMapper);
		}

		public List<TEntity> QueryMany(Action<TEntity, dynamic> customMapper)
		{
			return GetPreparedDbCommand().QueryMany<TEntity>(customMapper);
		}

		public TList QueryMany<TList>(Action<TEntity, IDataReader> customMapper = null) where TList : IList<TEntity>
		{
			return GetPreparedDbCommand().QueryMany<TEntity, TList>(customMapper);
		}

		public TList QueryMany<TList>(Action<TEntity, dynamic> customMapper) where TList : IList<TEntity>
		{
			return GetPreparedDbCommand().QueryMany<TEntity, TList>(customMapper);
		}

		public void QueryComplexMany(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper)
		{
			GetPreparedDbCommand().QueryComplexMany<TEntity>(list, customMapper);
		}

		public void QueryComplexMany(IList<TEntity> list, Action<IList<TEntity>, dynamic> customMapper)
		{
			GetPreparedDbCommand().QueryComplexMany<TEntity>(list, customMapper);
		}

		public TEntity QuerySingle(Action<TEntity, IDataReader> customMapper = null)
		{
			return GetPreparedDbCommand().QuerySingle<TEntity>(customMapper);
		}

		public TEntity QuerySingle(Action<TEntity, dynamic> customMapper)
		{
			return GetPreparedDbCommand().QuerySingle<TEntity>(customMapper);
		}

		public TEntity QueryComplexSingle(Func<IDataReader, TEntity> customMapper)
		{
			return GetPreparedDbCommand().QueryComplexSingle(customMapper);
		}

		public TEntity QueryComplexSingle(Func<dynamic, TEntity> customMapper)
		{
			return GetPreparedDbCommand().QueryComplexSingle(customMapper);
		}
	}
}
