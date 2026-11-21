namespace GroupProject421_521.Models
{
    /// <summary>
    /// Form model for the "create prompt" page.
    /// </summary>
    public class UserPromptVM
    {
        public string PromptText { get; set; } = string.Empty;

        public string Preferences { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }

        public List<Wine> WineSuggestions { get; set; } = new();
    }
}
