using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentData.Providers
{
	[TestClass]
	public class ProviderFactoryTests
	{
		[TestMethod]
		public void Test_installed_providers()
		{
			var providers = DbProviderFactories.GetFactoryClasses();


		}
	}
}
