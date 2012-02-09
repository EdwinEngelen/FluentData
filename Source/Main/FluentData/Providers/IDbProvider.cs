using System;
using System.Data;
using System.Text;

namespace FluentData
{
	public interface IDbProvider
	{
		string ProviderName { get; }
		bool SupportsMultipleResultset { get; }
		bool SupportsMultipleQueries { get; }
		bool SupportsOutputParameters { get; }
		bool SupportsStoredProcedures { get; }
		IDbConnection CreateConnection(string connectionString);
		string GetParameterName(string parameterName);
		string GetSqlForInsertBuilder(BuilderData data);
		string GetSqlForUpdateBuilder(BuilderData data);
		string GetSqlForDeleteBuilder(BuilderData data);
		string GetSqlForStoredProcedureBuilder(BuilderData data);
		DataTypes GetDbTypeForClrType(Type clrType);
		void FixInStatement(StringBuilder sql, ParameterCollection dbCommand);
		T ExecuteReturnLastId<T>(DbCommandData data, string identityColumnName);
		void PrepareCommandBeforeExecute(DbCommandData data);
	}
}
