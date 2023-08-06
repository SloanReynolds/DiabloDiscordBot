using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiabloDiscordBot.DiabloStuff;
using DiabloDiscordBot.DiscordStuff.DatabaseStuff;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace DiabloDiscordBot.DiscordStuff.SlashCommands {
	[SlashCommandGroup("worldboss", "Worldboss Information/Setup Commands")]
	internal class Worldboss : ApplicationCommandModule {
		[SlashCommand("setup", "Sets up the server for D4 Worldboss Alerts.")]
		[SlashRequirePermissions(DSharpPlus.Permissions.ManageRoles)]
		public async Task WorldbossSetup(
			InteractionContext ctx,

			[Option("bossChannel", "Text channel where Worldboss pings will show up")]
			DiscordChannel bossChannel,
			[Option("bossRole", "Role to ping when a Worldboss Event is near")]
			DiscordRole bossRole
			) {
			await ctx.DeferAsync(true);

			GuildRecord.UpdateWorldboss(ctx.Guild.Id, bossChannel.Id, bossRole.Id);

			await ctx.FollowUpAsync($"All set! Worldboss Pings will go to the roles: {bossRole.Mention} in channel:{bossChannel.Mention}", true);

			ILogger.Service.WriteLine("Worldboss Setup " + ctx.Guild.Id);
		}

		[SlashCommand("remove", "Removes current settings for Worldboss alerts.")]
		[SlashRequirePermissions(DSharpPlus.Permissions.ManageRoles)]
		public async Task WorldbossRemove(InteractionContext ctx) {
			await ctx.DeferAsync(true);
			GuildRecord.UpdateWorldboss(ctx.Guild.Id, 0, 0);
			await ctx.FollowUpAsync($"You will no longer receive alerts for Worldboss Events.", true);
		}

		[SlashCommand("when", "Lets you know when the next Worldboss event will occur.")]
		public async Task WorldBossWhen(InteractionContext ctx) {
			await ctx.DeferAsync(true);
			EventDetails evt = WorldBossEvent.GetDetails();
			await ctx.FollowUpAsync($"{evt.Message}", true);
		}
	}
}
