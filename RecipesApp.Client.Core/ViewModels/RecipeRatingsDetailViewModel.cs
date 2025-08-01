using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RecipesApp.Client.Core.Features.Ratings;
using RecipesApp.Client.Core.Features.Recipes;
using RecipesApp.Client.Core.Navigation;
using RecipesApp.Client.Core.Services;

namespace RecipesApp.Client.Core.ViewModels;

public class
    RecipeRatingsDetailViewModel
    : ObservableObject, INavigationParameterReceiver, INavigatedTo,
        INavigatedFrom //, IOnNavigatingFromAware, IOnNavigatingToAware, IOnNavigatedToAware
{
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;
    private readonly IRatingsService ratingsService;

    private List<RatingGroup> _groupedReviews = new();

    private string _recipeTitle = string.Empty;

    public RecipeRatingsDetailViewModel(
        INavigationService navigationService,
        IRatingsService ratingsService,
        IDialogService dialogService)
    {
        this.ratingsService = ratingsService;
        this.dialogService = dialogService;
        this.navigationService = navigationService;

        ReportReviewsCommand = new RelayCommand(ReportReviews, () => SelectedReviews.Any());
        GoBackCommand = new RelayCommand(() => navigationService.GoBack());

        SelectedReviews.CollectionChanged += SelectedReviews_CollectionChanged;
    }

    public string RecipeTitle { get => _recipeTitle; set => SetProperty(ref _recipeTitle, value); }

    public List<RatingGroup> GroupedReviews
    {
        get => _groupedReviews;
        private set => SetProperty(ref _groupedReviews, value);
    }

    public ObservableCollection<object> SelectedReviews { get; } = new();

    public RelayCommand ReportReviewsCommand { get; }
    public RelayCommand GoBackCommand { get; }

    public Task OnNavigatedFrom(NavigationType navigationType) { return Task.CompletedTask; }

    public Task OnNavigatedTo(NavigationType navigationType) { return Task.CompletedTask; }

    public Task OnNavigatedTo(Dictionary<string, object> parameters)
    {
        return LoadData(parameters["recipe"]
            as RecipeDetail);
    }

    private async Task LoadData(RecipeDetail recipe)
    {
        RecipeTitle = recipe.Name;
        var loadRatings = await ratingsService.LoadRatings(recipe.Id);

        if (loadRatings is { IsSuccess: true, Data: var ratings })
        {
            GroupedReviews = ratings
                .Select(r => new UserReviewViewModel(r.UserName, r.Score, r.Review))
                .GroupBy(r => Math.Round(r.Rating / .5) * .5)
                .OrderByDescending(g => g.Key)
                .Select(g => new RatingGroup(g.Key.ToString(), g.ToList()))
                .ToList();
        }
        else
        {
            var shouldRetry = await dialogService.AskYesNo("Failed to load", "Retry?");
            if (shouldRetry)
                await LoadData(recipe);
            else
                await navigationService.GoBack();
        }
    }

    private void SelectedReviews_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        ReportReviewsCommand.NotifyCanExecuteChanged();
    }

    private void ReportReviews()
    {
        var selectedReviews = SelectedReviews
            .Cast<UserReviewViewModel>().ToList();
        //do reporting
        SelectedReviews.Clear();
    }
}