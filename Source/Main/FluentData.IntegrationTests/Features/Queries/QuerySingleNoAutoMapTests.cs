using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Queries
{
	[TestClass]
	public class QueryNoAutoMapTests
	{

		[TestMethod]
		public void Test_map_using_data_reader()
		{
			var category = TestHelper.Context().Sql("select top 1 * from Category").QuerySingleComplex<Category>(MapCategoryReader);
			Assert.IsNotNull(category);
			Assert.IsTrue(category.Name.Length > 0);
			Assert.IsTrue(category.CategoryId > 0);
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
			var category = TestHelper.Context().Sql("select top 1 * from Category").QuerySingleComplex<Category>(MapCategoryDynamic);
			Assert.IsNotNull(category);
			Assert.IsTrue(category.Name.Length > 0);
			Assert.IsTrue(category.CategoryId > 0);
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
