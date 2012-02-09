namespace FluentData
{
	public class TableColumn
	{
		public string ColumnName { get; set; }
		public bool IsSql { get; set; }
		public string ParameterName { get; set; }
		public object Value { get; set; }

		public TableColumn(string columnName, bool isSql, object value, string parameterName)
		{
			ColumnName = columnName;
			IsSql = isSql;
			Value = value;
			ParameterName = parameterName;
		}
	}
}
