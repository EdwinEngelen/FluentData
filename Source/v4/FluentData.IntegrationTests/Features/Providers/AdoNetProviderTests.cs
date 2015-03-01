using System.Data.Common;
using System.Data.SqlClient;
using FluentData;
using IntegrationTests._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Features.Providers
{
	[TestClass]
	public class AdoNetProviderTests
	{
		[TestMethod]
		public void Test1()
		{
			var context = new DbContext().ConnectionString(TestHelper.GetConnectionStringValue("SqlServer"), new SqlServerProvider(), new CustomDbProviderFactory());
			var product = context.Sql("select * from Product where ProductId = 1").QuerySingle<Product>();

			Assert.IsNotNull(product);
		}

		[TestMethod]
		public void Test2()
		{
			var context = new DbContext().ConnectionStringName("ProviderTest", new SqlServerProvider());
			Assert.AreEqual("AnyConnectionString", context.Data.ConnectionString);
			Assert.AreEqual(typeof(SqlClientFactory), context.Data.AdoNetProvider.GetType());
		}

		[TestMethod]
		public void Test3()
		{
			var context = new DbContext().ConnectionStringName("ProviderTest2", new SqlServerProvider());
			Assert.AreEqual("AnyConnectionString", context.Data.ConnectionString);
			Assert.AreEqual(typeof(SqlClientFactory), context.Data.AdoNetProvider.GetType());
		}
	}

	public class CustomDbProviderFactory : System.Data.Common.DbProviderFactory
	{
		public override DbConnection CreateConnection()
		{
			return new SqlConnection();
		}
	}
}
