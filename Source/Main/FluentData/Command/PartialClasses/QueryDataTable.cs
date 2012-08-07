using System;
using System.Data;

namespace FluentData
{
	internal partial class DbCommand
	{
		public DataTable QueryDataTable()
		{
			var dataTable = new DataTable();

			_data.ExecuteQueryHandler.ExecuteQuery(true, () => dataTable.Load(_data.InnerReader, LoadOption.OverwriteChanges));

			return dataTable;
		}
	}
}
