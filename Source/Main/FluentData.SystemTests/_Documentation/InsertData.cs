using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
{
	[TestClass]
	public class InsertData : BaseDocumentation
	{

		[TestMethod]
		public void Insert_data_sql()
		{
			var productId = Context().Sql("insert into Product(Name, CategoryId) values(@0, @1);")
							.Parameters("The Warren Buffet Way", 1)
							.ExecuteReturnLastId();

			Assert.IsTrue(productId > 0);
		}

		[TestMethod]
		public void Insert_data_builder_no_automapping()
		{
			var productId = Context().Insert("Product")
								.Column("CategoryId", 1)
								.Column("Name", "The Warren Buffet Way")
								.ExecuteReturnLastId();

			Assert.IsTrue(productId > 0);
		}

		[TestMethod]
		public void Insert_data_builder_automapping()
		{
			var product = new Product();
			product.CategoryId = 1;
			product.Name = "The Warren Buffet Way";

			var productId = Context().Insert<Product>("Product", product)
								.IgnoreProperty(x => x.ProductId)
								.AutoMap()
								.ExecuteReturnLastId();

			Assert.IsTrue(productId > 0);
		}
	}
}
