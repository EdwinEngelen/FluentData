using System;

namespace FluentData
{
	public class DbContextData
	{
		public bool UseTransaction { get; set; }
		public bool UseSharedConnection { get; set; }
		public System.Data.IDbConnection Connection { get; set; }
		public System.Data.Common.DbProviderFactory AdoNetProvider { get; set; }
		public IsolationLevel IsolationLevel { get; set; }
		public System.Data.IDbTransaction Transaction { get; set; }
		public IDbProvider FluentDataProvider { get; set; }
		public string ConnectionString { get; set; }
		public IEntityFactory EntityFactory { get; set; }
		public bool IgnoreIfAutoMapFails { get; set; }
		public int CommandTimeout { get; set; }
        public Action<ConnectionEventArgs> OnConnectionOpening { get; set; }
        public Action<ConnectionEventArgs> OnConnectionOpened { get; set; }
		public Action<ConnectionEventArgs> OnConnectionClosed { get; set; }
		public Action<CommandEventArgs> OnExecuting { get; set; }
		public Action<CommandEventArgs> OnExecuted { get; set; }
		public Action<ErrorEventArgs> OnError { get; set; }

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
