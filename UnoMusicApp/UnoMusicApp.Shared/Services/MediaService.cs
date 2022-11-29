using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using MP = LibVLCSharp.Shared.MediaPlayer;

namespace UnoMusicApp.Services;
sealed class MediaService
{
	static readonly Lazy<MediaService> mediaServiceLazy = new(() => new());

	public static MediaService Current => mediaServiceLazy.Value;

	readonly LibVLC libVLC = new();
	readonly MP mp;
	public YoutubeMediaFile? CurrentMedia { get; private set; }
	public List<YoutubeMediaFile> MediaFiles { get; } = new(5);

	public string TotalDuration => TimeSpan.FromMilliseconds(mp.Media?.Duration ?? 0).ToStringTime();

	public bool IsPlaying => mp.IsPlaying;

	public bool IsStoped { get; private set; }

	public Action<TimeSpan>? OnTimeChanged { get; set; }

	public Action? OnMediaChanged { get; set; }

	public Action<float>? OnPositionChanged { get; set; }

	public Action? OnFinished { get; set; }

	public Action<TimeSpan>? OnMediaInfo { get; set; }

	public Action<bool>? OnIsPlayingChanged { get; set; }

	public bool IsRandomMode { get; set; }

	public bool IsRepeateMode { get; set; } = true;

	bool isInit;

	MediaService()
	{
		mp = new(libVLC);
	}

	public void SetSong(in YoutubeMediaFile mediaFile)
	{
		if (mp.Media is not null)
			mp.Stop();

		CurrentMedia = mediaFile;
		mp.Media = new(libVLC, mediaFile.Url, FromType.FromLocation);
		mp.Media.AddOption(":no-video");
		IsStoped = false;
		mp.Play();
		AddToPlaylist();
		Init();

#if ANDROID
		DependencyService.Get<INotification>().ShowNotification();
#endif
	}

	void AddToPlaylist()
	{
		if (CurrentMedia is null || MediaFiles.Any(x => x.Id == CurrentMedia.Id))
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
		ThreadHelpers.BeginInvokeOnMainThread(() => OnIsPlayingChanged?.Invoke(false));

	void Playing(object? sender, EventArgs e) =>
		ThreadHelpers.BeginInvokeOnMainThread(() => OnIsPlayingChanged?.Invoke(true));

	void EndReached(object? sender, EventArgs e) =>
		ThreadHelpers.BeginInvokeOnMainThread(() =>
		{
			OnFinished?.Invoke();

			if (ShouldReturn())
				return;

			NextMusic();


			bool ShouldReturn()
			{
				var media = CurrentMedia;

				if (media is null)
					return true;

				var index = MediaFiles.IndexOf(media);
				return !IsRepeateMode && (++index == MediaFiles.Count);
			}
		});


	void LengthChanged(object? sender, MediaPlayerLengthChangedEventArgs e) =>
		ThreadHelpers.BeginInvokeOnMainThread(() =>
		{
			if (e.Length > 0)
			{
				var duration = TimeSpan.FromMilliseconds(e.Length);
				OnMediaInfo?.Invoke(duration);
				mp.LengthChanged -= LengthChanged;

				if (CurrentMedia is null)
					return;

				CurrentMedia = CurrentMedia with
				{
					Duration = duration
				};
			}
		});

	void PositionChanged(object? sender, MediaPlayerPositionChangedEventArgs e) =>
		ThreadHelpers.BeginInvokeOnMainThread(() => OnPositionChanged?.Invoke(e.Position));

	void TimeChanged(object? sender, MediaPlayerTimeChangedEventArgs e) =>
		ThreadHelpers.BeginInvokeOnMainThread(() => OnTimeChanged?.Invoke(TimeSpan.FromMilliseconds(e.Time)));

	void OnMpMediaChanged(object? sender, MediaPlayerMediaChangedEventArgs e) =>
		ThreadHelpers.BeginInvokeOnMainThread(() =>
		{
			mp.LengthChanged -= LengthChanged;
			mp.LengthChanged += LengthChanged;
			OnMediaChanged?.Invoke();
		});

	void OnStop(object? sender, EventArgs e) =>
		ThreadHelpers.BeginInvokeOnMainThread(() => IsStoped = true);

	public void NextMusic()
	{
		if (CurrentMedia is null)
			return;

		var media = CurrentMedia;
		var totalMedia = MediaFiles.Count;
		var index = MediaFiles.IndexOf(media);
		index = index is -1 ? 0 : index;

		if (index == totalMedia - 1)
			return;

		if (!IsRepeateMode)
			index = IsRandomMode
				? Random.Shared.Next(0, totalMedia - 1)
				: ++index;
		else
			++index;

		if (index > totalMedia)
			index = totalMedia;

		SetCurrentMusic(ref index);
	}

	public void PreviousMusic()
	{
		if (CurrentMedia is null)
			return;

		var media = CurrentMedia;
		var index = MediaFiles.IndexOf(media);

		var totalMedia = MediaFiles.Count;
		if (index == totalMedia - 1)
			return;

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
			mp.Stopped += OnStop;
		}
	}
}
