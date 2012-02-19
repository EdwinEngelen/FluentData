using System.Data;
using System.Data.Common;

namespace FluentData.Providers.Common
{
	internal class ConnectionFactory
	{
		public static IDbConnection CreateConnection(string providerName, string connectionString)
		{
			var factory = DbProviderFactories.GetFactory(providerName);

			var connection = factory.CreateConnection();
			connection.ConnectionString = connectionString;
			return connection;
		}
	}
}
