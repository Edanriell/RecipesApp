using RecipesApp.Client.Core.ViewModels;

namespace RecipesApp.Mobile;

public partial class AddRatingPage : ContentPage
{
    public AddRatingPage(AddRatingViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}