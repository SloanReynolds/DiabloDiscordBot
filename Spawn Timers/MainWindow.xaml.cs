using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using DiabloBotShared;

namespace Spawn_Timers {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public static MainWindow Singleton => SingletonContainer.I.GetService<MainWindow>();

		private Timer _tick;
		private AlertAlarm _alarm = new();

		private List<Timer> _expiringTimers = new();
		private Color _origBG;

		public MainWindow() {
			InitializeComponent();

			SingletonContainer.I
				.RegisterService<HttpHelper>(new HttpHelper())
				.RegisterService<EventStore>(new EventStore())
				.RegisterService<MainWindow>(this)
			;

			_tick = new Timer(Tick, null, 0, 1000);
			_origBG = ((SolidColorBrush)this.Background).Color;

			//FlashBackground(Colors.Green);
			//_ = new Timer(Tick, null, 0, Timeout.Infinite);
		}

		private void Tick(object? state) {
			Application.Current.Dispatcher.Invoke(() => {
				var ev = EventStore.Singleton;
				timeHelltide.Content = TimespanFormat(ev.NextHelltide);
				timeWorldboss.Content = TimespanFormat(ev.NextWorldboss);
				timeLegion.Content = TimespanFormat(ev.NextLegion);

				labelWorldboss.Content = $"World Boss ({ev.NextWorldbossName}):";

				_alarm.CheckTriggers(ev.NextHelltide, ev.NextWorldboss, ev.NextLegion);

				ev.RefreshIfNeeded();
			});
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

		private void WorldbossAlarm_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			_alarm.ToggleWorldbossAlarm();

			if (_alarm.IsWorldbossAlarmOn) {
				toggleWorldbossAlarm.Foreground = new SolidColorBrush(Colors.Gold);
				toggleWorldbossAlarm.Content = "🔔";
			} else {
				toggleWorldbossAlarm.Foreground = new SolidColorBrush(Colors.DimGray);
				toggleWorldbossAlarm.Content = "🔕";
			}
		}

		private void _ChangeBackgroundColor(Color color) {
			Application.Current.Dispatcher.Invoke(() => {
				this.Background = new SolidColorBrush(color);
			});
		}

		internal void FlashBackground(Color color) {
			int cycleHalf = 400;
			int cycles = 5;

			for (int i = 0; i <= cycles; i++) {
				var cycleStart = i * cycleHalf * 2;
				_AddExpTimer((state) => { _ChangeBackgroundColor(color); }, cycleStart);
				_AddExpTimer((state) => { _ChangeBackgroundColor(_origBG); }, cycleStart + cycleHalf);
			}

			_AddExpTimer((state) => { _expiringTimers.Clear(); }, cycleHalf * (cycles + 1));
		}

		private void _AddExpTimer(TimerCallback action, int delay) {
			_expiringTimers.Add(new Timer(action, null, delay, Timeout.Infinite));
		}
	}
}
