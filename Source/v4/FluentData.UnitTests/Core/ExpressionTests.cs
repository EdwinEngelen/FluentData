using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SomeNamespace;

namespace FluentData.Core
{
	[TestClass]
	public class ExpressionTests
	{
		[TestMethod]
		public void GetPropertyNameFromExpression()
		{
			var propertyName = GetPropertyName<Category>(x => x.Name);
			Assert.AreEqual("Name", propertyName);

			propertyName = GetPropertyName<Category>(x => x.Parent.Name);
			Assert.AreEqual("Parent.Name", propertyName);

			propertyName = GetPropertyName<Category>(x => x.CategoryId);
			Assert.AreEqual("CategoryId", propertyName);

			propertyName = GetPropertyName<Category>(xx => xx.Parent.CategoryId);
			Assert.AreEqual("Parent.CategoryId", propertyName);

			propertyName = GetPropertyName<Category>(x => x.Parent.Parent.Parent.Name);
			Assert.AreEqual("Parent.Parent.Parent.Name", propertyName);
		}

		protected string GetPropertyName<T>(Expression<Func<T, object>> expression)
		{
			return ReflectionHelper.GetPropertyNameFromExpression(expression);
		}
	}
}

namespace SomeNamespace
{
	public class Category
	{
		public Categories CategoryId { get; set; }
		public Category Parent { get; set; }
		public string Name { get; set; }
	}

	public enum Categories
	{
		Books = 1,
		Movies = 2
	}
}