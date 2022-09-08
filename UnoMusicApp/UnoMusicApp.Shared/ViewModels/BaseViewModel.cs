using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UnoMusicApp.Services;

namespace UnoMusicApp.ViewModels
{
	[INotifyPropertyChanged]
	abstract partial class BaseViewModel
	{
		[ObservableProperty]
		bool isBusy;

		protected YoutubeService YoutubeService => YoutubeService.Current;

		protected MediaService MediaService => MediaService.Current;

		public virtual ValueTask InitializeAsync(Dictionary<string, object> args) => ValueTask.CompletedTask;

		//protected virtual ValueTask InitAsync(ReadOnlyDictionary<string, object> args)
		//{
		//	return ValueTask.CompletedTask;
		//}
	}
}
