using System;
using IntegrationTests._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Features.Builders
{
	[TestClass]
	public class MultipleCallToAutoMapTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void InsertBuilder_AutoMapIsCalledMultipleTimes_Throw()
		{
			var product = new Product();
			product.Name = "Test";
			try
			{
				Context.Insert("Product", product).AutoMap().AutoMap();	
			}
			catch (Exception ex)
			{
				Assert.AreEqual("AutoMap cannot be called more than once.", ex.Message);
				return;
			}
			Assert.Fail();
		}

		[TestMethod]
		public void UpdateBuilder_AutoMapIsCalledMultipleTimes_Throw()
		{
			var product = new Product();
			product.Name = "Test";
			try
			{
				Context.Update("Product", product).AutoMap().AutoMap();
			}
			catch(Exception ex)
			{
				Assert.AreEqual("AutoMap cannot be called more than once.", ex.Message);
				return;
			}
			Assert.Fail();
		}
	}
}