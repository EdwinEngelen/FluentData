using System.Collections.Generic;
using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
{
	[TestClass]
	public class Query : BaseDocumentation
	{
		[TestMethod]
		public void Query_many_dynamic()
		{
			var products = Context().Sql("select * from Product").Query();

			Assert.IsTrue(products.Count > 0);
		}

		[TestMethod]
		public void Query_many_strongly_typed()
		{
			List<Product> products = Context().Sql("select * from Product").Query<Product>();

			Assert.IsTrue(products.Count > 0);
		}

		[TestMethod]
		public void Query_many_strongly_typed_custom_collection()
		{
			ProductionCollection products = Context().Sql("select * from Product").Query<Product, ProductionCollection>();

			Assert.IsTrue(products.Count > 0);
		}

		[TestMethod]
		public void In_query()
		{
			var ids = new List<int>() { 1, 2, 3, 4 };

			var products = Context().Sql("select * from Product where ProductId in(@0)", ids).Query();

			Assert.AreEqual(4, products.Count);
		}
	}
}
