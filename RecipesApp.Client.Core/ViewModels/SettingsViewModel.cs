using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using RecipesApp.Client.Core.Messages;
using RecipesApp.Client.Core.Navigation;
using RecipesApp.Client.Core.Services;
using RecipesApp.Localization;

namespace RecipesApp.Client.Core.ViewModels;

public class SettingsViewModel : ObservableObject, INavigationParameterReceiver
{
    private readonly IDialogService _dialogService;
    private readonly ILocalizationManager _localizationManager;
    private readonly INavigationService _navigationService;
    private readonly ILocalizedResourcesProvider _resources;

    private string currentLanguage;

    public SettingsViewModel(
        INavigationService service, IDialogService dialogService,
        ILocalizationManager localizationManager, ILocalizedResourcesProvider resources)
    {
        _dialogService = dialogService;
        _localizationManager = localizationManager;
        _resources = resources;
        _navigationService = service;

        SelectLanguageCommand = new AsyncRelayCommand(ChooseLanguage);

        currentLanguage = CultureInfo.CurrentCulture.Name;
    }

    public string CurrentLanguage { get => currentLanguage; set => SetProperty(ref currentLanguage, value); }

    public AsyncRelayCommand SelectLanguageCommand { get; }

    public Task OnNavigatedTo(Dictionary<string, object> parameters)
    {
        if (parameters is not null && parameters.ContainsKey("SelectedLanguage"))
            return LanguageUpdated(parameters["SelectedLanguage"] as string);
        return Task.CompletedTask;
    }

    private Task ChooseLanguage() { return _navigationService.GoToChooseLanguage(CurrentLanguage); }

    private async Task LanguageUpdated(string newLanguage)
    {
        var confirm = await ConfirmSwitchLanguage();

        if (confirm)
        {
            SwitchLanguage(newLanguage);
            await NotifySwitch();
        }
    }

    private Task<bool> ConfirmSwitchLanguage()
    {
        return _dialogService.AskYesNo(
            _resources["SwitchLanguageDialogTitle"],
            _resources["SwitchLanguageDialogText"],
            _resources["YesDialogButton"],
            _resources["NoDialogButton"]);
    }

    private Task NotifySwitch()
    {
        return _dialogService.Notify(
            _resources["LanguageSwitchedTitle"],
            _resources["LanguageSwitchedText"],
            _resources["OKDialogButton"]);
    }

    private void SwitchLanguage(string newLanguage)
    {
        CurrentLanguage = newLanguage;

        var newCulture = new CultureInfo(newLanguage);

        _localizationManager
            .UpdateUserCulture(newCulture);

        WeakReferenceMessenger.Default.Send(
            new CultureChangedMessage(newCulture));
    }
}