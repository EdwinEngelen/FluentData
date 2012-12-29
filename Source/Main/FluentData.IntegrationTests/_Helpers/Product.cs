namespace IntegrationTests._Helpers
{
	public class Product
	{
		public Product()
		{
		}
		public int ProductId { get; set; }
		public string Name { get; set; }
		public Category Category { get; set; }
		public int CategoryId { get; set; }
	}
}
