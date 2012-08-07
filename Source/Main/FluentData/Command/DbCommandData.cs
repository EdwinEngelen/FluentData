using System.Text;

namespace FluentData
{
	public class DbCommandData
	{
		public IDbCommand Command { get; set; }
		public DbContext Context { get; set; }
		public DbContextData ContextData { get; set; }
		public System.Data.IDbCommand InnerCommand { get; set; }
		public StringBuilder Sql { get; set; }
		public bool UseMultipleResultsets { get; set; }
		public IDataReader Reader { get; set; }
		public System.Data.IDataReader InnerReader { get; set; }
		internal ExecuteQueryHandler ExecuteQueryHandler;
		public DbCommandTypes CommandType { get; set; }

		public DbCommandData()
		{
			CommandType = DbCommandTypes.Text;
			Sql = new StringBuilder();
		}
	}
}
