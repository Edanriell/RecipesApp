using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RecipesApp.Client.Core.Features.Favorites;
using RecipesApp.Client.Core.Features.Ratings;
using RecipesApp.Client.Core.Features.Recipes;
using RecipesApp.Client.Core.Navigation;
using RecipesApp.Client.Core.Services;

namespace RecipesApp.Client.Core.ViewModels;

public class RecipeDetailViewModel : ObservableObject, INavigationParameterReceiver, INavigatedTo, INavigatedFrom
{
    private readonly IDialogService dialogService;
    private readonly IFavoritesService favoritesService;
    private readonly int maxUpdatedAllowed = 5;
    private readonly INavigationService navigationService;
    private readonly IRatingsService ratingsService;
    private readonly IRecipeService recipeService;

    private string[] _allergens = new string[0];

    private string _author;

    private int? _calories;

    private bool _hideAllergenInformation = true;

    private string _image;

    private IngredientsListViewModel _ingredientsList;

    private List<InstructionBaseViewModel> _instructions;

    private bool _isFavorite;

    private bool _isLoading = true;

    private DateTime _lastUpdated;

    private RecipeRatingsSummaryViewModel _ratingSummary;

    private int? _readyInMinutes;

    private string _title;

    private RecipeDetail recipeDto;

    private int updateCount;

    public RecipeDetailViewModel(
        IRecipeService recipeService,
        IFavoritesService favoritesService,
        IRatingsService ratingsService, INavigationService navigationService,
        IDialogService dialogService)
    {
        this.recipeService = recipeService;
        this.favoritesService = favoritesService;
        this.ratingsService = ratingsService;
        this.navigationService = navigationService;
        this.dialogService = dialogService;

        AddAsFavoriteCommand =
            new AsyncRelayCommand(AddAsFavorite, CanAddAsFavorite);
        RemoveAsFavoriteCommand =
            new AsyncRelayCommand(RemoveAsFavorite, CanRemoveAsFavorite);

        FavoriteToggledCommand = new AsyncRelayCommand<bool>(
            FavoriteToggled,
            e => updateCount < maxUpdatedAllowed);

        UserIsBrowsingCommand = new RelayCommand(UserIsBrowsing);
        AddToShoppingListCommand = new RelayCommand<RecipeIngredientViewModel>(AddToShoppingList);
        RemoveFromShoppingListCommand = new RelayCommand<RecipeIngredientViewModel>(RemoveFromShoppingList);
        NavigateToRatingsCommand = new AsyncRelayCommand(NavigateToRatings);
        NavigateToAddRatingCommand = new AsyncRelayCommand(NavigateToAddRating);
    }

    public string Title { get => _title; set => SetProperty(ref _title, value); }

    public string[] Allergens { get => _allergens; set => SetProperty(ref _allergens, value); }

    public int? Calories { get => _calories; set => SetProperty(ref _calories, value); }

    public int? ReadyInMinutes { get => _readyInMinutes; set => SetProperty(ref _readyInMinutes, value); }

    public DateTime LastUpdated { get => _lastUpdated; set => SetProperty(ref _lastUpdated, value); }

    public string Author { get => _author; set => SetProperty(ref _author, value); }

    public string Image { get => _image; set => SetProperty(ref _image, value); }

    public RecipeRatingsSummaryViewModel RatingSummary
    {
        get => _ratingSummary;
        set => SetProperty(ref _ratingSummary, value);
    }

    public List<InstructionBaseViewModel> Instructions
    {
        get => _instructions;
        set => SetProperty(ref _instructions, value);
    }

    public IngredientsListViewModel IngredientsList
    {
        get => _ingredientsList;
        set => SetProperty(ref _ingredientsList, value);
    }

    public bool HideAllergenInformation
    {
        get => _hideAllergenInformation;
        set => SetProperty(ref _hideAllergenInformation, value);
    }

