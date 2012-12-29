namespace FluentData
{
	public class BuilderTableColumn
	{
		public string ColumnName { get; set; }
		public string ParameterName { get; set; }
		public object Value { get; set; }

		public BuilderTableColumn(string columnName, object value, string parameterName)
		{
			ColumnName = columnName;
			Value = value;
			ParameterName = parameterName;
		}
	}
}
