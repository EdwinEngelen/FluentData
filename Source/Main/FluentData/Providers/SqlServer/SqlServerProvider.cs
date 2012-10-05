using System;
using System.Data;
using FluentData.Providers.Common;
using FluentData.Providers.Common.Builders;

namespace FluentData.Providers.SqlServer
{
	internal class SqlServerProvider : IDbProvider
	{
		public string ProviderName
		{ 
			get
			{
				return "System.data.SqlClient";
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
			return "@" + parameterName;
		}

		public string GetSelectBuilderAlias(string name, string alias)
		{
			return name + " as " + alias;
		}

		public string GetSqlForSelectBuilder(BuilderData data)
		{
			var sql = "";
			if (data.PagingItemsPerPage == 0)
			{
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
			}
			else if (data.PagingItemsPerPage > 0)
			{
				sql += " from " + data.From;
				if (data.WhereSql.Length > 0)
					sql += " where " + data.WhereSql;
				if (data.GroupBy.Length > 0)
					sql += " group by " + data.GroupBy;
				if (data.Having.Length > 0)
					sql += " having " + data.Having;

				sql = string.Format(@"with PagedPersons as
										(
											select top 100 percent {0}, row_number() over (order by {1}) as FLUENTDATA_ROWNUMBER
											{2}
										)
										select *
										from PagedPersons
										where fluentdata_RowNumber between {3} and {4}",
											data.Select,
											data.OrderBy,
											sql,
											data.GetFromItems(),
											data.GetToItems());
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

		public T ExecuteReturnLastId<T>(IDbCommand command, string identityColumnName = null)
		{
			if(command.Data.InnerCommand.CommandText[command.Data.InnerCommand.CommandText.Length - 1] != ';')
				command.Data.InnerCommand.CommandText += ';';

			command.Data.InnerCommand.CommandText += "select SCOPE_IDENTITY()";

			var lastId = default(T);

			command.Data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				var value = command.Data.InnerCommand.ExecuteScalar();

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
			return "[" + name + "]";
		}
	}
}
