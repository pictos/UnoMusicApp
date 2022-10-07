using CommunityToolkit.Mvvm.Messaging;

namespace UnoMusicApp.Messages;

public class IsPlayingMessage : ValueChangedMessage<(bool isPlaying, bool isStoped)>
{
	public IsPlayingMessage((bool, bool) value) : base(value)
	{
	}
}
