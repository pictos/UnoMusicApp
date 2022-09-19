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
	
	const float StartAngle = 15;
	const float SweepAngle = 270;

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
namespace UnoMusicApp.Controls;

public partial class CircleProgress : Microsoft.UI.Xaml.Controls.UserControl
{ 
}
#endif