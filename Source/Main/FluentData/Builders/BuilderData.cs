using System.Collections.Generic;

namespace FluentData
{
	public class BuilderData
	{
		public int PagingCurrentPage { get; set; }
		public int PagingItemsPerPage { get; set; }
		public List<TableColumn> Columns { get; set; }
		public object Item { get; set; }
		public string ObjectName { get; set; }
		public IDbCommand Command { get; set; }
		public List<TableColumn> Where { get; set; }
		public string Having { get; set; }
		public string GroupBy { get; set; }
		public string OrderBy { get; set; }
		public string From { get; set; }
		public string Select { get; set; }
		public string WhereSql { get; set; }

		public BuilderData(IDbCommand command, string objectName)
		{
			ObjectName = objectName;
			Command = command;
			Columns = new List<TableColumn>();
			Where = new List<TableColumn>();
			Having = "";
			GroupBy = "";
			OrderBy = "";
			From = "";
			Select = "";
			WhereSql = "";
			PagingCurrentPage = 1;
			PagingItemsPerPage = 0;
		}

		internal int GetFromItems()
		{
			return (GetToItems() - PagingItemsPerPage + 1);
		}

		internal int GetToItems()
		{
			return (PagingCurrentPage*PagingItemsPerPage);
		}
	}
}
