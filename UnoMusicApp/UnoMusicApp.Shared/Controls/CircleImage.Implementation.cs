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

	protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
	{
		var info = e.Info;
		var surface = e.Surface;
		var canvas = surface.Canvas;

		canvas.Clear(SKColors.Transparent);

		if (bitmap is null)
			return;

		canvas.Scale(info.Width / baseSize);
		
		var size = baseSize - 15;
		var x = size / 2;
		var y = size / 2;
		canvas.Translate(6.65f, 26.5f);
		using var circleFill = new SKPaint();

		circleFill.Shader = SKShader.CreateRadialGradient(new SKPoint(x, y),
			size/ 2,
			new SKColor[] { SKColors.Transparent, SKColors.White },
			new float[] { 0.8f, 1 },
			SKShaderTileMode.Clamp);
		canvas.Translate(0, -22);
		var rect = SKRect.Create(size, size);
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