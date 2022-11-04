using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System.Diagnostics.CodeAnalysis;
using UnoMusicApp.Controls;
using UnoMusicApp.Messages;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UnoMusicApp.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PlayerPage : Page
	{
		static PlayerPage? current;
		public static PlayerPage Current => current ??= new();

		internal PlayerViewModel Vm => (PlayerViewModel)DataContext;

		PlayerPage()
		{
			this.InitializeComponent();
			CreateCircleProgress();
			DataContext = PlayerViewModel.Current;
			WeakReferenceMessenger.Default.Register<IsPlayingMessage>(this, OnIsPlayingChanged);
			gridImg.SizeChanged += (_, __) =>
			{
				var size = Math.Min(gridImg.ActualWidth, gridImg.ActualHeight);
				cicleImg.Width = size - 50;
				circleP.Width = gridImg.ActualHeight + 20;
				cicleImg.Invalidate();
				circleP.Invalidate();
			};
		}

		CircleProgress? circleP;

		[MemberNotNull(nameof(circleP))]
		void CreateCircleProgress()
		{
			circleP = new CircleProgress
			{
				StrokeWidth = 2,
				ProgressColor = Colors.Red,
				LineBackgroundColor = Colors.Black,
				IsHitTestVisible = false
			};

			var progressBind = new Binding
			{
				Source = Vm,
				Path = new("Progress"),
				Mode = BindingMode.OneWay
			};

			circleP.SetBinding(CircleProgress.ProgressProperty, progressBind);

			Grid.SetRow(circleP, 0);
			gridImg.Children.Add(circleP);
		}


		bool isFirstMusicTime = true;
		void OnIsPlayingChanged(object recipient, IsPlayingMessage message)
		{
			var (isPlaying, isStoped) = message.Value;
			if (isPlaying && isFirstMusicTime)
			{
				rotateImg.Begin();
				isFirstMusicTime = false;
			}
			else if (isPlaying)
				rotateImg.Resume();
			else if (isStoped)
			{
				rotateImg.Stop();
				isFirstMusicTime = true;
			}
			else
				rotateImg.Pause();
		}
	}
}
