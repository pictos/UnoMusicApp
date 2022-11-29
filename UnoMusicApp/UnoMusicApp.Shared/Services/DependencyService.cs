using System.Collections.Generic;

namespace UnoMusicApp.Services;

public static class DependencyService
{
	static readonly Dictionary<Type, object> instances = new();

	public static void RegisterDependency<T>(object implementation)
	{
		var type = typeof(T);
		instances[type] = implementation;
	}


	public static T Get<T>()
	{
		var type = typeof(T);
		return (T)instances[type];
	}
}
