using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Command
{
	[TestClass]
	public class PotentialErrorsTests
	{
		[TestMethod]
		public void AutoMap_same_name_but_different_type()
		{
			try
			{
				var category = TestHelper.Context().Sql("select 1 as Name, 'AB' as CategoryId from Category where CategoryId = 1")
										.QuerySingle<Category>();

				Assert.Fail();
			}
			catch (FluentDbException)
			{
				
			}
		}
	}
}
