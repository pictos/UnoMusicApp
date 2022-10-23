using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnoMusicApp.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UnoMusicApp.Controls
{
	public sealed partial class MusicCard
	{
		public static readonly DependencyProperty MediaFileProperty =
			DependencyProperty.Register("MediaFile", typeof(YoutubeMediaFile), typeof(MusicCard), new PropertyMetadata(default));

		public YoutubeMediaFile MediaFile
		{
			get => (YoutubeMediaFile)GetValue(MediaFileProperty);
			set => SetValue(MediaFileProperty, value);
		}

		static internal bool wasLongPress;

		public MusicCard()
		{
			this.InitializeComponent();
		}

		void OnCellHolding(object sender, HoldingRoutedEventArgs e)
		{
			if (e.HoldingState != Microsoft.UI.Input.HoldingState.Started)
				return;

			FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);

			Console.WriteLine("Started");
			wasLongPress = true;
		}

		void OnPlayLast(object sender, RoutedEventArgs e)
		{
			PlaylistService.PlayLast(MediaFile);
		}

		void OnPlayNext(object sender, RoutedEventArgs e)
		{
			PlaylistService.PlayNext(MediaFile);
		}
	}
}
