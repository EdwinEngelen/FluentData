using Microsoft.VisualStudio.TestTools.UnitTesting;
using SomeNamespace;

namespace FluentData.UnitTests.Core
{
	[TestClass]
	public class ReflectionTests
	{
		[TestMethod]
		public void GetPropertyValueTests()
		{
			var category = new Category();
			category.CategoryId = Categories.Movies;
			var value = ReflectionHelper.GetPropertyValue(category, "CategoryId");
			Assert.AreEqual(2, value);
		}
	}
}
