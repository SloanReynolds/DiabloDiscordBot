using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiabloDiscordBot.DiabloStuff {
	internal abstract class AbstractEvent {
		public abstract AlertType AlertType { get; }
	}
}
