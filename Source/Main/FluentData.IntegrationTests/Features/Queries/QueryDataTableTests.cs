using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Queries
{
	[TestClass]
    public class QueryDataTableTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test()
		{
			var dataTable = Context.Sql("select top 3 * from Product").QueryManyDataTable();

			Assert.AreEqual(3, dataTable.Rows.Count);
		}
	}
}
