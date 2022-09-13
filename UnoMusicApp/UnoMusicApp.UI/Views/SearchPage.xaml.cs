using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static UnoMusicApp.Models.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UnoMusicApp.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SearchPage : Page
{

	public SearchViewModel.BindableSearchViewModel Vm => (SearchViewModel.BindableSearchViewModel)DataContext;

	public SearchPage()
	{
		this.InitializeComponent();
	}

	void ItemClicked(object sender, ItemClickEventArgs e)
	{
		var item = (YoutubeMediaFile)e.ClickedItem;
		Vm.PlaySongCommand.Execute(item);
	}

	private void TextBox_KeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
	{
		if (e.Key != Windows.System.VirtualKey.Enter)
			return;

		e.Handled = true;
		var textBlock = (TextBox)sender;
		LoseFocus(textBlock);
		_ = Vm.Model.SearchForQueryCommand();
		//Vm.SearchForQueryCommand.Execute(null);
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
