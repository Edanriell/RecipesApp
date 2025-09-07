using RecipesApp.Client.Core.ViewModels;

namespace RecipesApp.Mobile;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}