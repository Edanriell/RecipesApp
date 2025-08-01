using RecipesApp.Client.Core;
using RecipesApp.Shared.Dto;
using Refit;

namespace RecipesApp.Client.Repositories.Api;

public interface IFavoritesApi
{
    [Get("/users/{userId}/favorites")]
    Task<ApiResponse<string[]>> GetFavorites(string userId);

    [Post("/users/{userId}/favorites")]
    Task<ApiResponse<Nothing>> AddFavorite(string userId, FavoriteDto favorite);

    [Delete("/users/{userId}/favorites/{recipeId}")]
    Task<ApiResponse<Nothing>> DeleteFavorite(string userId, string recipeId);
}