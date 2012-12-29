using System.Collections.Generic;
using FluentData;
using IntegrationTests._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Documentation
{
	[TestClass]
    public class MapperDocumentCode : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Query_auto_mapping_match()
		{
			List<Product> products = Context.Sql(@"select *
													from Product")
									.QueryMany<Product>();

			Assert.IsNotNull(products[0]);
		}

		[TestMethod]
		public void Query_auto_mapping_custom_collection()
		{
			List<Product> products = Context.Sql(@"select *
													from Product")
									.QueryMany<Product, ProductionCollection>();

			Assert.IsNotNull(products[0]);
		}

		[TestMethod]
		public void Query_auto_mapping_alias_manual()
		{
			List<Product> products = Context.Sql(@"select p.*,
											c.CategoryId as Category_CategoryId,
											c.Name as Category_Name
											from Product p
											inner join Category c on p.CategoryId = c.CategoryId")
									.QueryMany<Product>();

			Assert.IsNotNull(products[0]);
			Assert.IsNotNull(products[0].Category);
			Assert.IsNotNull(products[0].Category.Name);
		}

		[TestMethod]
		public void Query_custom_mapping_dynamic()
		{
			List<Product> products = Context.Sql(@"select * from Product")
									.QueryMany<Product>(Custom_mapper_using_dynamic);

			Assert.IsNotNull(products[0].Name);
		}

		public void Custom_mapper_using_dynamic(Product product, dynamic row)
		{
			product.ProductId = row.ProductId;
			product.Name = row.Name;
		}

		[TestMethod]
		public void Query_custom_mapping_datareader()
		{
			List<Product> products = Context.Sql(@"select * from Product")
									.QueryMany<Product>(Custom_mapper_using_datareader);

			Assert.IsNotNull(products[0].Name);
		}

		public void Custom_mapper_using_datareader(Product product, IDataReader row)
		{
			product.ProductId = row.GetInt32("ProductId");
			product.Name = row.GetString("Name");
		}
	}
}
