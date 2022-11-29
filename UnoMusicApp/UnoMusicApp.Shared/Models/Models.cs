using System.Diagnostics;

namespace UnoMusicApp;

[DebuggerDisplay("{Title}")]
public sealed record YoutubeMediaFile(string Title, string Url, TimeSpan? Duration, string ArtUrl, string Id) : IEquatable<YoutubeMediaFile>
{
	public bool Equals(YoutubeMediaFile? other) =>
		(Title, Url, ArtUrl, Id) == (other?.Title, other?.Url, other?.ArtUrl, other?.Id);

	public override int GetHashCode() => base.GetHashCode();
}

public readonly record struct YoutubePlaylist(string Title, string Url, string Id);