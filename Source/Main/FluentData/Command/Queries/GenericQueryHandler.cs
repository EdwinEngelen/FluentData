using System;
using System.Collections.Generic;

namespace FluentData
{
	internal class GenericQueryHandler<TEntity>
	{
		internal TList ExecuteListReader<TList>(
									DbCommandData data,
									Action<IDataReader, TEntity> customMapperReader
					)
			where TList : IList<TEntity>
		{
			var items = (TList) data.ContextData.EntityFactory.Create(typeof(TList));

			if(ReflectionHelper.IsCustomEntity<TEntity>())
			{
				var autoMapper = new AutoMapper<TEntity>(data, typeof (TEntity));

				while (data.Reader.Read())
				{
					var item = (TEntity) data.ContextData.EntityFactory.Create(typeof (TEntity));

					if (customMapperReader == null)
						autoMapper.AutoMap(item);
					else
						customMapperReader(data.Reader, item);

					items.Add(item);
				}
			}
			else
			{
				while(data.Reader.Read())
				{
					TEntity value;

					if(data.Reader.GetFieldType(0) == typeof(TEntity))
						value = (TEntity)data.Reader.GetValue(0);
					else
						value = (TEntity)Convert.ChangeType(data.Reader.GetValue(0), typeof(TEntity));

					items.Add(value);
				}
			}

			return items;
		}

		internal TEntity ExecuteSingle(DbCommandData data,
										Action<IDataReader, TEntity> customMapper)
		{
			var item = default(TEntity);

			if(ReflectionHelper.IsCustomEntity<TEntity>())
			{
				AutoMapper<TEntity> autoMapper = null;

				autoMapper = new AutoMapper<TEntity>(data, typeof (TEntity));

				if (data.Reader.Read())
				{
					item = (TEntity) data.ContextData.EntityFactory.Create(typeof (TEntity));

					if (customMapper == null)
						autoMapper.AutoMap(item);
					else
						customMapper(data.Reader, item);
				}
			}
			else
			{
				if(data.Reader.Read())
				{
					if(data.Reader.GetFieldType(0) == typeof(TEntity))
						item = (TEntity)data.Reader.GetValue(0);
					else
						item = (TEntity)Convert.ChangeType(data.Reader.GetValue(0), typeof(TEntity));
				}
			}

			return item;
		}
	}
}
