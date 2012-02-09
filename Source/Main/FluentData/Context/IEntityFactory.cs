using System;

namespace FluentData
{
	public interface IEntityFactory
	{
		object Resolve(Type type);
	}
}
