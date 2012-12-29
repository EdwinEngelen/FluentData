namespace FluentData
{
	public class BuilderColumn
	{
		public string ColumnName { get; set; }
		public string ParameterName { get; set; }
		public object Value { get; set; }

		public BuilderColumn(string columnName, object value, string parameterName)
		{
			ColumnName = columnName;
			Value = value;
			ParameterName = parameterName;
		}
	}
}
