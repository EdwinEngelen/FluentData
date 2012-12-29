using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Documentation
{
	[TestClass]
	public class DeleteDataTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Delete_data_sql()
		{
			var productId = Context.Sql("insert into Product(Name, CategoryId) values(@0, @1);", "The Warren Buffet Way", 1)
							.ExecuteReturnLastId<int>();

			int rowsAffected = Context.Sql("delete from Product where ProductId = @0", productId)
						.Execute();

			Assert.AreEqual(1, rowsAffected);
		}

		[TestMethod]
		public void Delete_data_builder()
		{
			var productId = Context.Sql("insert into Product(Name, CategoryId) values(@0, @1);", "The Warren Buffet Way", 1)
							.ExecuteReturnLastId<int>();

			int rowsAffected = Context.Delete("Product")
										.Where("ProductId", productId)
										.Execute();

			Assert.AreEqual(1, rowsAffected);
		}
	}
}
