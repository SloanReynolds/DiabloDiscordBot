using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DiabloDiscordBot.DiscordStuff;
using DiabloDiscordBot.DiscordStuff.DatabaseStuff;

namespace DiabloDiscordBot.DiabloStuff {
	internal class DiabloEventWatcher {
		private bool _isRunning = false;
		private CancellationTokenSource _cts;
		private Discord _discord => SingletonContainer.I.GetService<Discord>();

		internal void Begin() {
			_cts = new CancellationTokenSource();

			Task.Run(_Run, _cts.Token);
		}

		public void CancelLoop() {
			_cts.Cancel();
		}

		private void _Run() {
			_isRunning = true;
			try {
				ILogger.Service.WriteLine("Watching for World Events");
				while (!_cts.IsCancellationRequested) {
					EventDetails[] events = new EventDetails[] {
						HelltideEvent.GetDetails(),
						LegionEvent.GetDetails(),
						WorldBossEvent.GetDetails(),
						WorldBossEvent.GetDetails(true),
					};

					for (int i = 0; i < events.Length; i++) {
						var details = events[i];
						if (details.MinutesUntilNext <= details.AlertMinutes) {
							ILogger.Service.WriteLine(details.AlertType.ToString());
							if (_discord.SendAlertsToAll(details)) {
								d4ArmoryScraper.Service.Scrape(details);
							}
						}
					}

					Thread.Sleep(30 * 1000);
				}
			} finally {
				ILogger.Service.WriteLine("Stopped watching for World Events");
				_isRunning = false;
			}
		}
	}
}
