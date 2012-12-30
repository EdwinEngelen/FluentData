using System;
using System.Data;
using System.Data.Common;
using FluentData.Providers.Common;
using FluentData.Providers.Common.Builders;

namespace FluentData
{
	public class OracleProvider : IDbProvider
	{
		private static Lazy<DbProviderFactory> _dbProviderFactory = new Lazy<DbProviderFactory>(CreateDbProviderFactory, true);

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
				return "Oracle.DataAccess.Client";
			} 
		}

		public bool SupportsOutputParameters
		{
			get { return true; }
		}

		public bool SupportsMultipleResultsets
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

		public string GetSelectBuilderAlias(string name, string alias)
		{
			return name + " " + alias;
		}

		public string GetSqlForSelectBuilder(SelectBuilderData data)
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

				sql = string.Format(@"select * from
										(
											select {0}, 
												row_number() over (order by {1}) FLUENTDATA_ROWNUMBER
											{2}
										)
										where fluentdata_RowNumber between {3} and {4}
										order by fluentdata_RowNumber",
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

		public object ExecuteReturnLastId<T>(IDbCommand command, string identityColumnName = null)
		{
			command.ParameterOut("FluentDataLastId", command.Data.Context.Data.Provider.GetDbTypeForClrType(typeof(T)));
			command.Sql(" returning " + identityColumnName + " into :FluentDataLastId");


			object lastId = null;

			command.Data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				command.Data.InnerCommand.ExecuteNonQuery();

				lastId = command.ParameterValue<object>("FluentDataLastId");
			});

			return lastId;
		}

		public void OnCommandExecuting(IDbCommand command)
		{
			if (command.Data.InnerCommand.CommandType == CommandType.Text)
			{
				dynamic innerCommand = command.Data.InnerCommand;
				innerCommand.BindByName = true;
			}
		}

		public string EscapeColumnName(string name)
		{
			return name;
		}
	}
}
