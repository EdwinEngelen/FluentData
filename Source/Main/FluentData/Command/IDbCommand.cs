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
		TList Query<TEntity, TList>() where TList : IList<TEntity>;
		TList Query<TEntity, TList>(Action<dynamic, TEntity> customMapper) where TList : IList<TEntity>;
		TList Query<TEntity, TList>(Action<IDataReader, TEntity> customMapper) where TList : IList<TEntity>;
		List<TEntity> Query<TEntity>();
		List<TEntity> Query<TEntity>(Action<dynamic, TEntity> customMapper);
		List<TEntity> Query<TEntity>(Action<IDataReader, TEntity> customMapper);
		TList QueryComplex<TEntity, TList>(Action<IDataReader, IList<TEntity>> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryComplex<TEntity>(Action<IDataReader, IList<TEntity>> customMapper);
		TList QueryNoAutoMap<TEntity, TList>(Func<dynamic, TEntity> customMapper) where TList : IList<TEntity>;
		TList QueryNoAutoMap<TEntity, TList>(Func<IDataReader, TEntity> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryNoAutoMap<TEntity>(Func<dynamic, TEntity> customMapper);
		List<TEntity> QueryNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper);
		dynamic QuerySingle();
		TEntity QuerySingle<TEntity>();
		TEntity QuerySingle<TEntity>(Action<IDataReader, TEntity> customMapper);
		TEntity QuerySingle<TEntity>(Action<dynamic, TEntity> customMapper);
		TEntity QuerySingleNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper);
		TEntity QuerySingleNoAutoMap<TEntity>(Func<dynamic, TEntity> customMapper);
		T QueryValue<T>();
		List<T> QueryValues<T>();
		DataTable QueryDataTable();
		IDbCommand Sql(string sql);
		IDbCommand Sql<T>(string sql, params Expression<Func<T, object>>[] mappingExpression);
		IDbCommand CommandType(DbCommandTypes dbCommandType);
	}
}
