using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Features.Command
{
	[TestClass]
    public class MultipleResultsetTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Command_with_multiple_resultset()
		{
			using (var cmd = Context.MultiResultSql)
			{
				var category1 = cmd.Sql(@"select * from Category where CategoryId = 1;
						select * from Category where CategoryId = 2;").QuerySingle<dynamic>();

				Assert.AreEqual("Books", category1.Name);

				var category2 = cmd.QuerySingle<dynamic>();
				Assert.AreEqual("Movies", category2.Name);
			}
		}
	}
}
