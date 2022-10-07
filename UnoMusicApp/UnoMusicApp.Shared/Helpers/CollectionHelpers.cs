using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnoMusicApp.Helpers
{
	static class CollectionHelpers
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T GetQueryValue<T>(this Dictionary<string, object> dic, string key)
		{
			return (T)dic[key];
		}
	}
}
