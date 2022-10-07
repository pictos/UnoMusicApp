using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using UnoMusicApp.Helpers;
using UnoMusicApp.Messages;
using UnoMusicApp.Services;
using YoutubeExplode.Videos.Streams;

namespace UnoMusicApp.ViewModels
{
	sealed partial class PlayerViewModel : BaseViewModel
	{
		static readonly Lazy<PlayerViewModel> lazyPlayer = new(() => new());

		internal static PlayerViewModel Current => lazyPlayer.Value;

		static readonly SolidColorBrush defaultColor = new(Colors.White);
		const int OFFSET = 5_000;
		YoutubeMediaFile mediaFile;

		string videoUrl = string.Empty;

		[ObservableProperty]
		string? currentTime;

		[ObservableProperty]
		string? albumArt;

		[ObservableProperty]
		float progress;

		[ObservableProperty]
		string? total;

		[ObservableProperty]
		Brush repeatActiveColor = defaultColor;

		[ObservableProperty]
		Brush shuffleActiveColor = defaultColor;

		[ObservableProperty]
		string playText = FA.Pause;

		public override async ValueTask InitializeAsync(Dictionary<string, object> args)
		{
			var manifestTask = args.GetQueryValue<Task<AudioOnlyStreamInfo>>("audioInfo");
			mediaFile = args.GetQueryValue<YoutubeMediaFile>("mediaFile");

			var videoManifest = await manifestTask;

			videoUrl = videoManifest.Url;

			mediaFile = mediaFile with
			{
				Url = videoUrl
			};

			StartUp();
			PlayPause(mediaFile);
			FillData(mediaFile);
		}

		void OnPositionChanged(float position) =>
			Progress = position;

		void OnMediaChanged()
		{
			Total = MediaService.TotalDuration;
			AlbumArt = MediaService.CurrentMedia.ArtUrl;
		}

		void OnProgressChanged(TimeSpan duration) =>
			CurrentTime = duration.ToStringTime();

		void OnMediaInfoChanged(TimeSpan totalDuration) =>
			Total = totalDuration.ToStringTime();

		void PlayPause(YoutubeMediaFile media) =>
			MediaService.SetSong(media);

		void OnIsPlayingChanged(bool isPlaying) =>
			ControlMusicAnimation();

		void OnMediaFinished() =>
			ControlMusicAnimation();

		void ControlMusicAnimation()
		{
			PlayText = MediaService.IsPlaying ? FA.Pause : FA.Play;
			Messagecenter.Send(new IsPlayingMessage((MediaService.IsPlaying, MediaService.IsStoped)));
		}

		void FillData(in YoutubeMediaFile media)
		{
			Title = media.Title;
			AlbumArt = media.ArtUrl;


			var duration = media.Duration;
			Total = duration is null
				? string.Empty
				: duration.Value.ToStringTime();
		}

		[RelayCommand]
		void PlayPause()
		{
			if (IsBusy)
				return;
			IsBusy = true;

			try
			{
				MediaService.PlayPause();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				IsBusy = false;
			}
		}

		[RelayCommand]
		void PlayerControl(string arg)
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				switch (arg)
				{
					case "FORWARD":
						MediaService.Forward(OFFSET);
						break;
					case "BACKWARD":
						MediaService.Rewind(OFFSET);
						break;
					case "NEXT":
						MediaService.NextMusic();
						mediaFile = MediaService.CurrentMedia;
						break;
					case "BACK":
						MediaService.PreviousMusic();
						mediaFile = MediaService.CurrentMedia;
						break;
					case "REPEAT":
						MediaService.IsRepeateMode = !MediaService.IsRepeateMode;
						RepeatActiveColor = MediaService.IsRepeateMode ? new SolidColorBrush(Colors.Fuchsia) : new SolidColorBrush(Colors.White);
						break;
					case "SHUFFLE":
						MediaService.IsRandomMode = !MediaService.IsRandomMode;
						ShuffleActiveColor = MediaService.IsRandomMode ? new SolidColorBrush(Colors.Fuchsia) : new SolidColorBrush(Colors.White);
						break;
					default:
						break;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				IsBusy = false;
			}
		}

		void StartUp()
		{
			MediaService.OnFinished -= OnMediaFinished;
			MediaService.OnTimeChanged -= OnProgressChanged;
			MediaService.OnMediaInfo -= OnMediaInfoChanged;
			MediaService.OnPositionChanged -= OnPositionChanged;
			MediaService.OnIsPlayingChanged -= OnIsPlayingChanged;
			MediaService.OnMediaChanged -= OnMediaChanged;

			MediaService.OnFinished += OnMediaFinished;
			MediaService.OnTimeChanged += OnProgressChanged;
			MediaService.OnMediaInfo += OnMediaInfoChanged;
			MediaService.OnPositionChanged += OnPositionChanged;
			MediaService.OnIsPlayingChanged += OnIsPlayingChanged;
			MediaService.OnMediaChanged += OnMediaChanged;
		}
	}
}
