using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiabloDiscordBot.DiscordStuff.DatabaseStuff;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands.Attributes;
using DSharpPlus.SlashCommands;
using DiabloDiscordBot.DiabloStuff;

namespace DiabloDiscordBot.DiscordStuff.SlashCommands {
	[SlashCommandGroup("helltide", "Helltide Information/Setup Commands")]
	internal class Helltide : ApplicationCommandModule {
		[SlashCommand("setup", "Sets up the server for D4 Helltide Alerts.")]
		[SlashRequirePermissions(DSharpPlus.Permissions.ManageRoles)]
		public async Task HelltideSetup(
			InteractionContext ctx,

			[Option("hellChannel", "Text channel where Helltide pings will show up")]
			DiscordChannel hellChannel,
			[Option("hellRole", "Role to ping when a Helltide is near")]
			DiscordRole hellRole
			) {
			await ctx.DeferAsync(true);

			GuildRecord.UpdateHelltide(ctx.Guild.Id, hellChannel.Id, hellRole.Id);

			await ctx.FollowUpAsync($"All set! Helltide Pings will go to the roles: {hellRole.Mention} in channel:{hellChannel.Mention}", true);

			ILogger.Service.WriteLine("Helltide Setup " + ctx.Guild.Id);
		}

		[SlashCommand("remove", "Removes alerts for Helltides.")]
		[SlashRequirePermissions(DSharpPlus.Permissions.ManageRoles)]
		public async Task HelltideRemove(InteractionContext ctx) {
			await ctx.DeferAsync(true);
			GuildRecord.UpdateHelltide(ctx.Guild.Id, 0, 0);
			await ctx.FollowUpAsync($"You will no longer receive alerts for Helltides.", true);
		}

		[SlashCommand("when", "Lets you know when the next Helltide event will occur.")]
		public async Task HelltideWhen(InteractionContext ctx) {
			await ctx.DeferAsync(true);
			EventDetails evt = HelltideEvent.GetDetails();
			await ctx.FollowUpAsync($"{evt.Message}", true);
		}
	}
}
