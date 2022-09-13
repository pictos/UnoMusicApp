﻿using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;
using Uno.Extensions;
using UnoMusicApp.Helpers;
using UnoMusicApp.Services;
using Windows.UI.Popups;
using static UnoMusicApp.Models.Models;

namespace UnoMusicApp.Presentation;
public sealed partial class SearchViewModel : ViewModelBase
{
	string query = "Cícero";
	private readonly INavigator navigator;

	public string Query
	{
		get => query;
		set
		{
			if (query == value)
				return;

			query = value;

			OnQueryChanged(value);
		}
	}

	public ObservableRangeCollection<YoutubeMediaFile> Medias { get; } = new();

	void OnQueryChanged(string value)
	{
		Medias.Clear();
	}

	public SearchViewModel(
		INavigator navigator)
	{
		this.navigator = navigator;
	}

	public async Task SearchForQueryCommand()
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
			await foreach (var item in YoutubeService.SearchMedia(query))
			{
				list.Add(item);
			}

			Medias.AddRange(list);
		}
		finally
		{
			IsBusy = false;
		}
	}

	public async Task PlaySongCommand(YoutubeMediaFile mediaFile)
	{
		var manifesTask = YoutubeService.GetManifestMedia(mediaFile);
		var values = new Dictionary<string, object>
			{
				{"audioInfo", manifesTask },
				{"mediaFile", mediaFile }
			};

		await navigator.NavigateViewModelAsync<PlayerViewModel>(this);
	}
}