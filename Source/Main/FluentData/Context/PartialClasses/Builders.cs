using System.Dynamic;
using FluentData;

namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		public IInsertBuilder Insert(string tableName)
		{
			return new InsertBuilder(ContextData.DbProvider, CreateCommand, tableName);
		}

		public IInsertBuilder<T> Insert<T>(string tableName, T item)
		{
			return new InsertBuilder<T>(ContextData.DbProvider, CreateCommand, tableName, item);
		}

		public IInsertBuilderDynamic Insert(string tableName, ExpandoObject item)
		{
			return new InsertBuilderDynamic(ContextData.DbProvider, CreateCommand, tableName, item);
		}

		public IUpdateBuilder Update(string tableName)
		{
			return new UpdateBuilder(ContextData.DbProvider, CreateCommand, tableName);
		}

		public IUpdateBuilder<T> Update<T>(string tableName, T item)
		{
			return new UpdateBuilder<T>(ContextData.DbProvider, CreateCommand, tableName, item);
		}

		public IUpdateBuilderDynamic Update(string tableName, ExpandoObject item)
		{
			return new UpdateBuilderDynamic(ContextData.DbProvider, CreateCommand, tableName, item);
		}

		public IDeleteBuilder Delete(string tableName)
		{
			return new DeleteBuilder(ContextData.DbProvider, CreateCommand, tableName);
		}

		public IDeleteBuilder<T> Delete<T>(string tableName, T item)
		{
			return new DeleteBuilder<T>(ContextData.DbProvider, CreateCommand, tableName, item);
		}

		private void VerifyStoredProcedureSupport()
		{
			if (!ContextData.DbProvider.SupportsStoredProcedures)
				throw new FluentDataException("The selected database does not support stored procedures.");
		}

		public IStoredProcedureBuilder StoredProcedure(string storedProcedureName)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilder(ContextData.DbProvider, CreateCommand, storedProcedureName);
		}

		public IStoredProcedureBuilder MultiResultStoredProcedure(string storedProcedureName)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilder(ContextData.DbProvider, CreateCommand.UseMultipleResultset, storedProcedureName);
		}

		public IStoredProcedureBuilder<T> StoredProcedure<T>(string storedProcedureName, T item)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilder<T>(ContextData.DbProvider, CreateCommand, storedProcedureName, item);
		}

		public IStoredProcedureBuilder<T> MultiResultStoredProcedure<T>(string storedProcedureName, T item)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilder<T>(ContextData.DbProvider, CreateCommand.UseMultipleResultset, storedProcedureName, item);
		}

		public IStoredProcedureBuilderDynamic StoredProcedure(string storedProcedureName, ExpandoObject item)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilderDynamic(ContextData.DbProvider, CreateCommand, storedProcedureName, item);
		}

		public IStoredProcedureBuilderDynamic MultiResultStoredProcedure(string storedProcedureName, ExpandoObject item)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilderDynamic(ContextData.DbProvider, CreateCommand.UseMultipleResultset, storedProcedureName, item);
		}
	}
}
