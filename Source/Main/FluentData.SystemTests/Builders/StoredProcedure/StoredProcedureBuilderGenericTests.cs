using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData
{
	[TestClass]
	public class StoredProcedureGenericTests
	{
		[TestMethod]
		public void Test_No_Automap()
		{
			var product = new Product();
			product.Name = "TestProduct";
			product.Category = new Category();
			product.CategoryId = 1;
			
			using (var context = TestHelper.Context().UseTransaction)
			{
				var storedProcedure = context.StoredProcedure<Product>("ProductInsert", product)
							.ParameterOut("ProductId", DataTypes.Int32)
							.Parameter("Name", product.Name)
							.Parameter("CategoryId", product.Category.CategoryId);

				storedProcedure.Execute();
				product.ProductId = storedProcedure.ParameterValue<int>("ProductId");

				Assert.IsTrue(product.ProductId > 0);
			}
		}

		[TestMethod]
		public void TestAutomap()
		{
			var product = new Product();
			product.Name = "TestProduct";
			product.CategoryId = 1;

			using (var context = TestHelper.Context().UseTransaction)
			{
				var storedProcedure = context.StoredProcedure<Product>("ProductInsert", product)
					.ParameterOut("ProductId", DataTypes.Int32)
					.IgnoreProperty(x => x.ProductId)
					.AutoMap();

				storedProcedure.Execute();
				product.ProductId = storedProcedure.ParameterValue<int>("ProductId");

				Assert.IsTrue(product.ProductId > 0);
			}
		}
	}
}
