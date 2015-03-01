namespace FluentData
{
	internal partial class DbCommand
	{
		public int Execute()
		{
			var recordsAffected = 0;

			Data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				recordsAffected = Data.InnerCommand.ExecuteNonQuery();
			});
			return recordsAffected;
		}
	}
}
