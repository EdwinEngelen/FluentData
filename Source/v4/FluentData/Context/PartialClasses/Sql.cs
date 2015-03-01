using System.Data;

namespace FluentData
{
	public partial class DbContext
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
					{
						Data.Connection = (IDbConnection) Data.AdoNetProvider.CreateConnection();
						Data.Connection.ConnectionString = Data.ConnectionString;
					}
					connection = Data.Connection;
				}
				else
				{
					connection = (IDbConnection)Data.AdoNetProvider.CreateConnection();
					connection.ConnectionString = Data.ConnectionString;
				}
				var cmd = connection.CreateCommand();
				cmd.Connection = connection;

				return new DbCommand(this, cmd);
			}
		}

		public IDbCommand Sql(string sql, params object[] parameters)
		{
			var command = CreateCommand.Sql(sql).Parameters(parameters);
			return command;
		}

		public IDbCommand MultiResultSql
		{
            get
	        {
	            var command = CreateCommand.UseMultiResult(true);
	            return command;
	        }
	    }
	}
}
