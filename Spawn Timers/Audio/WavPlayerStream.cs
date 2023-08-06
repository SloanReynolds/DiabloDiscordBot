using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Media;
using System.Runtime.InteropServices.ComTypes;

namespace Spawn_Timers {
	[DesignerCategory("")] //This stops VS from trying to open it in design view
	internal class WavPlayerStream : SoundPlayer {
		private float _currentVolume = 1f;
		private int _byteRate = 0;
		private Stream _originalStream;

		private List<SubChunkHeader> _subChunks = new List<SubChunkHeader>();

		public WavPlayerStream(UnmanagedMemoryStream wavStream) : base(wavStream) {
			_originalStream = new MemoryStream(new byte[wavStream.Length], true);
			Stream = new MemoryStream(new byte[wavStream.Length], true);

			_Copy(wavStream, _originalStream);
			_Copy(wavStream, Stream);

			{ // Make subchucks
				long offset = wavStream.Position;

				//Get byteRate
				wavStream.Seek(28, SeekOrigin.Begin);
				_byteRate = _Read4Bytes(wavStream);

				//Get SubchunkHeaders
				wavStream.Position = 36;
				SubChunkHeader latest;

				int count = 1;
				Console.WriteLine("Length: " + wavStream.Length);
				do {
					Console.WriteLine("SubChunk: " + count);
					count++;
					latest = new SubChunkHeader(wavStream);
					_subChunks.Add(latest);
					Console.WriteLine("latest Size: " + latest.Size);
					Console.WriteLine("current Pos: " + wavStream.Position);
					Console.WriteLine("------------");
					wavStream.Position += latest.Size;
				} while (wavStream.Position < wavStream.Length);

				wavStream.Position = offset;
			}
		}

		private static void _PrintBytes(Stream stream, int count, int offset = 0) {
			long oldPos = stream.Position;

			if (offset > 0) stream.Seek(offset, SeekOrigin.Current);
			while (count > 0) {
				Console.Write(stream.ReadByte().ToString("x") + " ");
				count--;
			}
			Console.WriteLine();

			stream.Position = oldPos;
		}

		private void _Copy(Stream s1, Stream s2) {
			long offset = s1.Position;
			long offset2 = s2.Position;

			s1.CopyTo(s2);

			s1.Position = offset;
			s2.Position = offset2;
		}

		public void SetVolume(float volume) {
			Console.WriteLine(_originalStream.Length.ToString());
			Console.WriteLine(Stream.Length.ToString());

			var newStream = new MemoryStream(new byte[_originalStream.Length], true);

			//Sync-O-Tron Max
			_originalStream.Seek(0, SeekOrigin.Begin);
			newStream.Seek(0, SeekOrigin.Begin);

			foreach (var sub in _subChunks) {
				long stop = sub.DataOffset + sub.Size;

				//Write headers
				while (newStream.Position < sub.DataOffset) {
					newStream.WriteByte((byte)_originalStream.ReadByte());
				}

				//Shiggity shiggity shwaa data
				while (newStream.Position < stop) {
					short newVal = (short)(_Read2Bytes(_originalStream) * volume);
					_Write2Bytes(newStream, newVal);
				}
			}

			_originalStream.Seek(0, SeekOrigin.Begin);
			newStream.Seek(0, SeekOrigin.Begin);

			Stream = newStream;

			_currentVolume = volume;
		}

		public new void Play() {
			base.Play();
		}

		private static short _Read2Bytes(Stream stream) {
			return (short)((stream.ReadByte() << (8 * 0)) + (stream.ReadByte() << (8 * 1)));
		}

		private static void _Write2Bytes(Stream stream, short num, Stream stream2 = null) {
			try {
				stream.WriteByte((byte)num);
				stream.WriteByte((byte)(num >> 8));
			} catch (Exception e) {
				Console.WriteLine(stream.Position);
				Console.WriteLine(stream2?.Position + "");
				throw e;
			}
		}

		private static int _Read4Bytes(Stream stream) {
			return (stream.ReadByte() << (8 * 0)) + (stream.ReadByte() << (8 * 1)) + (stream.ReadByte() << (8 * 2)) + (stream.ReadByte() << (8 * 3));
		}

		private struct SubChunkHeader {
			public int SubChunkID;
			public long DataOffset;
			public int Size;
			public long HeaderOffset;

			public SubChunkHeader(Stream stream) {
				_PrintBytes(stream, 16, -4);

				if (stream.Position == 36) {
					HeaderOffset = 0;
				} else {
					HeaderOffset = stream.Position;
				}

				SubChunkID = _Read4Bytes(stream);
				Size = _Read4Bytes(stream);
				DataOffset = stream.Position;

				Console.WriteLine(DataOffset.ToString());
				Console.WriteLine(Size.ToString());
			}
		}
	}
}
