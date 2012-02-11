using System.Data;

namespace FluentData
{
	public class DbContextData
	{
		public bool UseTransaction { get; set; }
		public IDbConnection Connection { get; set; }
		public IsolationLevel IsolationLevel { get; set; }
		public IDbTransaction Transaction { get; set; }
		public TransactionStates TransactionState { get; set; }
		public IDbProvider DbProvider { get; set; }
		public string ConnectionString { get; set; }
		public IEntityFactory EntityFactory { get; set; }
		public DbProviderTypes Provider { get; set; }
		public bool ThrowExceptionIfAutoMapFails { get; set; }

		public DbContextData()
		{
			ThrowExceptionIfAutoMapFails = false;
			UseTransaction = false;
			IsolationLevel = IsolationLevel.ReadCommitted;
			EntityFactory = new EntityFactory();
		}
	}
}
