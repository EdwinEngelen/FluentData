using System;
using System.Data;
using System.Text;
using FluentData.Providers.Common;
using FluentData.Providers.Common.Builders;

namespace FluentData.Providers.PostgreSql
{
	internal class PostgreSqlProvider : IDbProvider
	{
		public string ProviderName
		{ 
			get
			{
				return "Npgsql";
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
			return ":" + parameterName;
		}

		public string GetSelectBuilderAlias(string name, string alias)
		{
			return name + " as " + alias;
		}

		public string GetSqlForSelectBuilder(BuilderData data)
		{
			var sql = "";
			sql = "select " + data.Select;
			sql += " from " + data.From;
			if (data.WhereSql.Length > 0)
				sql += " where " + data.WhereSql;
			if (data.GroupBy.Length > 0)
				sql += " group by " + data.GroupBy;
			if (data.Having.Length > 0)
				sql += " having " + data.Having;
			if (data.OrderBy.Length > 0)
				sql += " order by " + data.OrderBy;
			if (data.PagingItemsPerPage > 0)
			{
				if (data.PagingItemsPerPage > 0)
					sql += " limit " + data.PagingItemsPerPage;
				sql += " offset " + (data.GetFromItems() - 1);
			}

			
			return sql;
		}

		public string GetSqlForInsertBuilder(BuilderData data)
		{
			return new InsertBuilderSqlGenerator().GenerateSql(this, ":", data);
		}

		public string GetSqlForUpdateBuilder(BuilderData data)
		{
			return new UpdateBuilderSqlGenerator().GenerateSql(this, ":", data);
		}

		public string GetSqlForDeleteBuilder(BuilderData data)
		{
			return new DeleteBuilderSqlGenerator().GenerateSql(this, ":", data);
		}

		public string GetSqlForStoredProcedureBuilder(BuilderData data)
		{
			return data.ObjectName;
		}

		public DataTypes GetDbTypeForClrType(Type clrType)
		{
			return new DbTypeMapper().GetDbTypeForClrType(clrType);
		}

		public T ExecuteReturnLastId<T>(IDbCommand command, string identityColumnName = null)
		{
			if (command.Data.Sql[command.Data.Sql.Length - 1] != ';')
				command.Data.Sql.Append(';');

			command.Data.Sql.Append("select lastval();");

			T lastId = default(T);

			command.Data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				object value = command.Data.InnerCommand.ExecuteScalar();

				if (value.GetType() == typeof(T))
					lastId = (T) value;

				lastId = (T) Convert.ChangeType(value, typeof(T));
			});

			return lastId;
		}

		public void OnCommandExecuting(IDbCommand command)
		{
		}

		public string EscapeColumnName(string name)
		{
			return name;
		}
	}
}
