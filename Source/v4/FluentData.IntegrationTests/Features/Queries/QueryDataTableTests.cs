using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace IntegrationTests.Features.Queries
{
	[TestClass]
    public class QueryDataTableTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test_QueryMany()
		{
			var dataTable = Context.Sql("select top 3 * from Product").QueryMany<DataTable>();
			Assert.AreEqual(3, dataTable[0].Rows.Count);
		}

		[TestMethod]
		public void Test_QuerySingle()
		{
			var dataTable = Context.Sql("select top 3 * from Product").QuerySingle<DataTable>();
			Assert.AreEqual(3, dataTable.Rows.Count);
		}
	}
}
