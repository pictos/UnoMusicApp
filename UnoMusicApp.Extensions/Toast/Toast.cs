using System.ComponentModel;

namespace UnoMusicApp.Extensions.Toast;
public partial class Toast
{
	string text = string.Empty;
	ToastDuration duration = ToastDuration.Short;
	double textSize = 10;

	/// <inheritdoc/>
	public string Text
	{
		get => text;
		init => text = value ?? throw new ArgumentNullException(nameof(value));
	}

	/// <inheritdoc/>
	public ToastDuration Duration
	{
		get => duration;
		init
		{
			if (!Enum.IsDefined(typeof(ToastDuration), value))
			{
				throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(ToastDuration));
			}

			duration = value;
		}
	}

	/// <inheritdoc/>
	public double TextSize
	{
		get => textSize;
		init
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "Toast font size must be positive");
			}

			textSize = value;
		}
	}

	/// <summary>
	/// Create new Toast
	/// </summary>
	/// <param name="message">Toast message</param>
	/// <param name="duration">Toast duration</param>
	/// <param name="textSize">Toast font size</param>
	/// <returns>New instance of Toast</returns>
	public static Toast Make(
		string message,
		ToastDuration duration = ToastDuration.Short,
		double textSize = 10)
	{
		return new Toast
		{
			Text = message,
			Duration = duration,
			TextSize = textSize
		};
	}

	/// <summary>
	/// Show Toast
	/// </summary>
	public virtual Task Show(CancellationToken token = default)
	{
		ShowPlatform(token);
		return Task.CompletedTask;
	}

	/// <summary>
	/// Dismiss Toast
	/// </summary>
	public virtual Task Dismiss(CancellationToken token = default)
	{
		DismissPlatform(token);
		return Task.CompletedTask;
	}

	partial void ShowPlatform(CancellationToken token);
	partial void DismissPlatform(CancellationToken token);

	static TimeSpan GetDuration(ToastDuration duration) => duration switch
	{
		ToastDuration.Short => TimeSpan.FromSeconds(2),
		ToastDuration.Long => TimeSpan.FromSeconds(3.5),
		_ => throw new InvalidEnumArgumentException(nameof(Duration), (int)duration, typeof(ToastDuration))
	};
}

