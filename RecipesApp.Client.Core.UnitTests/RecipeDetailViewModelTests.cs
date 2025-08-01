using AutoBogus;
using Moq;
using RecipesApp.Client.Core.Features.Favorites;
using RecipesApp.Client.Core.Features.Ratings;
using RecipesApp.Client.Core.Features.Recipes;
using RecipesApp.Client.Core.Navigation;
using RecipesApp.Client.Core.Services;
using RecipesApp.Client.Core.ViewModels;

namespace RecipesApp.Client.Core.UnitTests;

public class RecipeDetailViewModelTests
{
    private readonly Mock<IDialogService> _dialogServiceMock;
    private readonly Mock<IFavoritesService> _favoritesServiceMock;
    private readonly Mock<INavigationService> _navigationServiceMock;
    private readonly Mock<IRatingsService> _ratingsServiceMock;
    private readonly Mock<IRecipeService> _recipeServiceMock;

    private readonly RecipeDetailViewModel sut;

    public RecipeDetailViewModelTests()
    {
        _recipeServiceMock = new Mock<IRecipeService>();
        _favoritesServiceMock = new Mock<IFavoritesService>();
        _ratingsServiceMock = new Mock<IRatingsService>();
        _navigationServiceMock = new Mock<INavigationService>();
        _dialogServiceMock = new Mock<IDialogService>();

        _ratingsServiceMock
            .Setup(m => m.LoadRatingsSummary(It.IsAny<string>()))
            .ReturnsAsync(Result<RatingsSummary>
                .Success(AutoFaker.Generate<RatingsSummary>()));

        sut =
            new RecipeDetailViewModel(_recipeServiceMock.Object,
                _favoritesServiceMock.Object, _ratingsServiceMock.Object,
                _navigationServiceMock.Object, _dialogServiceMock.Object);
    }

    [Fact]
    public async Task OnNavigatedTo_Should_Load_Recipe()
    {
        //Arrange
        var recipeId = AutoFaker.Generate<string>();

        var parameters = new Dictionary<string, object>
        {
            { "id", recipeId }
        };

        _recipeServiceMock
            .Setup(m => m.LoadRecipe(It.IsAny<string>()))
            .ReturnsAsync(Result<RecipeDetail>
                .Success(AutoFaker.Generate<RecipeDetail>()));

        //Act
        await sut.OnNavigatedTo(parameters);

        //Assert
        _recipeServiceMock.Verify(m => m.LoadRecipe(recipeId), Times.Once);
    }

    [Fact]
    public async Task OnNavigatedTo_Should_Map_RecipeDetail()
    {
        //Arrange
        var recipeDetail = AutoFaker.Generate<RecipeDetail>();

        var parameters = new Dictionary<string, object>
        {
            { "id", AutoFaker.Generate<string>() }
        };

        _recipeServiceMock
            .Setup(m => m.LoadRecipe(It.IsAny<string>()))
            .ReturnsAsync(Result<RecipeDetail>
                .Success(recipeDetail));

        //Act
        await sut.OnNavigatedTo(parameters);

        //Assert
        Assert.Equal(recipeDetail.Name, sut.Title);
        Assert.Equal(recipeDetail.Author, sut.Author);
    }

    [Fact]
    public async Task FailedLoad_Should_ShowDialog()
    {
        //Arrange
        var parameters = new Dictionary<string, object>
        {
            { "id", AutoFaker.Generate<string>() }
        };

        _recipeServiceMock
            .Setup(m => m.LoadRecipe(It.IsAny<string>()))
            .ReturnsAsync(Result<RecipeDetail>
                .Fail(AutoFaker.Generate<string>()));

        _dialogServiceMock
            .Setup(m => m.AskYesNo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        //Act
        await sut.OnNavigatedTo(parameters);

        //Assert
        _dialogServiceMock.Verify(
            m => m.AskYesNo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
        _navigationServiceMock.Verify(m => m.GoBack(), Times.Once);
    }
}