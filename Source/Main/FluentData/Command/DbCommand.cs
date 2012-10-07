using System;

namespace FluentData
{
	internal partial class DbCommand : IDbCommand
	{
		public DbCommandData Data { get; private set; }

		public DbCommand(
			DbContext dbContext,
			System.Data.IDbCommand innerCommand)
		{
			Data = new DbCommandData(dbContext, innerCommand);
			Data.ExecuteQueryHandler = new ExecuteQueryHandler(this);
		}

		public IDbCommand UseMultipleResultset
		{
			get
			{
				if (!Data.Context.Data.Provider.SupportsMultipleResultset)
					throw new FluentDataException("The selected database does not support multiple resultset");

				Data.UseMultipleResultsets = true;
				return this;
			}
		}

		public IDbCommand CommandType(DbCommandTypes dbCommandType)
		{
			Data.InnerCommand.CommandType = (System.Data.CommandType) dbCommandType;
			return this;
		}

		internal void ClosePrivateConnection()
		{
			if (!Data.Context.Data.UseTransaction
				&& !Data.Context.Data.UseSharedConnection)
			{
				Data.InnerCommand.Connection.Close();

				if (Data.Context.Data.OnConnectionClosed != null)
					Data.Context.Data.OnConnectionClosed(new OnConnectionClosedEventArgs(Data.InnerCommand.Connection));
			}
		}

		public void Dispose()
		{
			if (Data.Reader != null)
				Data.Reader.Close();

			ClosePrivateConnection();
		}
	}
}
