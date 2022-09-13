using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnoMusicApp.Models.Models;
using Windows.Media.Devices;
using YoutubeExplode.Videos.Streams;

namespace UnoMusicApp.Presentation;
public sealed partial class PlayerViewModel : ViewModelBase
{
	YoutubeMediaFile mediaFile;

	string videoUrl = string.Empty;

	public override async ValueTask InitializeAsync(Dictionary<string, object> args)
	{
		var manifestTask = (Task<AudioOnlyStreamInfo>)args["audioInfo"];
		mediaFile = (YoutubeMediaFile)args["mediaFile"];

		var videoManifest = await manifestTask;

		videoUrl = videoManifest.Url;

		mediaFile = mediaFile with
		{
			Url = videoUrl
		};

		MediaService.SetSong(mediaFile);
	}
}
