using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
{
	[TestClass]
	public class QueryValuesTests : BaseDocumentation
	{
		[TestMethod]
		public void Test()
		{
			List<int> productIds = Context().Sql(@"select ProductId
												from Product").QueryMany<int>();

			Assert.IsTrue(productIds.Count > 0);
		}
	}
}
