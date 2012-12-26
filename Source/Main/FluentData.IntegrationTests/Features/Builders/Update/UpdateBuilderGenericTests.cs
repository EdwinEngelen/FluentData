using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData
{
	[TestClass]
    public class UpdateBuilderGenericTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test_No_Automap()
		{
			using (var context = Context.UseTransaction(true))
			{
				var productId = TestHelper.InsertProduct(context, "OldTestProduct", 1);

				var product = TestHelper.GetProduct(context, productId);
				product.Name = "NewTestProduct";
				product.CategoryId = 2;

				var rowsAffected = context.Update<Product>("Product", product)
						.Column(x => x.Name)
						.Column(x => x.CategoryId)
						.Where(x => x.ProductId)
						.Execute();

				Assert.AreEqual(1, rowsAffected);

				product = TestHelper.GetProduct(context, productId);

				Assert.AreEqual("NewTestProduct", product.Name);
				Assert.AreEqual(2, product.CategoryId);
				Assert.IsNotNull(product);
			}
		}

		[TestMethod]
		public void Test_Automap()
		{
			using (var context = Context.UseTransaction(true))
			{
				var productId = TestHelper.InsertProduct(context, "OldTestProduct", 1);

				var product = TestHelper.GetProduct(context, productId);
				product.Name = "NewTestProduct";
				product.CategoryId = 2;

				var rowsAffected = context.Update<Product>("Product", product)
									.AutoMap(x => x.ProductId, x => x.Category)
									.Where(x => x.ProductId)
									.Execute();

				Assert.AreEqual(1, rowsAffected);

				product = TestHelper.GetProduct(context, productId);

				Assert.AreEqual("NewTestProduct", product.Name);
				Assert.AreEqual(2, product.CategoryId);
				Assert.IsNotNull(product);
			}
		}
	}
}
