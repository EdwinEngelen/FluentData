using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
{
	[TestClass]
	public class MapperDocumentCode : BaseDocumentation
	{

		[TestMethod]
		public void Query_auto_mapping_alias()
		{
			var product = Context().Sql(@"select p.*,
											c.CategoryId as Category_CategoryId,
											c.Name as Category_Name
											from Product p
											inner join Category c on p.CategoryId = c.CategoryId
											where ProductId = 1")
									.QuerySingle<Product>();

			Assert.IsNotNull(product);
			Assert.IsNotNull(product.Category);
			Assert.IsNotNull(product.Category.Name);
		}

		[TestMethod]
		public void Query_custom_mapping_dynamic()
		{
			var products = Context().Sql(@"select * from Product")
									.QueryNoAutoMap<Product>(Custom_mapper_using_dynamic);

			Assert.IsNotNull(products[0].Name);
		}

		public void Custom_mapper_using_dynamic(dynamic row, Product product)
		{
			product.ProductId = row.ProductId;
			product.Name = row.Name;
		}

		[TestMethod]
		public void Query_custom_mapping_datareader()
		{
			var products = Context().Sql(@"select * from Product")
									.QueryNoAutoMap<Product>(Custom_mapper_using_datareader);

			Assert.IsNotNull(products[0].Name);
		}

		public void Custom_mapper_using_datareader(IDataReader row, Product product)
		{
			product.ProductId = row.GetInt32("ProductId");
			product.Name = row.GetString("Name");
		}
	}
}
