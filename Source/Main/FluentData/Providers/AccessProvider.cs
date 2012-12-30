using System;
using System.Data;
using System.Data.Common;
using FluentData.Providers.Common;
using FluentData.Providers.Common.Builders;

namespace FluentData
{
	public class AccessProvider : IDbProvider
	{
		private static readonly Lazy<DbProviderFactory> _dbProviderFactory = new Lazy<DbProviderFactory>(CreateDbProviderFactory, true);

		private static DbProviderFactory CreateDbProviderFactory()
		{
			return DbProviderFactories.GetFactory(ProviderName);
		}

		public IDbConnection CreateConnection()
		{
			return _dbProviderFactory.Value.CreateConnection();
		}

		public static string ProviderName
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

		public bool SupportsMultipleResultsets
		{
			get { return false; }
		}

		public bool SupportsStoredProcedures
		{
			get { return false; }
		}

		public bool RequiresIdentityColumn
		{
			get { return false; }
		}

		public string GetParameterName(string parameterName)
		{
			return "@" + parameterName;
		}

		public string GetSelectBuilderAlias(string name, string alias)
		{
			return name + " as " + alias;
		}

		public string GetSqlForSelectBuilder(SelectBuilderData data)
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

		public object ExecuteReturnLastId<T>(IDbCommand command, string identityColumnName = null)
		{
			object lastId = null;

			command.Data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				var recordsAffected = command.Data.InnerCommand.ExecuteNonQuery();

				if (recordsAffected > 0)
				{
					command.Data.InnerCommand.CommandText = "select @@Identity";

					lastId = command.Data.InnerCommand.ExecuteScalar();
				}
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

		public bool IsColumnNameEscaped(string name)
		{
			if (name.Contains("["))
				return true;
			return false;
		}
	}
}
