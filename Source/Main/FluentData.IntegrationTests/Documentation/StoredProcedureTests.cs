using FluentData;
using IntegrationTests._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Documentation
{
	[TestClass]
    public class StoredProcedureTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Stored_procedure_sql()
		{
			var rowsAffected = Context.Sql("execute ProductUpdate @ProductId = @0, @Name = @1", 1, "The Warren Buffet Way")
							.Execute();

			Assert.AreEqual(1, rowsAffected);
		}

		[TestMethod]
		public void Stored_procedure_sql_stored_procedure_command_type()
		{
var rowsAffected = Context.Sql("ProductUpdate")
							.CommandType(DbCommandTypes.StoredProcedure)
							.Parameter("ProductId", 1)
							.Parameter("Name", "The Warren Buffet Way")
							.Execute();

			Assert.AreEqual(1, rowsAffected);
		}

		[TestMethod]
		public void Stored_procedure_builder()
		{
			var rowsAffected = Context.StoredProcedure("ProductUpdate")
										.Parameter("Name", "The Warren Buffet Way")
										.Parameter("ProductId", 1).Execute();

			Assert.AreEqual(1, rowsAffected);
		}

		[TestMethod]
		public void StoredProcedure_builder_automapping()
		{
			var product = Context.Sql("select * from Product where ProductId = 1")
							.QuerySingle<Product>();

			product.Name = "The Warren Buffet Way";

			var rowsAffected = Context.StoredProcedure<Product>("ProductUpdate", product)
											.AutoMap(x => x.CategoryId, x => x.Category).Execute();

			Assert.AreEqual(1, rowsAffected);
		}

		[TestMethod]
		public void StoredProcedure_builder_using_expression()
		{
			var product = Context.Sql("select * from Product where ProductId = 1")
							.QuerySingle<Product>();
			product.Name = "The Warren Buffet Way";

			var rowsAffected = Context.StoredProcedure<Product>("ProductUpdate", product)
											.Parameter(x => x.ProductId)
											.Parameter(x => x.Name).Execute();

			Assert.AreEqual(1, rowsAffected);
		}
	}
}
