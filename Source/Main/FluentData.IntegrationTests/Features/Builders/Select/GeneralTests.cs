using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Builders.Select
{
	[TestClass]
	public class GeneralTests
	{
		[TestMethod]
		public void Test1()
		{
			var context = TestHelper.Context();

			var products = context.Select<Product>("ProductId, p.Name, c.Category as Category_CategoryId, c.Name as Category_Name")
				.From(@"Product p
						inner join Category c on p.ProductId = c.CategoryId")
				.OrderBy("c.Name")
																		
				.Paging(1, 30).QueryMany();

			Assert.IsTrue(products.Count > 0);
		}

		[TestMethod]
		public void Test2()
		{
			var context = TestHelper.Context();
			var categories = context
								.Select<Category>("CategoryId, Name")
								.From("Category").QueryMany();

			Assert.IsTrue(categories.Count > 0);
		}

		[TestMethod]
		public void Test3()
		{
			var context = TestHelper.Context();
			var category = context.Select<Category>("CategoryId, Name")
				.From("Category")
				.Where("CategoryId = @CategoryId").Parameter("CategoryId", 1).QuerySingle();

			Assert.IsNotNull(category);
		}

		[TestMethod]
		public void Test_Paging()
		{
			var context = TestHelper.Context();

			var category = context
				.Select<Category>("CategoryId, Name")
				.From("Category")
				.OrderBy("Name asc")
				.Paging(1, 1).QuerySingle();
			Assert.AreEqual("Books", category.Name);

			category = context
				.Select<Category>("CategoryId, Name")
				.From("Category")
				.OrderBy("Name asc")
				.Paging(2, 1).QuerySingle();
			Assert.AreEqual("Movies", category.Name);
		}

		[TestMethod]
		public void Test4_Manual_mapping()
		{
			var context = TestHelper.Context();

			var products = context
				.Select<Category>("CategoryId, Name")
				.From(@"Product p
						inner join Category c on p.ProductId = c.CategoryId")
				.OrderBy("c.Name").QueryMany();

			Assert.IsTrue(products.Count > 0);
		}

		public void Test_GroupBy()
		{
			var context = TestHelper.Context();

			var products = context.Select<Product>("c.Name")
				.Select("count(*) as Products").QueryMany();

			Assert.IsTrue(products.Count > 0);
			
		}
	}
}
