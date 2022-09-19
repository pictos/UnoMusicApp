#if __ANDROID__ || __IOS__ || __MACCATALYST__

using Microsoft.UI.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Windows;
using System;
using System.IO;
using System.Net.Http;

namespace UnoMusicApp.Controls;

public partial class CircleImage : SKXamlCanvas
{
	static HttpClient httpClient = new();

	protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
	{
		var info = e.Info;
		var surface = e.Surface;
		var canvas = surface.Canvas;

		canvas.Clear(SKColors.Transparent);

		if (bitmap is null)
			return;

		var sizeValue = Math.Min(info.Width, info.Height);

		var x = info.Width /2;
		var y = info.Height/2;

		using var circleFill = new SKPaint();

		circleFill.Shader = SKShader.CreateRadialGradient(new SKPoint(x, y),
			sizeValue / 2, 
			new SKColor[] { SKColors.Transparent,SKColors.White },
			new float[] { 0.8f, 1 },
			SKShaderTileMode.Clamp);
		canvas.Translate(0, -22);
		var rect = SKRect.Create(bitmap.Width, bitmap.Height);
		canvas.DrawBitmap(bitmap, info.Rect);
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