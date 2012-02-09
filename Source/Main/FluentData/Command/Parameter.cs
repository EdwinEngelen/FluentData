namespace FluentData
{
	public class Parameter
	{
		public string ParameterName { get; set; }
		public DataTypes DataTypes { get; set; }
		public object Value { get; set; }
		public ParameterDirection Direction { get; set; }
		public bool IsId { get; set; }

		public string GetParameterName(IDbProvider provider)
		{
			return provider.GetParameterName(ParameterName);
		}
	}
}
