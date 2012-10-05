using System.Collections.Generic;
using FluentData.Providers.MySql;
using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Providers.DB2
{
	[TestClass]
	public class DB2ProviderTests : IDbProviderTests
	{
		public DB2ProviderTests()
		{
			var found = Context().Sql("select count(*) from sysibm.systables where name ='CATEGORY';").QuerySingle<int>();
			if (found > 0)
			    Context().Sql("drop table ADMIN.CATEGORY;").Execute();

			found = Context().Sql("select count(*) from sysibm.systables where name ='PRODUCT';").QuerySingle<int>();
			if (found > 0)
			    Context().Sql("drop table ADMIN.PRODUCT;").Execute();

			found = Context().Sql("select count(*) from sysibm.sysroutines where routinename ='PRODUCTUPDATE';").QuerySingle<int>();
			if (found > 0)
				Context().Sql("drop procedure ADMIN.PRODUCTUPDATE;").Execute();

			Context().Sql(@"CREATE TABLE Category(
								CategoryId INTEGER NOT NULL,
								Name VARCHAR(50),
								PRIMARY KEY(CategoryId));

								CREATE TABLE Product(
								ProductId INTEGER NOT NULL GENERATED ALWAYS AS IDENTITY (START WITH 1, INCREMENT BY 1),
								Name VARCHAR(50),
								CategoryId INTEGER NOT NULL,
								Primary Key(ProductId));

							insert into Category(CategoryId, Name) values(1, 'Books');
							insert into Category(CategoryId, Name) values(2, 'Movies');

							insert into Product(Name, CategoryId) values('The Warren Buffet Way', 1);
							insert into Product(Name, CategoryId) values('Bill Gates Bio', 1);
							insert into Product(Name, CategoryId) values('James Bond - Goldeneye', 2);
							insert into Product(Name, CategoryId) values('The Bourne Identity', 2);

							create procedure ProductUpdate(in ParamProductId integer, ParamName varchar(50))
								update Product set Name = ParamName where ProductId = ParamProductId;
							").Execute();
		}

		protected IDbContext Context()
		{
			return new DbContext().ConnectionString(TestHelper.GetConnectionStringValue("DB2"), DbProviderTypes.DB2);
		}

		[TestMethod]
		public void Query_many_dynamic()
		{
			var products = Context().Sql("select * from Product").Query();

			Assert.IsTrue(products.Count > 0);
		}

		[TestMethod]
		public void Query_single_dynamic()
		{
			var product = Context().Sql("select * from Product where ProductId = 1").QuerySingle();

			Assert.IsNotNull(product);
		}

		[TestMethod]
		public void Query_many_strongly_typed()
		{
			var products = Context().Sql("select * from Product").Query<Product>();

			Assert.IsTrue(products.Count > 0);
		}

		[TestMethod]
		public void Query_single_strongly_typed()
		{
			var product = Context().Sql("select * from Product where ProductId = 1").QuerySingle<Product>();

			Assert.IsNotNull(product);
		}

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
									.Query<Product>(Custom_mapper_using_dynamic);

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
									.Query<Product>(Custom_mapper_using_datareader);

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
			var categories = Context().Sql("select CategoryId from Category order by CategoryId").Query<int>();

			Assert.AreEqual(2, categories.Count);
			Assert.AreEqual(1, categories[0]);
			Assert.AreEqual(2, categories[1]);
		}

		[TestMethod]
		public void Unnamed_parameters_one()
		{
			var product = Context().Sql("select * from Product where ProductId = @0", 1).QuerySingle();

			Assert.IsNotNull(product);
		}

		[TestMethod]
		public void Unnamed_parameters_many()
		{
			var products = Context().Sql("select * from Product where ProductId = @0 or ProductId = @1", 1, 2)
									.Query();

			Assert.AreEqual(2, products.Count);
		}

		[TestMethod]
		public void Named_parameters()
		{
			var products = Context().Sql("select * from Product where ProductId = @ProductId1 or ProductId = @ProductId2")
									.Parameter("ProductId1", 1)
									.Parameter("ProductId2", 2)
									.Query();

			Assert.AreEqual(2, products.Count);
		}

		[TestMethod]
		public void In_query()
		{
			var ids = new List<int>() { 1, 2, 3, 4 };

			var products = Context().Sql("select * from Product where ProductId in(@0)", ids)
									.Query();

			Assert.AreEqual(4, products.Count);
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
				.Paging(1, 1).Query();
			Assert.AreEqual("Books", category[0].Name);

			category = context
				.Select<Category>("CategoryId", x => x.CategoryId)
				.Select("Name", x => x.Name)
				.From("Category")
				.OrderBy("Name asc")
				.Paging(2, 1).Query();
			Assert.AreEqual("Movies", category[0].Name);
		}

		[TestMethod]
		public void MultipleResultset()
		{
			using (var command = Context().MultiResultSql())
			{
				var categories = command.Sql(@"select * from Category;
													select * from Product;").Query();
				var products = command.Query();

				Assert.IsTrue(categories.Count > 0);
				Assert.IsTrue(products.Count > 0);
			}
		}

		[TestMethod]
		public void Insert_data_sql()
		{
			var productId = Context().Sql("insert into Product(Name, CategoryId) values(@0, @1);", "The Warren Buffet Way", 1)
							.ExecuteReturnLastId<int>();

			Assert.IsTrue(productId > 0);
		}

		[TestMethod]
		public void Insert_data_builder_no_automapping()
		{
			var productId = Context().Insert("Product")
								.Column("CategoryId", 1)
								.Column("Name", "The Warren Buffet Way")
								.ExecuteReturnLastId<int>();

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
								.ExecuteReturnLastId<int>();

			Assert.IsTrue(productId > 0);
		}

		[TestMethod]
		public void Update_data_sql()
		{
			var rowsAffected = Context().Sql("update Product set Name = @0 where ProductId = @1", "The Warren Buffet Way", 1)
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
										.Where(x => x.ProductId)
										.AutoMap(x => x.ProductId)
										.Execute();

			Assert.AreEqual(1, rowsAffected);
		}

		[TestMethod]
		public void Delete_data_sql()
		{
			var productId = Context().Sql("insert into Product(Name, CategoryId) values(@0, @1);", "The Warren Buffet Way", 1)
							.ExecuteReturnLastId<int>();

			var rowsAffected = Context().Sql("delete from Product where ProductId = @0", productId)
									.Execute();

			Assert.AreEqual(1, rowsAffected);
		}

		[TestMethod]
		public void Delete_data_builder()
		{
			var productId = Context().Sql(@"insert into Product(Name, CategoryId) values(@0, @1)", "The Warren Buffet Way", 1)
								.ExecuteReturnLastId<int>();

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
				context.Sql("update Product set Name = @0 where ProductId = @1", "The Warren Buffet Way", 1)
							.Execute();

				context.Sql("update Product set Name = @0 where ProductId = @1", "Bill Gates Bio", 2)
							.Execute();

				context.Commit();
			}
		}

		[TestMethod]
		public void Stored_procedure_sql()
		{
			Context().Sql("ProductUpdate")
										.CommandType(DbCommandTypes.StoredProcedure)
										.Parameter("ParamProductId", 1)
										.Parameter("ParamName", "The Warren Buffet Way")
										.Execute();
		}

		[TestMethod]
		public void Stored_procedure_builder()
		{
			Context().StoredProcedure("ProductUpdate")
										.Parameter("ParamProductId", 1)
										.Parameter("ParamName", "The Warren Buffet Way")
										.Execute();
		}

		[TestMethod]
		public void StoredProcedure_builder_automapping()
		{
			var product = Context().Sql("select * from Product where ProductId = 1")
							.QuerySingle<Product>();
			product.Name = "The Warren Buffet Way";

			var mysqlProduct = new MySqlProduct(product);

			Context().StoredProcedure<MySqlProduct>("ProductUpdate", mysqlProduct)
											.AutoMap(x => x.ParamCategoryId).Execute();
		}

		[TestMethod]
		public void StoredProcedure_builder_using_expression()
		{
			var product = Context().Sql("select * from Product where ProductId = 1")
							.QuerySingle<Product>();
			product.Name = "The Warren Buffet Way";

			var mysqlProduct = new MySqlProduct(product);
			Context().StoredProcedure<MySqlProduct>("ProductUpdate", mysqlProduct)
											.Parameter(x => x.ParamProductId)
											.Parameter(x => x.ParamName).Execute();
		}
	}
}
