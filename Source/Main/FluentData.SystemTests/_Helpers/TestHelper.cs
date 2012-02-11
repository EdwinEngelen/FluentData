using System.Dynamic;
namespace FluentData._Helpers
{
	public static class TestHelper
	{
		public static IDbContext Context()
		{
			return new DbContext().ConnectionStringName("SqlServer", DbProviderTypes.SqlServer).ThrowExceptionIfAutoMapFails;
		}

		public static Product GetProduct(IDbContext context, int productId)
		{
			var product = context
							.Sql("select * from product where productid = @0")
							.Parameters(productId)
							.QuerySingle<Product>();

			return product;
		}

		public static ExpandoObject GetProductDynamic(IDbContext context, int productId)
		{
			var product = context
							.Sql("select * from product where productid = @0")
							.Parameters(productId)
							.QuerySingle();

			return product;
		}

		public static int InsertProduct(IDbContext context, string name, int categoryId)
		{
			var productId = context.Insert("Product")
									.Column("Name", name)
									.Column("CategoryId", categoryId)
									.ExecuteReturnLastId();
			return productId;
		}
	}
}
