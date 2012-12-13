using System.Configuration;

namespace FluentData
{
	public partial class DbContext
	{
		public IDbContext ConnectionString(string connectionString, IDbProvider dbProvider)
		{
			Data.ConnectionString = connectionString;
			Data.Provider = dbProvider;
			return this;
		}

		public IDbContext ConnectionStringName(string connectionstringName, IDbProvider dbProvider)
		{
			ConnectionString(GetConnectionStringFromConfig(connectionstringName), dbProvider);
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
