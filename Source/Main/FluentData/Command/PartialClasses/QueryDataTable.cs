using System.Data;

namespace FluentData
{
	internal partial class DbCommand
	{
		public DataTable QueryDataTable()
		{
			var dataTable = new DataTable();

			Data.ExecuteQueryHandler.ExecuteQuery(true, () => dataTable.Load(Data.Reader.InnerReader, LoadOption.OverwriteChanges));

			return dataTable;
		}
	}
}
