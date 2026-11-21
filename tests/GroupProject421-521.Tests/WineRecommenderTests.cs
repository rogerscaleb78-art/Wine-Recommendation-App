using GroupProject421_521.Services;

namespace GroupProject421_521.Tests
{
    public class WineRecommenderTests
    {
        [Theory]
        [InlineData("We're celebrating a promotion tonight", "Champagne or Prosecco")]
        [InlineData("I need to relax after a long week", "Pinot Noir")]
        [InlineData("Planning a romantic date night", "Rosé Wine")]
        [InlineData("Hot summer afternoon on the patio", "Sauvignon Blanc or Pinot Grigio")]
        [InlineData("Cozy winter night by the fireplace", "Cabernet Sauvignon or Malbec")]
        [InlineData("Grilling steak this weekend", "Cabernet Sauvignon or Syrah")]
        [InlineData("Making salmon", "Chardonnay or Pinot Grigio")]
        [InlineData("Homemade pizza", "Chianti or Sangiovese")]
        [InlineData("A cheese board for guests", "Riesling or Pinot Noir")]
        [InlineData("Chocolate cake", "Port or Moscato")]
        public void Recommend_MatchesKeywordCategory(string input, string expectedWine)
        {
            var result = WineRecommender.Recommend(input);

            Assert.Contains(expectedWine, result);
        }

        [Fact]
        public void Recommend_IsCaseInsensitive()
        {
            var result = WineRecommender.Recommend("STEAK AND A PARTY");

            Assert.Contains("Champagne or Prosecco", result);
            Assert.Contains("Cabernet Sauvignon or Syrah", result);
        }

        [Fact]
        public void Recommend_CombinesMultipleCategories()
        {
            var result = WineRecommender.Recommend("A hot summer celebration with shrimp");

            Assert.Contains("Champagne or Prosecco", result);
            Assert.Contains("Sauvignon Blanc or Pinot Grigio", result);
            Assert.Contains("Chardonnay or Pinot Grigio", result);
        }

        [Fact]
        public void Recommend_OnlyFirstMatchPerCategoryIsUsed()
        {
            // "steak" and "salmon" are both food keywords; steak is checked first.
            var result = WineRecommender.Recommend("steak or salmon");

            Assert.Contains("Cabernet Sauvignon or Syrah", result);
            Assert.DoesNotContain("Chardonnay or Pinot Grigio", result);
        }

        [Theory]
        [InlineData("Something nice please")]
        [InlineData("")]
        [InlineData(null)]
        public void Recommend_FallsBackToDefault_WhenNoKeywordsMatch(string? input)
        {
            var result = WineRecommender.Recommend(input);

            Assert.StartsWith(WineRecommender.DefaultRecommendation, result);
        }

        [Theory]
        [InlineData("party")]
        [InlineData("nothing relevant")]
        public void Recommend_AlwaysEndsWithServingTip(string input)
        {
            var result = WineRecommender.Recommend(input);

            Assert.EndsWith(WineRecommender.ServingTip, result);
        }
    }
}
