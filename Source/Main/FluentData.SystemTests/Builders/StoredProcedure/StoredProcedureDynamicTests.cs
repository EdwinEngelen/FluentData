using System.Dynamic;
using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData
{
	[TestClass]
	public class StoredProcedureBuilderDynamicTests
	{
		[TestMethod]
		public void Test_No_Automap()
		{
			dynamic product = new ExpandoObject();
			product.CategoryId = 1;
			product.Name = "TestProduct";

			using (var context = TestHelper.Context().UseTransaction(true))
			{
				var storedProcedure = context.StoredProcedure("ProductInsert", (ExpandoObject) product)
							.ParameterOut("ProductId", DataTypes.Int32)
							.Parameter("Name", (string) product.Name)
							.Parameter("CategoryId", (int) product.CategoryId);

				storedProcedure.Execute();
				product.ProductId = storedProcedure.ParameterValue<int>("ProductId");

				Assert.IsTrue(product.ProductId > 0);
			}
		}

		[TestMethod]
		public void Test_Automap()
		{
			dynamic product = new ExpandoObject();
			product.CategoryId = 1;
			product.Name = "TestProduct";

			using (var context = TestHelper.Context().UseTransaction(true))
			{
				var storedProcedure = context.StoredProcedure("ProductInsert", (ExpandoObject) product)
					.ParameterOut("ProductId", DataTypes.Int32)
					.AutoMap("ProductId");

				storedProcedure.Execute();
				product.ProductId = storedProcedure.ParameterValue<int>("ProductId");

				Assert.IsTrue(product.ProductId > 0);
			}
		}
	}
}
