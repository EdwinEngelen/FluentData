using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace FluentData
{
	public interface IDbCommand : IDisposable
	{
		IDbCommand ParameterOut(string name, DataTypes parameterType, int size = 0);
		IDbCommand Parameter(string name, object value);
		IDbCommand Parameter(string name, object value, DataTypes parameterType, ParameterDirection direction, int size = 0);
		IDbCommand Parameters(params object[] parameters);
		TParameterType ParameterValue<TParameterType>(string outputParameterName);
		int Execute();
		int ExecuteReturnLastId();
		T ExecuteReturnLastId<T>();
		int ExecuteReturnLastId(string identityColumnName);
		T ExecuteReturnLastId<T>(string identityColumnName);
		List<dynamic> Query();
		TList Query<TEntity, TList>(Action<IDataReader, TEntity> customMapper = null) where TList : IList<TEntity>;
		List<TEntity> Query<TEntity>(Action<IDataReader, TEntity> customMapper = null);
		TList QueryComplex<TEntity, TList>(Action<IDataReader, IList<TEntity>> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryComplex<TEntity>(Action<IDataReader, IList<TEntity>> customMapper);
		TList QueryNoAutoMap<TEntity, TList>(Func<IDataReader, TEntity> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper);
		dynamic QuerySingle();
		TEntity QuerySingle<TEntity>(Action<IDataReader, TEntity> customMapper = null);
		TEntity QuerySingleNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper);
		T QueryValue<T>();
		List<T> QueryValues<T>();
		DataTable QueryDataTable();
		IDbCommand Sql(string sql);
		IDbCommand Sql<T>(string sql, params Expression<Func<T, object>>[] mappingExpression);
		IDbCommand CommandType(DbCommandTypes dbCommandType);
	}
}
