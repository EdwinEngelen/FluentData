using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
{
	[TestClass]
	public class ParametersTests : BaseDocumentation
	{
		[TestMethod]
		public void Indexed_parameters()
		{
			dynamic products = Context().Sql("select * from Product where ProductId = @0 or ProductId = @1", 1, 2).Query<dynamic>();

			Assert.AreEqual(2, products.Count);
		}

		[TestMethod]
		public void Indexed_parameters_alternative()
		{
			dynamic products = Context().Sql("select * from Product where ProductId = @0 or ProductId = @1", 1, 2).Query<dynamic>();

			Assert.AreEqual(2, products.Count);
		}

		[TestMethod]
		public void Named_parameters()
		{
			dynamic products = Context().Sql("select * from Product where ProductId = @ProductId1 or ProductId = @ProductId2")
									.Parameter("ProductId1", 1)
									.Parameter("ProductId2", 2)
									.Query<dynamic>();

			Assert.AreEqual(2, products.Count);
		}

		[TestMethod]
		public void List_of_parameters_in_Query()
		{
			List<int> ids = new List<int>() { 1, 2, 3, 4 };

			dynamic products = Context().Sql("select * from Product where ProductId in(@0)", ids).Query<dynamic>();

			Assert.AreEqual(4, products.Count);
		}

		[TestMethod]
		public void Out_parameter()
		{
			var command = Context().Sql("select @ProductName = Name from Product where ProductId=1")
							.ParameterOut("ProductName", DataTypes.String, 100);
			command.Execute();
			string productName = command.ParameterValue<string>("ProductName");

			Assert.IsNotNull(productName);
		}
	}
}
