using System;
using DiabloDiscordBot.DiabloStuff;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DiabloDiscordBot {
	internal record D4ArmoryEvent {
		public string EventName { get; }
		public DateTime Time { get; }
		public int X { get; }
		public int Y { get; }

		[JsonConstructor()]
		public D4ArmoryEvent(string eventName, long time, int x, int y) {
			EventName = eventName;
			Time = UTCHelper.UnixTimeStampToDateTime(time);
			X = x;
			Y = y;
		}

		public WorldBoss.Type Boss {
			get {
				if (EventName == "Ashava")
					return WorldBoss.Type.Ashava;
				if (EventName == "Avarice")
					return WorldBoss.Type.Avarice;
				if (EventName == "The Wandering Death")
					return WorldBoss.Type.WanderingDeath;
				return WorldBoss.Type.UNKNOWN;
			}
		}
	}
}