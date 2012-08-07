using System;
using System.Data;
using FluentData.Providers.Common;
using FluentData.Providers.Common.Builders;

namespace FluentData.Providers.SqlServerCompact
{
	internal class SqlServerCompactProvider : IDbProvider
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

		public T ExecuteReturnLastId<T>(DbCommandData data, string identityColumnName = null)
		{
			var lastId = default(T);

			data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				lastId = HandleExecuteReturnLastId<T>(data);
			});

			return lastId;
		}

		public void OnCommandExecuting(DbCommandData data)
		{
			
		}

		public string EscapeColumnName(string name)
		{
			return "[" + name + "]";
		}

		private T HandleExecuteReturnLastId<T>(DbCommandData data, string identityColumnName = null)
		{
			int recordsAffected = data.InnerCommand.ExecuteNonQuery();

			T lastId = default(T);

			if (recordsAffected > 0)
			{
				data.InnerCommand.CommandText = "select cast(@@identity as int)";

				var value = data.InnerCommand.ExecuteScalar();

				lastId = (T) value;
			}

			return lastId;
		}
	}
}
