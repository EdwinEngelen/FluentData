using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Command
{
	[TestClass]
	public class SqlTests
	{
		[TestMethod]
		public void Test_append()
		{
			var command = TestHelper.Context().Sql("select * from Product;");
			Assert.AreEqual(command.GetSql(), "select * from Product;");

			command.Sql("select * from Category;");
			Assert.AreEqual(command.GetSql(), "select * from Product;select * from Category;");
		}
	}
}
