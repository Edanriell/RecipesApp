using CommunityToolkit.Mvvm.Messaging;
using RecipesApp.Client.Core.Messages;

namespace RecipesApp.Client.Core.Features.Favorites;

public class FavoritesService : IFavoritesService
{
    private readonly IFavoritesRepository _favoritesRepository;
    private List<string> favorites;

    public FavoritesService(IFavoritesRepository favoritesRepository) { _favoritesRepository = favoritesRepository; }

    public async Task<Result<Nothing>> Add(string id)
    {
        var result = await _favoritesRepository
            .Add(GetCurrentUserId(), id);

        if (result.IsSuccess)
        {
            if (favorites is not null
                && !favorites.Contains(id))
                favorites.Add(id);

            WeakReferenceMessenger.Default
                .Send(
                    new FavoriteUpdateMessage(id, true));
        }

        return result;
    }

    public async Task<Result<Nothing>> Remove(string id)
    {
        var result = await _favoritesRepository
            .Remove(GetCurrentUserId(), id);

        if (result.IsSuccess)
        {
            if (favorites is not null
                && favorites.Contains(id))
                favorites.Remove(id);

            WeakReferenceMessenger.Default
                .Send(
                    new FavoriteUpdateMessage(id, false));
        }

        return result;
    }

    public async Task<bool> IsFavorite(string id)
    {
        await LoadList();
        return favorites is not null && favorites.Contains(id);
    }

    public async Task<IReadOnlyCollection<string>?> LoadFavorites()
    {
        await LoadList();
        return favorites?.AsReadOnly();
    }

    private async ValueTask LoadList()
    {
        if (favorites is null)
        {
            var loadResult = await _favoritesRepository.LoadFavorites(GetCurrentUserId());
            if (loadResult.IsSuccess) favorites = loadResult.Data.ToList();
        }
    }

    private string GetCurrentUserId()
    {
        return "3";
        //Dummy implementation, could be retrieved via injected 
    }
}