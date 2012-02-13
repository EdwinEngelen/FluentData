using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
{
	[TestClass]
	public class DeleteData : BaseDocumentation
	{
		[TestMethod]
		public void Delete_data_sql()
		{
			var productId = Context().Sql("insert into Product(Name, CategoryId) values(@0, @1);")
							.Parameters("The Warren Buffet Way", 1)
							.ExecuteReturnLastId();

			var rowsAffected = Context().Sql("delete from Product where ProductId = @0")
									.Parameters(productId)
									.Execute();

			Assert.AreEqual(1, rowsAffected);
		}

		[TestMethod]
		public void Delete_data_builder()
		{
			var productId = Context().Sql("insert into Product(Name, CategoryId) values(@0, @1);")
							.Parameters("The Warren Buffet Way", 1)
							.ExecuteReturnLastId();

			var rowsAffected = Context().Delete("Product")
										.Where("ProductId", productId)
										.Execute();

			Assert.AreEqual(1, rowsAffected);
		}
	}
}
