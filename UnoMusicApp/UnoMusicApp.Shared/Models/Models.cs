using System;

namespace UnoMusicApp;
public class Models
{
	public record struct YoutubeMediaFile(string Title, string Url, TimeSpan? Duration, string ArtUrl, string Id)
	{
		public bool Equals(YoutubeMediaFile other) =>
			(Title, Url, ArtUrl, Id) == (other.Title, other.Url, other.ArtUrl, other.Id);
	}

	public record struct YoutubePlaylist(string Title, string Url, string Id);
}
