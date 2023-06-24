using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DiabloDiscordBot.DiabloStuff.WorldBossEvent;

namespace DiabloDiscordBot.DiabloStuff {
	internal class WorldBossEvent {
		private const int _alertMinutes = 20;

		private static EventDetails _cached;

		public static EventDetails GetDetails(bool secondTime = false) {
			if (secondTime) {
				return new EventDetails(AlertType.WorldBoss2, _cached.MinutesUntilNext, _cached.Message, 5);
			}

			while (DateTime.Now >= _next.DateTime) {
				_next = _next.GetNext();
				ILogger.Service.WriteLine($"{_next.Boss}");
			}

			int minutes = (int)(_next.DateTime - DateTime.Now).TotalMinutes;
			ILogger.Service.WriteLine($"Worldboss ({_next.Boss}): {minutes}");

			if (!secondTime) {
				_cached = new EventDetails(AlertType.WorldBoss, minutes, $"World Boss '{_next.Boss}' incoming in {minutes} minutes!", _alertMinutes);
			}

			return _cached;
		}



		private static readonly WorldBossEventData _start = new WorldBossEventData(UTCHelper.UnixTimeStampToDateTime(1686635315), 1, 0);
		private static WorldBossEventData _next = _start.GetNext();
	}

	public enum WorldBoss {
		Ashava,
		Avarice,
		WanderingDeath
	}

	public struct WorldBossEventData {
		private static readonly double[] _timePattern = new double[] {
			325.22f,
			353.00f,
			353.49f,
			325.72f,
			353.49f
		};
		private static readonly WorldBoss[] _bossPattern = new WorldBoss[] {
			WorldBoss.WanderingDeath, WorldBoss.WanderingDeath,
			WorldBoss.Avarice, WorldBoss.Avarice,

			WorldBoss.Ashava, WorldBoss.Ashava, WorldBoss.Ashava,
			WorldBoss.WanderingDeath, WorldBoss.WanderingDeath,

			WorldBoss.Avarice, WorldBoss.Avarice, WorldBoss.Avarice,
			WorldBoss.Ashava, WorldBoss.Ashava,

			WorldBoss.WanderingDeath, WorldBoss.WanderingDeath, WorldBoss.WanderingDeath,
			WorldBoss.Avarice, WorldBoss.Avarice,

			WorldBoss.Ashava, WorldBoss.Ashava, WorldBoss.Ashava,
			WorldBoss.WanderingDeath, WorldBoss.WanderingDeath,

			WorldBoss.Avarice, WorldBoss.Avarice, WorldBoss.Avarice,
			WorldBoss.Ashava, WorldBoss.Ashava,

			WorldBoss.WanderingDeath, WorldBoss.WanderingDeath, WorldBoss.WanderingDeath,
			WorldBoss.Avarice, WorldBoss.Avarice,

			WorldBoss.Ashava, WorldBoss.Ashava, WorldBoss.Ashava,
		};

		public DateTime DateTime { get; }
		public WorldBoss Boss => _bossPattern[_bossPatternIndex];

		private int _timePatternIndex { get; }
		private int _bossPatternIndex { get; }

		public WorldBossEventData(DateTime dateTime, int timePatternIndex, int bossPatternIndex) {
			this.DateTime = dateTime;
			this._timePatternIndex = timePatternIndex;
			this._bossPatternIndex = bossPatternIndex;
		}

		public WorldBossEventData GetNext() {
			int index = this._timePatternIndex == _timePattern.Length - 1 ? 0 : this._timePatternIndex + 1;
			int bossIndex = this._bossPatternIndex == _bossPattern.Length - 1 ? 0 : this._bossPatternIndex + 1;

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
