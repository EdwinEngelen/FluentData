using IntegrationTests._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Features.Builders.Select
{
	[TestClass]
	public class GeneralTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test1()
		{
			var products = Context.Select<Product>("p.ProductId, p.Name, c.CategoryId as Category_CategoryId, c.Name as Category_Name")
				.From(@"Product p
						inner join Category c on p.ProductId = c.CategoryId")
				.OrderBy("c.Name")
                .Paging(1, 30).QueryMany();

			Assert.IsTrue(products.Count > 0);
		}

		[TestMethod]
		public void Test2()
		{
			var categories = Context
								.Select<Category>("CategoryId, Name")
                                .From("Category").QueryMany();

			Assert.IsTrue(categories.Count > 0);
		}

		[TestMethod]
		public void Test3()
		{
			var context = Context;
			var category = context.Select<Category>("CategoryId, Name")
				.From("Category")
				.Where("CategoryId = @CategoryId").Parameter("CategoryId", 1).QuerySingle();

			Assert.IsNotNull(category);
		}

		[TestMethod]
		public void Test_Paging()
		{
			var category = Context
				.Select<Category>("CategoryId, Name")
				.From("Category")
				.OrderBy("Name asc")
                .Paging(1, 1).QuerySingle();
			Assert.AreEqual("Books", category.Name);

			category = Context
				.Select<Category>("CategoryId, Name")
				.From("Category")
				.OrderBy("Name asc")
                .Paging(2, 1).QuerySingle();
			Assert.AreEqual("Movies", category.Name);
		}

		[TestMethod]
		public void Test4_Manual_mapping()
		{
			var products = Context
				.Select<Category>("c.CategoryId, c.Name")
				.From(@"Product p
						inner join Category c on p.ProductId = c.CategoryId")
                .OrderBy("c.Name").QueryMany();

			Assert.IsTrue(products.Count > 0);
		}

		public void Test_GroupBy()
		{
			var products = Context.Select<Product>("c.Name")
                .Select("count(*) as Products").QueryMany();

			Assert.IsTrue(products.Count > 0);
		}
		
		[TestMethod]
		public void Test_WhereOr()
		{
			var categories = Context.Select<Category>("CategoryId, Name")
			          .From("Category")
			          .Where("CategoryId = 1")
			          .OrWhere("CategoryId = 2").QueryMany();
			Assert.IsTrue(categories.Count == 2);
		}

		[TestMethod]
		public void Test_WhereAnd()
		{
			var categories = Context.Select<Category>("CategoryId, Name")
					  .From("Category")
					  .Where("CategoryId = 1")
					  .AndWhere("CategoryId = 1").QueryMany();
			Assert.IsTrue(categories.Count == 1);
		}

		[TestMethod]
		public void Test_dynamic()
		{
			var categories = Context.Select<dynamic>("CategoryId, Name").From("Category").QueryMany();
			Assert.IsTrue(categories.Count > 0);
		}
	}
}
