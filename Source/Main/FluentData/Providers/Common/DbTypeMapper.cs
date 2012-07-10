using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FluentData.Providers.Common
{
	internal class DbTypeMapper
	{
		private static Dictionary<Type, DataTypes> _types;
		private static readonly object _locker = new object();

		public DataTypes GetDbTypeForClrType(Type clrType)
		{
			if (_types == null)
			{
				lock (_locker)
				{
					if (_types == null)
					{
						_types = new Dictionary<Type, DataTypes>();

						_types.Add(typeof(Int16), DataTypes.Int16);
						_types.Add(typeof(Int32), DataTypes.Int32);
						_types.Add(typeof(Int64), DataTypes.Int64);
						_types.Add(typeof(string), DataTypes.String);
						_types.Add(typeof(DateTime), DataTypes.DateTime);
						_types.Add(typeof(XDocument), DataTypes.Xml);
						_types.Add(typeof(decimal), DataTypes.Decimal);
						_types.Add(typeof(Guid), DataTypes.Guid);
						_types.Add(typeof(Boolean), DataTypes.Boolean);
						_types.Add(typeof(char), DataTypes.String);
						_types.Add(typeof(DBNull), DataTypes.String);
					}
				}
			}

			if (!_types.ContainsKey(clrType))
				return DataTypes.Object;

			var dbType = _types[clrType];
			return dbType;
		}
	}
}
