using LibVLCSharp.Shared;
using System;
using MP = LibVLCSharp.Shared.MediaPlayer;

namespace UnoMusicApp.Services;
sealed class MediaService
{
	static readonly Lazy<MediaService> mediaServiceLazy = new(() => new());

	public static MediaService Current => mediaServiceLazy.Value;

	readonly LibVLC libVLC = new();
	readonly MP mp;

	MediaService()
	{
		mp = new(libVLC);
	}

	public void SetSong(in YoutubeMediaFile mediaFile)
	{
		if (mp.Media is not null)
			mp.Stop();

		mp.Media = new(libVLC, mediaFile.Url, FromType.FromLocation);
		mp.Media.AddOption(":no-video");
		mp.Play();
	}
}
