using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Parameters
{
	[TestClass]
	public class OutParametersTests
	{
		[TestMethod]
		public void Test()
		{
			var command = TestHelper.Context().Sql("select top 1 @CategoryName = Name from Category")
												.ParameterOut("CategoryName", DataTypes.String, 50);
			command.Execute();

			var categoryName = command.ParameterValue<string>("CategoryName");

			Assert.IsFalse(string.IsNullOrEmpty(categoryName));
		}
	}
}
