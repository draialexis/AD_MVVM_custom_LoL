using System.Globalization;
using ViewModel;

namespace View.Converters
{
    public class SelectedItemEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as SelectionChangedEventArgs;

            // It's fine that it might return null, but we can't change the methods return type accordingly
            // if we want to implement the interface
#pragma warning disable CS8603 // Possible null reference return.
            return eventArgs?.CurrentSelection.Count > 0 ? eventArgs.CurrentSelection[0] as ChampionVM : null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
