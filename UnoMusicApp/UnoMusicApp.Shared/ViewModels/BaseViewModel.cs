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

		[ObservableProperty]
		string title = string.Empty;

		protected YoutubeService YoutubeService => YoutubeService.Current;

		protected MediaService MediaService => MediaService.Current;

		protected WeakReferenceMessenger Messagecenter => WeakReferenceMessenger.Default;

		public virtual ValueTask InitializeAsync(Dictionary<string, object> args) => ValueTask.CompletedTask;

		//protected virtual ValueTask InitAsync(ReadOnlyDictionary<string, object> args)
		//{
		//	return ValueTask.CompletedTask;
		//}
	}
}
