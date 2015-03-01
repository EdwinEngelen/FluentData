using System;

namespace FluentData
{
	public interface IUpdateBuilderDynamic : IExecute
	{
		BuilderData Data { get; }
		dynamic Item { get; }		
		IUpdateBuilderDynamic AutoMap(params string[] ignoreProperties);
		IUpdateBuilderDynamic Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
		IUpdateBuilderDynamic Column(string propertyName, DataTypes parameterType = DataTypes.Object, int size = 0);
		IUpdateBuilderDynamic Where(string name, DataTypes parameterType = DataTypes.Object, int size = 0);
		IUpdateBuilderDynamic Where(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
		IUpdateBuilderDynamic Fill(Action<IInsertUpdateBuilderDynamic> fillMethod);
	}
}