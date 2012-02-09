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
		public void GetPropertyNameFromExpression_single_level()
		{
			var propertyName = GetPropertyName<Category, string>(x => x.Name);

			Assert.AreEqual("Name", propertyName);
		}

		[TestMethod]
		public void GetPropertyNameFromExpression_multiple_levels()
		{
			var propertyName = GetPropertyName<Category, string>(x => x.Parent.Parent.Parent.Name);

			Assert.AreEqual("Parent.Parent.Parent.Name", propertyName);
		}

		protected string GetPropertyName<T, TProp>(Expression<Func<T, TProp>> expression)
		{
			return ReflectionHelper.GetPropertyNameFromExpression(expression);
		}
	}
}

namespace SomeNamespace
{
	public class Category
	{
		public Category Parent { get; set; }
		public string Name { get; set; }
	}
}