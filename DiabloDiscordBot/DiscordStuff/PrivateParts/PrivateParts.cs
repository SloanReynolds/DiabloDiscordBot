using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;

namespace DiabloDiscordBot.DiscordStuff {
	internal static class PrivateParts {
		private static string _localAppDataPath => $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create)}\\DiabloDiscordBot";

		private static string _token = null;
		internal static string Token => _token ??= _LoadToken();

		private static string _LoadToken() {
			using (var file = File.OpenText(Path.Join(_localAppDataPath, "token.txt"))) {
				return file.ReadLine();
			}

			throw new Exception("Couldn't Read File Or Something");
		}
	}
}
