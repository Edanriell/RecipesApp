using RecipesApp.Client.Core.Features.Recipes;
using RecipesApp.Client.Core.Navigation;

namespace RecipesApp.Mobile.Navigation;

public class NavigationService : INavigationService, INavigationInterceptor
{
    private WeakReference<INavigatedFrom> previousFrom;

    public async Task OnNavigatedTo(
        object bindingContext,
        NavigationType navigationType)
    {
        if (previousFrom is not null && previousFrom
            .TryGetTarget(out var from))
            await from.OnNavigatedFrom(navigationType);

        if (bindingContext
            is INavigatedTo to)
            await to.OnNavigatedTo(navigationType);

        if (bindingContext is INavigatedFrom navigatedFrom)
            previousFrom = new WeakReference<INavigatedFrom>(navigatedFrom);
        else
            previousFrom = null;
    }

    public Task<bool> CanNavigate(object bindingContext, NavigationType type)
    {
        if (bindingContext is INavigatable navigatable)
            return navigatable.CanNavigateFrom(type);

        return Task.FromResult(true);
    }

    public Task GoToRecipeDetail(string recipeId)
    {
        return Navigate("RecipeDetail",
            new Dictionary<string, object> { { "id", recipeId } });
    }

    public Task GoToRecipeRatingDetail(RecipeDetail recipe)
    {
        return Navigate("RecipeRating",
            new Dictionary<string, object> { { "recipe", recipe } });
    }

    public Task GoToChooseLanguage(string currentLanguage)
    {
        return Navigate("PickLanguagePage",
            new Dictionary<string, object> { { "language", currentLanguage } });
    }

    public Task GoToAddRating(RecipeDetail recipe)
    {
        return Navigate("AddRating",
            new Dictionary<string, object> { { "recipe", recipe } });
    }

    public Task GoBack() { return Shell.Current.GoToAsync(".."); }

    public async Task GoBackAndReturn(Dictionary<string, object> parameters)
    {
        await GoBack();

        if (Shell.Current.CurrentPage.BindingContext
            is INavigationParameterReceiver receiver)
            await receiver.OnNavigatedTo(parameters);
    }

    public Task GoToOverview() { throw new NotImplementedException(); }

    private async Task Navigate(
        string pageName,
        Dictionary<string, object> parameters)
    {
        await Shell.Current.GoToAsync(pageName);

        if (Shell.Current.CurrentPage.BindingContext
            is INavigationParameterReceiver receiver)
            await receiver.OnNavigatedTo(parameters);
    }
}