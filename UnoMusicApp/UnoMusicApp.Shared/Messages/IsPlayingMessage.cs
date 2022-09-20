using CommunityToolkit.Mvvm.Messaging;

namespace UnoMusicApp.Messages;

public class IsPlayingMessage : ValueChangedMessage<string>
{
	public IsPlayingMessage(string value) : base(value)
	{
		
	}
}
