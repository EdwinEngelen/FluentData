using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Queries
{
	[TestClass]
	public class QueryValuesTests
	{
		[TestMethod]
		public void Test_int()
		{
			var categories = TestHelper.Context().Sql("select CategoryId from Category order by CategoryId").Query<int>();

			Assert.AreEqual(2, categories.Count);
			Assert.AreEqual(1, categories[0]);
			Assert.AreEqual(2, categories[1]);
		}

		[TestMethod]
		public void Test_string()
		{
			var categories = TestHelper.Context().Sql("select Name from Category order by Name").Query<string>();

			Assert.AreEqual(2, categories.Count);
			Assert.AreEqual("Books", categories[0]);
			Assert.AreEqual("Movies", categories[1]);
		}

		[TestMethod]
		public void Test_null()
		{
			var categoryId = TestHelper.Context().Sql("select null").QuerySingle<int>();

			Assert.AreEqual(0, categoryId);
		}
	}
}
