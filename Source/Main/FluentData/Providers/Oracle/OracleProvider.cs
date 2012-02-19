using System;
using System.Data;
using System.Text;
using FluentData.Providers.Common;
using FluentData.Providers.Common.Builders;

namespace FluentData.Providers.Oracle
{
	internal class OracleProvider : IDbProvider
	{
		public string ProviderName
		{ 
			get
			{
				return "Oracle.DataAccess.Client";
			} 
		}

		public bool SupportsOutputParameters
		{
			get { return true; }
		}

		public bool SupportsMultipleResultset
		{
			get { return false; }
		}

		public bool SupportsMultipleQueries
		{
			get { return true; }
		}

		public bool SupportsStoredProcedures
		{
			get { return true; }
		}

		public bool SupportsExecuteReturnLastIdWithNoIdentityColumn
		{
			get { return false; }
		}

		public IDbConnection CreateConnection(string connectionString)
		{
			return ConnectionFactory.CreateConnection(ProviderName, connectionString);
		}

		public string GetParameterName(string parameterName)
		{
			return ":" + parameterName;
		}

		public string GetSqlForInsertBuilder(BuilderData data)
		{
			return new InsertBuilderSqlGenerator().GenerateSql(":", data);
		}

		public string GetSqlForUpdateBuilder(BuilderData data)
		{
			return new UpdateBuilderSqlGenerator().GenerateSql(":", data);
		}

		public string GetSqlForDeleteBuilder(BuilderData data)
		{
			return new DeleteBuilderSqlGenerator().GenerateSql(":", data);
		}

		public string GetSqlForStoredProcedureBuilder(BuilderData data)
		{
			return data.ObjectName;
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
			data.DbCommand.ParameterOut("FluentDataLastInsertedId", data.DbContextData.DbProvider.GetDbTypeForClrType(typeof(T)));
			data.Sql.Append(string.Format(" returning {0} into :FluentDataLastInsertedId", identityColumnName));

			var lastId = default(T);

			data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				data.InnerCommand.ExecuteNonQuery();

				var parameter = (IDbDataParameter) data.InnerCommand.Parameters[":FluentDataLastInsertedId"];
				lastId = (T) parameter.Value;
			});

			return lastId;
		}

		public void BeforeDbCommandExecute(DbCommandData data)
		{
			if (data.InnerCommand.CommandType == CommandType.Text)
			{
				dynamic innerCommand = data.InnerCommand;
				innerCommand.BindByName = true;
			}
		}
	}
}
