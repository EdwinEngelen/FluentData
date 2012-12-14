using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Command
{
	[TestClass]
	public class MultipleResultsetTests
	{
		[TestMethod]
		public void Command_with_multiple_resultset()
		{
			using (var cmd = TestHelper.Context().MultiResultSql)
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
