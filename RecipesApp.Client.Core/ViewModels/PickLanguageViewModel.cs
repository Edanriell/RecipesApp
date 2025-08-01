using CommunityToolkit.Mvvm.ComponentModel;
using RecipesApp.Client.Core.Navigation;

namespace RecipesApp.Client.Core.ViewModels;

public class PickLanguageViewModel
    : ObservableObject,
        INavigationParameterReceiver
{
    private readonly INavigationService _navigationService;

    private string _selectedLanguage;

    public PickLanguageViewModel(INavigationService navigationService) { _navigationService = navigationService; }

    public string SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            if (SetProperty(ref _selectedLanguage, value)) LanguagePicked();
        }
    }

    public List<string> Languages { get; set; } = new()
    {
        "en-US",
        "fr-FR"
    };

    public async Task OnNavigatedTo(Dictionary<string, object> parameters)
    {
        _selectedLanguage = parameters["language"] as string;
        OnPropertyChanged(nameof(SelectedLanguage));
    }

    private Task LanguagePicked()
    {
        return _navigationService.GoBackAndReturn(
            new Dictionary<string, object>
            {
                { "SelectedLanguage", SelectedLanguage }
            });
    }
}