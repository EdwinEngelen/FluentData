namespace FluentData
{
	public enum DbCommandTypes
	{
		// Summary:
		//     An SQL text command. (Default.)
		Text = 1,
		//
		// Summary:
		//     The name of a stored procedure.
		StoredProcedure = 4,
		//
		// Summary:
		//     The name of a table.
		TableDirect = 512,
	}
}
