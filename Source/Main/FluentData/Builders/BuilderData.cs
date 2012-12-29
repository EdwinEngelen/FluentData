using System.Collections.Generic;

namespace FluentData
{
	public class BuilderData
	{
		public List<BuilderTableColumn> Columns { get; set; }
		public object Item { get; set; }
		public string ObjectName { get; set; }
		public IDbCommand Command { get; set; }
		public List<BuilderTableColumn> Where { get; set; }

		public BuilderData(IDbCommand command, string objectName)
		{
			ObjectName = objectName;
			Command = command;
			Columns = new List<BuilderTableColumn>();
			Where = new List<BuilderTableColumn>();
		}
	}
}
