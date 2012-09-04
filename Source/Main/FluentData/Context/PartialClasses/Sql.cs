using System;
using System.Data;
using System.Linq.Expressions;
using System.Linq;

namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		private DbCommand CreateCommand
		{
			get
			{
				IDbConnection connection = null;

				if (ContextData.UseTransaction
					|| ContextData.UseSharedConnection)
				{
					if (ContextData.Connection == null)
						ContextData.Connection = ContextData.Provider.CreateConnection(ContextData.ConnectionString);
					connection = ContextData.Connection;
				}
				else
					connection = ContextData.Provider.CreateConnection(ContextData.ConnectionString);

				var cmd = connection.CreateCommand();
				cmd.Connection = connection;

				return new DbCommand(this, cmd, ContextData);
			}
		}

		public IDbCommand Sql(string sql, params object[] parameters)
		{
			var command = CreateCommand.Sql(sql);
			if(parameters != null)
			{
				for(var i = 0; i < parameters.Count(); i++)
					command.Parameter(i.ToString(), parameters[i]);
			}
			return command;
		}

		public IDbCommand Sql<T>(string sql, params Expression<Func<T, object>>[] mappingExpressions)
		{
			var command = CreateCommand.Sql(sql, mappingExpressions);
			
			return command;
		}

		public IDbCommand MultiResultSql()
		{
			return CreateCommand.UseMultipleResultset;
		}

		public IDbCommand MultiResultSql(string sql, params object[] parameters)
		{
			var command = CreateCommand.UseMultipleResultset.Sql(sql);
			if(parameters != null)
			{
				for(var i = 0; i < parameters.Count(); i++)
					command.Parameter(i.ToString(), parameters[i]);
			}
			return command;
		}

		public IDbCommand MultiResultSql<T>(string sql, params Expression<Func<T, object>>[] mappingExpressions)
		{
			var command = CreateCommand.UseMultipleResultset.Sql(sql, mappingExpressions);
			return command;
		}
	}
}
