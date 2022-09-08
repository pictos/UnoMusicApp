using Microsoft.UI.Xaml.Controls;
using System;

namespace UnoMusicApp.Services
{
	public static class NavigationService
	{
		static Frame CurrentFrame => Microsoft.UI.Xaml.Window.Current.Content as Frame;

		public static void NavigateTo(Type pageType)
		{
			CurrentFrame.Navigate(pageType);
		}


		public static void NavigateTo(Type pageType, object parameter)
		{
			CurrentFrame.Navigate(pageType, parameter);
		}

		public static void NavigateBack()
		{
			if (CurrentFrame.CanGoBack)
				CurrentFrame.GoBack();
		}
	}
}
