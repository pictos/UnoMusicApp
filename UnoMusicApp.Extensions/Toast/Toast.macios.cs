using System.Runtime.InteropServices;

namespace UnoMusicApp.Extensions.Toast;
public partial class Toast
{
	static PlatformToast? PlatformToast { get; set; }

	partial void DismissPlatform(CancellationToken token)
	{
		if (PlatformToast is not null)
		{
			token.ThrowIfCancellationRequested();

			PlatformToast.Dismiss();
		}
	}

	/// <summary>
	/// Show Toast
	/// </summary>
	partial void ShowPlatform(CancellationToken token)
	{
		DismissPlatform(token);
		token.ThrowIfCancellationRequested();

		var cornerRadius = CreateCornerRadius();
		var padding = GetMaximum(cornerRadius.X, cornerRadius.Y, cornerRadius.Width, cornerRadius.Height);

		PlatformToast = new PlatformToast(Text,
											UIColor.White,
											cornerRadius,
											UIColor.Black,
											UIFont.SystemFontOfSize((NFloat)TextSize),
											10,
											padding)
		{
			Duration = GetDuration(Duration)
		};

		PlatformToast.Show();

		static T? GetMaximum<T>(params T[] items) => items.Max();
	}

	static CGRect CreateCornerRadius(int radius = 4)
	{
		return new CGRect(radius, radius, radius, radius);
	}
}

