namespace UnoMusicApp.Extensions;
public static partial class Platform
{
	internal static Activity? CurrentActivity { get; private set; }


	public static void Init(Activity activity) => CurrentActivity = activity;
}
