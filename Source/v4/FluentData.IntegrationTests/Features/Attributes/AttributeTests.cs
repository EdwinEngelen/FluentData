using IntegrationTests._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Features.Attributes
{
	[TestClass]
	public class AttributeTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void WithAttribute()
		{
			using(var context = Context.UseTransaction(true))
			{
				var product = new ProductWithIgnoreAttribute();
				product.Name = "Test";
				product.CategoryId = (int) Categories.Movies;
				product.ProductId = context.Insert("Product", product).AutoMap(x => x.ProductId).ExecuteReturnLastId<int>();

				context.Sql("select * from Product where ProductId=@0", product.ProductId).QuerySingle<ProductWithIgnoreAttribute>();
			}
		}
	}
}
