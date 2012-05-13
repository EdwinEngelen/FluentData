using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData
{
	[TestClass]
	public class StoredProcedureBuilderTests
	{
		[TestMethod]
		public void Test()
		{
			using (var context = TestHelper.Context().UseTransaction(true))
			{
				var storedProcedure = context.StoredProcedure("ProductInsert")
							.ParameterOut("ProductId", DataTypes.Int32)
							.Parameter("Name", "TestProduct")
							.Parameter("CategoryId", 1);

				storedProcedure.Execute();
				var productId = storedProcedure.ParameterValue<int>("ProductId");

				Assert.IsTrue(productId > 0);
			}
		}
	}
}
