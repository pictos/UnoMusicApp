
//using Application = Microsoft.UI.Xaml.Application;
using Android.Views;
using Microsoft.Extensions.DependencyInjection;

namespace UnoMusicApp
{
	[Activity(
			MainLauncher = true,
			ConfigurationChanges = global::Uno.UI.ActivityHelper.AllConfigChanges,
			WindowSoftInputMode = SoftInput.AdjustPan | SoftInput.StateHidden
		)]
	public class MainActivity : Microsoft.UI.Xaml.ApplicationActivity
	{
		public override void OnBackPressed()
		{
			// It will break
			var navigation = (Microsoft.UI.Xaml.Application.Current as App)!.Host.Services.GetRequiredService<INavigator>();
		}
	}
}

