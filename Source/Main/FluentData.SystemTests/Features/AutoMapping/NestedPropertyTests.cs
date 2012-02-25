using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.AutoMapping
{
	[TestClass]
	public class NestedPropertyTests
	{
		[TestMethod]
		public void Test_multiple_property_levels()
		{
			var report = TestHelper.Context().Sql(@"select o.*,
												l.OrderLineId as OrderLine_OrderLineId,
												p.ProductId as OrderLine_Product_ProductId,
												p.Name as OrderLine_Product_Name,
												c.CategoryId as OrderLine_Product_Category_CategoryId,
												c.Name as OrderLine_Product_Category_Name
											from [Order] o
											inner join OrderLine l on o.OrderId = l.OrderId
											inner join Product p on l.ProductId = p.ProductId
											inner join Category c on p.CategoryId = c.CategoryId")
									.Query<OrderReport>();

			Assert.IsTrue(report.Count > 0);
		}
	}
}
