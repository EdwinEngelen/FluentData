using FluentData;

namespace IntegrationTests._Helpers
{
	public class ProductWithIgnoreAttribute
	{
		[IgnoreProperty]
		public string PropertyThatDoesNotExistInDb { get; set; }
		public int ProductId { get; set; }
		public string Name { get; set; }
		public int CategoryId { get; set; }
	}
}
