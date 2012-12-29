using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Features.Queries
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
