using System;
using System.Collections.Generic;
using System.Data;

namespace FluentData
{
	public interface IDbCommand : IDisposable
	{
		DbCommandData Data { get; }
		IDbCommand ParameterOut(string name, DataTypes parameterType, int size = 0);
		IDbCommand Parameter(string name, object value, DataTypes parameterType = DataTypes.Object, ParameterDirection direction = ParameterDirection.Input, int size = 0);
		IDbCommand Parameters(params object[] parameters);
		TParameterType ParameterValue<TParameterType>(string outputParameterName);
		int Execute();
		T ExecuteReturnLastId<T>(string identityColumnName = null);
		List<TEntity> QueryMany<TEntity>(Action<TEntity, IDataReader> customMapper = null);
		TList QueryMany<TEntity, TList>(Action<TEntity, IDataReader> customMapper = null) where TList : IList<TEntity>;
		void QueryComplexMany<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper);
		TEntity QuerySingle<TEntity>(Action<TEntity, IDataReader> customMapper = null);
		TEntity QueryComplexSingle<TEntity>(Func<IDataReader, TEntity> customMapper);
		DataTable QueryDataTable();
		IDbCommand Sql(string sql);
		IDbCommand CommandType(DbCommandTypes dbCommandType);
	}
}
