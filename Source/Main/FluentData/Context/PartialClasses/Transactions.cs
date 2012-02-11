namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		public IDbContext UseTransaction
		{
			get
			{
				ContextData.UseTransaction = true;
				return this;
			}
		}

		public IDbContext IsolationLevel(IsolationLevel isolationLevel)
		{
			ContextData.IsolationLevel = isolationLevel;
			return this;
		}

		public IDbContext Commit()
		{
			VerifyTransactionSupport();

			if (ContextData.TransactionState == TransactionStates.Rollbacked)
				throw new FluentDbException("The transaction has already been rolledback");

			ContextData.Transaction.Commit();
			ContextData.TransactionState = TransactionStates.Commited;
			return this;
		}

		public IDbContext Rollback()
		{
			VerifyTransactionSupport();

			if (ContextData.TransactionState == TransactionStates.Commited)
				throw new FluentDbException("The transaction has already been commited");

			ContextData.Transaction.Rollback();
			ContextData.TransactionState = TransactionStates.Rollbacked;
			ContextData.Connection.Close();
			return this;
		}

		private void VerifyTransactionSupport()
		{
			if (!ContextData.UseTransaction)
				throw new FluentDbException("Transaction support has not been enabled.");
		}
	}
}
