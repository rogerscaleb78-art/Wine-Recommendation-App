namespace GroupProject421_521.Models
{
    public class UserPromptVM
    {
        public string PromptText { get; set; }
        public string Preferences { get; set; }
        public bool IsDeleted { get; set; } 
        public List<Wine> WineSuggestions { get; set; }
    }
}
