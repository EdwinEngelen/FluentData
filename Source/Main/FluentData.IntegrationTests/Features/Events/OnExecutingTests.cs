using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Events
{
	[TestClass]
	public class OnExecutingTests
	{
		[TestMethod]
		public void Test()
		{
			var eventFired = false;

			var products = TestHelper.Context().OnExecuting(args =>
			                                                	{
			                                                		eventFired = true;
			                                                		args.Command.CommandText = "select top 1 * from Product";
			                                                	})
				.Sql("sql with error").Query<dynamic>();

			Assert.IsTrue(products.Count == 1);
			Assert.IsTrue(eventFired);
		}
	}
}
