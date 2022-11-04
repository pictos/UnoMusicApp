using Windows.UI.Notifications;
using static UnoMusicApp.Extensions.Extensions.ToastNotificationExtensions;

namespace UnoMusicApp.Extensions.Toast;
public partial class Toast
{
	static Windows.UI.Notifications.ToastNotification? PlatformToast { get; set; }

	partial void DismissPlatform(CancellationToken token)
	{
		if (PlatformToast is not null)
		{
			token.ThrowIfCancellationRequested();
			ToastNotificationManager.History.Clear();

			PlatformToast.ExpirationTime = DateTimeOffset.Now;
			PlatformToast = null;
		}
	}

	partial void ShowPlatform(CancellationToken token)
	{
		DismissPlatform(token);
		token.ThrowIfCancellationRequested();

		PlatformToast = new ToastNotification(BuildToastNotificationContent(Text))
		{
			ExpirationTime = DateTimeOffset.Now.Add(GetDuration(Duration))
		};

		ToastNotificationManager.CreateToastNotifier().Show(PlatformToast);
	}
}

