using System;
using System.Collections.Generic;

namespace FluentData
{
	public interface IDbCommand : IDisposable
	{
		IDbCommand ParameterOut(string name, DataTypes parameterType);
		IDbCommand Parameter(string name, object value);
		IDbCommand Parameter(string name, object value, DataTypes parameterType, ParameterDirection direction);
		IDbCommand Parameters(params object[] parameters);
		TParameterType ParameterValue<TParameterType>(string outputParameterName);
		int Execute();
		int ExecuteReturnLastId();
		T ExecuteReturnLastId<T>();
		/// <summary>
		/// Only needed for Oracle.
		/// </summary>
		/// <param name="identityColumnName">Name of the column with the auto/sequence number.</param>
		int ExecuteReturnLastId(string identityColumnName);
		/// <summary>
		/// Only needed for Oracle.
		/// </summary>
		/// <param name="identityColumnName">Name of the column with the auto/sequence number.</param>
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
		TList QueryNoAutoMap<TEntity, TList>(Action<dynamic, TEntity> customMapper) where TList : IList<TEntity>;
		TList QueryNoAutoMap<TEntity, TList>(Action<IDataReader, TEntity> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryNoAutoMap<TEntity>(Action<dynamic, TEntity> customMapper);
		List<TEntity> QueryNoAutoMap<TEntity>(Action<IDataReader, TEntity> customMapper);
		dynamic QuerySingle();
		TEntity QuerySingle<TEntity>();
		TEntity QuerySingle<TEntity>(Action<IDataReader, TEntity> customMapper);
		T QueryValue<T>();
		IDbCommand Sql(string sql);
		IDbCommand CommandType(DbCommandTypes dbCommandType);
	}
}
