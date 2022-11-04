using Android.Text.Style;
using Android.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoMusicApp.Extensions.Toast;
public partial class Toast
{
	static Android.Widget.Toast? PlatformToast { get; set; }

	partial void DismissPlatform(CancellationToken token)
	{
		if (PlatformToast is not null)
		{
			token.ThrowIfCancellationRequested();

			PlatformToast.Cancel();
		}
	}

	partial void ShowPlatform(CancellationToken token)
	{
		DismissPlatform(token);

		token.ThrowIfCancellationRequested();

		var styledText = new SpannableStringBuilder(Text);
		styledText.SetSpan(new AbsoluteSizeSpan((int)TextSize, true), 0, Text.Length, 0);

		PlatformToast = Android.Widget.Toast.MakeText(Platform.CurrentActivity?.Window?.DecorView.FindViewById(Android.Resource.Id.Content)?.RootView?.Context, styledText, GetToastLength(Duration))
						  ?? throw new Exception("Unable to create toast");

		PlatformToast.Show();
	}

	static ToastLength GetToastLength(ToastDuration duration) => (ToastLength)(int)duration;
}

