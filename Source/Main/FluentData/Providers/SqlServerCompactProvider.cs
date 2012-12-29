using System;
using System.Data;
using FluentData.Providers.Common;
using FluentData.Providers.Common.Builders;

namespace FluentData
{
	public class SqlServerCompactProvider : IDbProvider
	{
		public string ProviderName
		{
			get
			{
				return "System.Data.SqlServerCe.4.0";
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

		public string GetSqlForSelectBuilder(SelectBuilderData data)
		{
			var sql = "";
			sql = "select " + data.Select;
			sql += " from " + data.From;
			if(data.WhereSql.Length > 0)
				sql += " where " + data.WhereSql;
			if(data.GroupBy.Length > 0)
				sql += " group by " + data.GroupBy;
			if (data.Having.Length > 0)
				sql += " having " + data.Having;
			if (data.OrderBy.Length > 0)
				sql += " order by " + data.OrderBy;
			if (data.PagingItemsPerPage > 0)
			{
				sql += " offset " + (data.GetFromItems() - 1) + " rows";
				if (data.PagingItemsPerPage > 0)
					sql += " fetch next " + data.PagingItemsPerPage + " rows only";
			}

			return sql;
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
					command.Data.InnerCommand.CommandText = "select cast(@@identity as int)";

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
			if (name.Contains("["))
				return name;
			return "[" + name + "]";
		}
	}
}
