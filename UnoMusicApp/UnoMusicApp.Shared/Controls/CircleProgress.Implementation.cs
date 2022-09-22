#if ANDROID || IOS || MACCATALYST
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using SkiaSharp;
using SkiaSharp.Views.Windows;
using System;
using Windows.ApplicationModel.Background;
using Windows.UI;

namespace UnoMusicApp.Controls;

public partial class CircleProgress : SKXamlCanvas
{
	
	const float StartAngle = 15;
	const float SweepAngle = 270;
	const float baseSize = 100f;


	protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
	{
		var info = e.Info;
		var surface = e.Surface;
		var canvas = surface.Canvas;

		canvas.Scale(info.Width / baseSize );
		canvas.Translate(0, 0);
		canvas.Clear();
		canvas.Save();
		canvas.RotateDegrees(120, baseSize / 2, baseSize / 2);
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
		using var path = new SKPath();
		var rect = new SKRect(
			StrokeWidth,
			StrokeWidth,
			baseSize - StrokeWidth,
			baseSize - StrokeWidth);

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