using System.Globalization;

namespace View.Converters
{
    public class StringStringStringTupleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3 &&
                values[0] is string name &&
                values[1] is string type &&
                values[2] is string description)
            {
                return new Tuple<string, string, string>(name, type, description);
            }
            // It's fine that it might return null, but we can't change the methods return type accordingly
            // if we want to implement the interface
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var tuple = (Tuple<string, string, string>)value;
            return new object[] { tuple.Item1, tuple.Item2, tuple.Item3 };
        }
    }

}
