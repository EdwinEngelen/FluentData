using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Transaction
{
	[TestClass]
	public class TransactionTests
	{
		[TestMethod]
		public void Continue_after_rollback_or_commit()
		{
			using (var context = TestHelper.Context().UseTransaction(true))
			{
				var category = context.Sql("select top 1 * from category").QuerySingle();

				context.Commit();

				category = context.Sql("select top 1 * from category").QuerySingle();
			}
		}
	}
}
