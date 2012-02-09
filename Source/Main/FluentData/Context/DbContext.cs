namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		protected DbContextData ContextData;

		public DbContext()
		{
			ContextData = new DbContextData();
		}

		public void Dispose()
		{
			if (ContextData.UseTransaction)
			{
				if (ContextData.TransactionState == TransactionStates.None)
					Rollback();
			}

			ContextData.Connection.Close();
		}
	}
}
