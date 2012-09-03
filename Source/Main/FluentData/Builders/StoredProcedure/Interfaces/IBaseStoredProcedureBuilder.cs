using System;
using System.Collections.Generic;

namespace FluentData
{
	public interface IBaseStoredProcedureBuilder
	{
		TValue ParameterValue<TValue>(string name);
		int Execute();
		List<dynamic> Query();
		TList Query<TEntity, TList>(Action<IDataReader, TEntity> customMapper) where TList : IList<TEntity>;
		List<TEntity> Query<TEntity>(Action<IDataReader, TEntity> customMapper = null);
		void QueryComplex<TEntity>(IList<TEntity> list, Action<IDataReader, IList<TEntity>> customMapper);
		dynamic QuerySingle();
		TEntity QuerySingle<TEntity>(Action<IDataReader, TEntity> customMapper = null);
		TEntity QuerySingleComplex<TEntity>(Func<IDataReader, TEntity> customMapper);
		TValue QueryValue<TValue>();
	}
}
