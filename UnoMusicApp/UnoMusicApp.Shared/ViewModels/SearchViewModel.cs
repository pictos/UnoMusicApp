
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnoMusicApp.Pages;
using UnoMusicApp.Services;
using Windows.UI.Popups;

namespace UnoMusicApp.ViewModels
{
	sealed partial class SearchViewModel : BaseViewModel
	{
		[ObservableProperty]
		string query = "Cícero";

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
			try
			{
				if (string.IsNullOrWhiteSpace(query))
				{
					var messageDialog = new MessageDialog("Você deve digitar um termo antes de pesquisar.");
					messageDialog.Commands.Add(new UICommand("Ok"));
					await messageDialog.ShowAsync().AsTask();
					return;
				}

				var list = new List<YoutubeMediaFile>();
				await foreach (var item in YoutubeService.SearchMedia(query).ConfigureAwait(false))
				{
					list.Add(item);
				}
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