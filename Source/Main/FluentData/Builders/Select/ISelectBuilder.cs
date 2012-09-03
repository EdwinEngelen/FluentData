using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentData
{
	public interface ISelectBuilder<TEntity>
	{
		ISelectBuilder<TEntity> Select(string sql);
		ISelectBuilder<TEntity> Select(string sql, Expression<Func<TEntity, object>> mapToProperty);
		ISelectBuilder<TEntity> From(string sql);
		ISelectBuilder<TEntity> Where(string sql);
		ISelectBuilder<TEntity> AndWhere(string sql);
		ISelectBuilder<TEntity> OrWhere(string sql);
		ISelectBuilder<TEntity> GroupBy(string sql);
		ISelectBuilder<TEntity> OrderBy(string sql);
		ISelectBuilder<TEntity> Having(string sql);
		ISelectBuilder<TEntity> Paging(int currentPage, int itemsPerPage);

		ISelectBuilder<TEntity> Parameter(string name, object value);
		ISelectBuilder<TEntity> Parameters(params object[] parameters);

		TList Query<TList>(Action<IDataReader, TEntity> customMapper = null) where TList : IList<TEntity>;
		List<TEntity> Query(Action<IDataReader, TEntity> customMapper = null);
		void QueryComplex(IList<TEntity> list, Action<IDataReader, IList<TEntity>> customMapper);
		TEntity QuerySingle(Action<IDataReader, TEntity> customMapper = null);
		TEntity QuerySingleComplex(Func<IDataReader, TEntity> customMapper);
	}
}
