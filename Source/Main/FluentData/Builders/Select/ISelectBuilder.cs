using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentData
{
	public interface ISelectBuilder<TEntity>
	{
		ISelectBuilder<TEntity> Select(string sql);
		ISelectBuilder<TEntity> From(string sql);
		ISelectBuilder<TEntity> Where(string sql);
		ISelectBuilder<TEntity> WhereAnd(string sql);
		ISelectBuilder<TEntity> WhereOr(string sql);
		ISelectBuilder<TEntity> GroupBy(string sql);
		ISelectBuilder<TEntity> OrderBy(string sql);
		ISelectBuilder<TEntity> Having(string sql);
		ISelectBuilder<TEntity> Paging(int currentPage, int itemsPerPage);

		ISelectBuilder<TEntity> Parameter(string name, object value);

		TList QueryMany<TList>(Action<TEntity, IDataReader> customMapper = null) where TList : IList<TEntity>;
		List<TEntity> QueryMany(Action<TEntity, IDataReader> customMapper = null);
		void QueryComplexMany(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper);
		TEntity QuerySingle(Action<TEntity, IDataReader> customMapper = null);
		TEntity QueryComplexSingle(Func<IDataReader, TEntity> customMapper);
	}
}
