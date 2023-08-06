namespace DiabloBotShared {
	public class HttpHelper {
		public static HttpHelper Singleton => SingletonContainer.I.GetService<HttpHelper>();

		public string GetBodyText(string url) {
			var client = new HttpClient();
			var response = client.GetAsync(url).GetAwaiter().GetResult();
			return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
		}
	}
}
