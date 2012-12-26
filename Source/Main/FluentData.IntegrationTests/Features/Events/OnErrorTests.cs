using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Events
{
	[TestClass]
    public class OnErrorTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test()
		{
			var eventFired = false;

			try
			{
				Context.OnError(args => eventFired = true).Sql("sql with error").QueryMany<dynamic>();
			}
			catch
			{
			}

			Assert.IsTrue(eventFired);
		}
	}
}
