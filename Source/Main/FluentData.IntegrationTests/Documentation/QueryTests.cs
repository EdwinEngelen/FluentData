using System.Collections.Generic;
using IntegrationTests._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Documentation
{
	[TestClass]
    public class QueryTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Query_many_dynamic()
		{
			var products = Context.Sql("select * from Product").QueryMany<dynamic>();

			Assert.IsTrue(products.Count > 0);
		}

		[TestMethod]
		public void Query_many_strongly_typed()
		{
			List<Product> products = Context.Sql("select * from Product").QueryMany<Product>();

			Assert.IsTrue(products.Count > 0);
		}

		[TestMethod]
		public void Query_many_strongly_typed_custom_collection()
		{
			ProductionCollection products = Context.Sql("select * from Product").QueryMany<Product, ProductionCollection>();

			Assert.IsTrue(products.Count > 0);
		}

		[TestMethod]
		public void In_Query()
		{
			var ids = new List<int>() { 1, 2, 3, 4 };

			var products = Context.Sql("select * from Product where ProductId in(@0)", ids).QueryMany<dynamic>();

			Assert.AreEqual(4, products.Count);
		}
	}
}
