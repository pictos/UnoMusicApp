using UnoMusicApp.Services;

namespace UnoMusicApp.ViewModels
{
	[INotifyPropertyChanged]
	abstract partial class BaseViewModel
	{
		[ObservableProperty]
		bool isBusy;

		protected YoutubeService YoutubeService => YoutubeService.Current;
	}
}
