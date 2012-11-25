using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Events
{
	[TestClass]
	public class OnConnectionOpenedTests
	{
		[TestMethod]
		public void Test()
		{
			var eventFired = false;

			TestHelper.Context().OnExecuted(args => eventFired = true).Sql("select top 1 * from product").Query<dynamic>();

			Assert.IsTrue(eventFired);
		}
	}
}
