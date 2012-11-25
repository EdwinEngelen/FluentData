using System.Collections.Generic;
using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Providers.Oracle
{
	[TestClass]
	public class OracleTests : IDbProviderTests
	{
		protected IDbContext Context()
		{
			return new DbContext().ConnectionString(TestHelper.GetConnectionStringValue("Oracle"), DbProviderTypes.Oracle);
		}

		[TestMethod]
		public void Query_many_dynamic()
		{
			var products = Context().Sql("select * from Product")
									.QueryMany<dynamic>();

			Assert.IsTrue(products.Count > 0);
		}

		[TestMethod]
		public void Query_single_dynamic()
		{
			var product = Context().Sql("select * from Product where ProductId = 1")
									.QuerySingle<dynamic>();

			Assert.IsNotNull(product);
		}

		[TestMethod]
		public void Query_many_strongly_typed()
		{
			var products = Context().Sql("select * from Product")
									.QueryMany<Product>();

			Assert.IsTrue(products.Count > 0);
		}

		[TestMethod]
		public void Query_single_strongly_typed()
		{
			var product = Context().Sql("select * from Product where ProductId = 1")
									.QuerySingle<Product>();

			Assert.IsNotNull(product);
		}

		[TestMethod]
		public void Query_auto_mapping_alias()
		{
			var product = Context().Sql(@"select p.*,
											c.CategoryId Category_CategoryId,
											c.Name Category_Name
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
									.QueryMany<Product>(Custom_mapper_using_dynamic);

			Assert.IsNotNull(products[0].Name);
		}

		public void Custom_mapper_using_dynamic(Product product, IDataReader row)
		{
			product.ProductId = row.Value.ProductId;
			product.Name = row.Value.Name;
		}

		[TestMethod]
		public void Query_custom_mapping_datareader()
		{
			var products = Context().Sql(@"select * from Product")
									.QueryMany<Product>(Custom_mapper_using_datareader);

			Assert.IsNotNull(products[0].Name);
		}

		public void Custom_mapper_using_datareader(Product product, IDataReader row)
		{
			product.ProductId = row.GetInt32("ProductId");
			product.Name = row.GetString("Name");
		}

		[TestMethod]
		public void QueryValue()
		{
			int categoryId = Context().Sql("select CategoryId from Product where ProductId = 1")
										.QuerySingle<int>();

			Assert.AreEqual(1, categoryId);
		}

		[TestMethod]
		public void QueryValues()
		{
			var categories = Context().Sql("select CategoryId from Category order by CategoryId").QueryMany<int>();

			Assert.AreEqual(2, categories.Count);
			Assert.AreEqual(1, categories[0]);
			Assert.AreEqual(2, categories[1]);
		}

		[TestMethod]
		public void Unnamed_parameters_one()
		{
			var product = Context().Sql("select * from Product where ProductId = :0", 1)
									.QuerySingle<dynamic>();

			Assert.IsNotNull(product);
		}

		[TestMethod]
		public void Unnamed_parameters_many()
		{
			var products = Context().Sql("select * from Product where ProductId = :0 or ProductId = :1", 1, 2)
									.QueryMany<dynamic>();

			Assert.AreEqual(2, products.Count);
		}

		[TestMethod]
		public void Named_parameters()
		{
			var products = Context().Sql("select * from Product where ProductId = :ProductId1 or ProductId = :ProductId2")
									.Parameter("ProductId1", 1)
									.Parameter("ProductId2", 2)
									.QueryMany<dynamic>();

			Assert.AreEqual(2, products.Count);
		}

		[TestMethod]
		public void In_Query()
		{
			var ids = new List<int>() { 1, 2, 3, 4 };

			var products = Context().Sql("select * from Product where ProductId in(:0)", ids)
									.QueryMany<dynamic>();

			Assert.AreEqual(3, products.Count);
		}

		[TestMethod]
		public void SelectBuilder_Paging()
		{
			var context = Context();

			var category = context
				.Select<Category>("CategoryId", x => x.CategoryId)
				.Select("Name", x => x.Name)
				.From("Category")
				.OrderBy("Name asc")
				.Paging(1, 1).QuerySingle();
			Assert.AreEqual("Books", category.Name);

			category = context
				.Select<Category>("CategoryId", x => x.CategoryId)
				.Select("Name", x => x.Name)
				.From("Category")
				.OrderBy("Name asc")
				.Paging(2, 1).QuerySingle();
			Assert.AreEqual("Movies", category.Name);
		}

		[TestMethod]
		public void MultipleResultset()
		{
			try
			{
				var command = Context().MultiResultSql();
				Assert.Fail();
			}
			catch (FluentDataException ex)
			{
				if (!ex.Message.Contains("The selected database does not support"))
					Assert.Fail();
			}
			catch
			{
				Assert.Fail();
			}
		}

		[TestMethod]
		public void Insert_data_sql()
		{
			var productId = Context().Sql("insert into Product(Name, CategoryId) values(:0, :1)", "The Warren Buffet Way", 1)
								.ExecuteReturnLastId<int>("ProductId");

			Assert.IsTrue(productId > 0);
		}

		[TestMethod]
		public void Insert_data_builder_no_automapping()
		{
			var productId = Context().Insert("Product")
								.Column("CategoryId", 1)
								.Column("Name", "The Warren Buffet Way")
								.ExecuteReturnLastId<int>("ProductId");

			Assert.IsTrue(productId > 0);
		}

		[TestMethod]
		public void Insert_data_builder_automapping()
		{
			var product = new Product();
			product.CategoryId = 1;
			product.Name = "The Warren Buffet Way";

			var productId = Context().Insert<Product>("Product", product)
								.AutoMap(x => x.ProductId)
								.ExecuteReturnLastId<int>("ProductId");

			Assert.IsTrue(productId > 0);
		}

		[TestMethod]
		public void Update_data_sql()
		{
			var rowsAffected = Context().Sql("update Product set Name = :0 where ProductId = :1", "The Warren Buffet Way", 1)
								.Execute();

			Assert.AreEqual(1, rowsAffected);
		}

		[TestMethod]
		public void Update_data_builder()
		{
			var rowsAffected = Context().Update("Product")
								.Column("Name", "The Warren Buffet Way")
								.Where("ProductId", 1)
								.Execute();

			Assert.AreEqual(1, rowsAffected);
		}

		[TestMethod]
		public void Update_data_builder_automapping()
		{
			var product = Context().Sql("select * from Product where ProductId = 1")
								.QuerySingle<Product>();

			product.Name = "The Warren Buffet Way";

			var rowsAffected = Context().Update<Product>("Product", product)
										.AutoMap(x => x.CategoryId)
										.Where(x => x.ProductId)
										.Execute();

			Assert.AreEqual(1, rowsAffected);
		}

		[TestMethod]
		public void Delete_data_sql()
		{
			var productId = Context().Sql(@"insert into Product(Name, CategoryId) values(:0, :1)", "The Warren Buffet Way", 1)
								.ExecuteReturnLastId<int>("ProductId");

			var rowsAffected = Context().Sql("delete from Product where ProductId = :0", productId)
									.Execute();

			Assert.AreEqual(1, rowsAffected);
		}

		[TestMethod]
		public void Delete_data_builder()
		{
			var productId = Context().Sql(@"insert into Product(Name, CategoryId) values(:0, :1)", "The Warren Buffet Way", 1)
								.ExecuteReturnLastId<int>("ProductId");

			var rowsAffected = Context().Delete("Product")
									.Where("ProductId", productId)
									.Execute();

			Assert.AreEqual(1, rowsAffected);
		}

		[TestMethod]
		public void Transactions()
		{
			using (var context = Context().UseTransaction(true))
			{
				context.Sql("update Product set Name = :0 where ProductId = :1", "The Warren Buffet Way", 1)
							.Execute();

				context.Sql("update Product set Name = :0 where ProductId = :1", "Bill Gates Bio", 2)
							.Execute();

				context.Commit();
			}
		}

		[TestMethod]
		public void Stored_procedure_sql()
		{
		}

		[TestMethod]
		public void Stored_procedure()
		{
			var product = Context().Sql("select * from Product where ProductId = 1")
							.QuerySingle<Product>();

			product.Name = "The Warren Buffet Way";

			Context().StoredProcedure("ProductUpdate", product)
								.Parameter("p_productId", product.ProductId)
								.Parameter("p_name", product.Name).Execute();
			//.OutParameter
			// OracleBindName
		}

		[TestMethod]
		public void StoredProcedure_builder_automapping()
		{
		}
		
		[TestMethod]
		public void StoredProcedure_builder_using_expression()
		{
		}

		[TestMethod]
		public void Stored_procedure_builder()
		{
			
		}
	}
}
