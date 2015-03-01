using System;
using System.Linq.Expressions;

namespace FluentData
{
	public interface IInsertUpdateBuilder<T>
	{
		BuilderData Data { get; }
		T Item { get; }
		IInsertUpdateBuilder<T> Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
		IInsertUpdateBuilder<T> Column(Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0);
	}
}
