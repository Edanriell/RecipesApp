using System.Text;
using System.Text.Json;
using RecipesApp.Shared.Dto;

namespace RecipesApp.Web.Api;

public class RatingsService
{
    public RatingsSummaryDto LoadRatingsSummary(string recipeId)
    {
        var ratings = ReadRatingsFromStream();

        var recipeRatings = LoadRatings(recipeId);
        return new RatingsSummaryDto(recipeRatings.Count(), 4,
            recipeRatings.Sum(r => r.Rating) / recipeRatings.Count());
    }

    public RatingDto[] LoadRatings(string recipeId)
    {
        var ratings = ReadRatingsFromStream();
        return ratings.Where(r => r.RecipeId == recipeId).ToArray();
    }

    private RatingDto[] ReadRatingsFromStream()
    {
        var json = string.Empty;
        using (var reader = new StreamReader("ratings.json", Encoding.UTF8)) { json = reader.ReadToEnd(); }

        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Deserialize<RatingDto[]>(json, serializeOptions) ?? new RatingDto[0];
    }
}