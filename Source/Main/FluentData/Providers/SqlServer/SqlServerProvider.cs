using System;
using System.Data;
using System.Data.Common;
using System.Text;
using FluentData.Providers.Common;
using FluentData.Providers.SqlServer.Builders;

namespace FluentData.Providers.SqlServer
{
	internal class SqlServerProvider : IDbProvider
	{
		public string ProviderName
		{ 
			get
			{
				return "System.Data.SqlClient";
			} 
		}

		public bool SupportsOutputParameters
		{
			get { return true; }
		}

		public bool SupportsMultipleResultset
		{
			get { return true; }
		}

		public bool SupportsMultipleQueries
		{
			get { return true; }
		}

		public bool SupportsStoredProcedures
		{
			get { return true; }
		}

		public bool SupportsBindByNameForText
		{
			get { return false; }
		}

		public bool SupportsBindByNameForStoredProcedure
		{
			get { return false; }
		}

		public IDbConnection CreateConnection(string connectionString)
		{
			return ConnectionCreator.CreateConnection(ProviderName, connectionString);
		}

		public string GetParameterName(string parameterName)
		{
			return "@" + parameterName;
		}

		public string GetSqlForInsertBuilder(BuilderData data)
		{
			return new InsertBuilderSqlGenerator().GenerateSql(data);
		}

		public string GetSqlForUpdateBuilder(BuilderData data)
		{
			return new UpdateBuilderSqlGenerator().GenerateSql(data);
		}

		public string GetSqlForDeleteBuilder(BuilderData data)
		{
			return new DeleteBuilderSqlGenerator().GenerateSql(data);
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
			var lastInsertedParameterName = GetParameterName(GlobalConstants.LastInsertedIdParameterName);
			bool found = false;

			foreach (DbParameter parameter in data.InnerCommand.Parameters)
			{
				if (parameter.ParameterName == lastInsertedParameterName)
					found = true;
			}

			if (!found)
			{
				data.DbCommand.ParameterOut(GlobalConstants.LastInsertedIdParameterName, data.DbContextData.DbProvider.GetDbTypeForClrType(typeof(T)));
				data.Sql.Append(";set @LastInsertedId = SCOPE_IDENTITY();");
			}

			T lastId = default(T);

			data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				lastId = Execute<T>(data);
			});

			return lastId;
		}

		public void PrepareCommandBeforeExecute(DbCommandData data)
		{
			
		}

		private T Execute<T>(DbCommandData data)
		{
			object recordsAffected = data.InnerCommand.ExecuteNonQuery();

			var parameter = (IDbDataParameter) data.InnerCommand.Parameters["@" + GlobalConstants.LastInsertedIdParameterName];
			return (T) parameter.Value;
		}
	}
}
