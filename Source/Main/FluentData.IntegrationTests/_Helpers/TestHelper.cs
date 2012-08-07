using System.Dynamic;
using System.Linq;
using System.Xml.Linq;

namespace FluentData._Helpers
{
	public static class TestHelper
	{
		private static bool _isInitialized = false;
		private static readonly object Locker = new object();
		public static IDbContext Context()
		{
			var context = new DbContext().ConnectionString(GetConnectionStringValue("SqlServer"), DbProviderTypes.SqlServer);

			if (!_isInitialized)
			{
				lock (Locker)
				{
					if (!_isInitialized)
					{
						context.Sql(@"
						if  exists (select * from sys.objects where object_Id = object_Id('product') and type in ('u'))
							drop table Product
						if  exists (select * from sys.objects where object_Id = object_Id('orderline') and type in ('u'))
							drop table OrderLine
						if  exists (select * from sys.objects where object_Id = object_Id('order') and type in ('u'))
							drop table [Order]
						if  exists (select * from sys.objects where object_Id = object_Id('datatypevalue') and type in ('u'))
							drop table DataTypeValue
						if  exists (select * from sys.objects where object_Id = object_Id('customer') and type in ('u'))
							drop table Customer
						if  exists (select * from sys.objects where object_Id = object_Id('category') and type in ('u'))
							drop table Category
						IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ProductInsert]') AND type in (N'P', N'PC'))
							DROP PROCEDURE [ProductInsert]
						IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ProductManyByCategoryId]') AND type in (N'P', N'PC'))
							DROP PROCEDURE [ProductManyByCategoryId]
						IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ProductUpdate]') AND type in (N'P', N'PC'))
							DROP PROCEDURE [ProductUpdate]
						IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TestOutputParameter]') AND type in (N'P', N'PC'))
							DROP PROCEDURE [TestOutputParameter]

						create table Product(
							ProductId int Identity(1,1) not null primary key,
							Name nvarchar(50) not null,
							CategoryId int not null)
	
						set Identity_insert Product on;

						create table OrderLine(
							OrderLineId int not null primary key,
							OrderId int not null,
							ProductId int not null)

						create table [Order](
							OrderId int not null primary key,
							Created datetime not null)

						create table DataTypeValue(
							Id int Identity(1,1) not null primary key,
							StringValue nvarchar(50) null,
							DecimalValue numeric(18, 0) null,
							DatetimeValue datetime null,
							FloatValue real null)

						create table Customer(
							CustomerId int Identity(1,1) not null primary key,
							Name nvarchar(100) null
						)

						create table Category(
							CategoryId int not null primary key,
							Name nvarchar(50) not null,
							ProductCount int null)

						insert into Category(CategoryId, Name)
						select 1, 'Books'
						union select 2, 'Movies';

						insert into Product(ProductId, Name, CategoryId)
						select 1, 'The Warren Buffet Way', 1
						union select 2, 'Bill Gates Bio', 1
						union select 3, 'James Bond - Goldeneye', 2
						union select 4, 'The Bourne Identity', 2;

						insert into [Order](OrderId, Created)
						values(1, getdate());

						insert into [OrderLine](OrderLineId, OrderId, ProductId)
						values(1, 1, 1);
						").Execute();

						context.Sql(@"
								CREATE PROCEDURE [ProductInsert]
								(
									@ProductId int output,
									@Name nvarchar(100),
									@CategoryId int
								)
								AS
								BEGIN
									SET NOCOUNT ON;

									insert into Product(Name, CategoryId)
									values(@Name, @CategoryId);
    
									set @ProductId = scope_identity();
								END").Execute();

						context.Sql(@"
								CREATE PROCEDURE [ProductManyByCategoryId]
								(
									@CategoryId int
								)

								AS
								BEGIN
									SET NOCOUNT ON;

									select * from Product
									where CategoryId = @CategoryId;
								END").Execute();

						context.Sql(@"
								CREATE PROCEDURE [ProductUpdate]
									@ProductId int,
									@Name nvarchar(100)
								AS
								BEGIN
									update Product
									set Name = @Name
									where ProductId = @ProductId;
								END").Execute();

								context.Sql(@"CREATE PROCEDURE [TestOutputParameter]
								(
									@ProductName nvarchar(50) output
								)
								AS
								BEGIN
									SET NOCOUNT ON;

									set @ProductName = (select top 1 Name from Product);
								END").Execute();
						_isInitialized = true;
					}
				}
			}

			return context;
		}

		public static string GetConnectionStringValue(string key)
		{
			var appSettings = XDocument.Load(@"D:\Google Drive\AppSettings\FluentData.IntegrationTests\App.config");
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
