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

				if (Data.UseTransaction
					|| Data.UseSharedConnection)
				{
					if (Data.Connection == null)
						Data.Connection = Data.Provider.CreateConnection(Data.ConnectionString);
					connection = Data.Connection;
				}
				else
					connection = Data.Provider.CreateConnection(Data.ConnectionString);

				var cmd = connection.CreateCommand();
				cmd.Connection = connection;

				return new DbCommand(this, cmd);
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
	}
}
