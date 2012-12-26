using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Connection
{
	[TestClass]
    public class SharedConnectionTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test_shared_connection()
		{
			using (var context = Context.UseSharedConnection(true))
			{
				context.Sql("select top 1 * from category").QuerySingle<dynamic>();

				context.Sql("select top 1 * from category").QuerySingle<dynamic>();
			}
		}
	}
}
