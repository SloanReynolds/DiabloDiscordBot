using System.Threading.Tasks;
using DiabloDiscordBot.DiscordStuff.DatabaseStuff;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace DiabloDiscordBot.DiscordStuff {
	internal class SlashCommands : ApplicationCommandModule {
		[SlashCommand("GenbarHelltide", "Sets up the server for D4 Helltide Alerts.")]
		[SlashRequirePermissions(DSharpPlus.Permissions.ManageRoles)]
		public async Task GenbarHelltide(
			InteractionContext ctx,

			[Option("hellChannel", "Text channel where Helltide pings will show up")]
			DiscordChannel hellChannel,
			[Option("hellRole", "Role to ping when a Helltide is near")]
			DiscordRole hellRole
			) {
			await ctx.DeferAsync(true);

			GuildRecord.UpdateHelltide(ctx.Guild.Id, hellChannel.Id, hellRole.Id);

			await ctx.FollowUpAsync($"All set! Worldboss Pings will go to the roles: {hellRole.Mention} in channel:{hellChannel.Mention}", true);

			ILogger.Service.WriteLine("Legion Setup " + ctx.Guild.Id);
		}

		[SlashCommand("GenbarLegion", "Sets up the server for D4 Legion Alerts.")]
		[SlashRequirePermissions(DSharpPlus.Permissions.ManageRoles)]
		public async Task GenbarLegion(
			InteractionContext ctx,

			[Option("legionChannel", "Text channel where Legion pings will show up")]
			DiscordChannel legionChannel,
			[Option("legionRole", "Role to ping when a Legion Event is near")]
			DiscordRole legionRole
			) {
			await ctx.DeferAsync(true);

			GuildRecord.UpdateLegion(ctx.Guild.Id, legionChannel.Id, legionRole.Id);

			await ctx.FollowUpAsync($"All set! Legion Pings will go to the roles: {legionRole.Mention} in channel:{legionChannel.Mention}", true);

			ILogger.Service.WriteLine("Legion Setup " + ctx.Guild.Id);
		}

		[SlashCommand("GenbarWorldboss", "Sets up the server for D4 Worldboss Alerts.")]
		[SlashRequirePermissions(DSharpPlus.Permissions.ManageRoles)]
		public async Task GenbarWorldboss(
			InteractionContext ctx,

			[Option("bossChannel", "Text channel where Worldboss pings will show up")]
			DiscordChannel bossChannel,
			[Option("bossRole", "Role to ping when a Worldboss Event is near")]
			DiscordRole bossRole
			) {
			await ctx.DeferAsync(true);

			GuildRecord.UpdateWorldboss(ctx.Guild.Id, bossChannel.Id, bossRole.Id);

			await ctx.FollowUpAsync($"All set! Worldboss Pings will go to the roles: {bossRole.Mention} in channel:{bossChannel.Mention}", true);

			ILogger.Service.WriteLine("Legion Setup " + ctx.Guild.Id);
		}





		[SlashCommand("GenbarRemoveHelltide", "Removes alerts for Helltides.")]
		[SlashRequirePermissions(DSharpPlus.Permissions.ManageRoles)]
		public async Task GenbarRemoveHelltide(InteractionContext ctx) {
			await ctx.DeferAsync(true);
			GuildRecord.UpdateHelltide(ctx.Guild.Id, 0, 0);
			await ctx.FollowUpAsync($"You will no longer receive alerts for Helltides.", true);
		}

		[SlashCommand("GenbarRemoveLegion", "Sets up the server for D4 Worldboss Alerts.")]
		[SlashRequirePermissions(DSharpPlus.Permissions.ManageRoles)]
		public async Task GenbarRemoveLegion(InteractionContext ctx) {
			await ctx.DeferAsync(true);
			GuildRecord.UpdateLegion(ctx.Guild.Id, 0, 0);
			await ctx.FollowUpAsync($"You will no longer receive alerts for Legion Events.", true);
		}

		[SlashCommand("GenbarRemoveWorldboss", "Sets up the server for D4 Worldboss Alerts.")]
		[SlashRequirePermissions(DSharpPlus.Permissions.ManageRoles)]
		public async Task GenbarRemoveWorldboss(InteractionContext ctx) {
			await ctx.DeferAsync(true);
			GuildRecord.UpdateWorldboss(ctx.Guild.Id, 0, 0);
			await ctx.FollowUpAsync($"You will no longer receive alerts for Worldboss Events.", true);
		}





		[SlashCommand("GenbarWrong", "Report that an alert gave false information")]
		public async Task GenbarWrong(
			InteractionContext ctx,
			[Option("information", "Describe what went wrong")]
			string information
			) {
			await ctx.DeferAsync(true);

			new WrongReportRecord(information).SaveNew();

			await ctx.FollowUpAsync($"Thanks for your feedback.", true);
		}
	}
}
