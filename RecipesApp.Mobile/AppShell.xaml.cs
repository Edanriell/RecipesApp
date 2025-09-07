using RecipesApp.Client.Core.Navigation;
using RecipesApp.Mobile.Navigation;

namespace RecipesApp.Mobile;

public partial class AppShell : Shell
{
    private readonly INavigationInterceptor interceptor;

    public AppShell(INavigationInterceptor interceptor)
    {
        this.interceptor = interceptor;
        InitializeComponent();
    }

    protected override async void OnNavigating(ShellNavigatingEventArgs args)
    {
        base.OnNavigating(args);

        var token = args.GetDeferral();

        if (token is not null)
        {
            var canNavigate = await interceptor
                .CanNavigate(CurrentPage?.BindingContext, GetNavigationType(args.Source));

            if (canNavigate)
                token.Complete();
            else
                args.Cancel();
        }
    }

    protected override async void OnNavigated(ShellNavigatedEventArgs args)
    {
        var navigationType = GetNavigationType(args.Source);

        base.OnNavigated(args);

        await interceptor.OnNavigatedTo(
            CurrentPage?.BindingContext, navigationType);
    }

    private NavigationType GetNavigationType(ShellNavigationSource source)
    {
        return source switch
        {
            ShellNavigationSource.Push or
                ShellNavigationSource.Insert
                => NavigationType.Forward,
            ShellNavigationSource.Pop or
                ShellNavigationSource.PopToRoot or
                ShellNavigationSource.Remove
                => NavigationType.Back,
            ShellNavigationSource.ShellItemChanged or
                ShellNavigationSource.ShellSectionChanged or
                ShellNavigationSource.ShellContentChanged
                => NavigationType.SectionChange,
            _ => NavigationType.Unknown
        };
    }
}