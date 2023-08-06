using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DiabloDiscordBot.DiscordStuff;

namespace DiabloDiscordBot.WebStuff {
	public class HttpHelper {
		public static HttpHelper Service => SingletonContainer.I.GetService<HttpHelper>();

		public string GetBodyText(string url) {
			var client = new HttpClient();
			var response = client.GetAsync(url).GetAwaiter().GetResult();
			return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
		}
	}
}
