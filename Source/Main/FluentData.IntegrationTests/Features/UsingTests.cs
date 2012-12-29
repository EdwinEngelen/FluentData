using IntegrationTests._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Features
{
	[TestClass]
	public class UsingTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Context_uneeded_using_statement_must_not_throw_an_exception()
		{
			using (var context = Context)
			{
				var categories = context.Sql(@"select *
											from Category").QueryMany<Category>();

				Assert.IsTrue(categories.Count > 0);
			}
		}

		[TestMethod]
		public void Command_uneeded_using_statement_must_not_throw_an_exception()
		{
			using (var command = Context.Sql(@"select *
											from Category"))
			{
				var categories = command.QueryMany<Category>();

				Assert.IsTrue(categories.Count > 0);
			}
		}
	}
}
