using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using RecipesApp.Client.Core.Features.Favorites;
using RecipesApp.Client.Core.Features.Recipes;
using RecipesApp.Client.Core.Messages;
using RecipesApp.Client.Core.Navigation;

namespace RecipesApp.Client.Core.ViewModels;

public class RecipesOverviewViewModel : ObservableObject, INavigatedTo, INavigatedFrom
{
    private readonly IFavoritesService favoritesService;
    private readonly INavigationService navigationService;
    private readonly IRecipeService recipeService;

    private bool _loadFailed;

    private AsyncRelayCommand _reloadCommand;

    private RecipeListItemViewModel? _selectedRecipe;

    private int _totalNumberOfRecipes;

    public RecipesOverviewViewModel(
        IRecipeService recipeService,
        IFavoritesService favoritesService, INavigationService navigationService)
    {
        this.recipeService = recipeService;
        this.favoritesService = favoritesService;
        this.navigationService = navigationService;

        Recipes = new ObservableCollection<RecipeListItemViewModel>();
        TryLoadMoreItemsCommand =
            new AsyncRelayCommand(TryLoadMoreItems);
        NavigateToSelectedDetailCommand =
            new AsyncRelayCommand(NavigateToSelectedDetail);

        WeakReferenceMessenger.Default
            .Register<CultureChangedMessage>(this, (r, m) =>
            {
                Recipes.Clear();
                (r as RecipesOverviewViewModel).LoadRecipes(7, 0);
            });


        LoadRecipes(7, 0);
    }

    public ObservableCollection<RecipeListItemViewModel> Recipes { get; }

    public RecipeListItemViewModel? SelectedRecipe
    {
        get => _selectedRecipe;
        set => SetProperty(ref _selectedRecipe, value);
    }

    public int TotalNumberOfRecipes
    {
        get => _totalNumberOfRecipes;
        set => SetProperty(ref _totalNumberOfRecipes, value);
    }

    public bool LoadFailed { get => _loadFailed; set => SetProperty(ref _loadFailed, value); }

    public AsyncRelayCommand TryLoadMoreItemsCommand { get; }
    public AsyncRelayCommand NavigateToSelectedDetailCommand { get; }
    public AsyncRelayCommand ReloadCommand { get => _reloadCommand; set => SetProperty(ref _reloadCommand, value); }

    public Task OnNavigatedFrom(NavigationType navigationType) { return Task.CompletedTask; }


    public Task OnNavigatedTo(NavigationType navigationType) { return Task.CompletedTask; }

    private async Task LoadRecipes(int pageSize, int page)
    {
        LoadFailed = false;

        var loadRecipesTask = recipeService.LoadRecipes(pageSize, page);
        var loadFavoritesTask = favoritesService.LoadFavorites();

        await Task.WhenAll(loadRecipesTask, loadFavoritesTask);

        var recipesResult = loadRecipesTask.Result;
        var favoritesResult = loadFavoritesTask.Result;

        if (recipesResult.IsSuccess)
        {
            TotalNumberOfRecipes = recipesResult.Data.TotalItems;
            recipesResult.Data.Recipes.ToList().ForEach(recipe =>
            {
                var isFavorite = favoritesResult?.Contains(recipe.Id) ?? false;
                Recipes.Add(new RecipeListItemViewModel(recipe.Id, recipe.Title, isFavorite, recipe.Image));
            });
        }
        else
        {
            LoadFailed = true;
            ReloadCommand = new AsyncRelayCommand(() => LoadRecipes(pageSize, page));
        }
    }

    private async Task NavigateToSelectedDetail()
    {
        if (SelectedRecipe is not null)
        {
            await navigationService.GoToRecipeDetail(SelectedRecipe.Id);
            SelectedRecipe = null;
        }
    }

    private async Task TryLoadMoreItems() { await LoadRecipes(7, Recipes.Count / 7); }
}