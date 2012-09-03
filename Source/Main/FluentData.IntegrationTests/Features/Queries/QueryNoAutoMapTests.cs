using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Queries
{
	[TestClass]
	public class QuerySingleNoAutoMapTests
	{
		[TestMethod]
		public void Test_map_using_data_reader()
		{
			var categories = TestHelper.Context().Sql("select * from Category").QueryNoAutoMap<Category>(MapCategoryReader);
			Assert.IsTrue(categories.Count > 0);
			Assert.IsTrue(categories[0].Name.Length > 0);
			Assert.IsTrue(categories[0].CategoryId > 0);
		}

		private Category MapCategoryReader(IDataReader reader)
		{
			var category = new Category();
			category.CategoryId = (Categories) reader.GetInt32("CategoryId");
			category.Name = reader.GetString("Name");
			return category;
		}

		[TestMethod]
		public void Test_map_using_dynamic()
		{
			var categories = TestHelper.Context().Sql("select * from Category").QueryNoAutoMap<Category>(MapCategoryDynamic);
			Assert.IsTrue(categories.Count > 0);
			Assert.IsTrue(categories[0].Name.Length > 0);
			Assert.IsTrue(categories[0].CategoryId > 0);
		}

		private Category MapCategoryDynamic(IDataReader reader)
		{
			var category = new Category();
			category.CategoryId = (Categories) reader.Value.CategoryId;
			category.Name = reader.Value.Name;
			return category;
		}
	}
}
