using System.Data;

namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		protected DbContextData ContextData;

		public DbContext()
		{
			ContextData = new DbContextData();
		}

		internal void CloseSharedConnection()
		{
			if (ContextData.Connection == null)
				return;

			if (ContextData.UseTransaction
				&& ContextData.Transaction != null)
					Rollback();

			ContextData.Connection.Close();

			if (ContextData.OnConnectionClosed != null)
				ContextData.OnConnectionClosed(new OnConnectionClosedEventArgs(ContextData.Connection));
		}

		public void Dispose()
		{
			CloseSharedConnection();
		}
	}
}
