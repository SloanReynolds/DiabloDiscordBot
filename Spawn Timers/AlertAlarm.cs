using System;
using System.Windows;
using System.Windows.Media;

namespace Spawn_Timers {
	internal class AlertAlarm {
		private const int WORLDBOSS_ALARM_INTERVAL = 10;
		private const int WORLDBOSS_ALARM2_INTERVAL = 5;

		public bool IsWorldbossAlarmOn { get; private set; }

		private DateTime _Worldboss1Cooldown = DateTime.Now.AddMinutes(-10000);
		private DateTime _Worldboss2Cooldown = DateTime.Now.AddMinutes(-10000);

		internal void CheckTriggers(DateTime nextHelltide, DateTime nextWorldboss, DateTime nextLegion) {
			var worldbossMinutes = (nextWorldboss - DateTime.Now).TotalMinutes;
			if (IsWorldbossAlarmOn) {
				if (worldbossMinutes < WORLDBOSS_ALARM2_INTERVAL) {
					_ActivateWorldbossAlarm2();
				} else if (worldbossMinutes < WORLDBOSS_ALARM_INTERVAL) {
					_ActivateWorldbossAlarm();
				}
			}
		}

		internal void ToggleWorldbossAlarm() {
			IsWorldbossAlarmOn = !IsWorldbossAlarmOn;
		}

		private void _ActivateWorldbossAlarm() {
			if (DateTime.Now < _Worldboss1Cooldown)
				return;

			_Worldboss1Cooldown = DateTime.Now.AddMinutes(WORLDBOSS_ALARM_INTERVAL * 2);

			var flasher = new FlashWindowHelper(Application.Current);
			flasher.FlashApplicationWindow();

			CustomAudio.PlayWorldbossAlarm();
		}

		private void _ActivateWorldbossAlarm2() {
			if (DateTime.Now < _Worldboss2Cooldown)
				return;
			_Worldboss2Cooldown = DateTime.Now.AddMinutes(WORLDBOSS_ALARM2_INTERVAL * 2);

			MainWindow.Singleton.FlashBackground(Colors.Green);
		}
	}
}
