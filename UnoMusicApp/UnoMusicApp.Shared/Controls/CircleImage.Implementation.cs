#if __ANDROID__ || __IOS__ || __MACCATALYST__
using SkiaSharp;
using SkiaSharp.Views.Windows;
using System;
using System.IO;
using System.Net.Http;

namespace UnoMusicApp.Controls;


// https://dotnetdevaddict.co.za/2020/01/12/who-cares-about-the-view-anyway/
public partial class CircleImage : SKXamlCanvas
{
	static HttpClient httpClient = new();
	const float baseSize = 100f;
	const float halfBaseSize = 50f;

	protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
	{
		var info = e.Info;
		var surface = e.Surface;
		var canvas = surface.Canvas;

		canvas.Clear(SKColors.Transparent);

		if (bitmap is null)
			return;

		var min = Math.Min(info.Height, info.Width);

		var scale = min / baseSize;

		canvas.Scale(scale);

		var newH = info.Height / scale;
		var newW = info.Width / scale;
		var dx = (newW - baseSize) / 2;
		var dy = (newH - baseSize) / 2;
		canvas.Translate(dx,dy);
		using var circleFill = new SKPaint();

		circleFill.Shader = SKShader.CreateRadialGradient(new SKPoint(halfBaseSize, halfBaseSize),
			halfBaseSize,
			new SKColor[] { SKColors.Transparent, SKColors.White },
			new float[] { 0.8f, 1 },
			SKShaderTileMode.Clamp);
		var rect = SKRect.Create(baseSize, baseSize);
		canvas.DrawBitmap(bitmap, rect);
		canvas.DrawRect(info.Rect, circleFill);
	}

	SKBitmap? bitmap;

	async Task GetImageAsync()
	{
		using var stream = await httpClient.GetStreamAsync(Source);
		using var memoryStream = new MemoryStream();

		await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
		memoryStream.Seek(0, SeekOrigin.Begin);
		bitmap = SKBitmap.Decode(memoryStream);
	}
}
#else
namespace UnoMusicApp.Controls;

public partial class CircleImage : Microsoft.UI.Xaml.Controls.UserControl
{ 
}
#endif