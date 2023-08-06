using System;
using DiabloBotShared;
using Newtonsoft.Json;

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
	}
}