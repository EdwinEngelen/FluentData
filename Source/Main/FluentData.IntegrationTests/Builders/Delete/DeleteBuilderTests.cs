using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData
{
	[TestClass]
	public class DeleteBuilderTests
	{
		[TestMethod]
		public void Test()
		{
			using (var db = TestHelper.Context().UseTransaction)
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
