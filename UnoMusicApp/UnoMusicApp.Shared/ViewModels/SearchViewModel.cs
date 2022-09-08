
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UnoMusicApp.Services;
using Windows.UI.Popups;

namespace UnoMusicApp.ViewModels
{
	sealed partial class SearchViewModel : BaseViewModel
	{
		[ObservableProperty]
		string query = "Cícero";

		public ObservableCollection<YoutubeMediaFile> Medias { get; } = new();

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
				await foreach (var item in YoutubeService.SearchMedia(query).ConfigureAwait(false))
				{
					Medias.Add(item);
				}
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}