namespace GroupProject421_521.Models
{
    public class WineVM
    {
        public string Name { get; set; } 
        public string Type { get; set; } 
        public decimal Price { get; set; } 
        public List<UserPromptVM> RelatedPrompts { get; set; }
    }
}
