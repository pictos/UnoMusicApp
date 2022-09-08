using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UnoMusicApp.Services;
using YoutubeExplode.Videos.Streams;

namespace UnoMusicApp.ViewModels
{
	sealed partial class PlayerViewModel : BaseViewModel
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
}
