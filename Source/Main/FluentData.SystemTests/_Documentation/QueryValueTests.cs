using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
{
	[TestClass]
	public class QueryValueTests : BaseDocumentation
	{
		[TestMethod]
		public void Test()
		{
			int numberOfProducts = Context().Sql(@"select count(*)
													from Product").QueryValue<int>();

			Assert.IsTrue(numberOfProducts > 0);
		}
	}
}
