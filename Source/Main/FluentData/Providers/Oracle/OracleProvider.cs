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

		public string GetSelectBuilderAlias(string name, string alias)
		{
			return name + " " + alias;
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

		public T ExecuteReturnLastId<T>(DbCommandData data, string identityColumnName = null)
		{
			data.Command.ParameterOut("FluentDataLastId", data.ContextData.Provider.GetDbTypeForClrType(typeof(T)));
			data.Command.Sql(" returning " + identityColumnName + " into :FluentDataLastId");

			var lastId = default(T);

			data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				data.InnerCommand.ExecuteNonQuery();

				lastId = data.Command.ParameterValue<T>("FluentDataLastId");
			});

			return lastId;
		}

		public void OnCommandExecuting(DbCommandData data)
		{
			if (data.InnerCommand.CommandType == CommandType.Text)
			{
				dynamic innerCommand = data.InnerCommand;
				innerCommand.BindByName = true;
			}
		}

		public string EscapeColumnName(string name)
		{
			return name;
		}
	}
}
