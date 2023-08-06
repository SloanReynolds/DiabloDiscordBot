using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabloDiscordBot.DiabloStuff {
	public static class UTCHelper {
		public static long UnixNow => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		public static long UnixMinutesAgo(int minutes) => DateTimeOffset.UtcNow.ToUnixTimeSeconds() - (minutes * 60);

		public static DateTime JavaTimeStampToDateTime(double javaTimeStamp) {
			// Java timestamp is milliseconds past epoch
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dateTime = dateTime.AddMilliseconds(javaTimeStamp).ToLocalTime();
			return dateTime;
		}

		public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) {
			// Unix timestamp is seconds past epoch
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dateTime;
		}

		internal static object ToUnixTimestamp(DateTime next) {
			return new DateTimeOffset(next).ToUnixTimeSeconds();
		}
	}
}
