using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using MP = LibVLCSharp.Shared.MediaPlayer;

namespace UnoMusicApp.Services;
sealed class MediaService
{
	static readonly Lazy<MediaService> mediaServiceLazy = new(() => new());

	public static MediaService Current => mediaServiceLazy.Value;

	readonly LibVLC libVLC = new();
	readonly MP mp;
	public YoutubeMediaFile CurrentMedia { get; private set; }
	public List<YoutubeMediaFile> MediaFiles { get; } = new(5);

	public string TotalDuration => TimeSpan.FromMilliseconds(mp.Media?.Duration ?? 0).ToStringTime();

	public bool IsPlaying => mp.IsPlaying;

	public Action<TimeSpan>? OnTimeChanged { get; set; }

	public Action? OnMediaChanged { get; set; }

	public Action<float>? OnPositionChanged { get; set; }

	public Action? OnFinished { get; set; }

	public Action<TimeSpan>? OnMediaInfo { get; set; }

	public Action<bool>? OnIsPlayingChanged { get; set; }

	public bool IsRandomMode { get; set; }

	public bool IsRepeateMode { get; set; }

	bool isInit;

	MediaService()
	{
		mp = new(libVLC);
	}

	public void SetSong(in YoutubeMediaFile mediaFile)
	{
		if (mp.Media is not null)
			mp.Stop();

		mp.Media = new(libVLC, mediaFile.Url, FromType.FromLocation);
		mp.Media.AddOption(":no-video");
		mp.Play();
		CurrentMedia = mediaFile;
		AddToPlaylist();
		Init();
	}

	void AddToPlaylist()
	{
		if (MediaFiles.Contains(CurrentMedia))
			return;

		MediaFiles.Add(CurrentMedia);
	}

	public void PlayPause()
	{
		if (!IsPlaying)
			mp.Play();
		else
			mp.Pause();
	}

	public void Forward(long offSet) =>
			mp.Time += offSet;

	public void Rewind(long offSet) =>
		mp.Time -= offSet;

	public void SetMediaTime(long time) =>
		mp.Time = time;

	void Paused(object? sender, EventArgs e) =>
		Task.Run(() => OnIsPlayingChanged?.Invoke(false));

	void Playing(object? sender, EventArgs e) =>
		Task.Run(() => OnIsPlayingChanged?.Invoke(true));

	void EndReached(object? sender, EventArgs e) =>
		Task.Run(() =>
		{
			OnFinished?.Invoke();
			NextMusic();
		});

	void LengthChanged(object? sender, MediaPlayerLengthChangedEventArgs e) =>
		Task.Run(() =>
		{
			if (e.Length > 0)
			{
				var duration = TimeSpan.FromMilliseconds(e.Length);
				OnMediaInfo?.Invoke(duration);
				mp.LengthChanged -= LengthChanged;
				CurrentMedia = CurrentMedia with
				{
					Duration = duration
				};
				//CurrentMedia = new(CurrentMedia.Title, CurrentMedia.ArtUrl, duration, CurrentMedia.ArtUrl, CurrentMedia.Id);
			}
		});

	void PositionChanged(object? sender, MediaPlayerPositionChangedEventArgs e) =>
		Task.Run(() => OnPositionChanged?.Invoke(e.Position));

	void TimeChanged(object? sender, MediaPlayerTimeChangedEventArgs e) =>
		Task.Run(() => OnTimeChanged?.Invoke(TimeSpan.FromMilliseconds(e.Time)));

	void OnMpMediaChanged(object? sender, MediaPlayerMediaChangedEventArgs e) =>
		Task.Run(() =>
		{
			mp.LengthChanged -= LengthChanged;
			mp.LengthChanged += LengthChanged;
			OnMediaChanged?.Invoke();
		});

	public void NextMusic()
	{
		if (CurrentMedia == default)
			return;
		var media = CurrentMedia;
		var index = MediaFiles.IndexOf(media);
		index = index is -1 ? 0 : index;

		if (!IsRepeateMode)
			index = IsRandomMode
				? Random.Shared.Next(0, MediaFiles.Count - 1)
				: ++index;

		if (index > MediaFiles.Count)
			index = MediaFiles.Count;

		SetCurrentMusic(ref index);
	}

	public void PreviousMusic()
	{
		if (CurrentMedia == default)
			return;
		var media = CurrentMedia;
		var index = MediaFiles.IndexOf(media);
		index--;
		SetCurrentMusic(ref index);
	}

	void SetCurrentMusic(ref int index)
	{
		var totalMedia = MediaFiles.Count;

		if (totalMedia is 0)
			return;

		if (index < 0)
			index = totalMedia - 1;

		index %= totalMedia;

		var music = MediaFiles[index];

		SetSong(music);
	}

	void Init()
	{
		// Utils.WhatThreadAmI();
		if (!isInit)
		{
			isInit = true;

			mp.TimeChanged += TimeChanged;
			mp.PositionChanged += PositionChanged;
			mp.LengthChanged += LengthChanged;
			mp.EndReached += EndReached;
			mp.Playing += Playing;
			mp.Paused += Paused;
			mp.MediaChanged += OnMpMediaChanged;
		}
	}

}
