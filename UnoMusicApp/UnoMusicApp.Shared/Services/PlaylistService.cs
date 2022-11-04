using UnoMusicApp.Extensions.Toast;

namespace UnoMusicApp.Services;

static class PlaylistService
{
	static MediaService MediaService => MediaService.Current;
	static YoutubeService YoutubeService => YoutubeService.Current;

	static Toast addedToPlaylist = UnoMusicApp.Extensions.Toast.Toast.Make("Music added to playlist.");

	public static async Task PlayNext(YoutubeMediaFile mediaFile)
	{
		var playMedia = await GetMediaUrl(mediaFile);
		var mediaFiles = MediaService.MediaFiles;
		int currentIndex = 0;

		if (MediaService.CurrentMedia is not null)
			currentIndex = mediaFiles.IndexOf(MediaService.CurrentMedia) + 1;

		mediaFiles.Insert(currentIndex, playMedia);
		await addedToPlaylist.Show();
		PlayIfStopped();
	}

	public static async Task PlayLast(YoutubeMediaFile mediaFile)
	{
		var playMedia = await GetMediaUrl(mediaFile);
		MediaService.MediaFiles.Add(playMedia);
		PlayIfStopped();
	}

	static async Task<YoutubeMediaFile> GetMediaUrl(YoutubeMediaFile mediaFile)
	{
		var manifest = await YoutubeService.GetManifestMedia(mediaFile).ConfigureAwait(false);

		if (manifest is null)
			return mediaFile;

		return mediaFile with
		{
			Url = manifest.Url
		};
	}

	static void PlayIfStopped()
	{
		if (MediaService.IsPlaying)
			return;

		MediaService.SetSong(MediaService.MediaFiles[0]);
	}
}
