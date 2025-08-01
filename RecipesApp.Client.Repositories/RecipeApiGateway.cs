using RecipesApp.Client.Core;
using RecipesApp.Client.Core.Features.Recipes;
using RecipesApp.Client.Repositories.Api;
using RecipesApp.Localization;
using static RecipesApp.Client.Repositories.Mappers.RecipeMapper;

namespace RecipesApp.Client.Repositories;

internal class RecipeApiGateway : ApiGateway, IRecipeRepository
{
    private readonly IRecipeApi _api;
    private readonly ILocalizationManager _localizationManager;

    public RecipeApiGateway(
        IRecipeApi api,
        ILocalizationManager localizationManager)
    {
        _api = api;
        _localizationManager = localizationManager;
    }

    public Task<Result<LoadRecipesResponse>> LoadRecipes(int pageSize, int page)
    {
        return InvokeAndMap(_api.GetRecipes(_localizationManager.GetUserCulture().Name, pageSize, page),
            MapRecipesOverview);
    }

    public Task<Result<RecipeDetail>> LoadRecipe(string id) { return InvokeAndMap(_api.GetRecipe(id), MapRecipe); }
}