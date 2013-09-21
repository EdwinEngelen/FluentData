using System.Configuration;

namespace FluentData
{
	public partial class DbContext
	{
		public IDbContext ConnectionString(string connectionString, IDbProvider fluentDataProvider, string providerName = null)
		{
			if(providerName == null)
				providerName = fluentDataProvider.ProviderName;
			var adoNetProvider = System.Data.Common.DbProviderFactories.GetFactory(providerName);
			return ConnectionString(connectionString, fluentDataProvider, adoNetProvider);
		}

		public IDbContext ConnectionString(string connectionString, IDbProvider fluentDataProvider, System.Data.Common.DbProviderFactory adoNetProviderFactory)
		{
			Data.ConnectionString = connectionString;
			Data.FluentDataProvider = fluentDataProvider;
			Data.AdoNetProvider = adoNetProviderFactory;
			return this;
		}

		public IDbContext ConnectionStringName(string connectionstringName, IDbProvider dbProvider)
		{
			var settings = ConfigurationManager.ConnectionStrings[connectionstringName];
			if(settings == null)
				throw new FluentDataException("A connectionstring with the specified name was not found in the .config file");
			
			ConnectionString(settings.ConnectionString, dbProvider, settings.ProviderName);
			return this;
		}
	}
}
