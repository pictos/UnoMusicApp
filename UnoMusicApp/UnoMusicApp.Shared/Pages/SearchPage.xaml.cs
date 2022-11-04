using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using UnoMusicApp.Controls;
using UnoMusicApp.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UnoMusicApp.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SearchPage : Page
	{
		static SearchPage? current;
		public static SearchPage Current => current ??= new();

		SearchViewModel Vm => (SearchViewModel)DataContext;
		SearchPage()
		{
			this.InitializeComponent();
			DataContext = new SearchViewModel();
		}

		async void ItemClicked(object sender, ItemClickEventArgs e)
		{
			//HACK: workaround to make sure the SelectedItem will be valid
			await Task.Delay(1);

			if (MusicCard.wasLongPress)
			{
				list.SelectedItem = null;
				MusicCard.wasLongPress = false;
				return;
			}

			var item = (YoutubeMediaFile)e.ClickedItem;
			Vm.PlaySongCommand.Execute(item);
			list.SelectedItem = null;
		}

		void TextBox_KeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
		{
			if (e.Key != Windows.System.VirtualKey.Enter)
				return;
			e.Handled = true;
			var textBlock = (TextBox)sender;
			RemoveFocus(textBlock);
			Vm.SearchForQueryCommand.Execute(textBlock.Text);
		}

		void RemoveFocus(object sender)
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
