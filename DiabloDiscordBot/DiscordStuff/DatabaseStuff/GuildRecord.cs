using System;
using DiabloDiscordBot.DiscordStuff.DatabaseStuff.Attributes;
using Microsoft.Data.Sqlite;

namespace DiabloDiscordBot.DiscordStuff.DatabaseStuff {
	[Table("GuildSettings")]
	internal record GuildRecord : AbstractRecord {
		public override ulong PrimaryKey => GuildID;

		[Column(ColumnType.Integer, true, false, true)]
		public ulong GuildID { get; }

		[Column(ColumnType.Integer, isNullable: false)]
		public ulong HellChannelID { get; }
		[Column(ColumnType.Integer, isNullable: false)]
		public ulong HellRole { get; }

		[Column(ColumnType.Integer, isNullable: false)]
		public ulong LegionChannelID { get; }
		[Column(ColumnType.Integer, isNullable: false)]
		public ulong LegionRole { get; }

		[Column(ColumnType.Integer, isNullable: false)]
		public ulong BossChannelID { get; }
		[Column(ColumnType.Integer, isNullable: false)]
		public ulong BossRole { get; }

		public GuildRecord(ulong guildId, ulong hellChannelId, ulong hellRole, ulong legionChannelId, ulong legionRole, ulong bossChannelId, ulong bossRole) {
			GuildID = guildId;
			HellChannelID = hellChannelId;
			HellRole = hellRole;
			LegionChannelID = legionChannelId;
			LegionRole = legionRole;
			BossChannelID = bossChannelId;
			BossRole = bossRole;
		}

		public GuildRecord(SqliteDataReader reader) {
			GuildID = (ulong)(long)reader[0];
			HellChannelID = (ulong)(long)reader[1];
			HellRole = (ulong)(long)reader[2];

			LegionChannelID = (ulong)(long)reader[3];
			LegionRole = (ulong)(long)reader[4];

			BossChannelID = (ulong)(long)reader[5];
			BossRole = (ulong)(long)reader[6];
		}

		public static void UpdateHelltide(ulong guildId, ulong channelId, ulong role) {
			_database.UpdateGuildRecord(guildId, new string[] { "HellChannelID", "HellRole" }, new ulong[] { channelId, role });
		}

		public static void UpdateLegion(ulong guildId, ulong channelId, ulong role) {
			_database.UpdateGuildRecord(guildId, new string[] { "LegionChannelID", "LegionRole" }, new ulong[] { channelId, role });
		}

		public static void UpdateWorldboss(ulong guildId, ulong channelId, ulong role) {
			_database.UpdateGuildRecord(guildId, new string[] { "BossChannelID", "BossRole" }, new ulong[] { channelId, role });
		}
	}
}
