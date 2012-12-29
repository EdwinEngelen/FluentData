using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentData
{
	public class SelectBuilderData : BuilderData
	{
		public int PagingCurrentPage { get; set; }
		public int PagingItemsPerPage { get; set; }
		public string Having { get; set; }
		public string GroupBy { get; set; }
		public string OrderBy { get; set; }
		public string From { get; set; }
		public string Select { get; set; }
		public string WhereSql { get; set; }

		public SelectBuilderData(IDbCommand command, string objectName) : base(command, objectName)
		{
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
