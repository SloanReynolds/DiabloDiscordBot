using System;
using DiabloBotShared;
using DiabloDiscordBot.DiscordStuff.DatabaseStuff.Attributes;
using Microsoft.Data.Sqlite;

namespace DiabloDiscordBot.DiscordStuff.DatabaseStuff {
	[Table("AlertsSent")]
	internal record AlertRecord : AbstractRecord {
		private SqliteDataReader _reader;

		public override ulong PrimaryKey => 0;

		[Column(ColumnType.Integer, isNullable: false)]
		public long TimeStamp { get; }
		[Column(ColumnType.Integer, isNullable: false)]
		public ulong GuildID { get; }
		[Column(ColumnType.Text, isNullable: false)]
		public AlertType AlertType { get; }
		[Column(ColumnType.Text)]
		public string Message { get; }

		public AlertRecord(ulong guildID, AlertType alertType, string message) {
			TimeStamp = UTCHelper.UnixNow;
			GuildID = guildID;
			AlertType = alertType;
			Message = message;
		}

		public AlertRecord(SqliteDataReader reader) {
			TimeStamp = (long)reader[0];
			GuildID = (ulong)(long)reader[1];
			AlertType = (AlertType)Enum.Parse(typeof(AlertType), reader[2].ToString());
			if (AlertType == AlertType.UNKNOWN)
				throw new Exception("Unknown AlertType parsed :(");
			Message = (string)reader[3];
		}
	}
}
