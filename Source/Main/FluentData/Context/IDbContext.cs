using System;
using System.Dynamic;

namespace FluentData
{
	public interface IDbContext : IDisposable
	{
		DbContextData Data { get; }
		IDbContext IgnoreIfAutoMapFails(bool ignoreIfAutoMapFails);
		IDbContext UseTransaction(bool useTransaction);
		IDbContext UseSharedConnection(bool useSharedConnection);
		IDbContext CommandTimeout(int timeout);
		IDbCommand Sql(string sql, params object[] parameters);
		IDbCommand MultiResultSql { get; }
		ISelectBuilder<TEntity> Select<TEntity>(string sql);
		IInsertBuilder Insert(string tableName);
		IInsertBuilder<T> Insert<T>(string tableName, T item);
		IInsertBuilderDynamic Insert(string tableName, ExpandoObject item);
		IUpdateBuilder Update(string tableName);
		IUpdateBuilder<T> Update<T>(string tableName, T item);
		IUpdateBuilderDynamic Update(string tableName, ExpandoObject item);
		IDeleteBuilder Delete(string tableName);
		IDeleteBuilder<T> Delete<T>(string tableName, T item);
		IStoredProcedureBuilder StoredProcedure(string storedProcedureName);
		IStoredProcedureBuilder<T> StoredProcedure<T>(string storedProcedureName, T item);
		IStoredProcedureBuilderDynamic StoredProcedure(string storedProcedureName, ExpandoObject item);
		IDbContext EntityFactory(IEntityFactory entityFactory);
		IDbContext ConnectionString(string connectionString, IDbProvider dbProvider);
		IDbContext ConnectionStringName(string connectionstringName, IDbProvider dbProvider);
		IDbContext IsolationLevel(IsolationLevel isolationLevel);
		IDbContext Commit();
		IDbContext Rollback();
        IDbContext OnConnectionOpening(Action<ConnectionEventArgs> action);
        IDbContext OnConnectionOpened(Action<ConnectionEventArgs> action);
		IDbContext OnConnectionClosed(Action<ConnectionEventArgs> action);
        IDbContext OnExecuting(Action<CommandEventArgs> action);
		IDbContext OnExecuted(Action<CommandEventArgs> action);
		IDbContext OnError(Action<ErrorEventArgs> action);
	}
}