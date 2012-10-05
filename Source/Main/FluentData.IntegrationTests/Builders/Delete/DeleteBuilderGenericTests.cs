using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData
{
	[TestClass]
	public class DeleteBuilderGenericTests
	{
		[TestMethod]
		public void Test()
		{
			using (var db = TestHelper.Context().UseTransaction(true))
			{
				var productId = db.Insert("Product")
									.Column("Name", "Test")
									.Column("CategoryId", 1)
									.ExecuteReturnLastId<int>();

				var product = TestHelper.GetProduct(db, productId);
				Assert.IsNotNull(product);

				db.Delete<Product>("Product", product)
					.Where("ProductId", product.ProductId)
					.Where(x => x.Name)
					.Execute();

				product = TestHelper.GetProduct(db, productId);
				Assert.IsNull(product);
			}
		}
	}
}
