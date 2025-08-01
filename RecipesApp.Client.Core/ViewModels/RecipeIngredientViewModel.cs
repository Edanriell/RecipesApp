using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using RecipesApp.Client.Core.Messages;

namespace RecipesApp.Client.Core.ViewModels;

public class RecipeIngredientViewModel : ObservableObject
{
    private readonly double baseAmount;
    private readonly int baseServings;

    private double? _displayAmount;


    public RecipeIngredientViewModel(
        string ingredientName,
        double baseAmount, string? measurement = null, int baseServings = 4)
    {
        IngredientName = ingredientName;
        Measurement = measurement;
        this.baseAmount = baseAmount;
        this.baseServings = baseServings;

        WeakReferenceMessenger.Default
            .Register<ServingsChangedMessage>(this, (r, m) =>
                ((RecipeIngredientViewModel)r)
                .UpdateServings(m.Value));
    }

    public string IngredientName { get; }

    public string? Measurement { get; }

    public double DisplayAmount { get => _displayAmount ?? baseAmount; set => SetProperty(ref _displayAmount, value); }

    private void UpdateServings(int servings)
    {
        var factor = servings / (double)baseServings;
        DisplayAmount = factor * baseAmount;
    }
}