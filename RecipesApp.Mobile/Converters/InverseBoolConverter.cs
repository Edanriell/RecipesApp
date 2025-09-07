using System.Globalization;

namespace RecipesApp.Mobile.Converters;

public class InverseBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Inverse(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Inverse(value);
    }

    private bool Inverse(object value)
    {
        return value switch
        {
            bool b => !b,
            _ => false
        };
    }
}