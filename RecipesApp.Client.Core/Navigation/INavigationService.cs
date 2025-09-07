using RecipesApp.Client.Core.Features.Recipes;

namespace RecipesApp.Client.Core.Navigation;

public interface INavigationService
{
    Task GoToOverview();
    Task GoToRecipeDetail(string recipeId);
    Task GoToRecipeRatingDetail(RecipeDetail recipe);
    Task GoBack();
    Task GoBackAndReturn(Dictionary<string, object> parameters);
    Task GoToChooseLanguage(string currentLanguage);
    Task GoToAddRating(RecipeDetail recipe);
}