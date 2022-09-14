#if ANDROID || IOS || MACCATALYST

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
	static readonly PropertyChangedCallback changedCallback = new(OnPropertyChanged);

	public static readonly DependencyProperty SourceProperty =
		DependencyProperty.Register(nameof(Source), typeof(string), typeof(CircleImage), new(string.Empty, changedCallback));

	public string Source
	{
		get => (string)GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}

	static async void OnPropertyChanged(DependencyObject bindable, DependencyPropertyChangedEventArgs e)
	{
		var circleImage = bindable as CircleImage;

		if (circleImage is null || e.NewValue is null)
			return;

		await circleImage.GetImageAsync();
		circleImage?.Invalidate();
	}
	protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
	{
		var info = e.Info;
		var surface = e.Surface;
		var canvas = surface.Canvas;

		canvas.Clear(SKColors.Fuchsia);

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
public class CircleImage : Microsoft.UI.Xaml.FrameworkElement
{ 
}
#endif