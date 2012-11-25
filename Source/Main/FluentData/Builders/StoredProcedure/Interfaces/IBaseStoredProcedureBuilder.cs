using System;
using System.Collections.Generic;

namespace FluentData
{
	public interface IBaseStoredProcedureBuilder
	{
		TValue ParameterValue<TValue>(string name);
		int Execute();
		TList QueryMany<TEntity, TList>(Action<TEntity, IDataReader> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryMany<TEntity>(Action<TEntity, IDataReader> customMapper = null);
		void QueryComplexMany<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper);
		TEntity QuerySingle<TEntity>(Action<TEntity, IDataReader> customMapper = null);
		TEntity QueryComplexSingle<TEntity>(Func<IDataReader, TEntity> customMapper);
	}
}
