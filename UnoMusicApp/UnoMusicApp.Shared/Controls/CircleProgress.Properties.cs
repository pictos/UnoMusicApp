using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace UnoMusicApp.Controls
{
	public partial class CircleProgress
    {
		static readonly PropertyChangedCallback changedCallback = new(OnPropertyChanged);
		static readonly SolidColorBrush defultColor = new(Color.FromArgb(0, 0, 0, 0));

		public static readonly DependencyProperty StrokeWidthProperty =
			DependencyProperty.Register(nameof(StrokeWidth), typeof(float), typeof(CircleProgress),
				  new PropertyMetadata(0f, changedCallback));
		public static readonly DependencyProperty ProgressProperty =
				DependencyProperty.Register(nameof(Progress), typeof(float), typeof(CircleProgress), new(0f, changedCallback));

		public static readonly DependencyProperty LineBackgroundColorProperty =
			DependencyProperty.Register(nameof(LineBackgroundColor), typeof(Brush), typeof(CircleProgress), new(defultColor, changedCallback));

		public static readonly DependencyProperty ProgressColorProperty =
			DependencyProperty.Register(nameof(ProgressColor), typeof(Color), typeof(CircleProgress), new(defultColor, changedCallback));

		public float Progress
		{
			get => (float)GetValue(ProgressProperty);
			set => SetValue(ProgressProperty, value);
		}

		public Color LineBackgroundColor
		{
			get => (Color)GetValue(LineBackgroundColorProperty);
			set => SetValue(LineBackgroundColorProperty, value);
		}

		public Color ProgressColor
		{
			get => (Color)GetValue(ProgressColorProperty);
			set => SetValue(ProgressColorProperty, value);
		}

		public float StrokeWidth
		{
			get => (float)GetValue(StrokeWidthProperty);
			set => SetValue(StrokeWidthProperty, value);
		}

		static void OnPropertyChanged(DependencyObject bindable, DependencyPropertyChangedEventArgs e)
		{
			var circleProgress = bindable as CircleProgress;
#if __ANDROID__ || __IOS__ || __MACCATALYST__
			circleProgress?.Invalidate();
#endif
		}

	}
}
