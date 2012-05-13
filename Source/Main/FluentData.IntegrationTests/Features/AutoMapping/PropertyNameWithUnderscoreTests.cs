using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.AutoMapping
{
	[TestClass]
	public class PropertyWithUnderscoreTests
	{
		[TestMethod]
		public void Test_fieldname_propertyname_with_underscore()
		{
			var product = TestHelper.Context()
										.Sql(@"select top 1
													p.ProductId as product_Id,
													p.Name,
													c.CategoryId as Category_Id,
													c.Name as Category_Name
												from Product p
												inner join Category c on p.CategoryId = c.CategoryId").QuerySingle<ProductWithUnderscore>();

			Assert.IsFalse(string.IsNullOrEmpty(product.Category_Name));
		}

		public class ProductWithUnderscore
		{
			public int Product_Id { get; set; }
			public string Name { get; set; }
			public int Category_Id { get; set; }
			public string Category_Name { get; set; }
		}
	}
}
