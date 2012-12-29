using IntegrationTests._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Features.Builders.Delete
{
	[TestClass]
    public class DeleteBuilderTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test()
		{
				using (var db = Context.UseTransaction(true))
				{
				var productId = TestHelper.InsertProduct(db, "Test", 1);

				var product = TestHelper.GetProduct(db, productId);
				Assert.IsNotNull(product);

				db.Delete("Product")
					.Where("ProductId", productId)
					.Where("Name", "Test")
					.Execute();

				product = TestHelper.GetProduct(db, productId);
				Assert.IsNull(product);
			}
		}
	}
}
