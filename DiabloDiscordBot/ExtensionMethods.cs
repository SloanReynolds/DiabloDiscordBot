using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace DiabloDiscordBot {
	internal static class ExtensionMethods {
		private const string _ALPHA_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		internal static char NextAlphaCharacter(this Random random) {
			return _ALPHA_CHARS[random.Next(_ALPHA_CHARS.Length)];
		}

		internal static string NextAlphaString(this Random random, int length) {
			var stringChars = new char[length];
			for (int i = 0; i < length; i++) {
				stringChars[i] = random.NextAlphaCharacter();
			}
			return new string(stringChars);
		}

		internal static string NextAlphaString(this Random random, int lengthMin, int lengthMax) {
			int wordLength = random.Next(lengthMin, lengthMax);
			return random.NextAlphaString(wordLength);
		}

		internal static string NextSentence(this Random random, int wordsCount = 10, int wordLengthMin = 3, int wordLengthMax = 15) {
			var words = new string[wordsCount];
			for (int i = 0; i < wordsCount; i++) {
				words[i] = random.NextAlphaString(wordLengthMin, wordLengthMax);
			}
			return string.Join(' ', words);
		}

		internal async static Task FollowUpAsync(this InteractionContext ctx, string message, bool ephemeral = false) {
			var followUp = new DiscordFollowupMessageBuilder().AsEphemeral(ephemeral);
			await ctx.FollowUpAsync(followUp.WithContent(message));
		}
	}
}
