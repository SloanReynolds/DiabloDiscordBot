using System;
using System.Text.Json;
using System.Threading;
using DiabloDiscordBot;
using DiabloDiscordBot.DiabloStuff;
using DiabloDiscordBot.DiscordStuff;
using DiabloDiscordBot.WebStuff;

namespace Spawn_Timers {
	class EventStore {
		public static EventStore Service => SingletonContainer.I.GetService<EventStore>();

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
				var json = HttpHelper.Service.GetBodyText(url);
				var update = JsonSerializer.Deserialize<D4ArmoryRecentEvents>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })!;

				NextWorldboss = UTCHelper.UnixTimeStampToDateTime(update.Boss.Expected);
				NextWorldbossName = update.Boss.ExpectedName;
				NextLegion = UTCHelper.UnixTimeStampToDateTime(update.Legion.Expected);
			} catch (Exception ex) {
				NextWorldboss = DateTime.Now.AddMinutes(60);
				NextWorldbossName = "Oopsies";
				NextLegion = DateTime.Now.AddMinutes(30);
			}

			NextHelltide = HelltideEvent.GetNextDateTime();
		}
	}
}
