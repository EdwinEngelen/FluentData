using System.Text;

namespace FluentData
{
	public class DbCommandData
	{
		public DbContext DbContext { get; set; }
		public DbContextData DbContextData { get; set; }
		public System.Data.IDbCommand InnerCommand { get; set; }
		public StringBuilder Sql { get; set; }
		public bool MultipleResultset { get; set; }
		public IDataReader Reader { get; set; }
		public IDbCommand Command { get; set; }
		public ParameterCollection Parameters { get; set; }
		internal ExecuteQueryHandler ExecuteQueryHandler;
		public DbCommandTypes DbCommandType { get; set; }

		public DbCommandData()
		{
			Parameters = new ParameterCollection();
			DbCommandType = DbCommandTypes.Text;
		}
	}
}
