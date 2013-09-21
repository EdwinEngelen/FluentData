using System;
using System.Data;
using FluentData.Providers.Common;
using FluentData.Providers.Common.Builders;

namespace FluentData
{
	public class MySqlProvider : IDbProvider
	{
		public string ProviderName
		{ 
			get
			{
				return "MySql.Data.MySqlClient";
			} 
		}

		public bool SupportsOutputParameters
		{
			get { return true; }
		}

		public bool SupportsMultipleResultsets
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

		public bool RequiresIdentityColumn
		{
			get { return false; }
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
			if (data.WhereSql.Length > 0)
				sql += " where " + data.WhereSql;
			if (data.GroupBy.Length > 0)
				sql += " group by " + data.GroupBy;
			if (data.Having.Length > 0)
				sql += " having " + data.Having;
			if (data.OrderBy.Length > 0)
				sql += " order by " + data.OrderBy;
			if (data.PagingItemsPerPage > 0
				&& data.PagingCurrentPage > 0)
			{
				sql += string.Format(" limit {0}, {1}", data.GetFromItems() - 1, data.GetToItems());
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
			return data.ObjectName;
		}

		public DataTypes GetDbTypeForClrType(Type clrType)
		{
			return new DbTypeMapper().GetDbTypeForClrType(clrType);
		}

		public object ExecuteReturnLastId<T>(IDbCommand command, string identityColumnName = null)
		{
			if(command.Data.Sql[command.Data.Sql.Length - 1] != ';')
				command.Sql(";");

			command.Sql("select LAST_INSERT_ID() as `LastInsertedId`");

			object lastId = null;

			command.Data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				lastId = command.Data.InnerCommand.ExecuteScalar();
			});

			return lastId;
		}

		public void OnCommandExecuting(IDbCommand command)
		{
		}

		public string EscapeColumnName(string name)
		{
			if (name.Contains("`"))
				return name;
			return "`" + name + "`";
		}
	}
}
