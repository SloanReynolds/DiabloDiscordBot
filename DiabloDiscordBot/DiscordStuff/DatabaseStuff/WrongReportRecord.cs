using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiabloBotShared;
using DiabloDiscordBot.DiabloStuff;
using DiabloDiscordBot.DiscordStuff.DatabaseStuff.Attributes;

namespace DiabloDiscordBot.DiscordStuff.DatabaseStuff {
	[Table("WrongReport")]
	internal record WrongReportRecord : AbstractRecord {
		public override ulong PrimaryKey => 0;

		[Column(ColumnType.Integer, isNullable: false)]
		public long TimeStamp { get; }
		[Column(ColumnType.Text)]
		public string Message { get; }

		public WrongReportRecord(string message) {
			TimeStamp = UTCHelper.UnixNow;
			Message = message;
		}
	}
}
