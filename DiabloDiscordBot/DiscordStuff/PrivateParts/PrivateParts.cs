using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiabloDiscordBot.DiscordStuff.DatabaseStuff;
using DSharpPlus.EventArgs;

namespace DiabloDiscordBot.DiscordStuff {
	internal static class PrivateParts {
		private static string _token = null;
		internal static string Token => _token ??= _LoadToken();

		private static string _LoadToken() {
			using (var file = File.OpenText(Path.Join(Database.LocalAppPath, "token.txt"))) {
				return file.ReadLine();
			}

			throw new Exception("Couldn't Read File Or Something");
		}
	}
}
