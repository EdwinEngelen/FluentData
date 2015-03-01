using FluentData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Features.Builders.StoredProcedure
{
	[TestClass]
    public class StoredProcedureBuilderTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Test()
		{
			using (var context = Context.UseTransaction(true))
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
