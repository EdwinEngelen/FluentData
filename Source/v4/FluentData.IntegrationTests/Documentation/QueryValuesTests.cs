using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Documentation
{
	[TestClass]
    public class QueryValuesTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test()
		{
			List<int> productIds = Context.Sql(@"select ProductId
												from Product").QueryMany<int>();

			Assert.IsTrue(productIds.Count > 0);
		}
	}
}
