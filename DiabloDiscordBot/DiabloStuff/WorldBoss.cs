using System.IO;
using DiabloDiscordBot.DiscordStuff.DatabaseStuff;
using Newtonsoft.Json;

namespace DiabloDiscordBot.DiabloStuff {
	public static class WorldBoss {
		public enum Type {
			Ashava,
			Avarice,
			WanderingDeath,
			UNKNOWN
		}

		private static string _filePath = Path.Join(Database.LocalAppPath, "pattern.json");

		private static Type[] _pattern = null;

		public static Type[] Pattern {
			get {
				if (_pattern != null)
					return _pattern;

				LoadPatternFile();
				return _pattern;
			}
		}

		public static void LoadPatternFile() {
			if (!File.Exists(_filePath)) {
				string json = JsonConvert.SerializeObject(Default);
				using (var stream = File.CreateText(_filePath)) {
					stream.Write(json);
				}
				_pattern = Default;

				ILogger.Service.WriteLine("Created New Default Pattern File!");
				return;
			}

			string text = File.ReadAllText(_filePath);
			var pattern = JsonConvert.DeserializeObject<Type[]>(text);

			ILogger.Service.WriteLine("Pattern file loaded.");
			_pattern = pattern;

			WorldBossEvent.Reset();
		}



		public static Type[] Default = new Type[] {
				Type.WanderingDeath,
				Type.WanderingDeath,
				Type.Avarice,
				Type.Avarice,

				Type.Ashava,
				Type.Ashava,
				Type.Ashava,
				Type.WanderingDeath,
				Type.WanderingDeath,

				Type.Avarice,
				Type.Avarice,
				Type.Avarice,
				Type.Ashava,
				Type.Ashava,

				Type.WanderingDeath,
				Type.WanderingDeath,
				Type.WanderingDeath,
				Type.Avarice,
				Type.Avarice,

				Type.Ashava,
				Type.Ashava,
				Type.Ashava,
				Type.WanderingDeath,
				Type.WanderingDeath,

				Type.Avarice,
				Type.Avarice,
				Type.Avarice,
				Type.Ashava,
				Type.Ashava,

				Type.WanderingDeath,
				Type.WanderingDeath,
				Type.WanderingDeath,
				Type.Avarice,
				Type.Avarice,

				Type.Ashava,
				Type.Ashava,
				Type.Ashava,



				Type.WanderingDeath,
				Type.WanderingDeath,
				Type.Avarice,
				Type.Avarice,

				Type.Ashava,
				Type.Ashava,
				Type.Ashava,
				Type.WanderingDeath,
				Type.WanderingDeath,

				Type.Avarice,
				Type.Avarice,
				Type.Avarice,
				Type.Ashava,
				Type.Ashava,

				Type.WanderingDeath,
				Type.WanderingDeath,
				Type.WanderingDeath,
				Type.Avarice,
				Type.Avarice,
				Type.Avarice,

				Type.Ashava,
				Type.Ashava,
				Type.WanderingDeath,
				Type.WanderingDeath,

				Type.Avarice,
				Type.Avarice,
				Type.Avarice,
				Type.Ashava,
				Type.Ashava,

				Type.WanderingDeath,
				Type.WanderingDeath,
				Type.WanderingDeath,
				Type.Avarice,
				Type.Avarice,

				Type.Ashava,
				Type.Ashava,
				Type.Ashava,



				Type.WanderingDeath,
				Type.WanderingDeath,
				Type.Avarice,
				Type.Avarice,
				Type.Avarice,

				Type.Ashava,
				Type.Ashava,
				Type.WanderingDeath,
				Type.WanderingDeath,
				Type.WanderingDeath,

				Type.Avarice,
				Type.Avarice,
				Type.Ashava,
				Type.Ashava,
				Type.Ashava,

				Type.WanderingDeath,
				Type.WanderingDeath,
				Type.Avarice,
				Type.Avarice,
				Type.Avarice,

				Type.Ashava,
				Type.Ashava,
				Type.WanderingDeath,
				Type.WanderingDeath,

				Type.Avarice,
				Type.Avarice,
				Type.Avarice,
				Type.Ashava,
				Type.Ashava,

				Type.WanderingDeath,
				Type.WanderingDeath,
				Type.WanderingDeath,
				Type.Avarice,
				Type.Avarice,
				Type.Avarice,

				Type.Ashava,
				Type.Ashava,
			};
	}
}
