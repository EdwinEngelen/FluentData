using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentData._Helpers
{
	public class DataTypeValue
	{
		public int Id { get; set; }
		public string StringValue { get; set; }
		public decimal? DecimalValue { get; set; }
		public DateTime? DateTimeValue { get; set; }
	}

	public class DataTypeValueNotNullable
	{
		public int Id { get; set; }
		public string StringValue { get; set; }
		public decimal DecimalValue { get; set; }
		public DateTime DateTimeValue { get; set; }
	}
}
