namespace DiabloBotShared {
	public struct EventDetails {
		public AlertType AlertType { get; }
		public int MinutesUntilNext { get; }
		public string Message { get; }
		public int AlertMinutes { get; }

		public EventDetails(AlertType alertType, int minutesUntilNext, string message, int alertMinutes) {
			this.AlertType = alertType;
			this.MinutesUntilNext = minutesUntilNext;
			this.Message = message;
			this.AlertMinutes = alertMinutes;
		}
	}
}
