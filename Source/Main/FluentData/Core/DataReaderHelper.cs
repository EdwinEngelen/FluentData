using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentData
{
	internal class DataReaderHelper
	{
		internal static List<DataReaderField> GetDataReaderFields(System.Data.IDataReader reader)
		{
			var columns = new List<DataReaderField>();

			for (var i = 0; i < reader.FieldCount; i++)
			{
				var column = new DataReaderField(i, reader.GetName(i), reader.GetFieldType(i));

				if (columns.SingleOrDefault(x => x.LowerName == column.LowerName) == null)
					columns.Add(column);
			}

			return columns;
		}
	}
}
