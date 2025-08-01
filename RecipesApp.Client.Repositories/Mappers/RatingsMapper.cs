using RecipesApp.Client.Core.Features.Ratings;
using RecipesApp.Shared.Dto;

namespace RecipesApp.Client.Repositories.Mappers;

internal static class RatingsMapper
{
    internal static IReadOnlyCollection<Rating> MapRatings(RatingDto[] dtos)
    {
        return dtos.Select(r => new Rating(r.Id, r.RecipeId,
                r.Rating,
                r.UserName,
                r.Review))
            .ToArray();
    }

    internal static RatingsSummary MapRatingSummary(RatingsSummaryDto dto)
    {
        return new RatingsSummary(dto.TotalReviews, dto.MaxRating, dto.AverageRating);
    }
}