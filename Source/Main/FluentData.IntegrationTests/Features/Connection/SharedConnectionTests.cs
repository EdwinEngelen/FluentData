using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Connection
{
	[TestClass]
	public class SharedConnectionTests
	{
		[TestMethod]
		public void Test_shared_connection()
		{
			using (var context = TestHelper.Context().UseSharedConnection(true))
			{
				context.Sql("select top 1 * from category").QuerySingle();

				context.Sql("select top 1 * from category").QuerySingle();
			}
		}
	}
}
