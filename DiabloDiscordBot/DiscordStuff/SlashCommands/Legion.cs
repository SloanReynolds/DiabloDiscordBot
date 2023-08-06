using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiabloBotShared;
using DiabloDiscordBot.DiscordStuff.DatabaseStuff;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace DiabloDiscordBot.DiscordStuff.SlashCommands {
	[SlashCommandGroup("legion", "legion event Information/Setup Commands")]
	internal class Legion : ApplicationCommandModule {
		[SlashCommand("setup", "Sets up the server for D4 Legion Alerts.")]
		[SlashRequirePermissions(DSharpPlus.Permissions.ManageRoles)]
		public async Task LegionSetup(
			InteractionContext ctx,

			[Option("legionChannel", "Text channel where Legion pings will show up")]
			DiscordChannel legionChannel,
			[Option("legionRole", "Role to ping when a Legion Event is near")]
			DiscordRole legionRole
			) {
			await ctx.DeferAsync(true);

			GuildRecord.UpdateLegion(ctx.Guild.Id, legionChannel.Id, legionRole.Id);

			await ctx.FollowUpAsync($"All set! Legion Pings will go to the roles: {legionRole.Mention} in channel:{legionChannel.Mention}", true);

			ILogger.Singleton.WriteLine("Legion Setup " + ctx.Guild.Id);
		}

		[SlashCommand("remove", "Removes current settings for Legion alerts.")]
		[SlashRequirePermissions(DSharpPlus.Permissions.ManageRoles)]
		public async Task LegionRemove(InteractionContext ctx) {
			await ctx.DeferAsync(true);
			GuildRecord.UpdateLegion(ctx.Guild.Id, 0, 0);
			await ctx.FollowUpAsync($"You will no longer receive alerts for Legion Events.", true);
		}

		[SlashCommand("when", "Lets you know when the next Legion event will occur.")]
		public async Task LegionWhen(InteractionContext ctx) {
			await ctx.DeferAsync(true);
			EventDetails evt = LegionEvent.GetDetails();
			await ctx.FollowUpAsync($"{evt.Message}", true);
		}
	}
}
