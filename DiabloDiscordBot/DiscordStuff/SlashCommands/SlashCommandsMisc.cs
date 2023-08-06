using System.Threading.Tasks;
using DiabloDiscordBot.DiscordStuff.DatabaseStuff;
using DSharpPlus.SlashCommands;

namespace DiabloDiscordBot.DiscordStuff.SlashCommands {
	internal class SlashCommandsMisc : ApplicationCommandModule {
		[SlashCommand("Report", "Report that an alert gave false information")]
		public async Task Report(
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
