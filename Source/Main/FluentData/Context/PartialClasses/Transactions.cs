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
				throw new FluentDataException("The transaction has already been rolledback");

			ContextData.Transaction.Commit();
			ContextData.TransactionState = TransactionStates.Committed;
			return this;
		}

		public IDbContext Rollback()
		{
			if (ContextData.TransactionState == TransactionStates.Rollbacked)
				return this;

			VerifyTransactionSupport();

			if (ContextData.TransactionState == TransactionStates.Committed)
				throw new FluentDataException("The transaction has already been commited");

			if (ContextData.Transaction != null)
				ContextData.Transaction.Rollback();
			ContextData.TransactionState = TransactionStates.Rollbacked;
			return this;
		}

		private void VerifyTransactionSupport()
		{
			if (!ContextData.UseTransaction)
				throw new FluentDataException("Transaction support has not been enabled.");
		}
	}
}
