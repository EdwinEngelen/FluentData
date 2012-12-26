using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
{
	[TestClass]
    public class UpdateDataTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Update_data_sql()
		{
			int rowsAffected = Context.Sql("update Product set Name = @0 where ProductId = @1", "The Warren Buffet Way", 1)
								.Execute();

			Assert.AreEqual(1, rowsAffected);
		}

		[TestMethod]
		public void Update_data_builder()
		{
			int rowsAffected = Context.Update("Product")
								.Column("Name", "The Warren Buffet Way")
								.Where("ProductId", 1)
								.Execute();

			Assert.AreEqual(1, rowsAffected);
		}

		[TestMethod]
		public void Update_data_builder_automapping()
		{
			Product product = Context.Sql("select * from Product where ProductId = 1")
								.QuerySingle<Product>();
			product.Name = "The Warren Buffet Way";

			int rowsAffected = Context.Update<Product>("Product", product)
										.Where(x => x.ProductId)
										.AutoMap(x => x.ProductId, x => x.Category)
										.Execute();

			Assert.AreEqual(1, rowsAffected);
		}
	}
}
