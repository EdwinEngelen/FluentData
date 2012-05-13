using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
{
	[TestClass]
	public class QuerySingle : BaseDocumentation
	{
		[TestMethod]
		public void Query_single_dynamic()
		{
			dynamic product = Context().Sql("select * from Product where ProductId = 1").QuerySingle();

			Assert.IsNotNull(product);
		}

		[TestMethod]
		public void Query_single_strongly_typed()
		{
			Product product = Context().Sql("select * from Product where ProductId = 1").QuerySingle<Product>();

			Assert.IsNotNull(product);
		}
	}
}
