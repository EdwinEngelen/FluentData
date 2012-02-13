using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
{
	[TestClass]
	public class InsertUpdateDataTests : BaseDocumentation
	{
		public void Test()
		{
			//var product = new Product();
			//product.Name = "The Warren Buffet Way";
			//product.CategoryId = 1;

			//var insertBuilder = Context().Insert<Product>("Product", product);
			//FillBuilder(insertBuilder);
			//var productId = insertBuilder.Execute();

			//Assert.IsTrue(productId > 0);

			//var updateBuilder = Context().Update<Product>("Product", product);
			//FillBuilder(updateBuilder);

			//int rowsAffected = updateBuilder.Execute();
		}

		public void FillBuilder(IInsertUpdateBuilder<Product> builder)
		{
			
		}
	}
}
