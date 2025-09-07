using System.Globalization;

namespace RecipesApp.Mobile.Converters;

public class RatingToStarsConverter : IValueConverter
{
    public object Convert(
        object value, Type targetType,
        object parameter, CultureInfo culture)
    {
        if (value is not double rating
            || rating < 0 || rating > 5)
            return string.Empty;

        var fullStar = "\ue838";
        var halfStar = "\ue839";

        var fullStars = (int)rating;
        var hasHalfStar = rating % 1 >= 0.5;

        return string.Concat(
            string.Join("", Enumerable.Repeat(fullStar, fullStars)),
            hasHalfStar ? halfStar : "");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}