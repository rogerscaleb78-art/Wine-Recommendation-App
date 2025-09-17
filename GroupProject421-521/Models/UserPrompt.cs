namespace GroupProject421_521.Models
{
    public class UserPrompt
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PromptText { get; set; }
        public string Preferences { get; set; }
        public string WineSuggestion { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        // public Users User { get; set; }
    }
}