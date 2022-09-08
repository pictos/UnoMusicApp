using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace UnoMusicApp.Services
{
	public static class NavigationService
	{
		static Frame CurrentFrame => Microsoft.UI.Xaml.Window.Current.Content as Frame;

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
		}

		public static void NavigateBack()
		{
			if (CurrentPage.Frame.CanGoBack)
				CurrentPage.Frame.GoBack();
		}
	}
}
