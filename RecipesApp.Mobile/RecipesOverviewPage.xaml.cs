using RecipesApp.Client.Core.ViewModels;

namespace RecipesApp.Mobile;

public partial class RecipesOverviewPage : ContentPage
{
    public RecipesOverviewPage(
        RecipesOverviewViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}