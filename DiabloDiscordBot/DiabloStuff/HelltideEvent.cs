using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabloDiscordBot.DiabloStuff {
	internal static class HelltideEvent {
		private const int _alertMinutes = 5;

		private const int _spawnMinutes = 135; // 2hr, 15min
		private static DateTime _next = UTCHelper.UnixTimeStampToDateTime(1686056400);
		//private const int _helltideActiveMinutes = 60;

		private static int _MinutesUntilNext() {
			while (_next < DateTime.Now) {
				_next = _next.AddMinutes(_spawnMinutes);
			}

			int interval = (int)(_next - DateTime.Now).TotalMinutes;
			int nextStart = interval % _spawnMinutes;
			//if (nextStart < _helltideActiveMinutes) {
			//	//we active now
			//}

			return nextStart;
		}

		public static EventDetails GetDetails() {
			int minutes = _MinutesUntilNext();
			ILogger.Service.WriteLine("Helltide: " + minutes);
			return new EventDetails(AlertType.Helltide, minutes, $"Helltide incoming in {minutes} minutes!", _alertMinutes);
		}
	}
}
