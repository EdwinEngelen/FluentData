using System;
using System.Data;

namespace FluentData
{
	public interface IDbProvider
	{
		string ProviderName { get; }
		bool SupportsMultipleResultset { get; }
		bool SupportsMultipleQueries { get; }
		bool SupportsOutputParameters { get; }
		bool SupportsStoredProcedures { get; }
		bool SupportsExecuteReturnLastIdWithNoIdentityColumn { get; }
		IDbConnection CreateConnection(string connectionString);
		string GetParameterName(string parameterName);
		string GetSelectBuilderAlias(string name, string alias);
		string GetSqlForSelectBuilder(BuilderData data);
		string GetSqlForInsertBuilder(BuilderData data);
		string GetSqlForUpdateBuilder(BuilderData data);
		string GetSqlForDeleteBuilder(BuilderData data);
		string GetSqlForStoredProcedureBuilder(BuilderData data);
		DataTypes GetDbTypeForClrType(Type clrType);
		T ExecuteReturnLastId<T>(IDbCommand command, string identityColumnName);
		void OnCommandExecuting(IDbCommand command);
		string EscapeColumnName(string name);
	}
}
