using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UnoMusicApp.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PlayerPage : Page
	{
		internal PlayerViewModel Vm => (PlayerViewModel)DataContext;

		public PlayerPage()
		{
			this.InitializeComponent();
			DataContext = PlayerViewModel.Current;
			
		}
	}
}
