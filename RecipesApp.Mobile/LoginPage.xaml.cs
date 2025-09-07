using CommunityToolkit.Mvvm.Input;
using RecipesApp.Client.Core.Navigation;

namespace RecipesApp.Mobile;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

public class LoginPageViewModel
{
    private readonly INavigationService navigationService;

    public LoginPageViewModel(INavigationService navigation)
    {
        navigationService = navigation;
        //LoginCommand = new RelayCommand(() => navigationService.LoadApp());
    }

    public RelayCommand LoginCommand { get; }
}