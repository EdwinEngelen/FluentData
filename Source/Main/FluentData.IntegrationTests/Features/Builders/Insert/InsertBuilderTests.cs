using IntegrationTests._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Features.Builders.Insert
{
	[TestClass]
    public class InsertBuilderTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test()
		{
			using (var context = Context.UseTransaction(true))
			{
				var productId = context.Insert("Product")
									.Column("Name", "TestProduct")
									.Column("CategoryId", 1)
									.ExecuteReturnLastId<int>();

				var product = TestHelper.GetProduct(context, productId);
				Assert.AreEqual("TestProduct", product.Name);
				Assert.AreEqual(1, product.CategoryId);
				Assert.IsNotNull(product);
			}
		}
	}
}
