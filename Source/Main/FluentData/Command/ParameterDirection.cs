namespace FluentData
{
	public enum ParameterDirection
	{
		// The parameter is an input parameter.
		Input = 1,

		// The parameter is an output parameter.
		Output = 2,

		// The parameter is capable of both input and output.
		InputOutput = 3,

		// The parameter represents a return value from an operation such as a stored
		// procedure, built-in function, or user-defined function.
		ReturnValue = 6,
	}
}
