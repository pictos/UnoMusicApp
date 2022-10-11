using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using UnoMusicApp.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UnoMusicApp.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SearchPage : Page
	{
		SearchViewModel Vm => (SearchViewModel)DataContext;
		public SearchPage()
		{
			this.InitializeComponent();
			DataContext = new SearchViewModel();
		}

		async void ItemClicked(object sender, ItemClickEventArgs e)
		{
			if (wasLongPress)
			{
				//HACK: workaround to make sure the item will not be selected
				await Task.Delay(1);
				list.SelectedItem = null;
				wasLongPress = false;
				return;
			}

			var item = (YoutubeMediaFile)e.ClickedItem;
			Vm.PlaySongCommand.Execute(item);
		}

		bool wasLongPress;

		void OnCellHolding(object sender, HoldingRoutedEventArgs e)
		{
			if (e.HoldingState != Microsoft.UI.Input.HoldingState.Started)
				return;

			Console.WriteLine("Started");
			wasLongPress = true;
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
