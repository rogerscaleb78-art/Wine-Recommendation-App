namespace GroupProject421_521.Services
{
    /// <summary>
    /// Rule-based wine recommendation engine. Scans the user's text for keywords
    /// about mood, weather, food and occasion, and builds a recommendation from
    /// every category that matches.
    /// </summary>
    public static class WineRecommender
    {
        public const string DefaultRecommendation =
            "🍷 **Pinot Noir** - A versatile, crowd-pleasing wine that works well in most situations. It's smooth, approachable, and pairs with many foods.";

        public const string ServingTip =
            "\n💡 **Pro Tip**: Serve red wines slightly below room temperature (60-65°F) and white wines chilled (45-50°F) for the best experience.";

        public static string Recommend(string? userInput)
        {
            var input = (userInput ?? string.Empty).ToLowerInvariant();
            var recommendations = new List<string>();

            // Mood-based recommendations
            if (ContainsAny(input, "celebrat", "party", "happy", "excit", "cheer"))
            {
                recommendations.Add("🍾 **Champagne or Prosecco** - Perfect for celebrations! The bubbles and crisp acidity will enhance your joyful mood. Try a Brut Champagne or Italian Prosecco.");
            }
            else if (ContainsAny(input, "relax", "unwind", "stress", "tired", "calm"))
            {
                recommendations.Add("🍷 **Pinot Noir** - A gentle, smooth red wine perfect for relaxation. Its soft tannins and cherry notes will help you unwind.");
            }
            else if (ContainsAny(input, "romantic", "date", "love", "intimate"))
            {
                recommendations.Add("🌹 **Rosé Wine** - Elegant and romantic, a dry rosé from Provence or a sparkling rosé would be perfect for your special moment.");
            }

            // Weather-based recommendations
            if (ContainsAny(input, "hot", "summer", "warm", "sunny", "beach"))
            {
                recommendations.Add("🥂 **Sauvignon Blanc or Pinot Grigio** - Crisp, refreshing white wines perfect for warm weather. They'll cool you down beautifully.");
            }
            else if (ContainsAny(input, "cold", "winter", "snow", "cozy", "fireplace"))
            {
                recommendations.Add("🔥 **Cabernet Sauvignon or Malbec** - Rich, full-bodied reds that will warm you up on cold days. Perfect with hearty meals.");
            }

            // Food pairing recommendations
            if (ContainsAny(input, "steak", "beef", "meat", "bbq", "grill"))
            {
                recommendations.Add("🥩 **Cabernet Sauvignon or Syrah** - Bold reds that pair perfectly with red meat. The tannins will complement the richness of your meal.");
            }
            else if (ContainsAny(input, "fish", "seafood", "salmon", "shrimp", "lobster"))
            {
                recommendations.Add("🐟 **Chardonnay or Pinot Grigio** - Light, crisp whites that won't overpower delicate seafood flavors.");
            }
            else if (ContainsAny(input, "pasta", "pizza", "italian", "tomato"))
            {
                recommendations.Add("🍝 **Chianti or Sangiovese** - Italian reds that are perfect with tomato-based dishes and pasta.");
            }
            else if (ContainsAny(input, "cheese", "appetizer", "snack"))
            {
                recommendations.Add("🧀 **Riesling or Pinot Noir** - Versatile wines that pair beautifully with various cheeses and appetizers.");
            }

            // Occasion-based recommendations
            if (ContainsAny(input, "dinner", "meal", "cooking"))
            {
                recommendations.Add("🍽️ **Consider the main dish** - For red meat, choose a bold red. For fish or chicken, a crisp white or light red works well.");
            }
            else if (ContainsAny(input, "dessert", "sweet", "chocolate"))
            {
                recommendations.Add("🍰 **Port or Moscato** - Sweet wines that complement desserts beautifully. A Ruby Port with chocolate is divine!");
            }

            // Default recommendation if no specific context found
            if (recommendations.Count == 0)
            {
                recommendations.Add(DefaultRecommendation);
            }

            recommendations.Add(ServingTip);

            return string.Join("\n\n", recommendations);
        }

        private static bool ContainsAny(string input, params string[] keywords)
        {
            return keywords.Any(keyword => input.Contains(keyword));
        }
    }
}
