using Microsoft.UI.Xaml.Controls;

namespace UnoMusicApp.Pages
{
	public sealed partial class ShellPage : Page
	{
		public ShellPage()
		{
			this.InitializeComponent();
			this.contentFrame.Navigate(typeof(SearchPage));
		}
	}
}
