using System;
using System.Collections.Generic;

namespace DiabloDiscordBot.DiabloStuff {
	internal class WorldBossEvent {
		private const int _alertMinutes = 20;
		private const int _alertMinutes2 = 5;

		private static readonly DateTime _startTime = UTCHelper.UnixTimeStampToDateTime(1686463595);

		public static void Reset() {
			_history = new() {
				new WorldBossEventData(_startTime, 1, 0)
			};
			_next = _history[0].GetNext();
		}

		//public static EventDetails FallBackDetails(bool secondWarning = false) {
		//	while (DateTime.Now >= _next.DateTime) {
		//		_history.Add(_next);
		//		_next = _next.GetNext();
		//		ILogger.Service.WriteLine($"????? - Fell Back");
		//	}

		//	int minutes = (int)(_next.DateTime - DateTime.Now).TotalMinutes;
		//	ILogger.Service.WriteLine($"Worldboss (Unknown?): {minutes}");

		//	return new EventDetails(AlertType.WorldBoss, minutes, $"World Boss '{_next.Boss}' <t:{UTCHelper.ToUnixTimestamp(_next.DateTime)}:R>!", secondWarning ? _alertMinutes2 : _alertMinutes);
		//}

		public static EventDetails GetDetails(bool secondWarning = false) {
			while (DateTime.Now >= _next.DateTime) {
				_history.Add(_next);
				_next = _next.GetNext();
				ILogger.Service.WriteLine($"{_next.Boss}");
			}

			int minutes = (int)(_next.DateTime - DateTime.Now).TotalMinutes;
			ILogger.Service.WriteLine($"Worldboss ({_next.Boss}): {minutes}");

			return new EventDetails(AlertType.WorldBoss, minutes, $"World Boss '{_next.Boss}' <t:{UTCHelper.ToUnixTimestamp(_next.DateTime)}:R>!", secondWarning ? _alertMinutes2 : _alertMinutes);
		}

		private static List<WorldBossEventData> _history = new() {
			new WorldBossEventData(_startTime, 1, 0)
		};

		private static WorldBossEventData _next = _history[0].GetNext();

		public static List<WorldBossEventData> History => _history;
	}

	public record WorldBossEventData {
		private static readonly double[] _timePattern = new double[] {
			353.49f,
			325.72f,
			353.49f,
			325.22f,
			353.00f,
		};


		public DateTime DateTime { get; }
		public WorldBoss.Type Boss => WorldBoss.Pattern[_bossPatternIndex];

		private int _timePatternIndex { get; }
		private int _bossPatternIndex { get; }

		public WorldBossEventData(DateTime dateTime, int timePatternIndex, int bossPatternIndex) {
			this.DateTime = dateTime;
			this._timePatternIndex = timePatternIndex;
			this._bossPatternIndex = bossPatternIndex;
		}

		public WorldBossEventData GetNext() {
			int index = this._timePatternIndex == _timePattern.Length - 1 ? 0 : this._timePatternIndex + 1;
			int bossIndex = this._bossPatternIndex == WorldBoss.Pattern.Length - 1 ? 0 : this._bossPatternIndex + 1;

			var time = this.DateTime.AddMinutes(_timePattern[index]);
			if (_NeedsTwoHours(time)) {
				time = time.AddHours(2);
			}

			return new WorldBossEventData(time, index, bossIndex);
		}

		private static bool _NeedsTwoHours(DateTime dt) {
			//This method lets us know if the event falls within the no-go zone as defined by blizzard
			//    ¯\(°_o)/¯
			int offset = 0;
			DateTime dtu = dt.ToUniversalTime();
			DateTime midnightish = dtu.Date.AddMinutes(30);

			while (offset < 24) {
				DateTime checkLow = midnightish.AddHours(offset);
				DateTime checkHigh = checkLow.AddHours(4);

				if (checkLow <= dtu && dtu < checkHigh) {
					return true;
				}

				offset += 6;
			}
			return false;
		}
	}
}
