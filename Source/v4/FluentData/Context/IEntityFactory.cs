using System;

namespace FluentData
{
	public interface IEntityFactory
	{
		object Create(Type type);
	}
}
