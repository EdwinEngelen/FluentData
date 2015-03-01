using System;
using System.Collections.Generic;
using System.Data;

namespace FluentData
{
	public interface ISelectBuilder<TEntity>
	{
		SelectBuilderData Data { get; set; }
		ISelectBuilder<TEntity> Select(string sql);
		ISelectBuilder<TEntity> From(string sql);
		ISelectBuilder<TEntity> Where(string sql);
		ISelectBuilder<TEntity> AndWhere(string sql);
		ISelectBuilder<TEntity> OrWhere(string sql);
		ISelectBuilder<TEntity> GroupBy(string sql);
		ISelectBuilder<TEntity> OrderBy(string sql);
		ISelectBuilder<TEntity> Having(string sql);
		ISelectBuilder<TEntity> Paging(int currentPage, int itemsPerPage);
		ISelectBuilder<TEntity> Parameter(string name, object value, DataTypes parameterType = DataTypes.Object, ParameterDirection direction = ParameterDirection.Input, int size = 0);
		ISelectBuilder<TEntity> Parameters(params object[] parameters);

		List<TEntity> QueryMany(Action<TEntity, IDataReader> customMapper = null);
		List<TEntity> QueryMany(Action<TEntity, dynamic> customMapper);
		TList QueryMany<TList>(Action<TEntity, IDataReader> customMapper = null) where TList : IList<TEntity>;
		TList QueryMany<TList>(Action<TEntity, dynamic> customMapper) where TList : IList<TEntity>;
		void QueryComplexMany(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper);
		void QueryComplexMany(IList<TEntity> list, Action<IList<TEntity>, dynamic> customMapper);
		TEntity QuerySingle(Action<TEntity, IDataReader> customMapper = null);
		TEntity QuerySingle(Action<TEntity, dynamic> customMapper);
		TEntity QueryComplexSingle(Func<IDataReader, TEntity> customMapper);
		TEntity QueryComplexSingle(Func<dynamic, TEntity> customMapper);
	}
}
