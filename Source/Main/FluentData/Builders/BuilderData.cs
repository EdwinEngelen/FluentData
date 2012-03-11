using System.Collections.Generic;

namespace FluentData
{
	public class BuilderData
	{
		public List<TableColumn> Columns { get; set; }
		public List<Parameter> Parameters { get; set; }
		public object Item { get; set; }
		public string ObjectName { get; set; }
		public DbCommandData CommandData { get; set; }
		public IDbProvider Provider { get; set; }
		public IDbCommand Command { get; set; }
		public List<TableColumn> Where { get; set; }

		public BuilderData(IDbProvider provider, IDbCommand command, string objectName)
		{
			Provider = provider;
			ObjectName = objectName;
			Command = command;
			Parameters = new List<Parameter>();
			Columns = new List<TableColumn>();
			Where = new List<TableColumn>();
		}
	}
}
