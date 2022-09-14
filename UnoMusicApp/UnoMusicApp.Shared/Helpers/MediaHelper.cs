using System;

namespace UnoMusicApp.Helpers
{
	public static class MediaHelper
	{
		public static string ToStringTime(this TimeSpan timeSpan) =>
			timeSpan.Hours > 0
			? timeSpan.ToString(@"hh\:mm\:ss")
			: timeSpan.ToString(@"mm\:ss");
	}
}
