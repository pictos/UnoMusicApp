using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace UnoMusicApp.Services
{
	public static class NavigationService
	{
		static Frame CurrentFrame => App.Window.Content as Frame; //Microsoft.UI.Xaml.Window.Current.Content as Frame;

		static Page CurrentPage => CurrentFrame.Content as Page;

		public static void NavigateTo(Type pageType)
		{
			CurrentPage.Frame.Navigate(pageType);
		}

		public static async ValueTask NavigateTo(Type pageType, object parameter)
		{
			CurrentPage.Frame.Navigate(pageType, parameter);
			if (CurrentPage.DataContext is BaseViewModel vm)
				await vm.InitializeAsync((Dictionary<string, object>)parameter);
			
			// Why this doesn't work on Win UI?
			//var v = SystemNavigationManager.GetForCurrentView();
			//v.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
		}

		public static void NavigateBack()
		{
			var stack = CurrentPage.Frame.BackStack;
			var frame = CurrentPage.Frame;
			if (CurrentPage.Frame.CanGoBack)
				CurrentPage.Frame.GoBack();
		}
	}
}
