namespace FluentData
{
	internal partial class DbCommand
	{
		/// <returns>Numbers of records affected.</returns>
		public int Execute()
		{
			var recordsAffected = 0;

			Data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				recordsAffected = new ExecuteHandler().Execute<int>(Data);
			});
			return recordsAffected;
		}
	}
}
