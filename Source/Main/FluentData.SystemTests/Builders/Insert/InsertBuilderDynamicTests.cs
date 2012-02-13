﻿using System.Dynamic;
using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData
{
	[TestClass]
	public class InsertBuilderDynamicTests
	{
		[TestMethod]
		public void Test_No_Automap()
		{
			dynamic product = new ExpandoObject();
			product.CategoryId = 1;
			product.Name = "TestProduct";

			using (var context = TestHelper.Context().UseTransaction)
			{
				product.ProductId = context.Insert("Product", (ExpandoObject) product)
									.Column("Name", (string) product.Name)
									.Column("CategoryId", (int) product.CategoryId)
									.ExecuteReturnLastId();

				var createdProduct = TestHelper.GetProduct(context, product.ProductId);
				Assert.AreEqual("TestProduct", product.Name);
				Assert.AreEqual(1, product.CategoryId);
				Assert.IsNotNull(createdProduct);
			}
		}

		[TestMethod]
		public void Test_Automap()
		{
			dynamic product = new ExpandoObject();
			product.CategoryId = 1;
			product.Name = "TestProduct";

			using (var context = TestHelper.Context().UseTransaction)
			{
				product.ProductId = context.Insert("Product", (ExpandoObject) product)
									.IgnoreProperty("ProductId")
									.AutoMap()
									.ExecuteReturnLastId();

				var createdProduct = TestHelper.GetProduct(context, product.ProductId);
				Assert.AreEqual("TestProduct", product.Name);
				Assert.AreEqual(1, product.CategoryId);
				Assert.IsNotNull(createdProduct);
			}
		}
	}
}