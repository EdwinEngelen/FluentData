using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
{
	[TestClass]
	public class SampleCode
	{
		protected IDbContext Context()
		{
			return new DbContext().ConnectionStringName("SqlServer", DbProviderTypes.SqlServer);
		}

		[TestMethod]
		public void Get_a_single_product()
		{
			var productId = 1;

			var product = Context().Sql(@"select *	from Product where ProductId = @0")
					.Parameters(productId)
					.QuerySingle<Product>();

			Assert.IsNotNull(product);
		}

		[TestMethod]
		public void Get_many_products()
		{
			var products = Context().Sql(@"select *	from Product;")
					.Query<Product>();

			Assert.IsTrue(products.Count > 0);
		}

		[TestMethod]
		public void Create_a_new_product()
		{
			var productId = Context().Sql("insert into Product(Name, CategoryId) values(@0, @1);")
					.Parameters("The Warren Buffet Way", 1)
					.ExecuteReturnLastId();

			Assert.IsTrue(productId > 0);
		}

		[TestMethod]
		public void Update_existing_product()
		{
			var rowsAffected = Context().Sql("update Product set Name = @0 where ProductId = @1")
					.Parameters("The Warren Buffet Way", 1)
					.Execute();
		}

		[TestMethod]
		public void Delete_a_product()
		{
			var product = new Product();
			product.Name = "The Warren Buffet Way";
			product.CategoryId = 1;

			var productId = Context().Insert<Product>("Product", product)
								.IgnoreProperty(x => x.ProductId)
								.AutoMap()
								.ExecuteReturnLastId();

			var rowsAffected = Context().Sql("delete from Product where ProductId = @0")
					.Parameters(productId)
					.Execute();
		}
	}
}
