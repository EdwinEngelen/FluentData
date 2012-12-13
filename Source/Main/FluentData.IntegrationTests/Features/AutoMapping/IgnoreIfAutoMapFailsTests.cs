using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.AutoMapping
{
	[TestClass]
	public class IgnoreIfAutoMapFailsTests
	{
		[TestMethod]
		public void Test_different_columns_and_properties_automap_must_fail()
		{
			try
			{
				var result = TestHelper.Context().Sql(@"select CategoryId as CategoryIdNotExist, Name
															from Category").QueryMany<Category>();

				Assert.Fail();
			}
			catch (FluentDataException exception)
			{
				Assert.AreEqual("Could not map: CategoryIdNotExist", exception.Message);
			}
		}

		[TestMethod]
		public void Test_ignoreIfAutoMapFails_different_columns_and_properties_automap_must_not_fail()
		{
			var result = TestHelper.Context().IgnoreIfAutoMapFails(true)
												.Sql(@"select CategoryId as CategoryIdNotExist, Name
														from Category").QueryMany<Category>();
		}

		[TestMethod]
		public void Test_same_columns_and_properties_automap_must_not_fail()
		{
			var result = TestHelper.Context().Sql(@"select CategoryId, Name
											from Category").QueryMany<Category>();

			Assert.IsTrue(result.Count > 0);
		}
	}
}
