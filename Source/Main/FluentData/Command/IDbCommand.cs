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
		TParameterType ParameterValue<TParameterType>(string outputParameterName);
		int Execute();
		T ExecuteReturnLastId<T>(string identityColumnName = null);
		List<dynamic> Query();
		dynamic QuerySingle();
		List<TEntity> Query<TEntity>(Action<TEntity, IDataReader> customMapper = null);
		TList Query<TEntity, TList>(Action<TEntity, IDataReader> customMapper = null) where TList : IList<TEntity>;
		void QueryComplex<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper);
		TEntity QuerySingle<TEntity>(Action<TEntity, IDataReader> customMapper = null);
		TEntity QueryComplexSingle<TEntity>(Func<IDataReader, TEntity> customMapper);
		DataTable QueryDataTable();
		IDbCommand Sql(string sql);
		IDbCommand CommandType(DbCommandTypes dbCommandType);
	}
}
