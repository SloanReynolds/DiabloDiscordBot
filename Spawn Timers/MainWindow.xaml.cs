using System;
using System.Media;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using DiabloDiscordBot;
using DiabloDiscordBot.DiscordStuff;
using DiabloDiscordBot.WebStuff;
using Microsoft.Extensions.Logging;

namespace Spawn_Timers {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private const int WORLDBOSS_ALARM_INTERVAL = 10;
		private Timer _tick;

		public MainWindow() {
			InitializeComponent();

			SingletonContainer.I
				.RegisterService<HttpHelper>(new HttpHelper())
				.RegisterService<EventStore>(new EventStore())
			;

			_tick = new Timer(Tick, null, 0, 1000);
			//_ = new Timer(Tick, null, 0, Timeout.Infinite);
		}

		private void Tick(object? state) {
			Application.Current.Dispatcher.Invoke(() => {
				var ev = EventStore.Service;
				timeHelltide.Content = TimespanFormat(ev.NextHelltide);
				timeWorldboss.Content = TimespanFormat(ev.NextWorldboss);
				timeLegion.Content = TimespanFormat(ev.NextLegion);

				labelWorldboss.Content = $"World Boss ({ev.NextWorldbossName}):";

				if (_isWorldbossAlarm
					&& (ev.NextWorldboss - DateTime.Now).TotalMinutes < WORLDBOSS_ALARM_INTERVAL) {
					ActivateWorldbossAlarm();
				}

				ev.RefreshIfNeeded();

				//_tick = new Timer(Tick, null, 1000, Timeout.Infinite);
			});
		}

		private void ActivateWorldbossAlarm() {
			if (DateTime.Now < DontRingUntil)
				return;

			DontRingUntil = DateTime.Now.AddMinutes(WORLDBOSS_ALARM_INTERVAL * 2);

			var flasher = new FlashWindowHelper(Application.Current);
			flasher.FlashApplicationWindow();

			CustomAudio.PlayWorldbossAlarm();
		}

		private string TimespanFormat(DateTime next) {
			bool isNegative = DateTime.Now > next ? true : false;
			TimeSpan span = next - DateTime.Now;

			return $"{(isNegative ? "-" : "")}{span.Hours:D2}:{span.Minutes:D2}:{span.Seconds:D2}";
		}

		private bool _isPinned = false;
		private void pinWindow_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			_isPinned = !_isPinned;

			if (_isPinned) {
				togglePin.Foreground = new SolidColorBrush(Colors.White);
				this.Topmost = true;
			} else {
				togglePin.Foreground = new SolidColorBrush(Colors.DimGray);
				this.Topmost = false;
			}
		}

		private bool _isWorldbossAlarm = false;
		private DateTime DontRingUntil = DateTime.Now.AddMinutes(-20);
		private void WorldbossAlarm_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			_isWorldbossAlarm = !_isWorldbossAlarm;

			if (_isWorldbossAlarm) {
				toggleWorldbossAlarm.Foreground = new SolidColorBrush(Colors.Gold);
				toggleWorldbossAlarm.Content = "🔔";
			} else {
				toggleWorldbossAlarm.Foreground = new SolidColorBrush(Colors.DimGray);
				toggleWorldbossAlarm.Content = "🔕";
			}
		}
	}
}
