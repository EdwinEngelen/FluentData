namespace FluentData
{
	internal partial class DbCommand
	{
		/// <returns>Numbers of records affected.</returns>
		public int Execute()
		{
			int recordsAffected = 0;

			_data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				recordsAffected = new ExecuteHandler().Execute<int>(_data);
			});
			return recordsAffected;
		}
	}
}
