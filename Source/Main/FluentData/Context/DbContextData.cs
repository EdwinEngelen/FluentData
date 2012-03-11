using System;

namespace FluentData
{
	public class DbContextData
	{
		public bool UseTransaction { get; set; }
		public System.Data.IDbConnection Connection { get; set; }
		public IsolationLevel IsolationLevel { get; set; }
		public System.Data.IDbTransaction Transaction { get; set; }
		public TransactionStates TransactionState { get; set; }
		public IDbProvider Provider { get; set; }
		public string ConnectionString { get; set; }
		public IEntityFactory EntityFactory { get; set; }
		public DbProviderTypes ProviderType { get; set; }
		public bool IgnoreIfAutoMapFails { get; set; }
		public int CommandTimeout { get; set; }
		public Action<OnConnectionOpeningEventArgs> OnConnectionOpening { get; set; }
		public Action<OnConnectionOpenedEventArgs> OnConnectionOpened { get; set; }
		public Action<OnConnectionClosedEventArgs> OnConnectionClosed { get; set; }
		public Action<OnExecutingEventArgs> OnExecuting { get; set; }
		public Action<OnExecutedEventArgs> OnExecuted { get; set; }
		public Action<OnErrorEventArgs> OnError { get; set; }

		public DbContextData()
		{
			IgnoreIfAutoMapFails = false;
			UseTransaction = false;
			IsolationLevel = IsolationLevel.ReadCommitted;
			EntityFactory = new EntityFactory();
			CommandTimeout = Int32.MinValue;
		}
	}
}
