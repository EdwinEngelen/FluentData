using System.Dynamic;
using System.Linq;
using System.Xml.Linq;

namespace FluentData._Helpers
{
	public static class TestHelper
	{
		public static IDbContext Context()
		{
			return new DbContext().ConnectionString(GetConnectionStringValue("SqlServer"), DbProviderTypes.SqlServer);
		}

		public static string GetConnectionStringValue(string key)
		{
			var appSettings = XDocument.Load(@"C:\Data\AppSettings\FluentData.IntegrationTests\App.config");
			var addElements = appSettings.Element("configuration").Element("connectionStrings").Elements("add");
			var addElement = addElements.Single(x => x.Attribute("name").Value == key);
			return addElement.Attribute("connectionString").Value;
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
