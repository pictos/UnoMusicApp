using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using UnoMusicApp.Mobile.Android.Services;
using UnoMusicApp.Services;

namespace UnoMusicApp;
[Activity(
		MainLauncher = true,
		ConfigurationChanges = global::Uno.UI.ActivityHelper.AllConfigChanges,
		WindowSoftInputMode = SoftInput.AdjustPan | SoftInput.StateHidden
	)]
public class MainActivity : Microsoft.UI.Xaml.ApplicationActivity
{


	public MainActivity()
	{
		Extensions.Platform.Init(Current ?? this);
		DependencyService.RegisterDependency<INotification>(new NotificationDroid());
	}

	//public override void OnBackPressed()
	//{
	//	NavigationService.NavigateBack();
	//}
}

