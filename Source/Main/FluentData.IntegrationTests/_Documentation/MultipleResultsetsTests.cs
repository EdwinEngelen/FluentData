using System.Collections.Generic;
using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
{
	[TestClass]
	public class MultipleResultsetsTests : BaseDocumentation
	{
		[TestMethod]
		public void MultipleResultset()
		{
			using (var command = Context().MultiResultSql)
			{
				List<Category> categories = command.Sql(@"select * from Category;
												select * from Product;").QueryMany<Category>();

				List<Product> products = command.QueryMany<Product>();

				Assert.IsTrue(categories.Count > 0);
				Assert.IsTrue(products.Count > 0);
			}
		}
	}
}
