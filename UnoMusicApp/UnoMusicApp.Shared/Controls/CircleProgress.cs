#if ANDROID || IOS || MACCATALYST
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using SkiaSharp;
using SkiaSharp.Views.Windows;
using System;
using Windows.UI;

namespace UnoMusicApp.Controls;

public partial class CircleProgress : SKXamlCanvas
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

	private const float StartAngle = 15;
	private const float SweepAngle = 270;

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
		circleProgress?.Invalidate();
	}

	protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
	{
		var info = e.Info;
		var surface = e.Surface;
		var canvas = surface.Canvas;

		var max = Math.Max(info.Height, info.Width);
		var size = Math.Min(info.Width, info.Height);

		canvas.Translate((max - size) / 2, 0);

		canvas.Clear();
		canvas.Save();
		canvas.RotateDegrees(120, size / 2, size / 2);
		DrawBackgroundCircle(info, canvas);
		DrawProgressCircle(info, canvas);

		canvas.Restore();
	}

	void DrawBackgroundCircle(SKImageInfo info, SKCanvas canvas)
	{
		var paint = new SKPaint
		{
			Color = LineBackgroundColor.ToSKColor(),
			StrokeWidth = StrokeWidth,
			IsStroke = true,
			IsAntialias = true,
			StrokeCap = SKStrokeCap.Round
		};

		DrawCircle(info, canvas, paint, SweepAngle);
	}

	void DrawProgressCircle(SKImageInfo info, SKCanvas canvas)
	{
		float progressAngle = SweepAngle * Progress;

		var paint = new SKPaint
		{
			Color = ProgressColor.ToSKColor(),
			StrokeWidth = StrokeWidth,
			IsStroke = true,
			IsAntialias = true,
			StrokeCap = SKStrokeCap.Round
		};

		DrawCircle(info, canvas, paint, progressAngle);
	}

	void DrawCircle(SKImageInfo info, SKCanvas canvas, SKPaint paint, float angle)
	{
		int size = Math.Min(info.Width, info.Height);

		using var path = new SKPath();
		var rect = new SKRect(
			StrokeWidth,
			StrokeWidth,
			size - StrokeWidth,
			size - StrokeWidth);

		path.AddArc(rect, StartAngle, angle);

		canvas.DrawPath(path, paint);
	}
}
#else
public class CircleProgress : Microsoft.UI.Xaml.FrameworkElement
{ 
}
#endif