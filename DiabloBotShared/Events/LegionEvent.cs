﻿namespace DiabloBotShared {
	public class LegionEvent {
		private const int _alertMinutes = 5;

		private const int _spawnMinutes = 30; // 2hr, 15min
		private static DateTime _next = UTCHelper.UnixTimeStampToDateTime(1687455300);

		private static int _MinutesUntilNext() {
			_UpdateNext();

			int interval = (int)(_next - DateTime.Now).TotalMinutes;
			int nextStart = interval % _spawnMinutes;

			return nextStart;
		}

		public static EventDetails GetDetails() {
			int minutes = _MinutesUntilNext();
			//ILogger.Service.WriteLine("Legion: " + minutes);
			return new EventDetails(AlertType.Legion, minutes, $"Legion Event <t:{UTCHelper.ToUnixTimestamp(_next)}:R> (+/- 5 minutes)", _alertMinutes);
		}

		public static DateTime GetNextDateTime() {
			_UpdateNext();

			return _next;
		}

		private static void _UpdateNext() {
			while (_next < DateTime.Now) {
				_next = _next.AddMinutes(_spawnMinutes);
			}
		}
	}
}
