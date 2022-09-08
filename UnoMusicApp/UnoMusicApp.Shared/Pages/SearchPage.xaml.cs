using Microsoft.UI.Xaml.Controls;
using UnoMusicApp.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UnoMusicApp.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SearchPage : Page
	{
		SearchViewModel vm;
		public SearchPage()
		{
			this.InitializeComponent();
			DataContext = vm = new SearchViewModel();
		}

		void ItemClicked(object sender, ItemClickEventArgs e)
		{
			var item = (YoutubeMediaFile)e.ClickedItem;
			vm.PlaySongCommand.Execute(item);
		}

		private void TextBox_KeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
		{
			if (e.Key != Windows.System.VirtualKey.Enter)
				return;

			e.Handled = true;
			var textBlock = (TextBox)sender;
			LoseFocus(textBlock);
			vm.SearchForQueryCommand.Execute(textBlock.Text);
		}

		void LoseFocus(object sender)
		{
			var control = (Control)sender;
			var isTabStop = control.IsTabStop;
			control.IsTabStop = false;
			control.IsEnabled = false;
			control.IsEnabled = true;
			control.IsTabStop = isTabStop;
		}
	}
}
