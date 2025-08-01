using RecipesApp.Client.Core.ViewModels;

namespace RecipesApp.Mobile;

public partial class RecipeDetailPage : ContentPage
{
    public RecipeDetailPage(RecipeDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}