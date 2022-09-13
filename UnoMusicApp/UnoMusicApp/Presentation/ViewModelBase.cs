using System;
using System.Collections.Generic;
using System.Text;
using UnoMusicApp.Services;

namespace UnoMusicApp.Presentation;

public partial class ViewModelBase
{
	public bool IsBusy { get; set; }
	protected YoutubeService YoutubeService => YoutubeService.Current;

	protected MediaService MediaService => MediaService.Current;

	public virtual async ValueTask InitializeAsync(Dictionary<string, object> args) { }

	//protected virtual ValueTask InitAsync(ReadOnlyDictionary<string, object> args)
	//{
	//	return ValueTask.CompletedTask;
	//}
}
