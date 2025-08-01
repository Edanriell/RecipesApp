namespace RecipesApp.Client.Core.Features.Ratings;

public class RatingsService : IRatingsService
{
    private readonly IRatingsRepository _ratingsRepository;

    public RatingsService(IRatingsRepository ratingsRepository) { _ratingsRepository = ratingsRepository; }

    public Task<Result<RatingsSummary>> LoadRatingsSummary(string recipeId)
    {
        return _ratingsRepository.GetRatingsSummary(recipeId);
    }

    public Task<Result<IReadOnlyCollection<Rating>>> LoadRatings(string recipeId)
    {
        return _ratingsRepository.GetRatings(recipeId);
    }
}