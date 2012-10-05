using System.Text;

namespace FluentData
{
	public class DbCommandData
	{
		public DbContext Context { get; private set; }
		public System.Data.IDbCommand InnerCommand { get; private set; }
		public StringBuilder Sql { get; set; }
		public bool UseMultipleResultsets { get; set; }
		public IDataReader Reader { get; set; }
		internal ExecuteQueryHandler ExecuteQueryHandler;

		public DbCommandData(DbContext context, System.Data.IDbCommand innerCommand)
		{
			Context = context;
			InnerCommand = innerCommand;
			InnerCommand.CommandType = (System.Data.CommandType)DbCommandTypes.Text;
			Sql = new StringBuilder();
		}
	}
}
