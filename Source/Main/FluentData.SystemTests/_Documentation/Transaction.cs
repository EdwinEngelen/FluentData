using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData._Documentation
{
	[TestClass]
	public class Transaction : BaseDocumentation
	{
		[TestMethod]
		public void Transactions()
		{
			using (var db = Context().UseTransaction)
			{
				db.Sql("update Product set Name = @0 where ProductId = @1")
							.Parameters("The Warren Buffet Way", 1)
							.Execute();

				db.Sql("update Product set Name = @0 where ProductId = @1")
							.Parameters("Bill Gates Bio", 2)
							.Execute();

				db.Commit();
			}
		}
	}
}
