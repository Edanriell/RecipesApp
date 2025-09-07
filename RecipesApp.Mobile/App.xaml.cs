using RecipesApp.Localization;
using RecipesApp.Mobile.Navigation;

namespace RecipesApp.Mobile;

public partial class App : Application
{
    public App(
        INavigationInterceptor interceptor,
        ILocalizationManager manager)
    {
        manager.RestorePreviousCulture();

        Current.UserAppTheme = AppTheme.Light;
        InitializeComponent();

        MainPage = new AppShell(interceptor);
    }
}