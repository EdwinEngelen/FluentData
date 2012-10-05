using System.Data;

namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		public DbContextData Data { get; private set; }

		public DbContext()
		{
			Data = new DbContextData();
		}

		internal void CloseSharedConnection()
		{
			if (Data.Connection == null)
				return;

			if (Data.UseTransaction
				&& Data.Transaction != null)
					Rollback();

			Data.Connection.Close();

			if (Data.OnConnectionClosed != null)
				Data.OnConnectionClosed(new OnConnectionClosedEventArgs(Data.Connection));
		}

		public void Dispose()
		{
			CloseSharedConnection();
		}
	}
}
