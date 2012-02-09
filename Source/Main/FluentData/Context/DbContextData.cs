using System.Data;

namespace FluentData
{
	public class DbContextData
	{
		public bool UseTransaction;
		public IDbConnection Connection;
		public IsolationLevel IsolationLevel;
		public IDbTransaction Transaction;
		public TransactionStates TransactionState;
		public IDbProvider DbProvider;
		public string ConnectionString;
		public IEntityFactory EntityFactory;
		public DbProviderTypes Provider;
		public bool ThrowExceptionIfAutoMapFails;

		public DbContextData()
		{
			ThrowExceptionIfAutoMapFails = false;
			UseTransaction = false;
			IsolationLevel = System.Data.IsolationLevel.ReadCommitted;
			EntityFactory = new EntityFactory();
		}
	}
}
