using SkiaSharp;
using SkiaSharp.Views.Windows;

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

		var min = Math.Min(info.Height, info.Width);
		var scale = min / baseSize;
		canvas.Scale(scale);
		canvas.Clear(SKColors.Transparent);
		canvas.Save();

		//var newH = info.Height / scale;
		var newW = info.Width / scale;

		canvas.Translate((newW - baseSize) / 2, 0);

		canvas.RotateDegrees(120, baseSize / 2, baseSize / 2);
		DrawBackgroundCircle(canvas);
		DrawProgressCircle(canvas);

		canvas.Restore();
	}

	void DrawBackgroundCircle(SKCanvas canvas)
	{
		var paint = new SKPaint
		{
			Color = LineBackgroundColor.ToSKColor(),
			StrokeWidth = StrokeWidth,
			IsStroke = true,
			IsAntialias = true,
			StrokeCap = SKStrokeCap.Round
		};

		DrawCircle(canvas, paint, SweepAngle);
	}

	void DrawProgressCircle(SKCanvas canvas)
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

		DrawCircle(canvas, paint, progressAngle);
	}

	void DrawCircle(SKCanvas canvas, SKPaint paint, float angle)
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