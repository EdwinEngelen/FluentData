using System;

namespace FluentData._Helpers
{
	public class Order
	{
		public int OrderId { get; set; }
		public Product Product { get; set; }
		public DateTime Created { get; set; }
	}
}
