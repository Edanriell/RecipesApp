using System.Globalization;

namespace RecipesApp.Localization;

public interface ILocalizedResourcesProvider
{
    string this[string key] { get; }

    void UpdateCulture(CultureInfo cultureInfo);
}