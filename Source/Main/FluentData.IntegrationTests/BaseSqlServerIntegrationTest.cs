using FluentData._Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentData
{
    public abstract class BaseSqlServerIntegrationTest
    {
        public IDbContext Context
        {
            get { return TestHelper.Context(); }
        }
    }
}
