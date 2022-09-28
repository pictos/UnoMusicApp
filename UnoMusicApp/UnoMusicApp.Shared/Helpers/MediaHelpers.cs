using System;
using YoutubeExplode.Videos;

namespace UnoMusicApp.Helpers;
static class MediaHelpers
{
	public static YoutubeMediaFile ToYoutubeMediaFile(this IVideo videoResult) =>
		new(videoResult.Title, videoResult.Url, videoResult.Duration, videoResult.Thumbnails[^1].Url, videoResult.Id);

	public static string ToStringTime(this TimeSpan timeSpan) =>
		timeSpan.Hours > 0
		? timeSpan.ToString(@"hh\:mm\:ss")
		: timeSpan.ToString(@"mm\:ss");
}
