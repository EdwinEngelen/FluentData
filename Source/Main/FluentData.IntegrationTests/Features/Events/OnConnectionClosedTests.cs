using System.Data;
using FluentData._Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Events
{
	[TestClass]
	public class OnConnectionClosedTests
	{
		[TestMethod]
		public void Test_non_transaction()
		{
			var eventFiredCounter = 0;
			var connectionState = ConnectionState.Open;

			using (var context = TestHelper.Context().OnConnectionClosed(args => { eventFiredCounter++; connectionState = args.Connection.State; }))
			{
				context.Sql("select top 1 * from product").Query();
				context.Sql("select top 1 * from product").Query();
			}

			Assert.AreEqual(2, eventFiredCounter);
			Assert.AreEqual(ConnectionState.Closed, connectionState);
		}

		[TestMethod]
		public void Test_multiple_resultset()
		{
			var eventFired = false;
			var connectionState = ConnectionState.Open;

			var context = TestHelper.Context().OnConnectionClosed(args => { eventFired = true; connectionState = args.Connection.State; });
			using (var cmd = context.MultiResultSql("select top 1 * from product;select top 1 * from Product"))
			{
				cmd.Query();
				cmd.Query();
			}

			Assert.IsTrue(eventFired);
			Assert.AreEqual(ConnectionState.Closed, connectionState);
		}

		[TestMethod]
		public void Test_transaction()
		{
			var eventFiredCounter = 0;
			var connectionState = ConnectionState.Open;

			using (var context = TestHelper.Context().UseTransaction(true).OnConnectionClosed(args => { eventFiredCounter++; connectionState = args.Connection.State; }))
			{
				context.Sql("select top 1 * from product").Query();
				context.Sql("select top 1 * from product").Query();
			}

			Assert.AreEqual(1, eventFiredCounter);
			Assert.AreEqual(ConnectionState.Closed, connectionState);
		}
	}
}
