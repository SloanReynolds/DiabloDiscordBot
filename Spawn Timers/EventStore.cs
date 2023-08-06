using System;
using System.Text.Json;
using DiabloBotShared;
using DiabloDiscordBot;

namespace Spawn_Timers {
	class EventStore {
		public static EventStore Singleton => SingletonContainer.I.GetService<EventStore>();

		public event Action UpdateReceived;

		public DateTime NextHelltide = DateTime.Now;
		public DateTime NextWorldboss = DateTime.Now;
		public string NextWorldbossName;
		public DateTime NextLegion = DateTime.Now;


		internal void RefreshIfNeeded() {
			if (NextHelltide < DateTime.Now
				|| NextWorldboss < DateTime.Now
				|| NextLegion < DateTime.Now) {
				RefreshNow();
			}
		}

		public void RefreshNow() {
			try {
				var url = "https://d4armory.io/api/events/recent";
				var json = HttpHelper.Singleton.GetBodyText(url);
				var update = JsonSerializer.Deserialize<D4ArmoryRecentEvents>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })!;

				NextWorldboss = UTCHelper.UnixTimeStampToDateTime(update.Boss.Expected);
				NextWorldbossName = update.Boss.ExpectedName;
				NextLegion = UTCHelper.UnixTimeStampToDateTime(update.Legion.Expected);
			} catch (Exception ex) {
				_RefreshFallback();
			}

			NextHelltide = HelltideEvent.GetNextDateTime();
		}

		private void _RefreshFallback() {
			NextWorldboss = WorldbossEvent.GetNextDateTime();
			NextWorldbossName = "Oopsies";
			NextLegion = LegionEvent.GetNextDateTime();
		}
	}
}
