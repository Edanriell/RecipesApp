using Microsoft.Extensions.DependencyInjection;
using RecipesApp.Client.Core.Features.Favorites;
using RecipesApp.Client.Core.Features.Ratings;
using RecipesApp.Client.Core.Features.Recipes;
using RecipesApp.Client.Repositories.Api;
using Refit;

namespace RecipesApp.Client.Repositories;

public static class ServiceCollectionExtension
{
    public static IServiceCollection
        RegisterRepositories(
            this IServiceCollection services,
            RepositorySettings settings)
    {
        services.AddSingleton(s => RestService.For<IRatingsApi>(settings.HttpClient));
        services.AddSingleton(s => RestService.For<IRecipeApi>(settings.HttpClient));
        services.AddSingleton(s => RestService.For<IFavoritesApi>(settings.HttpClient));

        services.AddTransient<IRatingsRepository, RatingsApiGateway>();
        services.AddTransient<IRecipeRepository, RecipeApiGateway>();
        services.AddTransient<IFavoritesRepository, FavoritesApiGateway>();

        return services;
    }
}