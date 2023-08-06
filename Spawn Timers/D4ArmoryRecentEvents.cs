using System;
using DiabloDiscordBot.WebStuff;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using DiabloDiscordBot.DiabloStuff;

namespace DiabloDiscordBot {
	public record D4ArmoryRecentEvents {
		public WorldbossDetails Boss { get; set; }
		public HelltideDetails Helltide { get; set; }
		public LegionDetails Legion { get; set; }
		public List<WhisperDetails> Whispers { get; set; }

		public record WorldbossDetails {
			//"name":"Ashava",
			//"expectedName":"Ashava",			<---
			//"nextExpectedName":"Ashava",
			//"timestamp":1691232048,
			//"expected":1691258771,			<---
			//"nextExpected":1691279980,
			//"territory":"Seared Basin",
			//"zone":"Kehjistan"
			public string Name { get; set; }
			public string ExpectedName { get; set; }
			public string NextExpectedName { get; set; }
			public long Timestamp { get; set; }
			public long Expected { get; set; }
			public long NextExpected { get; set; }
			public string Territory { get; set; }
			public string Zone { get; set; }
		}

		public record HelltideDetails {
			//"timestamp":1691248500,
			//"zone":"scos",
			//"refresh":1691251200
			public long Timestamp { get; set; }
			public string Zone { get; set; }
			public long Refresh { get; set; }
		}

		public record LegionDetails {
			//"timestamp":1691253313,
			//"territory":"Norgoi Vigil",
			//"zone":"Dry Steppes",
			//"expected":1691255280,
			//"nextExpected":1691256990
			public long Timestamp { get; set; }
			public string Territory { get; set; }
			public string Zone { get; set; }
			public long Expected { get; set; }
			public long NextExpected { get; set; }
		}

		public record WhisperDetails {

			//PASS
		}
	}
}