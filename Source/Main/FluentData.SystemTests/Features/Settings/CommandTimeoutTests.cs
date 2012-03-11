using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Settings
{
	[TestClass]
	public class CommandTimeoutTests
	{
		[TestMethod]
		public void Test()
		{
			TestHelper.Context().OnExecuting(args => Assert.AreEqual(330, args.Command.CommandTimeout)).CommandTimeout(330).Sql("select top 1 * from product").Query();
		}
	}
}
