using FluentData._Helpers;

namespace FluentData._Documentation
{
	public class BaseDocumentation
	{
		public IDbContext Context()
		{
			return TestHelper.Context();
		}
	}
}
