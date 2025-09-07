using RecipesApp.Client.Core.Navigation;

namespace RecipesApp.Mobile.Navigation;

public interface INavigationInterceptor
{
    Task OnNavigatedTo(object bindingContext, NavigationType navigationType);

    Task<bool> CanNavigate(object bindingContext, NavigationType type);
}