using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		public ISelectBuilder<TEntity> Select<TEntity>(string sql)
		{
			return new SelectBuilder<TEntity>(ContextData.Provider, CreateCommand).Select(sql);
		}

		public ISelectBuilder<TEntity> Select<TEntity>(string sql, Expression<Func<TEntity, object>> mapToProperty)
		{
			return new SelectBuilder<TEntity>(ContextData.Provider, CreateCommand).Select(sql, mapToProperty);
		}

		public IInsertBuilder Insert(string tableName)
		{
			return new InsertBuilder(ContextData.Provider, CreateCommand, tableName);
		}

		public IInsertBuilder<T> Insert<T>(string tableName, T item)
		{
			return new InsertBuilder<T>(ContextData.Provider, CreateCommand, tableName, item);
		}

		public IInsertBuilderDynamic Insert(string tableName, ExpandoObject item)
		{
			return new InsertBuilderDynamic(ContextData.Provider, CreateCommand, tableName, item);
		}

		public IUpdateBuilder Update(string tableName)
		{
			return new UpdateBuilder(ContextData.Provider, CreateCommand, tableName);
		}

		public IUpdateBuilder<T> Update<T>(string tableName, T item)
		{
			return new UpdateBuilder<T>(ContextData.Provider, CreateCommand, tableName, item);
		}

		public IUpdateBuilderDynamic Update(string tableName, ExpandoObject item)
		{
			return new UpdateBuilderDynamic(ContextData.Provider, CreateCommand, tableName, item);
		}

		public IDeleteBuilder Delete(string tableName)
		{
			return new DeleteBuilder(ContextData.Provider, CreateCommand, tableName);
		}

		public IDeleteBuilder<T> Delete<T>(string tableName, T item)
		{
			return new DeleteBuilder<T>(ContextData.Provider, CreateCommand, tableName, item);
		}

		private void VerifyStoredProcedureSupport()
		{
			if (!ContextData.Provider.SupportsStoredProcedures)
				throw new FluentDataException("The selected database does not support stored procedures.");
		}

		public IStoredProcedureBuilder StoredProcedure(string storedProcedureName)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilder(ContextData.Provider, CreateCommand, storedProcedureName);
		}

		public IStoredProcedureBuilder MultiResultStoredProcedure(string storedProcedureName)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilder(ContextData.Provider, CreateCommand.UseMultipleResultset, storedProcedureName);
		}

		public IStoredProcedureBuilder<T> StoredProcedure<T>(string storedProcedureName, T item)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilder<T>(ContextData.Provider, CreateCommand, storedProcedureName, item);
		}

		public IStoredProcedureBuilder<T> MultiResultStoredProcedure<T>(string storedProcedureName, T item)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilder<T>(ContextData.Provider, CreateCommand.UseMultipleResultset, storedProcedureName, item);
		}

		public IStoredProcedureBuilderDynamic StoredProcedure(string storedProcedureName, ExpandoObject item)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilderDynamic(ContextData.Provider, CreateCommand, storedProcedureName, item);
		}

		public IStoredProcedureBuilderDynamic MultiResultStoredProcedure(string storedProcedureName, ExpandoObject item)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilderDynamic(ContextData.Provider, CreateCommand.UseMultipleResultset, storedProcedureName, item);
		}
	}
}
