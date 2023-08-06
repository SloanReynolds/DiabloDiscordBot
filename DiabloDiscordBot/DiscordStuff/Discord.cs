using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using DiabloDiscordBot.DiabloStuff;
using DiabloDiscordBot.DiscordStuff.DatabaseStuff;
using DiabloDiscordBot.DiscordStuff.SlashCommands;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;

namespace DiabloDiscordBot.DiscordStuff
{
    internal class Discord {
		private DiabloEventWatcher _diablo = SingletonContainer.I.GetService<DiabloEventWatcher>();
		private Database _database => SingletonContainer.I.GetService<Database>();
		private DiabloBotConsole _console => SingletonContainer.I.GetService<DiabloBotConsole>();
		private DiscordClient _client;
		private bool _isConnected = false;

		internal bool SendAlertsToAll(EventDetails eventDetails) {
			bool any = false;
			foreach (var guildSetting in _database.AllGuildSettings()) {
				if (SendAlert(guildSetting, eventDetails) && !any) {
					any = true;
				}
			}

			return any;
		}

		internal bool SendAlert(GuildRecord guildSettings, EventDetails eventDetails) {
			var guild = _client.GetGuildAsync(guildSettings.GuildID).GetAwaiter().GetResult();

			var type = eventDetails.AlertType;
			var message = eventDetails.Message;
			int cooldown = eventDetails.AlertMinutes * 2;

			var previous = _database.GetAlerts(guild, type, cooldown);
			if (previous.Any()) {
				//There are some alerts here, which means we've sent an alert of this type to this guild recently... ixnay bitch
				return false;
			}


			var roleId = guildSettings.BossRole;
			var channelId = guildSettings.BossChannelID;
			if (type == AlertType.Legion) {
				roleId = guildSettings.LegionRole;
				channelId = guildSettings.LegionChannelID;
			} else if (type == AlertType.Helltide) {
				roleId = guildSettings.HellRole;
				channelId = guildSettings.HellChannelID;
			}

			if (roleId == 0 || channelId == 0) //We don't want any alerts of this type, thanks much!
				return false;


			var role = guild.GetRole(roleId);
			var channel = guild.GetChannel(channelId);

			ILogger.Service.WriteLine($"Sending Alert to {guild.Id} -> {type}:'{message}'");
			_client
				.SendMessageAsync(channel, $"{role.Mention} - {message}");


			new AlertRecord(guild.Id, type, message).SaveNew();
			return true;
		}

		internal void Connect() {
			if (_isConnected)
				return;
			DiscordConfiguration config = new DiscordConfiguration() {
				Token = PrivateParts.Token,
				TokenType = TokenType.Bot,
			};

			_client = new DiscordClient(config);
			var slash = _client.UseSlashCommands();
			slash.RegisterCommands<SlashCommandsMisc>();
			slash.RegisterCommands<Worldboss>();
			slash.RegisterCommands<Helltide>();
			slash.RegisterCommands<Legion>();
			//slash.RegisterCommands<SlashCommands>();
			slash.SlashCommandErrored += async (s, e) => {
				if (e.Exception is SlashExecutionChecksFailedException slex) {
					foreach (var check in slex.FailedChecks)
						e.Context.CreateResponseAsync("You need to have the 'ManageRoles' permissions to run setup.").GetAwaiter().GetResult();
				}
			};

			_client.ConnectAsync().ContinueWith(_Connected);
		}

		private void _Connected(Task task) {
			_diablo.Begin();
			ILogger.Service.WriteLine("Discord Client Connected!");
			_console.Connected_Success();
			_isConnected = true;
		}

		internal void Disconnect() {
			if (!_isConnected)
				return;
			_client.DisconnectAsync().ContinueWith(_Disconnected);
		}

		private void _Disconnected(Task task) {
			_diablo.CancelLoop();
			ILogger.Service.WriteLine("Discord Client Disconnected!");
			_console.Disconnected_Success();
			_isConnected = false;
		}
	}
}
