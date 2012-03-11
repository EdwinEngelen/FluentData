using System.Dynamic;
using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData
{
	[TestClass]
	public class UpdateBuilderDynamicTests
	{
		[TestMethod]
		public void Test_No_Automap()
		{
			using (var context = TestHelper.Context().UseTransaction)
			{
				var productId = TestHelper.InsertProduct(context, "OldTestProduct", 1);

				dynamic product = TestHelper.GetProductDynamic(context, productId);
				product.Name = "NewTestProduct";
				product.CategoryId = 2;

				var rowsAffected = context.Update("Product", (ExpandoObject) product)
										.Column("Name", (string) product.Name)
										.Column("CategoryId")
										.Where("ProductId")
										.Execute();

				Assert.AreEqual(1, rowsAffected);

				product = TestHelper.GetProductDynamic(context, productId);

				Assert.AreEqual("NewTestProduct", product.Name);
				Assert.AreEqual(2, product.CategoryId);
				Assert.IsNotNull(product);
			}
		}

		[TestMethod]
		public void Test_Automap()
		{
			using (var context = TestHelper.Context().UseTransaction)
			{
				var productId = TestHelper.InsertProduct(context, "OldTestProduct", 1);

				dynamic product = TestHelper.GetProductDynamic(context, productId);
				product.Name = "NewTestProduct";
				product.CategoryId = 2;

				var rowsAffected = context.Update("Product", (ExpandoObject) product)
											.AutoMap("ProductId")
											.Where("ProductId")
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
