using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace Spawn_Timers {
	internal static class CustomAudio {
		private static bool _playing = false;
		private static object _playingLock = new object();
		private static Timer _playingTimer = null;
		private static float _currentVolume = 1f;

		private static List<WavPlayerStream> _all = new List<WavPlayerStream>() {
			new WavPlayerStream(Properties.Resources.lmao),
		};

		public static void PlayWorldbossAlarm() {
			_PlayFromList(0);
		}

		private static void _SetPlaying(bool playing, float duration = 0) {
			lock (_playingLock) {
				_playing = playing;
				if (_playing) {
					_playingTimer = new Timer((state) => _SetPlaying(false), null, (uint)(duration * 1000), Timeout.Infinite);
				} else {
					_playingTimer = null;
				}
			}
		}

		private static void _PlayFromList(int index) {
			if (_playing) return;

			var wp = _all[index];

			_SetPlaying(true, _GetWavDuration(wp.Stream));

			wp.Play();
		}

		private static float _GetWavDuration(Stream stream) {
			stream.Position = 28;
			int byteRate = _Read4Bytes(stream);
			stream.Position = 40;
			int size = _Read4Bytes(stream);
			stream.Position = 0;

			return (float)size / (float)byteRate;
		}

		private static int _Read4Bytes(Stream stream) {
			return (stream.ReadByte() << (8 * 0)) + (stream.ReadByte() << (8 * 1)) + (stream.ReadByte() << (8 * 2)) + (stream.ReadByte() << (8 * 3));
		}
	}
}
