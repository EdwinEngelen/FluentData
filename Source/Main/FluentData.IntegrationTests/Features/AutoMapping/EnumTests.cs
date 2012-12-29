using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Features.AutoMapping
{
	[TestClass]
	public class AutoMapEnumTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Int_to_Enum()
		{
			var product = Context.Sql(@"
											select top 1 CategoryId as Category, ProductId, Name, CategoryId
											from Product
											where CategoryId in(1,2)").QuerySingle<ProductWithEnem>();

			Assert.AreNotEqual(0, product.CategoryId);
			Assert.AreNotEqual(Categories.None, product.Category);
		}

		[TestMethod]
		public void String_to_Enum()
		{
			var product = Context.Sql(@"
											select top 1 c.Name as Category, p.ProductId, p.Name, p.CategoryId, c.Name as CategoryName
											from Product p
											inner join Category c on p.CategoryId = c.CategoryId
											where p.CategoryId in(1,2)").QuerySingle<ProductWithEnem>();

			Assert.AreNotEqual(0, product.CategoryId);
			Assert.AreNotEqual(Categories.None, product.Category);
			Assert.AreEqual(product.CategoryName, product.Category.ToString());
		}

		public class ProductWithEnem
		{
			public int ProductId { get; set; }
			public string Name { get; set; }
			public int CategoryId { get; set; }
			public string CategoryName { get; set; }
			public Categories Category { get; set; }
		}

		public enum Categories
		{
			None = 0,
			Books = 1,
			Movies = 2
		}
	}
}
