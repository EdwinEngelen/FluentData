namespace FluentData
{
	public class TableColumn
	{
		public string ColumnName { get; set; }
		public string ParameterName { get; set; }
		public object Value { get; set; }

		public TableColumn(string columnName, object value, string parameterName)
		{
			ColumnName = columnName;
			Value = value;
			ParameterName = parameterName;
		}
	}
}