    public bool IsFavorite
    {
        get => _isFavorite;
        set
        {
            if (SetProperty(ref _isFavorite, value))
            {
                AddAsFavoriteCommand.NotifyCanExecuteChanged();
                RemoveAsFavoriteCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }


    public ObservableCollection<RecipeIngredientViewModel> ShoppingList { get; } = new();

    public IRelayCommand AddAsFavoriteCommand { get; }
    public IRelayCommand RemoveAsFavoriteCommand { get; }
    public IRelayCommand FavoriteToggledCommand { get; }
    public IRelayCommand AddToShoppingListCommand { get; }
    public IRelayCommand RemoveFromShoppingListCommand { get; }
    public IRelayCommand UserIsBrowsingCommand { get; }
    public IAsyncRelayCommand NavigateToRatingsCommand { get; }
    public IAsyncRelayCommand NavigateToAddRatingCommand { get; }

    public Task OnNavigatedFrom(NavigationType navigationType) { return Task.CompletedTask; }


    public Task OnNavigatedTo(NavigationType navigationType) { return Task.CompletedTask; }

    public Task OnNavigatedTo(Dictionary<string, object> parameters) { return LoadRecipe(parameters["id"].ToString()); }

    private async Task LoadRecipe(string recipeId)
    {
        IsLoading = true;

        var loadRecipeTask = recipeService.LoadRecipe(recipeId);
        var loadIsFavoriteTask = favoritesService.IsFavorite(recipeId);
        var loadRatingsTask = ratingsService.LoadRatingsSummary(recipeId);

        await Task.WhenAll(loadRecipeTask, loadIsFavoriteTask, loadRatingsTask);

        if (!loadRecipeTask.Result.IsSuccess || !loadRatingsTask.Result.IsSuccess)
        {
            var result = await dialogService.AskYesNo("Unable to load recipe", "Want to retry?");
            if (result)
                await LoadRecipe(recipeId);
            else
                await navigationService.GoBack();
        }
        else { MapRecipeData(loadRecipeTask.Result.Data, loadRatingsTask.Result.Data, loadIsFavoriteTask.Result); }

        IsLoading = false;
    }

    private void MapRecipeData(RecipeDetail recipe, RatingsSummary ratings, bool isFavorite)
    {
        recipeDto = recipe;
        Title = recipe.Name;
        Allergens = recipe.Allergens;
        Calories = recipe.Calories;
        ReadyInMinutes = recipe.ReadyInMinutes;
        LastUpdated = recipe.LastUpdated;
        Author = recipe.Author;
        Image = recipe.Image ?? "fallback.png";

        var instructionVMs = new List<InstructionBaseViewModel>();
        foreach (var item in recipe.Instructions)
            if (item.IsNote)
                instructionVMs.Add(new NoteViewModel(item.Text));
            else
                instructionVMs.Add(new InstructionViewModel(item.Index ?? 0, item.Text));

        Instructions = instructionVMs;

        IngredientsList = new IngredientsListViewModel(recipe.Ingredients);

        IsFavorite = isFavorite;

        RatingSummary =
            new RecipeRatingsSummaryViewModel(ratings.TotalReviews, ratings.AverageRating, ratings.MaxRating);
    }

    private bool CanAddAsFavorite() { return !IsFavorite; }

    private Task AddAsFavorite() { return UpdateIsFavorite(true); }

    private Task RemoveAsFavorite() { return UpdateIsFavorite(false); }

    private Task UpdateIsFavorite(bool newValue)
    {
        IsFavorite = newValue;
        return FavoriteToggled(newValue);
    }

    private async Task FavoriteToggled(bool isFavorite)
    {
        if (isFavorite)
            await favoritesService.Add(recipeDto.Id);
        else
            await favoritesService.Remove(recipeDto.Id);

        updateCount++;
        FavoriteToggledCommand.NotifyCanExecuteChanged();
    }

    private bool CanRemoveAsFavorite() { return IsFavorite; }

    private void UserIsBrowsing()
    {
        //Do Logging
    }

    private void AddToShoppingList(RecipeIngredientViewModel viewModel)
    {
        if (ShoppingList.Contains(viewModel))
            return;
        ShoppingList.Add(viewModel);
    }

    private void RemoveFromShoppingList(RecipeIngredientViewModel viewModel)
    {
        if (ShoppingList.Contains(viewModel))
            ShoppingList.Remove(viewModel);
    }

    private Task NavigateToRatings() { return navigationService.GoToRecipeRatingDetail(recipeDto); }


    private Task NavigateToAddRating() { return navigationService.GoToAddRating(recipeDto); }
}