using System.Globalization;
using System.Resources;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RecipesApp.Localization;

public class LocalizedResourcesProvider : ObservableObject, ILocalizedResourcesProvider
{
    private readonly ResourceManager resourceManager;
    private CultureInfo currentCulture;

    public LocalizedResourcesProvider(ResourceManager resourceManager)
    {
        this.resourceManager = resourceManager;
        currentCulture = CultureInfo.CurrentUICulture;
        Instance = this;
    }

    public static LocalizedResourcesProvider Instance { get; private set; }

    public string this[string key]
        => resourceManager.GetString(key, currentCulture)
            ?? key;

    public void UpdateCulture(CultureInfo cultureInfo)
    {
        currentCulture = cultureInfo;
        OnPropertyChanged("Item");
    }
}