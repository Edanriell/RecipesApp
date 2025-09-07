using RecipesApp.Client.Core.ViewModels;

namespace RecipesApp.Mobile;

public partial class RecipeRatingDetailPage : ContentPage
{
    public RecipeRatingDetailPage(
        RecipeRatingsDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}