using Window = System.Windows.Window;

namespace UnoMusicApp.WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();

		root.Content =  new global::Uno.UI.Skia.Platform.WpfHost(Dispatcher, () => new UnoMusicApp.App());
	}
}
