using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using DiabloDiscordBot.DiabloStuff;
using DiabloDiscordBot.WebStuff;

namespace DiabloDiscordBot.DiscordStuff.DatabaseStuff {
	internal class d4ArmoryScraper {
		public static d4ArmoryScraper Service => SingletonContainer.I.GetService<d4ArmoryScraper>();

		internal void Scrape(EventDetails details) {
			var url = "https://d4armory.io/api/events/recent";
			bool error = false;

			string message;
			try {
				message = HttpHelper.Service.GetBodyText(url);
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
