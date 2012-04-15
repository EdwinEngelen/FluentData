using FluentData.Providers.Access;
using FluentData.Providers.MySql;
using FluentData.Providers.Oracle;
using FluentData.Providers.SqlServer;
using FluentData.Providers.SqlServerCompact;

namespace FluentData
{
	internal class DbProviderFactory
	{
		public virtual IDbProvider GetDbProvider(DbProviderTypes dbProvider)
		{
			IDbProvider provider = null;
			switch (dbProvider)
			{
				case DbProviderTypes.SqlServer:
				case DbProviderTypes.SqlAzure:
					provider = new SqlServerProvider();
					break;
				case DbProviderTypes.SqlServerCompact40:
					provider = new SqlServerCompactProvider();
					break;
				case DbProviderTypes.Oracle:
					provider = new OracleProvider();
					break;
				case DbProviderTypes.MySql:
					provider = new MySqlProvider();
					break;
				case DbProviderTypes.Access:
					provider = new AccessProvider();
					break;
			}

			return provider;
		}
	}
}
