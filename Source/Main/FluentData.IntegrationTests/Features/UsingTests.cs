using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features
{
	[TestClass]
	public class UsingTests
	{
		[TestMethod]
		public void Context_uneeded_using_statement_must_not_throw_an_exception()
		{
			using (var context = TestHelper.Context())
			{
				var categories = context.Sql(@"select *
											from Category").Query<Category>();

				Assert.IsTrue(categories.Count > 0);
			}
		}

		[TestMethod]
		public void Command_uneeded_using_statement_must_not_throw_an_exception()
		{
			using (var command = TestHelper.Context().Sql(@"select *
											from Category"))
			{
				var categories = command.Query<Category>();

				Assert.IsTrue(categories.Count > 0);
			}
		}
	}
}
