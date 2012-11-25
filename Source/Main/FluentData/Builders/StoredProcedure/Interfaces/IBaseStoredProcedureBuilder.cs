using System;
using System.Collections.Generic;

namespace FluentData
{
	public interface IBaseStoredProcedureBuilder
	{
		TValue ParameterValue<TValue>(string name);
		int Execute();
		TList Query<TEntity, TList>(Action<TEntity, IDataReader> customMapper) where TList : IList<TEntity>;
		List<TEntity> Query<TEntity>(Action<TEntity, IDataReader> customMapper = null);
		void QueryComplex<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper);
		TEntity QuerySingle<TEntity>(Action<TEntity, IDataReader> customMapper = null);
		TEntity QueryComplexSingle<TEntity>(Func<IDataReader, TEntity> customMapper);
	}
}
