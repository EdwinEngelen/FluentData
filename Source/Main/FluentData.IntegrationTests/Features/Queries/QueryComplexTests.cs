using System.Collections.Generic;
using FluentData;
using IntegrationTests._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Features.Queries
{
	[TestClass]
    public class QueryComplexTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test()
		{
			var categories = new List<Category>();
			Context.Sql("select * from Category").QueryComplexMany<Category>(categories, MapCategory);
			Assert.IsTrue(categories.Count > 0);
		}

		private void MapCategory(IList<Category> categories, IDataReader reader)
		{
			var category = new Category();
			category.CategoryId = (Categories) reader.GetInt32("CategoryId");
			category.Name = reader.GetString("Name");
			categories.Add(category);
		}
	}
}
