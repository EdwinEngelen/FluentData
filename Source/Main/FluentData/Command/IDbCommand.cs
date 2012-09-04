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
		//IDbCommand Parameters(params object[] parameters);
		TParameterType ParameterValue<TParameterType>(string outputParameterName);
		int Execute();
		int ExecuteReturnLastId(string identityColumnName = null);
		T ExecuteReturnLastId<T>(string identityColumnName = null);
		List<dynamic> Query();
		dynamic QuerySingle();
		List<TEntity> Query<TEntity>(Action<TEntity, IDataReader> customMapper = null);
		TList Query<TEntity, TList>(Action<TEntity, IDataReader> customMapper = null) where TList : IList<TEntity>;
		void QueryComplex<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper);
		TEntity QuerySingle<TEntity>(Action<TEntity, IDataReader> customMapper = null);
		TEntity QuerySingleComplex<TEntity>(Func<IDataReader, TEntity> customMapper);
		DataTable QueryDataTable();
		IDbCommand Sql(string sql);
		IDbCommand Sql<T>(string sql, params Expression<Func<T, object>>[] mappingExpression);
		IDbCommand CommandType(DbCommandTypes dbCommandType);
	}
}
