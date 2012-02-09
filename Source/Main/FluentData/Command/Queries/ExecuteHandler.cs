namespace FluentData
{
	internal class ExecuteHandler : BaseQueryHandler
	{
		public ExecuteHandler(DbCommandData data)
			: base(data)
		{
		}

		public T Execute<T>()
		{
			object recordsAffected = Data.InnerCommand.ExecuteNonQuery();

			return (T) recordsAffected;
		}
	}
}
