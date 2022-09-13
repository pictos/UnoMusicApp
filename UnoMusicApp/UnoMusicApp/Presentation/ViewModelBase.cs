using System;
using System.Collections.Generic;
using System.Text;
using UnoMusicApp.Services;

namespace UnoMusicApp.Presentation;

public abstract partial class ViewModelBase
{
	public bool IsBusy { get; set; }

	protected YoutubeService YoutubeService => YoutubeService.Current;

	protected MediaService MediaService => MediaService.Current;

	public virtual ValueTask InitializeAsync(Dictionary<string, object> args) => ValueTask.CompletedTask;

	//protected virtual ValueTask InitAsync(ReadOnlyDictionary<string, object> args)
	//{
	//	return ValueTask.CompletedTask;
	//}
}
