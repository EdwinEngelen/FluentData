using System;

namespace FluentData
{
	internal class InsertBuilder : BaseInsertBuilder, IInsertBuilder, IInsertUpdateBuilder
	{
		internal InsertBuilder(IDbCommand command, string name)
			: base(command, name)
		{
		}

		public IInsertBuilder Column(string columnName, object value, DataTypes parameterType, int size)
		{
			Actions.ColumnValueAction(columnName, value, parameterType, size);
			return this;
		}

		IInsertUpdateBuilder IInsertUpdateBuilder.Column(string columnName, object value, DataTypes parameterType, int size)
		{
			Actions.ColumnValueAction(columnName, value, parameterType, size);
			return this;
		}

		public IInsertBuilder Fill(Action<IInsertUpdateBuilder> fillMethod)
		{
			fillMethod(this);
			return this;
		}
	}
}
