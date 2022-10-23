using System;

namespace UnoMusicApp;
public class Models
{
	public record YoutubeMediaFile(string Title, string Url, TimeSpan? Duration, string ArtUrl, string Id)
	{
		public virtual bool Equals(YoutubeMediaFile other) =>
			(Title, Url, ArtUrl, Id) == (other.Title, other.Url, other.ArtUrl, other.Id);
	}

	public readonly record struct YoutubePlaylist(string Title, string Url, string Id);
}
