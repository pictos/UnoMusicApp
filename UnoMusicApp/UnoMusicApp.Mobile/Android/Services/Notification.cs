using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnoMusicApp.Services;
using static Android.Resource.Drawable;

namespace UnoMusicApp.Mobile.Android.Services;

sealed class NotificationDroid : INotification
{
	public void ShowNotification()
	{
		var act = Uno.UI.BaseActivity.Current;
		var intent = new Intent(act, typeof(ServiceNotification));
		intent.SetAction(ServiceNotification.ActionStart);
		act.StartService(intent);
	}
}


[Service]
sealed class ServiceNotification : Service
{

	const string NotificationIdString = "10";
	const string ChannelName = "MediaPlayerXChannel";
	const string Description = "NotificationPlayer";

	const int NotificationId = 10;
	NotificationChannel channel = default!;
	NotificationManager notificationManager = default!;
	static MediaService MediaService => MediaService.Current;
	static Activity Activity => Uno.UI.BaseActivity.Current;

	PendingIntent playIntent = default!;
	PendingIntent pauseIntent = default!;
	PendingIntent nextIntent = default!;
	PendingIntent previousIntent = default!;

	Notification notification = default!;
	Notification.Builder notificationBuilder = default!;

	public const string ActionPlay = "ActionPlay";
	public const string ActionPause = "ActionPause";
	public const string ActionNext = "ActionNext";
	public const string ActionPrevious = "ActionPrevious";
	public const string ActionStart = "ActionStart";
	public const string ActionStop = "ActionStop";

	int usingResource = 0;
	bool isStarted;

	public override void OnCreate()
	{
		base.OnCreate();
		CreateNotificationChannel();

		playIntent = PendingIntent.GetService(Activity, 100, CreateIntentByAction(ActionPlay), PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Mutable)!;
		pauseIntent = PendingIntent.GetService(Activity, 101, CreateIntentByAction(ActionPause), PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Mutable)!;
		nextIntent = PendingIntent.GetService(Activity, 102, CreateIntentByAction(ActionNext), PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Mutable)!;
		previousIntent = PendingIntent.GetService(Activity, 103, CreateIntentByAction(ActionPrevious), PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Mutable)!;


		MediaService.OnMediaChanged += OnMediaChanged;
		MediaService.OnIsPlayingChanged += OnIsPlayingChanged;

		static Intent CreateIntentByAction(string action)
		{
			var pkg = Activity.PackageName;
			return new Intent(Activity, typeof(ServiceNotification)).SetAction(action).SetPackage(pkg);
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();

		MediaService.OnMediaChanged -= OnMediaChanged;
		MediaService.OnIsPlayingChanged -= OnIsPlayingChanged;
	}

	void OnIsPlayingChanged(bool isPlaying)
	{
		UpdateNotification(isPlaying);
	}

	void OnMediaChanged()
	{
		UpdateNotification();
	}

	void CreateNotificationChannel()
	{
		channel = new NotificationChannel(NotificationIdString, ChannelName, NotificationImportance.Low)
		{
			Description = Description
		};

		notificationManager = (NotificationManager)Activity.GetSystemService(NotificationService)!;
		notificationManager.CreateNotificationChannel(channel);
	}

	void CreateNotification()
	{
		if (!isStarted)
		{

			var intent = new Intent(Activity, typeof(MainActivity)).SetAction("OpenPlayer");

			notificationBuilder = new Notification.Builder(Activity, NotificationIdString)
				.SetAutoCancel(false)
				.SetContentIntent(PendingIntent.GetActivity(Activity, 22, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Mutable))
				.SetStyle(new Notification.MediaStyle())
				.SetSmallIcon(Resource.Drawable.ic_mtrl_checked_circle)
				.SetContentTitle(MediaService.CurrentMedia?.Title ?? string.Empty)
				.AddAction(GenerateAction(IcMediaPrevious, "Previous", previousIntent))
				.AddAction(GeneratePlayPauseAction(MediaService.IsPlaying))
				.SetOnlyAlertOnce(true)
				.AddAction(GenerateAction(IcMediaNext, "Next", nextIntent))
				.SetOngoing(true);

			notification = notificationBuilder.Build();
			ContextCompat.StartForegroundService(Activity, new Intent(Activity, typeof(ServiceNotification)));
			StartForeground(NotificationId, notification);

			notificationManager = (NotificationManager)Activity.GetSystemService(NotificationService)!;
			isStarted = true;
		}
		else
			UpdateNotification();
	}

	[return: GeneratedEnum]
	public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags, int startId)
	{
		if (intent is null)
			goto End;

		var action = intent.Action;

		switch (action)
		{
			case ActionStart:
				CreateNotification();
				break;
			case ActionPlay:
			case ActionPause:
				MediaService.PlayPause();
				break;
			case ActionNext:
				MediaService.NextMusic();
				break;
			case ActionPrevious:
				MediaService.PreviousMusic();
				break;
			case ActionStop:
				MediaService.PlayPause();
				StopSelf();
				break;
			default:
				break;
		}

		End:
		return StartCommandResult.Sticky;
	}

	async void UpdateNotification(bool isPlaying = true)
	{
		// Just to be safe, some times the CurrentMedia takes a while to update
		await Task.Delay(100);

		if (!isPlaying)
			StopForeground(false);
		else
			StartForeground(NotificationId, notification);

		if (MediaService.CurrentMedia is null)
			return;

		notificationBuilder.SetContentTitle(MediaService.CurrentMedia.Title);
		notification = notificationBuilder.Build();

		if (notification.Actions is null || notification.Actions.Count is 0)
			return;

		notification.Actions[1] = GeneratePlayPauseAction(isPlaying);

		notificationManager.Notify(NotificationId, notification);
		Interlocked.Exchange(ref usingResource, 0);
	}

	static Notification.Action GenerateAction(int resource, string title, PendingIntent pendingIntent)
		=> new(resource, title, pendingIntent);

	Notification.Action GeneratePlayPauseAction(bool isPlaying) =>
		isPlaying
		? GenerateAction(IcMediaPause, "Pause", pauseIntent)
		: GenerateAction(IcMediaPlay, "Play", playIntent);

	public override IBinder? OnBind(Intent? intent) => null;
}
