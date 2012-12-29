using FluentData;
using IntegrationTests._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Features.Command
{
	[TestClass]
    public class PotentialErrorsTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void AutoMap_same_name_but_different_type()
		{
			try
			{
				var category = Context.Sql("select 1 as Name, 'AB' as CategoryId from Category where CategoryId = 1")
										.QuerySingle<Category>();

				Assert.Fail();
			}
			catch (FluentDataException)
			{
				
			}
		}
	}
}
