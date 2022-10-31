using System.Collections.Generic;
using UnoMusicApp.Pages;
using UnoMusicApp.Services;

namespace UnoMusicApp.ViewModels
{
	sealed partial class SearchViewModel : BaseViewModel
	{
		[ObservableProperty]
		string query = "Pink Floyd pigs";

		public ObservableRangeCollection<YoutubeMediaFile> Medias { get; } = new();

		partial void OnQueryChanged(string value)
		{
			Medias.Clear();
		}

		[RelayCommand]
		async Task SearchForQuery()
		{
			if (IsBusy)
				return;

			IsBusy = true;
			var list = new List<YoutubeMediaFile>();
			try
			{
				await Task.Run(async () =>
				{
					if (string.IsNullOrWhiteSpace(query))
						return;

					await foreach (var item in YoutubeService.SearchMedia(query))
						list.Add(item);
				});

				Medias.Clear();
				Medias.AddRange(list);
			}
			finally
			{
				IsBusy = false;
			}
		}

		[RelayCommand]
		async Task PlaySong(YoutubeMediaFile mediaFile)
		{
			var manifesTask = YoutubeService.GetManifestMedia(mediaFile);
			var values = new Dictionary<string, object>
			{
				{"audioInfo", manifesTask },
				{"mediaFile", mediaFile }
			};

			await NavigationService.NavigateTo(typeof(PlayerPage), values).ConfigureAwait(false);
		}
	}
}