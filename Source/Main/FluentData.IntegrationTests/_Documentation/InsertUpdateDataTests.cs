using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
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

			var insertBuilder = Context.Insert<Product>("Product", product);
			FillBuilder((IInsertUpdateBuilder<Product>) insertBuilder);
			product.ProductId = insertBuilder.ExecuteReturnLastId<int>();

			Assert.IsTrue(product.ProductId > 0);

			var updateBuilder = Context.Update<Product>("Product", product);
			FillBuilder((IInsertUpdateBuilder<Product>) updateBuilder);

			int rowsAffected = updateBuilder.Where(x => x.ProductId).Execute();

			Assert.IsTrue(rowsAffected > 0);
		}

		public void FillBuilder(IInsertUpdateBuilder<Product> builder)
		{
			builder.Column(x => x.Name);
			builder.Column(x => x.CategoryId);
		}
	}
}
