namespace RecipesApp.Localization.Maui;

[ContentProperty(nameof(Key))]
public class TranslateExtension : IMarkupExtension<BindingBase>
{
    public string Key { get; set; }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) { return ProvideValue(serviceProvider); }

    public BindingBase ProvideValue(IServiceProvider serviceProvider)
    {
        return new Binding
        {
            Mode = BindingMode.OneWay,
            Path = $"[{Key}]",
            Source = LocalizedResourcesProvider.Instance
        };
    }
}