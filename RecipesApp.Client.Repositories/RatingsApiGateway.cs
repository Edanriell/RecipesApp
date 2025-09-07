using RecipesApp.Client.Core;
using RecipesApp.Client.Core.Features.Ratings;
using RecipesApp.Client.Repositories.Api;
using static RecipesApp.Client.Repositories.Mappers.RatingsMapper;

namespace RecipesApp.Client.Repositories;

internal class RatingsApiGateway : ApiGateway, IRatingsRepository
{
    private readonly IRatingsApi _api;

    public RatingsApiGateway(IRatingsApi api) { _api = api; }

    public Task<Result<IReadOnlyCollection<Rating>>>
        GetRatings(string recipeId)
    {
        return InvokeAndMap(
            _api.GetRatings(recipeId), MapRatings);
    }

    public Task<Result<RatingsSummary>>
        GetRatingsSummary(string recipeId)
    {
        return InvokeAndMap(_api.GetRatingsSummary(recipeId),
            MapRatingSummary);
    }
}