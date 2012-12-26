using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Builders.Select
{
	[TestClass]
	public class GeneralTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test1()
		{
			var context = Context;

			var products = context.Select("p.ProductId, p.Name, c.CategoryId as Category_CategoryId, c.Name as Category_Name")
				.From(@"Product p
						inner join Category c on p.ProductId = c.CategoryId")
				.OrderBy("c.Name")

                .Paging(1, 30).QueryMany<Product>();

			Assert.IsTrue(products.Count > 0);
		}

		[TestMethod]
		public void Test2()
		{
			var context = Context;
			var categories = context
								.Select("CategoryId, Name")
                                .From("Category").QueryMany<Category>();

			Assert.IsTrue(categories.Count > 0);
		}

		[TestMethod]
		public void Test3()
		{
			var context = Context;
			var category = context.Select("CategoryId, Name")
				.From("Category")
				.Where("CategoryId = @CategoryId").Parameter("CategoryId", 1).QuerySingle<Category>();

			Assert.IsNotNull(category);
		}

		[TestMethod]
		public void Test_Paging()
		{
			var context = Context;

			var category = context
				.Select("CategoryId, Name")
				.From("Category")
				.OrderBy("Name asc")
                .Paging(1, 1).QuerySingle<Category>();
			Assert.AreEqual("Books", category.Name);

			category = context
				.Select("CategoryId, Name")
				.From("Category")
				.OrderBy("Name asc")
                .Paging(2, 1).QuerySingle<Category>();
			Assert.AreEqual("Movies", category.Name);
		}

		[TestMethod]
		public void Test4_Manual_mapping()
		{
			var context = Context;

			var products = context
				.Select("c.CategoryId, c.Name")
				.From(@"Product p
						inner join Category c on p.ProductId = c.CategoryId")
                .OrderBy("c.Name").QueryMany<Category>();

			Assert.IsTrue(products.Count > 0);
		}

		public void Test_GroupBy()
		{
			var context = Context;

			var products = context.Select("c.Name")
                .Select("count(*) as Products").QueryMany<Product>();

			Assert.IsTrue(products.Count > 0);
		}
		
		[TestMethod]
		public void Test_WhereOr()
		{
			var categories = Context.Select("CategoryId, Name")
			          .From("Category")
			          .Where("CategoryId = 1")
			          .OrWhere("CategoryId = 2").QueryMany<Category>();
			Assert.IsTrue(categories.Count == 2);
		}

		[TestMethod]
		public void Test_WhereAnd()
		{
			var categories = Context.Select("CategoryId, Name")
					  .From("Category")
					  .Where("CategoryId = 1")
					  .AndWhere("CategoryId = 1").QueryMany<Category>();
			Assert.IsTrue(categories.Count == 1);
		}
	}
}
