using Microsoft.UI.Xaml.Controls;
using UnoMusicApp.Services;

namespace UnoMusicApp.Pages
{
	public sealed partial class ShellPage : Page
	{
		public ShellPage()
		{
			this.InitializeComponent();
			//this.contentFrame.Navigate(typeof(SearchPage));
			this.navigationView.SelectedItem = this.navigationView.MenuItems[0];
			NavigationService.OnNavigation += OnNavigationHappened;
		}

		void OnNavigationHappened(Type obj)
		{
			var index = 0;
			if (obj == typeof(SearchPage))
				index = 0;
			else if (obj == typeof(PlayerPage))
				index = 1;

			navigationView.SelectedItem = this.navigationView.MenuItems[index];
		}

		void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
		{
			var item = (Microsoft.UI.Xaml.Controls.NavigationViewItem)args.SelectedItem;

			switch ((string)item.Content)
			{
				case "Search":
					contentFrame.Content = SearchPage.Current;
					break;
				case "Player":
					contentFrame.Content = PlayerPage.Current;
					break;
			}
		}
	}
}
