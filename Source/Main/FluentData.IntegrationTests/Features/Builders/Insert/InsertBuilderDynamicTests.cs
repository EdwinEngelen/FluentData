using System.Dynamic;
using IntegrationTests._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Features.Builders.Insert
{
	[TestClass]
    public class InsertBuilderDynamicTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test_No_Automap()
		{
			dynamic product = new ExpandoObject();
			product.CategoryId = 1;
			product.Name = "TestProduct";

			using (var context = Context.UseTransaction(true))
			{
				product.ProductId = context.Insert("Product", (ExpandoObject) product)
									.Column("Name", (string) product.Name)
									.Column("CategoryId", (int) product.CategoryId)
									.ExecuteReturnLastId<int>();

				var createdProduct = TestHelper.GetProduct(context, product.ProductId);
				Assert.AreEqual("TestProduct", product.Name);
				Assert.AreEqual(1, product.CategoryId);
				Assert.IsNotNull(createdProduct);
			}
		}

		[TestMethod]
		public void Test_Automap()
		{
			dynamic product = new ExpandoObject();
			product.CategoryId = 1;
			product.Name = "TestProduct";

			using (var context = Context.UseTransaction(true))
			{
				product.ProductId = context.Insert("Product", (ExpandoObject) product)
									.AutoMap("ProductId")
									.ExecuteReturnLastId<int>();

				var createdProduct = TestHelper.GetProduct(context, product.ProductId);
				Assert.AreEqual("TestProduct", product.Name);
				Assert.AreEqual(1, product.CategoryId);
				Assert.IsNotNull(createdProduct);
			}
		}
	}
}
