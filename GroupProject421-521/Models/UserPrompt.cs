namespace GroupProject421_521.Models
{
    /// <summary>
    /// A recommendation request and the suggestion it produced, saved to the
    /// user's history.
    /// </summary>
    public class UserPrompt
    {
        public int Id { get; set; }

        /// <summary>Identity user ID, or "Anonymous" for signed-out visitors.</summary>
        public string UserId { get; set; } = string.Empty;

        public string PromptText { get; set; } = string.Empty;

        public string Preferences { get; set; } = string.Empty;

        public string WineSuggestion { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        /// <summary>Soft-delete flag: deleted prompts stay in the database but are hidden.</summary>
        public bool IsDeleted { get; set; }
    }
}
