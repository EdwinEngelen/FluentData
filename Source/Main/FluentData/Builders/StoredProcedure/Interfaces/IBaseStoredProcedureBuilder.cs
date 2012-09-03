using System;
using System.Collections.Generic;

namespace FluentData
{
	public interface IBaseStoredProcedureBuilder
	{
		TValue ParameterValue<TValue>(string name);
		int Execute();
		List<dynamic> Query();
		TList Query<TEntity, TList>() where TList : IList<TEntity>;
		TList Query<TEntity, TList>(Action<IDataReader, TEntity> customMapper) where TList : IList<TEntity>;
		List<TEntity> Query<TEntity>();
		List<TEntity> Query<TEntity>(Action<IDataReader, TEntity> customMapper);
		TList QueryComplex<TEntity, TList>(Action<IDataReader, IList<TEntity>> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryComplex<TEntity>(Action<IDataReader, IList<TEntity>> customMapper);
		TList QueryNoAutoMap<TEntity, TList>(Func<IDataReader, TEntity> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper);
		dynamic QuerySingle();
		TEntity QuerySingle<TEntity>();
		TEntity QuerySingle<TEntity>(Action<IDataReader, TEntity> customMapper);
		TEntity QuerySingleNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper);
		TValue QueryValue<TValue>();
	}
}
