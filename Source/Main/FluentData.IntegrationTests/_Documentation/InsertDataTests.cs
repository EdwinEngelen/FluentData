using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
{
	[TestClass]
	public class InsertDataTests : BaseDocumentation
	{
		[TestMethod]
		public void Insert_data_sql()
		{
			int productId = Context().Sql("insert into Product(Name, CategoryId) values(@0, @1);", "The Warren Buffet Way", 1)
							.ExecuteReturnLastId<int>();

			Assert.IsTrue(productId > 0);
		}

		[TestMethod]
		public void Insert_data_builder_no_automapping()
		{
			int productId = Context().Insert("Product")
								.Column("Name", "The Warren Buffet Way")
								.Column("CategoryId", 1)
								.ExecuteReturnLastId<int>();

			Assert.IsTrue(productId > 0);
		}

		[TestMethod]
		public void Insert_data_builder_automapping()
		{
			Product product = new Product();
			product.Name = "The Warren Buffet Way";
			product.CategoryId = 1;

			product.ProductId = Context().Insert<Product>("Product", product)
								.AutoMap(x => x.ProductId, x => x.Category)
								.ExecuteReturnLastId<int>();

			Assert.IsTrue(product.ProductId > 0);
		}
	}
}
