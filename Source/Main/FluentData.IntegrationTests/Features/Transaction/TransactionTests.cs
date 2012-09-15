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

		[TestMethod]
		public void Multiple_commits_should_not_throw_exception()
		{
			using(var context = TestHelper.Context().UseTransaction(true))
			{
				var category = context.Sql("select top 1 * from category").QuerySingle();

				context.Commit();

				category = context.Sql("select top 1 * from category").QuerySingle();

				context.Commit();
			}
		}

		[TestMethod]
		public void Multiple_commits_without_command_executed_should_not_throw_exception()
		{
			using(var context = TestHelper.Context().UseTransaction(true))
			{
				context.Commit();
				context.Commit();
			}
		}

		[TestMethod]
		public void Rollback_when_no_command_executed_should_not_throw_exception()
		{
			using(var context = TestHelper.Context().UseTransaction(true))
			{
				context.Rollback();
			}
		}

		[TestMethod]
		public void Multiple_rollbacks_without_command_executed_should_not_throw_exception()
		{
			using(var context = TestHelper.Context().UseTransaction(true))
			{
				context.Rollback();
				context.Rollback();
			}
		}

	}
}
