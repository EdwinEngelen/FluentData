using System;

namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		public IDbContext UseTransaction(bool useTransaction)
		{
			ContextData.UseTransaction = useTransaction;
			return this;
		}

		public IDbContext UseSharedConnection(bool useSharedConnection)
		{
			ContextData.UseSharedConnection = useSharedConnection;
			return this;
		}

		public IDbContext IsolationLevel(IsolationLevel isolationLevel)
		{
			ContextData.IsolationLevel = isolationLevel;
			return this;
		}

		public IDbContext Commit()
		{
			TransactionAction(() => ContextData.Transaction.Commit());
			return this;
		}

		public IDbContext Rollback()
		{
			TransactionAction(() => ContextData.Transaction.Rollback());
			return this;
		}

		private void TransactionAction(Action action)
		{
			if(ContextData.Transaction == null)
				return;
			if(!ContextData.UseTransaction)
				throw new FluentDataException("Transaction support has not been enabled.");
			action();
			ContextData.Transaction = null;
		}
	}
}
