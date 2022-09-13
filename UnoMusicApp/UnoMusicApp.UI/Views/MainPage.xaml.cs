
namespace UnoMusicApp.Views;

public sealed partial class MainPage : Page
{
	public MainPage()
	{
		this.InitializeComponent();

		this.DataContextChanged += (_, __) =>
		{
			if (DataContext is null)
				return;
			var s = DataContext;
		};
	}
}
