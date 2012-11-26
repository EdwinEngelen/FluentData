using System.Configuration;

namespace FluentData
{
	public partial class DbContext
	{
		private void ConnectionStringInternal(string connectionString, DbProviderTypes dbProviderType, IDbProvider dbProvider)
		{
			Data.ConnectionString = connectionString;
			Data.ProviderType = dbProviderType;
			Data.Provider = dbProvider;
		}

		public IDbContext ConnectionString(string connectionString, DbProviderTypes dbProviderType)
		{
			ConnectionStringInternal(connectionString, dbProviderType, new DbProviderFactory().GetDbProvider(dbProviderType));
			return this;
		}

		public IDbContext ConnectionString(string connectionString, IDbProvider dbProvider)
		{
			ConnectionStringInternal(connectionString, DbProviderTypes.Custom, dbProvider);
			return this;
		}

		public IDbContext ConnectionStringName(string connectionstringName, DbProviderTypes dbProviderType)
		{
			ConnectionStringInternal(GetConnectionStringFromConfig(connectionstringName), dbProviderType, new DbProviderFactory().GetDbProvider(dbProviderType));
			return this;
		}

		public IDbContext ConnectionStringName(string connectionstringName, IDbProvider dbProvider)
		{
			ConnectionStringInternal(GetConnectionStringFromConfig(connectionstringName), DbProviderTypes.Custom, dbProvider);
			return this;
		}

		private string GetConnectionStringFromConfig(string connectionStringName)
		{
			var settings = ConfigurationManager.ConnectionStrings[connectionStringName];
			if (settings == null)
				throw new FluentDataException("A connectionstring with the specified name was not found in the .config file");
			return settings.ConnectionString;
		}
	}
}
