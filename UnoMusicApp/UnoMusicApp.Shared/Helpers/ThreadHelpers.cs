using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace UnoMusicApp.Helpers
{
	static class ThreadHelpers
	{
		static bool IsMainThread => synchronizationContext == SynchronizationContext.Current;

		public static bool WhatThreadAmI([CallerMemberName] string method = "", [CallerLineNumber] int line = 0)
		{
			Console.WriteLine("**************");
			Console.WriteLine("**************");
			Console.WriteLine($"{method}, line: {line}, IsMainThread: {IsMainThread}");
			Console.WriteLine("**************");
			Console.WriteLine("**************");
			return IsMainThread;
		}

		static SynchronizationContext? synchronizationContext = App.SynchronizationContext;

		public static void BeginInvokeOnMainThread(Action action)
		{
			if (synchronizationContext is not null && SynchronizationContext.Current != synchronizationContext)
				synchronizationContext.Post(_ => action(), null);
			else
				action();
		}
	}
}
