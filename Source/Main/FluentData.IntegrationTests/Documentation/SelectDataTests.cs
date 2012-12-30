using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntegrationTests._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Documentation
{
	[TestClass]
	public class SelectDataTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test()
		{
			var count = Context.Sql("select count(*) from Product").QuerySingle<int>();

			List<Product> products = Context.Select<Product>("p.*, c.Name as Category_Name")
			       .From(@"Product p 
						inner join Category c on c.CategoryId = p.CategoryId")
			       .Where("p.ProductId > 0 and p.Name is not null")
			       .OrderBy("p.Name")
			       .Paging(1, 10).QueryMany();

			Assert.AreEqual(count, products.Count);
		}
	}
}
