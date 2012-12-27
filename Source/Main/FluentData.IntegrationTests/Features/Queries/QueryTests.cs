using System.Dynamic;
using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Queries
{
	[TestClass]
    public class QuerySingleNoAutoMapTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test_map_using_data_reader()
		{
			var categories = Context.Sql("select * from Category").QueryMany<Category>(MapCategoryReader);
			Assert.IsTrue(categories.Count > 0);
			Assert.IsTrue(categories[0].Name.Length > 0);
			Assert.IsTrue(categories[0].CategoryId > 0);
		}

        public void Test()
        {
            var categories = Context.StoredProcedure("select * from Category").QueryMany<Category>(MapCategoryTest);

            dynamic proc = new ExpandoObject();
            proc.ProductId = 1;
            proc.Name = "Test";

           
        }

	    private void MapCategoryTest(Category arg1, dynamic arg2)
	    {
	    }


	    private void MapCategoryReader(Category category, IDataReader reader)
		{
			category.CategoryId = (Categories) reader.GetInt32("CategoryId");
			category.Name = reader.GetString("Name");
		}

		[TestMethod]
		public void Test_map_using_dynamic()
		{
			var categories = Context.Sql("select * from Category").QueryMany<Category>(MapCategoryDynamic);
			Assert.IsTrue(categories.Count > 0);
			Assert.IsTrue(categories[0].Name.Length > 0);
			Assert.IsTrue(categories[0].CategoryId > 0);
		}

		private void MapCategoryDynamic(Category category, dynamic reader)
		{
			category.CategoryId = (Categories)reader.CategoryId;
			category.Name = reader.Name;
		}
	}
}
