using System;
using System.Collections.Generic;
using System.Text;

namespace UnoMusicApp.Presentation;

[INotifyPropertyChanged]
abstract partial class ViewModelBase
{
	[ObservableProperty]
	bool isBusy;

	//protected YoutubeService YoutubeService => YoutubeService.Current;

	//protected MediaService MediaService => MediaService.Current;

	//public virtual ValueTask InitializeAsync(Dictionary<string, object> args) => ValueTask.CompletedTask;

	//protected virtual ValueTask InitAsync(ReadOnlyDictionary<string, object> args)
	//{
	//	return ValueTask.CompletedTask;
	//}
}
