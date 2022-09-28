using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UnoMusicApp.Pages;
using Windows.UI.Core;
using System.Threading;

namespace UnoMusicApp.Services
{
	public static class NavigationService
	{
		static ShellPage CurrentPage => ((App.Window.Content as Frame)!.Content as ShellPage)!; //Microsoft.UI.Xaml.Window.Current.Content as Frame;

		static Frame ContentFrame => CurrentPage.contentFrame;

		public static void NavigateTo(Type pageType)
		{
			ContentFrame.Navigate(pageType);
		}

		public static async ValueTask NavigateTo(Type pageType, object parameter)
		{
			ContentFrame.Navigate(pageType, parameter);
			if ((ContentFrame.Content as FrameworkElement)?.DataContext is BaseViewModel vm)
				await vm.InitializeAsync((Dictionary<string, object>)parameter);
			
			// Why this doesn't work on Win UI?
			//var v = SystemNavigationManager.GetForCurrentView();
			//v.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
		}

		public static void NavigateBack()
		{
			var stack = ContentFrame.BackStack;
			if (ContentFrame.CanGoBack)
				ContentFrame.GoBack();
		}
	}
}
