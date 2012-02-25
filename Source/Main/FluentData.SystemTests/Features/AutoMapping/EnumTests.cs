using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.AutoMapping
{
	[TestClass]
	public class AutoMapEnumTests
	{
		[TestMethod]
		public void MapToEnum()
		{
			var product = TestHelper.Context().Sql(@"
											select top 1 CategoryId as Category, ProductId, Name, CategoryId
											from Product
											where CategoryId in(1,2)").QuerySingle<ProductWithEnem>();

			Assert.AreNotEqual(0, product.CategoryId);
			Assert.AreNotEqual(Categories.None, product.Category);
		}

		public class ProductWithEnem
		{
			public int ProductId { get; set; }
			public string Name { get; set; }
			public int CategoryId { get; set; }
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
