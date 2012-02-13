using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
{
	[TestClass]
	public class UpdateData : BaseDocumentation
	{
		[TestMethod]
		public void Update_data_sql()
		{
			var rowsAffected = Context().Sql("update Product set Name = @0 where ProductId = @1")
								.Parameters("The Warren Buffet Way", 1)
								.Execute();

			Assert.AreEqual(1, rowsAffected);
		}

		[TestMethod]
		public void Update_data_builder()
		{
			var rowsAffected = Context().Update("Product")
								.Column("Name", "The Warren Buffet Way")
								.Where("ProductId", 1)
								.Execute();

			Assert.AreEqual(1, rowsAffected);
		}

		[TestMethod]
		public void Update_data_builder_automapping()
		{
			var product = Context().Sql("select * from Product where ProductId = 1")
								.QuerySingle<Product>();

			product.Name = "The Warren Buffet Way";

			var rowsAffected = Context().Update<Product>("Product", product)
										.Where(x => x.ProductId)
										.AutoMap()
										.Execute();

			Assert.AreEqual(1, rowsAffected);
		}
	}
}
