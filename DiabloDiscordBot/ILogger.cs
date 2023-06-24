using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiabloDiscordBot.DiscordStuff;

namespace DiabloDiscordBot {
	internal interface ILogger {
		public static ILogger Service => SingletonContainer.I.GetService<ILogger>();

		public void ErrorLine(string msg);
		public void WriteLine(string msg);
	}
}
