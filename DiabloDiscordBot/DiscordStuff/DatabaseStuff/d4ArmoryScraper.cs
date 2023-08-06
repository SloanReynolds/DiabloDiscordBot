using System;
using System.IO;
using DiabloBotShared;

namespace DiabloDiscordBot.DiscordStuff.DatabaseStuff {
	internal class d4ArmoryScraper {
		public static d4ArmoryScraper Singleton => SingletonContainer.I.GetService<d4ArmoryScraper>();

		internal void Scrape(EventDetails details) {
			var url = "https://d4armory.io/api/events/recent";
			bool error = false;

			string message;
			try {
				message = HttpHelper.Singleton.GetBodyText(url);
			} catch (Exception ex) {
				message = ex.Message;
				error = true;
			}

			using (var file = File.CreateText(Path.Join(Database.LocalAppPath, $"{UTCHelper.UnixNow} - {details.AlertType}{(error ? " - ERR" : "")}.txt"))) {
				file.Write(message);
			}
		}
	}
}
