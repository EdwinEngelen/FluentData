using System;
using System.Data;
using System.Text;
using FluentData;
using FluentData.Providers.Common;
using FluentData.Providers.Common.Builders;

namespace FluentData.Providers.Access
{
	internal class AccessProvider : IDbProvider
	{
		public string ProviderName
		{
			get
			{
				return "System.Data.OleDb";
			}
		}

		public bool SupportsOutputParameters
		{
			get { return false; }
		}

		public bool SupportsMultipleQueries
		{
			get { return false; }
		}

		public bool SupportsMultipleResultset
		{
			get { return false; }
		}

		public bool SupportsStoredProcedures
		{
			get { return false; }
		}

		public bool SupportsExecuteReturnLastIdWithNoIdentityColumn
		{
			get { return true; }
		}

		public IDbConnection CreateConnection(string connectionString)
		{
			return ConnectionFactory.CreateConnection(ProviderName, connectionString);
		}

		public string GetParameterName(string parameterName)
		{
			return "@" + parameterName;
		}

		public string GetSqlForInsertBuilder(BuilderData data)
		{
			return new InsertBuilderSqlGenerator().GenerateSql("@", data);
		}

		public string GetSqlForUpdateBuilder(BuilderData data)
		{
			return new UpdateBuilderSqlGenerator().GenerateSql("@", data);
		}

		public string GetSqlForDeleteBuilder(BuilderData data)
		{
			return new DeleteBuilderSqlGenerator().GenerateSql("@", data);
		}

		public string GetSqlForStoredProcedureBuilder(BuilderData data)
		{
			throw new NotImplementedException();
		}

		public DataTypes GetDbTypeForClrType(Type clrType)
		{
			return new DbTypeMapper().GetDbTypeForClrType(clrType);
		}

		public void FixInStatement(StringBuilder sql, ParameterCollection parameters)
		{
			new FixSqlInStatement().FixPotentialInSql(this, sql, parameters);
		}

		public T ExecuteReturnLastId<T>(DbCommandData data, string identityColumnName = null)
		{
			var lastId = default(T);

			data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				lastId = HandleExecuteReturnLastId<T>(data);
			});

			return lastId;
		}

		public void OnCommandExecuting(DbCommandData data)
		{
			
		}

		private T HandleExecuteReturnLastId<T>(DbCommandData data, string identityColumnName = null)
		{
			int recordsAffected = data.InnerCommand.ExecuteNonQuery();

			T lastId = default(T);

			if (recordsAffected > 0)
			{
				data.InnerCommand.CommandText = "select @@Identity";

				var value = data.InnerCommand.ExecuteScalar();

				lastId = (T) value;
			}

			return lastId;
		}
	}
}
