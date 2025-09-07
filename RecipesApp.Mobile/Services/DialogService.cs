using RecipesApp.Client.Core.Services;

namespace RecipesApp.Mobile.Services;

public class DialogService : IDialogService
{
    public Task Notify(string title, string message, string buttonText = "OK")
    {
        return Application.Current.MainPage.DisplayAlert(title, message, buttonText);
    }

    public Task<bool> AskYesNo(
        string title, string message, string trueButtonText = "Yes", string falseButtonText = "No")
    {
        return Application.Current.MainPage.DisplayAlert(title, message, trueButtonText, falseButtonText);
    }

    public Task<string?> Ask(
        string title, string message, string acceptButtonText = "OK", string cancelButtonText = "Cancel")
    {
        return Application.Current.MainPage.DisplayPromptAsync(title, message, acceptButtonText, cancelButtonText);
    }
}