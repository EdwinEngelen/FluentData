using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
{
	[TestClass]
	public class Parameters : BaseDocumentation
	{
		[TestMethod]
		public void Unnamed_parameters_one()
		{
			var product = Context().Sql("select * from Product where ProductId = @0", 1).QuerySingle();

			Assert.IsNotNull(product);
		}

		[TestMethod]
		public void Unnamed_parameters_many()
		{
			var products = Context().Sql("select * from Product where ProductId = @0 or ProductId = @1", 1, 2).Query();

			Assert.AreEqual(2, products.Count);
		}

		[TestMethod]
		public void Named_parameters()
		{
			var products = Context().Sql("select * from Product where ProductId = @ProductId1 or ProductId = @ProductId2")
									.Parameter("ProductId1", 1)
									.Parameter("ProductId2", 2)
									.Query();

			Assert.AreEqual(2, products.Count);
		}
	}
}
