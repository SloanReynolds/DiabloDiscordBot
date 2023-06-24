using System;
using System.Windows;
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

		public DiabloBotConsole() {
			InitializeComponent();

			SingletonContainer.I
				.RegisterService<DiabloEventWatcher>(new DiabloEventWatcher())
				.RegisterService<Database>(new Database())
				.RegisterService<Discord>(new Discord())
				.RegisterService<ILogger>(consoleOutput)
				.RegisterService<DiabloBotConsole>(this)
				;

			_Connect();
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
			WorldBossEvent.GetDetails();
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
	}
}
