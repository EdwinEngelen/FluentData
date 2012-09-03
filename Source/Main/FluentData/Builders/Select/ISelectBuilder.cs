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
		ISelectBuilder<TEntity> GroupBy(string sql);
		ISelectBuilder<TEntity> OrderBy(string sql);
		ISelectBuilder<TEntity> Having(string sql);
		ISelectBuilder<TEntity> Paging(int currentPage, int itemsPerPage);

		ISelectBuilder<TEntity> Parameter(string name, object value);
		ISelectBuilder<TEntity> Parameters(params object[] parameters);

		TList Query<TList>() where TList : IList<TEntity>;
		TList Query<TList>(Action<IDataReader, TEntity> customMapper) where TList : IList<TEntity>;
		List<TEntity> Query();
		List<TEntity> Query(Action<IDataReader, TEntity> customMapper);
		TList QueryComplex<TList>(Action<IDataReader, IList<TEntity>> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryComplex(Action<IDataReader, IList<TEntity>> customMapper);
		TList QueryNoAutoMap<TList>(Func<IDataReader, TEntity> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryNoAutoMap(Func<IDataReader, TEntity> customMapper);
		TEntity QuerySingle();
		TEntity QuerySingle(Action<IDataReader, TEntity> customMapper);
		TEntity QuerySingleNoAutoMap(Func<IDataReader, TEntity> customMapper);
		TValue QueryValue<TValue>();
	}
}
