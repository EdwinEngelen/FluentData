using System;
using System.Collections.Generic;

namespace FluentData
{
	public interface IBaseStoredProcedureBuilder
	{
		int Execute();
		List<dynamic> Query();
		TList Query<TEntity, TList>() where TList : IList<TEntity>;
		TList Query<TEntity, TList>(Action<dynamic, TEntity> customMapper) where TList : IList<TEntity>;
		TList Query<TEntity, TList>(Action<IDataReader, TEntity> customMapper) where TList : IList<TEntity>;
		List<TEntity> Query<TEntity>();
		List<TEntity> Query<TEntity>(Action<dynamic, TEntity> customMapper);
		List<TEntity> Query<TEntity>(Action<IDataReader, TEntity> customMapper);
		TList QueryComplex<TEntity, TList>(Action<IDataReader, IList<TEntity>> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryComplex<TEntity>(Action<IDataReader, IList<TEntity>> customMapper);
		TList QueryNoAutoMap<TEntity, TList>(Action<dynamic, TEntity> customMapper) where TList : IList<TEntity>;
		TList QueryNoAutoMap<TEntity, TList>(Action<IDataReader, TEntity> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryNoAutoMap<TEntity>(Action<dynamic, TEntity> customMapper);
		List<TEntity> QueryNoAutoMap<TEntity>(Action<IDataReader, TEntity> customMapper);
		dynamic QuerySingle();
		TEntity QuerySingle<TEntity>();
		TEntity QuerySingle<TEntity>(Action<IDataReader, TEntity> customMapper);
		TValue QueryValue<TValue>();
	}
}
