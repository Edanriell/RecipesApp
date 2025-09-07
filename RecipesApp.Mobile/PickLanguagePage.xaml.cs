using RecipesApp.Client.Core.ViewModels;

namespace RecipesApp.Mobile;

public partial class PickLanguagePage : ContentPage
{
    public PickLanguagePage(PickLanguageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}