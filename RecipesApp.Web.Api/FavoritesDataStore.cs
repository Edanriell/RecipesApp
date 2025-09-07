using RecipesApp.Shared.Dto;

internal static class FavoritesDataStore
{
    private static readonly List<string> favorites = new();

    public static string[] GetFavorites(string userId) { return favorites.ToArray(); }

    public static void StoreFavorite(string userId, FavoriteDto favorite)
    {
        if (favorites.Contains(favorite.RecipeId))
            return;
        favorites.Add(favorite.RecipeId);
    }

    public static void DeleteFavorite(string userId, string recipeId)
    {
        if (favorites.Contains(recipeId))
            favorites.Remove(recipeId);
    }
}