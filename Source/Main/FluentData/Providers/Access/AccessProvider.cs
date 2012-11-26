using System;
using System.Data;
using System.Text;
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
				return "System.command.Data.OleDb";
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

		public string GetSelectBuilderAlias(string name, string alias)
		{
			return name + " as " + alias;
		}

		public string GetSqlForSelectBuilder(BuilderData data)
		{
			throw new NotImplementedException();
		}

		public string GetSqlForInsertBuilder(BuilderData data)
		{
			return new InsertBuilderSqlGenerator().GenerateSql(this, "@", data);
		}

		public string GetSqlForUpdateBuilder(BuilderData data)
		{
			return new UpdateBuilderSqlGenerator().GenerateSql(this, "@", data);
		}

		public string GetSqlForDeleteBuilder(BuilderData data)
		{
			return new DeleteBuilderSqlGenerator().GenerateSql(this, "@", data);
		}

		public string GetSqlForStoredProcedureBuilder(BuilderData data)
		{
			throw new NotImplementedException();
		}

		public DataTypes GetDbTypeForClrType(Type clrType)
		{
			return new DbTypeMapper().GetDbTypeForClrType(clrType);
		}

		public T ExecuteReturnLastId<T>(IDbCommand command, string identityColumnName = null)
		{
			var lastId = default(T);

			command.Data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				lastId = HandleExecuteReturnLastId<T>(command);
			});

			return lastId;
		}

		public void OnCommandExecuting(IDbCommand command)
		{
			
		}

		public string EscapeColumnName(string name)
		{
			return "[" + name + "]";
		}

		private T HandleExecuteReturnLastId<T>(IDbCommand command)
		{
			int recordsAffected = command.Data.InnerCommand.ExecuteNonQuery();

			T lastId = default(T);

			if (recordsAffected > 0)
			{
				command.Data.InnerCommand.CommandText = "select @@Identity";

				var value = command.Data.InnerCommand.ExecuteScalar();

				lastId = (T) value;
			}

			return lastId;
		}
	}
}
