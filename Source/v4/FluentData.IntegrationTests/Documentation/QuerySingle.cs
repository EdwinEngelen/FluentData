using IntegrationTests._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Documentation
{
	[TestClass]
    public class QuerySingle : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Query_single_dynamic()
		{
			dynamic product = Context.Sql("select * from Product where ProductId = 1").QuerySingle<dynamic>();

			Assert.IsNotNull(product);
		}

		[TestMethod]
		public void Query_single_strongly_typed()
		{
			Product product = Context.Sql("select * from Product where ProductId = 1").QuerySingle<Product>();

			Assert.IsNotNull(product);
		}
	}
}
