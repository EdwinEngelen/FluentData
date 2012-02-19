using System.Collections.Generic;

namespace FluentData
{
	public class BuilderData
	{
		public List<TableColumn> Columns { get; set; }
		public List<Parameter> Parameters { get; set; }
		public HashSet<string> IgnoreProperties { get; set; }
		public object Item { get; set; }
		public string ObjectName { get; set; }
		public DbCommandData DbData { get; set; }
		public IDbProvider DbProvider { get; set; }
		public IDbCommand DbCommand { get; set; }
		public List<TableColumn> Where { get; set; }

		public BuilderData(IDbProvider provider, IDbCommand command, string objectName)
		{
			DbProvider = provider;
			ObjectName = objectName;
			DbCommand = command;
			Parameters = new List<Parameter>();
			Columns = new List<TableColumn>();
			IgnoreProperties = new HashSet<string>();
			Where = new List<TableColumn>();
		}
	}
}
