using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Settings
{
	[TestClass]
    public class CommandTimeoutTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test()
		{
			Context.OnExecuting(args => Assert.AreEqual(330, args.Command.CommandTimeout)).CommandTimeout(330).Sql("select top 1 * from product").QueryMany<dynamic>();
		}
	}
}
