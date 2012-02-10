using System;

namespace FluentData._Helpers
{
	public class OrderReport
	{
		public int OrderId { get; set; }
		public DateTime Created { get; set; }
		public OrderLine OrderLine { get; set; }
	}

	public class OrderLine
	{
		public Product Product { get; set; }
	}
}
