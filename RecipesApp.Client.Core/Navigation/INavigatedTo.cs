namespace RecipesApp.Client.Core.Navigation;

public interface INavigatedTo
{
    Task OnNavigatedTo(NavigationType navigationType);
}