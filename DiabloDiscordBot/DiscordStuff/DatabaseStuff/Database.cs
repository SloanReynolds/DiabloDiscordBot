using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DiabloDiscordBot.DiabloStuff;
using DiabloDiscordBot.DiscordStuff.DatabaseStuff.Attributes;
using DSharpPlus.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.Windows.Themes;

namespace DiabloDiscordBot.DiscordStuff.DatabaseStuff {
	internal class Database {
		//Oh yes, I am quite aware that this is rather poopy.
		//Rudimentary databases tend to be.
		private SqliteConnection _conn;

		private const string DB_FILENAME = "DiabloBase.db";
		private string _localAppDataPath => $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create)}\\DiabloDiscordBot";
		private string _dbFilePath => $"{_localAppDataPath}\\{DB_FILENAME}";

		public Database() {
			if (!Directory.Exists(_localAppDataPath)) {
				Directory.CreateDirectory(_localAppDataPath);
			}
			if (!File.Exists(_dbFilePath)) {
				_WriteAllTables();
			}
			_Connect();
		}

		private void _WriteAllTables() {
			_Connect();
			Open();

			_CreateTable(typeof(AlertRecord));
			_CreateTable(typeof(GuildRecord));
			_CreateTable(typeof(WrongReportRecord));

			Close();
			_Disconnect();
		}

		private bool _isConnected = false;
		private bool _isOpen = false;

		private void _Connect() {
			if (_isConnected)
				return;
			if (_conn != null)
				_DisposeConn();
			_conn = new SqliteConnection($"Data Source={_dbFilePath};");
			_isConnected = true;
		}

		private void _DisposeConn() {
			if (_conn != null) {
				if (_isOpen) {
					_conn.Close();
				}
				_conn.Dispose();
				_conn = null;
			}
		}

		private void _Disconnect() {
			if (!_isConnected)
				return;
			_DisposeConn();
			_isConnected = false;
		}

		internal void Open() {
			if (!_isOpen) {
				_conn.Open();
				_isOpen = true;
			}
		}

		internal void Close() {
			if (_isOpen) {
				_conn.Close();
				_isOpen = false;
			}
		}

		private void _CreateTable(Type type) {
			var tableName = TableAttribute.GetTableName(type);
			var columns = TableAttribute.GetColumns(type);
			Open();
			string sql = $"CREATE TABLE {tableName} ({string.Join(',', columns)});";

			Open();
			SqliteCommand command = new SqliteCommand(sql, _conn);
			command.ExecuteNonQuery();
			Close();
		}

		internal void SaveNew(AbstractRecord record) {
			var tableName = TableAttribute.GetTableName(record);
			var columns = TableAttribute.GetColumns(record);
			var primaryKey = columns.FirstOrDefault(col => col.IsPrimaryKey && col.IsUnique);
			if (primaryKey != null) {
				var reader = new SqliteCommand($"SELECT * FROM {tableName} WHERE {primaryKey.Name} = {record.PrimaryKey};", _conn).ExecuteReader();
				if (reader.Read()) {
					reader.Close();
					new SqliteCommand($"DELETE FROM {tableName} WHERE {primaryKey.Name} = {record.PrimaryKey};", _conn).ExecuteNonQuery();
				}
			}

			new SqliteCommand($"INSERT INTO {tableName} {record.GetValuesForInsert()};", _conn).ExecuteNonQuery();
		}

		//@@TODO: NEEDS A BATCH JOB
		internal IEnumerable<GuildRecord> AllGuildSettings() {
			List<GuildRecord> records = new();
			var tableName = TableAttribute.GetTableName<GuildRecord>();
			string sql = $"SELECT * FROM {tableName}";

			Open();
			var reader = new SqliteCommand(sql, _conn).ExecuteReader();
			while (reader.Read()) {
				records.Add(new GuildRecord(reader));
			}
			Close();

			return records;
		}

		internal IEnumerable<AlertRecord> GetAlerts(DiscordGuild guild, AlertType type, int cooldown) {
			var sql = $"SELECT * FROM {TableAttribute.GetTableName<AlertRecord>()} WHERE GuildID = {guild.Id} AND AlertType = '{type}' AND TimeStamp > {UTCHelper.UnixMinutesAgo(cooldown)}";

			Open();
			var reader = new SqliteCommand(sql, _conn).ExecuteReader();
			List<AlertRecord> records = new();
			while (reader.Read()) {
				records.Add(new AlertRecord(reader));
			}
			Close();

			return records;
		}

		internal GuildRecord GetGuildSettings(ulong id) {
			string sql = $"SELECT * FROM {TableAttribute.GetTableName<GuildRecord>()} WHERE GuildID = {id} LIMIT 1";

			Open();
			var reader = new SqliteCommand(sql, _conn).ExecuteReader();

			if (!reader.HasRows) {
				return null;
			}
			reader.Read();
			var newRecord = new GuildRecord(reader);
			Close();

			return newRecord;
		}

		internal void UpdateGuildRecord(ulong guildId, string[] names, ulong[] objects) {
			var cols = TableAttribute.GetColumns<GuildRecord>().Where(col => names.Contains(col.Name)).ToArray();
			if (GetGuildSettings(guildId) == null) {
				var recordNew = new GuildRecord(guildId, 0, 0, 0, 0, 0, 0);
				recordNew.SaveNew();
			}

			List<string> values = new();
			for (int i = 0; i < names.Length; i++) {
				values.Add($"{names[i]} = {objects[i]}");
			}
			string sql = $"UPDATE {TableAttribute.GetTableName<GuildRecord>()} SET {string.Join(',', values)} WHERE GuildID = {guildId}";

			Open();
			_ = new SqliteCommand(sql, _conn).ExecuteNonQuery();
			Close();
		}
	}
}
