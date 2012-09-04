using System.Collections.Generic;
using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Queries
{
	[TestClass]
	public class QueryComplexTests
	{
		[TestMethod]
		public void Test()
		{
			var categories = new List<Category>();
			TestHelper.Context().Sql("select * from Category").QueryComplex<Category>(categories, MapCategory);
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
