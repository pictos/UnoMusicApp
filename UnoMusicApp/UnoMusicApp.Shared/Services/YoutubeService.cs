using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnoMusicApp.Helpers;
using YoutubeExplode;
using YoutubeExplode.Search;
using YoutubeExplode.Videos.Streams;
using static UnoMusicApp.Models;

namespace UnoMusicApp.Services;

//TODO Move this to AzureFunctions
sealed class YoutubeService
{
	static readonly Lazy<YoutubeService> lazyYoutubeService = new(() => new YoutubeService());
	public readonly YoutubeClient client;
	IAsyncEnumerable<VideoSearchResult>? getVideosTask;
	IAsyncEnumerable<PlaylistSearchResult>? getPlaylistTask;
	string mediaQuery = string.Empty;
	string playlistQuery = string.Empty;
	public static YoutubeService Current => lazyYoutubeService.Value;

	YoutubeService() => client = new YoutubeClient();

	CancellationTokenSource? mediaLinkedCts;
	CancellationTokenSource? playlistLinkedCts;
	public async IAsyncEnumerable<YoutubeMediaFile> SearchMedia(string name, uint numberOfItems = 30, [EnumeratorCancellation] CancellationToken token = default)
	{
		if (name != mediaQuery)
		{
			mediaLinkedCts?.Cancel();
			mediaLinkedCts = CancellationTokenSource.CreateLinkedTokenSource(token);
			mediaQuery = name;
			getVideosTask = client.Search.GetVideosAsync(name, mediaLinkedCts.Token);
		}

		var count = 0;
		var enumerator = getVideosTask!.GetAsyncEnumerator();

		while (count <= numberOfItems && await enumerator.MoveNextAsync().ConfigureAwait(false))
		{
			count++;
			var videoResult = enumerator.Current;
			yield return videoResult.ToYoutubeMediaFile();
		}
	}

	public async IAsyncEnumerable<YoutubePlaylist> SearchPlaylist(string name, uint numberOfItems = 30, [EnumeratorCancellation] CancellationToken token = default)
	{
		if (name != playlistQuery)
		{
			playlistLinkedCts?.Cancel();
			playlistLinkedCts = CancellationTokenSource.CreateLinkedTokenSource(token);
			playlistQuery = name;
			getPlaylistTask = client.Search.GetPlaylistsAsync(name, playlistLinkedCts.Token);
		}

		var count = 0;

		var enumerator = getPlaylistTask!.GetAsyncEnumerator(token);

		while(count <= numberOfItems && await enumerator.MoveNextAsync().ConfigureAwait(false))
		{
			count++;
			var playlistResult = enumerator.Current;
			yield return new(playlistResult.Title, playlistResult.Url, playlistResult.Id);
		}
	}

	public async Task<List<YoutubeMediaFile>> GetMediaByPlaylist(string id)
	{
		var list = new List<YoutubeMediaFile>();
		await foreach(var item in client.Playlists.GetVideosAsync(id).ConfigureAwait(false))
		{
			list.Add(item.ToYoutubeMediaFile());
		}

		return list;
	}

	public Task<AudioOnlyStreamInfo?> GetManifestMedia(YoutubeMediaFile mediaFile) =>
		Task.Run(async () =>
		{
			var videoManifest = await client.Videos.Streams.GetManifestAsync(mediaFile.Id).ConfigureAwait(false);

			var audio = videoManifest.Streams
			.OfType<AudioOnlyStreamInfo>()
			.OrderByDescending(x => x.Bitrate)
			.FirstOrDefault();

			return audio;
		});
}
