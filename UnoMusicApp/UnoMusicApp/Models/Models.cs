namespace UnoMusicApp.Models;
public class Models
{
	public record struct YoutubeMediaFile(string Title, string Url, TimeSpan? Duration, string ArtUrl, string Id);

	public record struct YoutubePlaylist(string Title, string Url, string Id);
}
