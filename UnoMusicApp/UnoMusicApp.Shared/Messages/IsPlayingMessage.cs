using CommunityToolkit.Mvvm.Messaging;

namespace UnoMusicApp.Messages;

public class IsPlayingMessage : ValueChangedMessage<bool>
{
	public IsPlayingMessage(bool value) : base(value)
	{
		
	}
}
