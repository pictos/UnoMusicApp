using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace UnoMusicApp.Helpers
{
	sealed class Timer
	{
		readonly Stopwatch stopwatch = new ();

		public bool TimerWatch(uint timeOutMs) =>
			TimerResult(timeOutMs);

		public bool TimerWatch(uint timeOutMs, Action? message)
		{
			_ = message ?? throw new ArgumentNullException($"{nameof(message)} can't be null.");

			var result = TimerResult(timeOutMs);

			if (result)
				message();

			return result;
		}

		bool TimerResult(uint timeOutMs)
		{
			var running = stopwatch.IsRunning;
			if (stopwatch.ElapsedMilliseconds <= timeOutMs && running)
			{
				stopwatch.Stop();
				stopwatch.Reset();
				return false;
			}
			else if (running)
			{
				stopwatch.Stop();
				stopwatch.Reset();
			}
			else
				stopwatch.Start();

			return true;
		}

		//public void ShowToastMessage() =>
		//	MessagingCenter.Send(MessengerKeys.AppPlatform, MessengerKeys.QuitToast);
	}
}
