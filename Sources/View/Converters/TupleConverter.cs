using System.Globalization;

namespace View.Converters
{
    public class TupleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is string key && int.TryParse(values[1] as string, out int value))
            {
                return new Tuple<string, int>(key, value);
            }
            // It's fine that it might return null, but we can't change the methods return type accordingly
            // if we want to implement the interface
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
