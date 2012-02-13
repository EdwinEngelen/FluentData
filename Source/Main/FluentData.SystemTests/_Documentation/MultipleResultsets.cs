using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
{
	[TestClass]
	public class MultipleResultsets : BaseDocumentation
	{
		[TestMethod]
		public void MultipleResultset()
		{
			using (var command = Context().MultiResultSql())
			{
				var categories = command.Sql(@"select * from Category;
												select * from Product;").Query();

				var products = command.Query();

				Assert.IsTrue(categories.Count > 0);
				Assert.IsTrue(products.Count > 0);
			}
		}
	}
}
