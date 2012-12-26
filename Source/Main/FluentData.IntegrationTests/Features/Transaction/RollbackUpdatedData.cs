using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Features.Transaction
{
    [TestClass]
    public class RollbackUpdatedData : BaseSqlServerIntegrationTest
    {
        [TestMethod]
        public void Update_data_rollback()
        {
            using (var db = Context.UseTransaction(true))
            {
                var category = db.Sql("select * from Category where CategoryId = 1").QuerySingle<dynamic>();
                Assert.AreEqual("Books", category.Name);

                var affectedRows = db.Sql("update Category set Name = 'BooksChanged' where CategoryId=1").Execute();
                Assert.AreEqual(1, affectedRows);

                var updatedCategory = db.Sql("select * from Category where CategoryId = 1").QuerySingle<dynamic>();
                Assert.AreEqual("BooksChanged", updatedCategory.Name);
            }

            var rollbackedCategory = Context.Sql("select * from Category where CategoryId = 1").QuerySingle<dynamic>();

            Assert.AreEqual("Books", rollbackedCategory.Name);
        }
    }
}
