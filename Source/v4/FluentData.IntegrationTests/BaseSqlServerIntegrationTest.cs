using FluentData;
using IntegrationTests._Helpers;

namespace IntegrationTests
{
    public abstract class BaseSqlServerIntegrationTest
    {
        public IDbContext Context
        {
            get { return TestHelper.Context(); }
        }
    }
}
