using IntegrationTests._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Features.Sql
{
	[TestClass]
	public class LikeTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test()
		{
			var products = Context.Sql("select * from Product where Name like @Name")
								.Parameter("Name", "The %").QueryMany<Product>();

			Assert.IsTrue(products.Count > 0);
		}
	}
}