using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UnoMusicApp.Pages;
using Windows.UI.Core;
using System.Threading;
using System.Reflection.Metadata.Ecma335;

namespace UnoMusicApp.Services
{
	public static class NavigationService
	{
		public static Action<Type>? OnNavigation { get; set; }

		static ShellPage CurrentPage => ((App.Window.Content as Frame)!.Content as ShellPage)!; //Microsoft.UI.Xaml.Window.Current.Content as Frame;

		static Frame ContentFrame => CurrentPage.contentFrame;

		public static void NavigateTo(Type pageType)
		{
			ContentFrame.Navigate(pageType);
		}

		public static async ValueTask NavigateTo(Type pageType, object parameter)
		{
			ContentFrame.Content = GetPage(pageType);

			if ((ContentFrame.Content as FrameworkElement)?.DataContext is BaseViewModel vm)
				await vm.InitializeAsync((Dictionary<string, object>)parameter);
			
			OnNavigation?.Invoke(pageType);
		}

		static Page GetPage(Type type)
		{
			if (type == typeof(SearchPage))
				return SearchPage.Current;
			if (type == typeof(PlayerPage))
				return PlayerPage.Current;

			throw new IndexOutOfRangeException();
		}

		public static void NavigateBack()
		{
			var stack = ContentFrame.BackStack;
			if (ContentFrame.CanGoBack)
				ContentFrame.GoBack();
		}
	}
}
