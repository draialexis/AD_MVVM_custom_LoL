using System.Globalization;

namespace View.Converters
{
    public class Base64ImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string base64Image = value as string;

            if (string.IsNullOrEmpty(base64Image))
            {
                return null;
            }

            byte[] imageBytes = System.Convert.FromBase64String(base64Image);

            return ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
