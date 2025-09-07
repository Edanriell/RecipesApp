using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using RecipesApp.Client.Core.Features.Recipes;
using RecipesApp.Client.Core.Messages;

namespace RecipesApp.Client.Core.ViewModels;

public class IngredientsListViewModel : ObservableObject
{
    private int _numberOfServings = 4;

    public IngredientsListViewModel(IReadOnlyList<RecipeIngredient> ingredients)
    {
        Ingredients = ingredients.Select(i =>
                new RecipeIngredientViewModel(
                    i.IngredientName,
                    i.BaseAmount,
                    i.Measurement,
                    i.BaseServings))
            .ToList();
    }

    public int NumberOfServings
    {
        get => _numberOfServings;
        set
        {
            if (SetProperty(ref _numberOfServings, value))
                WeakReferenceMessenger.Default.Send(
                    new ServingsChangedMessage(value));
        }
    }

    public List<RecipeIngredientViewModel> Ingredients { get; }
}