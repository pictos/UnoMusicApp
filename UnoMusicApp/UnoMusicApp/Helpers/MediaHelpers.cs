using static UnoMusicApp.Models.Models;
using YoutubeExplode.Videos;

namespace UnoMusicApp.Helpers;
static class MediaHelpers
{
	public static YoutubeMediaFile ToYoutubeMediaFile(this IVideo videoResult) =>
		new(videoResult.Title, videoResult.Url, videoResult.Duration, videoResult.Thumbnails[0].Url, videoResult.Id);
}
