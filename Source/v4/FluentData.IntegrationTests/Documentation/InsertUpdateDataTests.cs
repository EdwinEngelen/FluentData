using FluentData;
using IntegrationTests._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Documentation
{
	[TestClass]
	public class InsertUpdateDataTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test()
		{
			var product = new Product();
			product.Name = "The Warren Buffet Way";
			product.CategoryId = 1;

			product.ProductId = Context.Insert<Product>("Product", product)
												.Fill(FillBuilder)
												.ExecuteReturnLastId<int>();

			Assert.IsTrue(product.ProductId > 0);

			var rowsAffected = Context.Update<Product>("Product", product).Fill(FillBuilder).Where(x => x.ProductId).Execute();

			Assert.IsTrue(rowsAffected > 0);
		}

		public void FillBuilder(IInsertUpdateBuilder<Product> builder)
		{
			builder.Column(x => x.Name);
			builder.Column(x => x.CategoryId);
		}
	}
}
