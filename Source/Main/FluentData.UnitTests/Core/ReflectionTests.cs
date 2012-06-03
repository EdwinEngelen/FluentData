using System;
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

		[TestMethod]
		public void IsBasicClrTypeTests()
		{
			Assert.IsTrue(ReflectionHelper.IsBasicClrType(typeof(bool)));
			Assert.IsTrue(ReflectionHelper.IsBasicClrType(typeof(byte)));
			Assert.IsTrue(ReflectionHelper.IsBasicClrType(typeof(long)));
			Assert.IsTrue(ReflectionHelper.IsBasicClrType(typeof(char)));
			Assert.IsTrue(ReflectionHelper.IsBasicClrType(typeof(string)));
			Assert.IsTrue(ReflectionHelper.IsBasicClrType(typeof(DateTime)));
			Assert.IsTrue(ReflectionHelper.IsBasicClrType(typeof(decimal)));
			Assert.IsTrue(ReflectionHelper.IsBasicClrType(typeof(double)));
			Assert.IsTrue(ReflectionHelper.IsBasicClrType(typeof(float)));
			Assert.IsTrue(ReflectionHelper.IsBasicClrType(typeof(Guid)));
			Assert.IsTrue(ReflectionHelper.IsBasicClrType(typeof(short)));
			Assert.IsTrue(ReflectionHelper.IsBasicClrType(typeof(int)));
		}
	}
}
