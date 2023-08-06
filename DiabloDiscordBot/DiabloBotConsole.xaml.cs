using System;
using System.Diagnostics;
using System.Windows;
using DiabloBotShared;
using DiabloDiscordBot.DiabloStuff;
using DiabloDiscordBot.DiscordStuff;
using DiabloDiscordBot.DiscordStuff.DatabaseStuff;

namespace DiabloDiscordBot {
	/// <summary>
	/// Interaction logic for DiabloBotConsole.xaml
	/// </summary>
	public partial class DiabloBotConsole : Window {
		private Discord _discord => SingletonContainer.I.GetService<Discord>();
		private Database _database => SingletonContainer.I.GetService<Database>();
		private DiabloEventWatcher _diablo => SingletonContainer.I.GetService<DiabloEventWatcher>();
		private ILogger _log => SingletonContainer.I.GetService<ILogger>();

		public DiabloBotConsole() {
			InitializeComponent();


			SingletonContainer.I
				.RegisterService<DiabloEventWatcher>(new DiabloEventWatcher())
				.RegisterService<Database>(new Database())
				.RegisterService<Discord>(new Discord())
				.RegisterService<ILogger>(consoleOutput)
				.RegisterService<DiabloBotConsole>(this)
				.RegisterService<d4ArmoryScraper>(new d4ArmoryScraper())
				.RegisterService<HttpHelper>(new HttpHelper())
			;

			_log.WriteLine("Setup Complete!");

			//_Connect();
		}

		private void Button_TestOutput(object sender, RoutedEventArgs e) {
			Random random = new();
			string sentence = random.NextSentence();
			consoleOutput.WriteLine(sentence);
		}

		private void Button_Connect(object sender, RoutedEventArgs e) {
			_Connect();
		}

		private void Button_Disconnect(object sender, RoutedEventArgs e) {
			_Disconnect();
		}

		private void Button_NextWorldBossMinutes(object sender, RoutedEventArgs e) {
			WorldbossEvent.GetDetails();
		}

		private void _Connect() {
			_discord.Connect();
		}

		public void Connected_Success() {
			Application.Current.Dispatcher.Invoke(() => {
				btnConnect.IsEnabled = false;
				btnDisconnect.IsEnabled = true;
			});
		}

		private void _Disconnect() {
			_discord.Disconnect();
		}

		public void Disconnected_Success() {
			Application.Current.Dispatcher.Invoke(() => {
				btnConnect.IsEnabled = true;
				btnDisconnect.IsEnabled = false;
			});
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			_Disconnect();
		}

		private void Button_CompareWorldBossSpawns(object sender, RoutedEventArgs e) {
			_log.ErrorLine("Oops this does nothing");
			//var url = "https://d4armory.io/api/events/all";
			//var json = HttpHelper.Service.GetBodyText(url);
			//var stuff = JsonConvert.DeserializeObject<D4ArmoryEvent[]>(json).Where(e => e.EventName != "Legion" && e.EventName != "Helltide" && e.EventName != "UNKNOWN");

			//WorldbossEvent.GetDetails();
			//var history = WorldbossEvent.History;

			//List<(WorldBossEventData data, D4ArmoryEvent armory)> pairs = new();
			//foreach (var data in history) {
			//	bool found = false;
			//	foreach (var armory in stuff) {
			//		if (armory.Time.AddMinutes(-5) < data.DateTime && armory.Time.AddMinutes(5) > data.DateTime) {
			//			//Found a match
			//			pairs.Add((data, armory));
			//			found = true;
			//			break;
			//		}
			//	}
			//	if (!found)
			//		pairs.Add((data, null));
			//}

			//foreach (var pair in pairs) {
			//	if (pair.armory == null) {
			//		_log.WriteLine($"{pair.data.Boss}   |   null");
			//		continue;
			//	}

			//	if (pair.data.Boss == pair.armory.Boss) {
			//		_log.WriteLine($"{pair.data.Boss}   |   {pair.armory.Boss}");
			//		continue;
			//	}

			//	_log.ErrorLine($"{pair.data.Boss}   |   {pair.armory.Boss}");
			//}

			//{ }
		}

		private void Button_ReloadPattern(object sender, RoutedEventArgs e) {
			_log.ErrorLine("Oops this does nothing");
			//Worldboss.LoadPatternFile();
		}

		private void Button_OpenDBFolder(object sender, RoutedEventArgs e) {
			ProcessStartInfo process = new ProcessStartInfo() {
				Arguments = Database.LocalAppPath,
				FileName = "explorer.exe"
			};

			Process.Start(process);
		}
	}
}
