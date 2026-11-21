namespace GroupProject421_521.Models
{
    /// <summary>
    /// A wine in the catalog.
    /// </summary>
    public class Wine
    {
        public int WineID { get; set; }

        public string Name { get; set; } = string.Empty;

        /// <summary>Red, white, rosé, sparkling, etc.</summary>
        public string? Type { get; set; }

        public string? Region { get; set; }

        public string? Description { get; set; }

        /// <summary>Tasting notes, e.g. "dark cherry, oak, soft tannins".</summary>
        public string? FlavorProfile { get; set; }

        public decimal Price { get; set; }
    }
}
