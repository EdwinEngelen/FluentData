using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Context
{
	[TestClass]
	public class TransactionTests
	{
		[TestMethod]
		public void Update_data_rollback()
		{
			using (var db = TestHelper.Context().UseTransaction(true))
			{
				var category = db.Sql("select * from Category where CategoryId = 1").QuerySingle();
				Assert.AreEqual("Books", category.Name);

				var affectedRows = db.Sql("update Category set Name = 'BooksChanged' where CategoryId=1").Execute();
				Assert.AreEqual(1, affectedRows);

				var updatedCategory = db.Sql("select * from Category where CategoryId = 1").QuerySingle();
				Assert.AreEqual("BooksChanged", updatedCategory.Name);
			}

			var rollbackedCategory = TestHelper.Context().Sql("select * from Category where CategoryId = 1").QuerySingle();

			Assert.AreEqual("Books", rollbackedCategory.Name);
		}
	}
}
