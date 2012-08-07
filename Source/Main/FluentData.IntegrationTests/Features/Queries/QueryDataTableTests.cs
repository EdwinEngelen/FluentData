using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Queries
{
	[TestClass]
	public class QueryDataTableTests
	{
		[TestMethod]
		public void Test()
		{
			var dataTable = TestHelper.Context().Sql("select top 3 * from Product").QueryDataTable();

			Assert.AreEqual(3, dataTable.Rows.Count);
		}
	}
}
