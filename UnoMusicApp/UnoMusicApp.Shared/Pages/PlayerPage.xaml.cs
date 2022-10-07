using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using UnoMusicApp.Messages;

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
			WeakReferenceMessenger.Default.Register<IsPlayingMessage>(this, OnIsPlayingChanged);
		}

		void OnIsPlayingChanged(object recipient, IsPlayingMessage message)
		{
			if (message.Value)
				rotateImg.Begin();
			else
				rotateImg.Stop();
		}
	}
}
