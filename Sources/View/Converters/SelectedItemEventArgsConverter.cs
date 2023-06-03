using System.Globalization;
using ViewModel;

namespace View.Converters
{
    public class SelectedItemEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as SelectionChangedEventArgs;
            return eventArgs?.CurrentSelection.FirstOrDefault() as ChampionVM;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
