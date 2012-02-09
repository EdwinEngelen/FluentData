using System;

namespace FluentData
{
	public class FluentDbException : Exception
	{
		public FluentDbException(string message)
			: base(message)
		{
		}
		public FluentDbException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
