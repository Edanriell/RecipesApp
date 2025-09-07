using RecipesApp.Client.Core;
using RecipesApp.Client.Core.Features.Favorites;
using RecipesApp.Client.Repositories.Api;
using RecipesApp.Shared.Dto;

namespace RecipesApp.Client.Repositories;

internal class FavoritesApiGateway : ApiGateway, IFavoritesRepository
{
    private readonly IFavoritesApi _api;

    public FavoritesApiGateway(IFavoritesApi api) { _api = api; }

    public Task<Result<Nothing>> Add(string userId, string id)
    {
        return InvokeAndMap(_api.AddFavorite(userId, new FavoriteDto(id)));
    }

    public Task<Result<string[]>> LoadFavorites(string userId) { return InvokeAndMap(_api.GetFavorites(userId)); }

    public Task<Result<Nothing>> Remove(string userId, string recipeId)
    {
        return InvokeAndMap(_api.DeleteFavorite(userId, recipeId));
    }
}