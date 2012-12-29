using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FluentData.Features.Transaction
{
	[TestClass]
    public class TransactionTests : BaseSqlServerIntegrationTest
	{
		[TestMethod]
		public void Continue_after_rollback_or_commit()
		{
			using (var context = Context.UseTransaction(true))
			{
				var category = context.Sql("select top 1 * from category").QuerySingle<dynamic>();

				context.Commit();

				category = context.Sql("select top 1 * from category").QuerySingle<dynamic>();
			}
		}

		[TestMethod]
		public void Multiple_commits_should_not_throw_exception()
		{
			using(var context = Context.UseTransaction(true))
			{
				var category = context.Sql("select top 1 * from category").QuerySingle<dynamic>();

				context.Commit();

				category = context.Sql("select top 1 * from category").QuerySingle<dynamic>();

				context.Commit();
			}
		}

		[TestMethod]
		public void Multiple_commits_without_command_executed_should_not_throw_exception()
		{
			using(var context = Context.UseTransaction(true))
			{
				context.Commit();
				context.Commit();
			}
		}

		[TestMethod]
		public void Rollback_when_no_command_executed_should_not_throw_exception()
		{
			using(var context = Context.UseTransaction(true))
			{
				context.Rollback();
			}
		}

		[TestMethod]
		public void Multiple_rollbacks_without_command_executed_should_not_throw_exception()
		{
			using(var context = Context.UseTransaction(true))
			{
				context.Rollback();
				context.Rollback();
			}
		}

		[TestMethod]
		public void Test()
		{
			float test = 1.1f;

			
			var testInt = (int) Convert.ChangeType(test, TypeCode.Int32);
			
			Assert.IsTrue(true);
			//FluentData.BuilderTableColumn	
		}
	}
}