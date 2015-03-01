using System;

namespace FluentData
{
	public partial class DbContext
	{
		public IDbContext UseTransaction(bool useTransaction)
		{
			Data.UseTransaction = useTransaction;
			return this;
		}

		public IDbContext UseSharedConnection(bool useSharedConnection)
		{
			Data.UseSharedConnection = useSharedConnection;
			return this;
		}

		public IDbContext IsolationLevel(IsolationLevel isolationLevel)
		{
			Data.IsolationLevel = isolationLevel;
			return this;
		}

		public IDbContext Commit()
		{
			TransactionAction(() => Data.Transaction.Commit());
			return this;
		}

		public IDbContext Rollback()
		{
			TransactionAction(() => Data.Transaction.Rollback());
			return this;
		}

		private void TransactionAction(Action action)
		{
			if(Data.Transaction == null)
				return;
			if(!Data.UseTransaction)
				throw new FluentDataException("Transaction support has not been enabled.");
			action();
			Data.Transaction = null;
		}
	}
}
