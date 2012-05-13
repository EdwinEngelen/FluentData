using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Builders.StoredProcedure
{
	[TestClass]
	public class ParameterTests
	{
		[TestMethod]
		public void Test_Output()
		{
			var builder = TestHelper.Context().StoredProcedure("TestOutputParameter").ParameterOut("ProductName", DataTypes.String, 50);

			builder.Execute();

			var value = builder.ParameterValue<string>("ProductName");

			Assert.IsNotNull(value);
		}
	}
}
