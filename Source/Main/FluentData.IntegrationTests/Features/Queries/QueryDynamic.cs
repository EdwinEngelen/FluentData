using System.Collections.Generic;
using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Queries
{
	[TestClass]
    public class QueryDynamic : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test()
		{
			var categories = Context.Sql("select * from Category").QueryMany<dynamic>();
			Assert.IsTrue(categories.Count > 0);
		}
	}
}
