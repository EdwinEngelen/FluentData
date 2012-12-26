using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Events
{
	[TestClass]
    public class OnExecutingTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test()
		{
			var eventFired = false;

			var products = Context.OnExecuting(args =>
			                                                	{
			                                                		eventFired = true;
			                                                		args.Command.CommandText = "select top 1 * from Product";
			                                                	})
				.Sql("sql with error").QueryMany<dynamic>();

			Assert.IsTrue(products.Count == 1);
			Assert.IsTrue(eventFired);
		}
	}
}
