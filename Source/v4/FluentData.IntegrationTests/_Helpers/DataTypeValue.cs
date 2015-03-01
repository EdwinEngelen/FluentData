using System;

namespace IntegrationTests._Helpers
{
	public class DataTypeValue
	{
		public int Id { get; set; }
		public string StringValue { get; set; }
		public decimal? DecimalValue { get; set; }
		public DateTime? DateTimeValue { get; set; }
		public float? FloatValue { get; set; }
	}

	public class DataTypeValueNotNullable
	{
		public int Id { get; set; }
		public string StringValue { get; set; }
		public decimal DecimalValue { get; set; }
		public DateTime DateTimeValue { get; set; }
		public float FloatValue { get; set; }
		public byte[] ByteValues { get; set; }
	}
}
